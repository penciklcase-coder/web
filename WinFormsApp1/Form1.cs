using System.Globalization;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private const int MaximumHistoryEntries = 500;
        private static readonly string HistoryFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WinFormsApp1",
            "history.txt");

        private readonly List<HistoryEntry> _history = [];

        public Form1()
        {
            InitializeComponent();
            searchEngineComboBox.SelectedIndex = 0;
            LoadHistory();
            CreateTab("https://www.google.com");
        }

        private async void CreateTab(string? initialUrl = null)
        {
            var page = new TabPage("Loading...");
            var webView = new WebView2
            {
                AllowExternalDrop = true,
                Dock = DockStyle.Fill,
                DefaultBackgroundColor = Color.White,
                Tag = initialUrl
            };

            page.Controls.Add(webView);
            tabs.TabPages.Add(page);
            tabs.SelectedTab = page;

            try
            {
                await webView.EnsureCoreWebView2Async();
                if (page.IsDisposed)
                {
                    return;
                }

                webView.CoreWebView2.DocumentTitleChanged += (_, _) => UpdateTabTitle(page, webView);
                webView.CoreWebView2.NavigationCompleted += (_, args) =>
                {
                    if (args.IsSuccess && webView.Source is not null)
                    {
                        AddHistoryEntry(webView.CoreWebView2.DocumentTitle, webView.Source.AbsoluteUri);
                    }

                    UpdateNavigationButtons();
                };
                webView.SourceChanged += (_, _) =>
                {
                    if (tabs.SelectedTab == page && webView.Source is not null)
                    {
                        addressTextBox.Text = webView.Source.AbsoluteUri;
                    }
                };

                UpdateTabTitle(page, webView);
                string? pendingUrl = webView.Tag as string;
                webView.Tag = null;
                if (!string.IsNullOrWhiteSpace(pendingUrl))
                {
                    webView.CoreWebView2.Navigate(pendingUrl);
                }

                UpdateNavigationButtons();
            }
            catch (Exception ex)
            {
                page.Text = "Browser error";
                MessageBox.Show(this, $"Unable to initialize WebView2: {ex.Message}", "WebView2 error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private WebView2? GetActiveWebView()
        {
            return tabs.SelectedTab?.Controls.OfType<WebView2>().FirstOrDefault();
        }

        private void NavigateTo(string url)
        {
            WebView2? webView = GetActiveWebView();
            if (webView is null)
            {
                return;
            }

            if (webView.CoreWebView2 is null)
            {
                webView.Tag = url;
                return;
            }

            webView.CoreWebView2.Navigate(url);
        }

        private void NavigateFromAddressBar()
        {
            string input = addressTextBox.Text.Trim();
            if (input.Length == 0)
            {
                return;
            }

            string destination;
            if (Uri.TryCreate(input, UriKind.Absolute, out Uri? uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                destination = uri.AbsoluteUri;
            }
            else if (!input.Any(char.IsWhiteSpace) &&
                     (input.Contains('.') || input.Equals("localhost", StringComparison.OrdinalIgnoreCase)))
            {
                destination = $"https://{input}";
            }
            else
            {
                string searchUrl = searchEngineComboBox.SelectedItem?.ToString() switch
                {
                    "Bing" => "https://www.bing.com/search?q=",
                    "DuckDuckGo" => "https://duckduckgo.com/?q=",
                    _ => "https://www.google.com/search?q="
                };
                destination = searchUrl + Uri.EscapeDataString(input);
            }

            NavigateTo(destination);
        }

        private void NavigateButton_Click(object? sender, EventArgs e)
        {
            NavigateFromAddressBar();
        }

        private void AddressTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                NavigateFromAddressBar();
                e.SuppressKeyPress = true;
            }
        }

        private void BackButton_Click(object? sender, EventArgs e)
        {
            WebView2? webView = GetActiveWebView();
            if (webView?.CoreWebView2?.CanGoBack == true)
            {
                webView.CoreWebView2.GoBack();
            }
        }

        private void ForwardButton_Click(object? sender, EventArgs e)
        {
            WebView2? webView = GetActiveWebView();
            if (webView?.CoreWebView2?.CanGoForward == true)
            {
                webView.CoreWebView2.GoForward();
            }
        }

        private void RefreshButton_Click(object? sender, EventArgs e)
        {
            GetActiveWebView()?.CoreWebView2?.Reload();
        }

        private void NewTabButton_Click(object? sender, EventArgs e)
        {
            CreateTab("https://www.google.com");
        }

        private void CloseTabButton_Click(object? sender, EventArgs e)
        {
            if (tabs.SelectedTab is null)
            {
                return;
            }

            TabPage closingPage = tabs.SelectedTab;
            tabs.TabPages.Remove(closingPage);
            closingPage.Dispose();

            if (tabs.TabPages.Count == 0)
            {
                CreateTab("https://www.google.com");
            }

            UpdateNavigationButtons();
        }

        private void Tabs_SelectedIndexChanged(object? sender, EventArgs e)
        {
            WebView2? webView = GetActiveWebView();
            addressTextBox.Text = webView?.Source?.AbsoluteUri ?? string.Empty;
            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            WebView2? webView = GetActiveWebView();
            backButton.Enabled = webView?.CoreWebView2?.CanGoBack == true;
            forwardButton.Enabled = webView?.CoreWebView2?.CanGoForward == true;
        }

        private void LoadHistory()
        {
            try
            {
                if (File.Exists(HistoryFilePath))
                {
                    foreach (string line in File.ReadLines(HistoryFilePath).Take(MaximumHistoryEntries))
                    {
                        string[] fields = line.Split('\t', 3);
                        if (fields.Length == 3 && DateTime.TryParse(
                            fields[0], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime visitedAt))
                        {
                            _history.Add(new HistoryEntry(fields[1], fields[2], visitedAt));
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                _history.Clear();
            }
        }

        private void AddHistoryEntry(string? title, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            _history.Insert(0, new HistoryEntry(
                string.IsNullOrWhiteSpace(title) ? url : title,
                url,
                DateTime.Now));
            if (_history.Count > MaximumHistoryEntries)
            {
                _history.RemoveRange(MaximumHistoryEntries, _history.Count - MaximumHistoryEntries);
            }

            SaveHistory();
        }

        private void SaveHistory()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(HistoryFilePath)!);
                string[] lines = _history.Select(entry =>
                {
                    string title = entry.Title.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ');
                    return $"{entry.VisitedAt.ToString("O", CultureInfo.InvariantCulture)}\t{title}\t{entry.Url}";
                }).ToArray();
                File.WriteAllLines(HistoryFilePath, lines);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
            }
        }

        private void HistoryButton_Click(object? sender, EventArgs e)
        {
            using var historyDialog = new Form
            {
                Text = "Browsing history",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(850, 500),
                MinimumSize = new Size(500, 300)
            };
            var historyList = new ListBox
            {
                Dock = DockStyle.Fill,
                IntegralHeight = false
            };
            historyList.Items.AddRange(_history.Cast<object>().ToArray());

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 46,
                Padding = new Padding(8),
                WrapContents = false
            };
            var closeButton = new Button { Text = "Close", AutoSize = true };
            var openButton = new Button { Text = "Open", AutoSize = true };
            openButton.Click += (_, _) =>
            {
                if (historyList.SelectedItem is HistoryEntry entry)
                {
                    NavigateTo(entry.Url);
                    historyDialog.Close();
                }
            };
            closeButton.Click += (_, _) => historyDialog.Close();
            historyList.DoubleClick += (_, _) => openButton.PerformClick();
            buttonPanel.Controls.Add(closeButton);
            buttonPanel.Controls.Add(openButton);
            historyDialog.Controls.Add(historyList);
            historyDialog.Controls.Add(buttonPanel);
            historyDialog.ShowDialog(this);
        }

        private static void UpdateTabTitle(TabPage page, WebView2 webView)
        {
            string title = webView.CoreWebView2.DocumentTitle;
            page.Text = string.IsNullOrWhiteSpace(title) ? "New tab" : title;
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            SaveHistory();
        }

        private sealed record HistoryEntry(string Title, string Url, DateTime VisitedAt)
        {
            public override string ToString() => $"{Title} — {Url} — {VisitedAt:g}";
        }
    }
}
