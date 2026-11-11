namespace WinFormsApp1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            toolbarPanel = new Panel();
            searchEngineComboBox = new ComboBox();
            addressTextBox = new TextBox();
            navigateButton = new Button();
            historyButton = new Button();
            newTabButton = new Button();
            closeTabButton = new Button();
            backButton = new Button();
            forwardButton = new Button();
            refreshButton = new Button();
            tabs = new TabControl();
            toolbarPanel.SuspendLayout();
            SuspendLayout();
            // 
            // toolbarPanel
            // 
            toolbarPanel.Controls.Add(searchEngineComboBox);
            toolbarPanel.Controls.Add(addressTextBox);
            toolbarPanel.Controls.Add(navigateButton);
            toolbarPanel.Controls.Add(historyButton);
            toolbarPanel.Controls.Add(newTabButton);
            toolbarPanel.Controls.Add(closeTabButton);
            toolbarPanel.Controls.Add(backButton);
            toolbarPanel.Controls.Add(forwardButton);
            toolbarPanel.Controls.Add(refreshButton);
            toolbarPanel.Dock = DockStyle.Top;
            toolbarPanel.Location = new Point(0, 0);
            toolbarPanel.Name = "toolbarPanel";
            toolbarPanel.Size = new Size(1100, 76);
            toolbarPanel.TabIndex = 0;
            // 
            // searchEngineComboBox
            // 
            searchEngineComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            searchEngineComboBox.Items.AddRange(new object[] { "Google", "Bing", "DuckDuckGo" });
            searchEngineComboBox.Location = new Point(12, 12);
            searchEngineComboBox.Name = "searchEngineComboBox";
            searchEngineComboBox.Size = new Size(130, 23);
            searchEngineComboBox.TabIndex = 0;
            // 
            // addressTextBox
            // 
            addressTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            addressTextBox.Location = new Point(148, 12);
            addressTextBox.Name = "addressTextBox";
            addressTextBox.PlaceholderText = "Enter a URL or search term";
            addressTextBox.Size = new Size(550, 23);
            addressTextBox.TabIndex = 1;
            addressTextBox.KeyDown += AddressTextBox_KeyDown;
            // 
            // navigateButton
            // 
            navigateButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            navigateButton.Location = new Point(704, 11);
            navigateButton.Name = "navigateButton";
            navigateButton.Size = new Size(65, 25);
            navigateButton.TabIndex = 2;
            navigateButton.Text = "Go";
            navigateButton.UseVisualStyleBackColor = true;
            navigateButton.Click += NavigateButton_Click;
            // 
            // historyButton
            // 
            historyButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            historyButton.Location = new Point(775, 11);
            historyButton.Name = "historyButton";
            historyButton.Size = new Size(75, 25);
            historyButton.TabIndex = 3;
            historyButton.Text = "History";
            historyButton.UseVisualStyleBackColor = true;
            historyButton.Click += HistoryButton_Click;
            // 
            // newTabButton
            // 
            newTabButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            newTabButton.Location = new Point(856, 11);
            newTabButton.Name = "newTabButton";
            newTabButton.Size = new Size(90, 25);
            newTabButton.TabIndex = 4;
            newTabButton.Text = "New tab";
            newTabButton.UseVisualStyleBackColor = true;
            newTabButton.Click += NewTabButton_Click;
            // 
            // closeTabButton
            // 
            closeTabButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeTabButton.Location = new Point(952, 11);
            closeTabButton.Name = "closeTabButton";
            closeTabButton.Size = new Size(75, 25);
            closeTabButton.TabIndex = 5;
            closeTabButton.Text = "Close tab";
            closeTabButton.UseVisualStyleBackColor = true;
            closeTabButton.Click += CloseTabButton_Click;
            // 
            // backButton
            // 
            backButton.Location = new Point(12, 43);
            backButton.Name = "backButton";
            backButton.Size = new Size(65, 25);
            backButton.TabIndex = 6;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += BackButton_Click;
            // 
            // forwardButton
            // 
            forwardButton.Location = new Point(83, 43);
            forwardButton.Name = "forwardButton";
            forwardButton.Size = new Size(65, 25);
            forwardButton.TabIndex = 7;
            forwardButton.Text = "Forward";
            forwardButton.UseVisualStyleBackColor = true;
            forwardButton.Click += ForwardButton_Click;
            // 
            // refreshButton
            // 
            refreshButton.Location = new Point(154, 43);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(75, 25);
            refreshButton.TabIndex = 8;
            refreshButton.Text = "Refresh";
            refreshButton.UseVisualStyleBackColor = true;
            refreshButton.Click += RefreshButton_Click;
            // 
            // tabs
            // 
            tabs.Dock = DockStyle.Fill;
            tabs.Location = new Point(0, 76);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(1100, 574);
            tabs.TabIndex = 1;
            tabs.SelectedIndexChanged += Tabs_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 650);
            Controls.Add(tabs);
            Controls.Add(toolbarPanel);
            MinimumSize = new Size(800, 450);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WebView2 Browser";
            FormClosing += Form1_FormClosing;
            toolbarPanel.ResumeLayout(false);
            toolbarPanel.PerformLayout();
            ResumeLayout(false);
        }

        private Panel toolbarPanel;
        private ComboBox searchEngineComboBox;
        private TextBox addressTextBox;
        private Button navigateButton;
        private Button historyButton;
        private Button newTabButton;
        private Button closeTabButton;
        private Button backButton;
        private Button forwardButton;
        private Button refreshButton;
        private TabControl tabs;
    }
}
