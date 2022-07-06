namespace TenguUI
{
    partial class App
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchGenresComboBox = new System.Windows.Forms.ComboBox();
            this.SearchTitleTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DownlaMaxPacketSizeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TenguHostsComboBox = new System.Windows.Forms.ComboBox();
            this.DownlaMaxConnectionsTextBox = new System.Windows.Forms.TextBox();
            this.DownlaDownloadPathTextBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.LoadMoreEpisodesButton = new System.Windows.Forms.Button();
            this.GetEpisodesButton = new System.Windows.Forms.Button();
            this.GetEpisodesComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.VideoComboBox = new System.Windows.Forms.ComboBox();
            this.StartStreamButton = new System.Windows.Forms.Button();
            this.StartDownloadButton = new System.Windows.Forms.Button();
            this.VideoProgressBar = new System.Windows.Forms.ProgressBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.VideoProgressBarUpdater = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SearchButton);
            this.groupBox1.Controls.Add(this.SearchGenresComboBox);
            this.groupBox1.Controls.Add(this.SearchTitleTextBox);
            this.groupBox1.Location = new System.Drawing.Point(11, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(386, 107);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Animes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(199, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Genres";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Title";
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(8, 78);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(372, 23);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchGenresComboBox
            // 
            this.SearchGenresComboBox.FormattingEnabled = true;
            this.SearchGenresComboBox.Location = new System.Drawing.Point(199, 49);
            this.SearchGenresComboBox.Name = "SearchGenresComboBox";
            this.SearchGenresComboBox.Size = new System.Drawing.Size(181, 23);
            this.SearchGenresComboBox.TabIndex = 1;
            // 
            // SearchTitleTextBox
            // 
            this.SearchTitleTextBox.Location = new System.Drawing.Point(8, 49);
            this.SearchTitleTextBox.Name = "SearchTitleTextBox";
            this.SearchTitleTextBox.Size = new System.Drawing.Size(185, 23);
            this.SearchTitleTextBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.DownlaMaxPacketSizeTextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.TenguHostsComboBox);
            this.groupBox2.Controls.Add(this.DownlaMaxConnectionsTextBox);
            this.groupBox2.Controls.Add(this.DownlaDownloadPathTextBox);
            this.groupBox2.Location = new System.Drawing.Point(403, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 195);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(194, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 15);
            this.label7.TabIndex = 5;
            this.label7.Text = "Packet Size";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Max Connections";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "Download Path";
            // 
            // DownlaMaxPacketSizeTextBox
            // 
            this.DownlaMaxPacketSizeTextBox.Location = new System.Drawing.Point(194, 165);
            this.DownlaMaxPacketSizeTextBox.Name = "DownlaMaxPacketSizeTextBox";
            this.DownlaMaxPacketSizeTextBox.Size = new System.Drawing.Size(185, 23);
            this.DownlaMaxPacketSizeTextBox.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Hosts";
            // 
            // TenguHostsComboBox
            // 
            this.TenguHostsComboBox.FormattingEnabled = true;
            this.TenguHostsComboBox.Location = new System.Drawing.Point(6, 49);
            this.TenguHostsComboBox.Name = "TenguHostsComboBox";
            this.TenguHostsComboBox.Size = new System.Drawing.Size(373, 23);
            this.TenguHostsComboBox.TabIndex = 0;
            // 
            // DownlaMaxConnectionsTextBox
            // 
            this.DownlaMaxConnectionsTextBox.Location = new System.Drawing.Point(6, 165);
            this.DownlaMaxConnectionsTextBox.Name = "DownlaMaxConnectionsTextBox";
            this.DownlaMaxConnectionsTextBox.Size = new System.Drawing.Size(181, 23);
            this.DownlaMaxConnectionsTextBox.TabIndex = 1;
            // 
            // DownlaDownloadPathTextBox
            // 
            this.DownlaDownloadPathTextBox.Location = new System.Drawing.Point(8, 121);
            this.DownlaDownloadPathTextBox.Name = "DownlaDownloadPathTextBox";
            this.DownlaDownloadPathTextBox.Size = new System.Drawing.Size(371, 23);
            this.DownlaDownloadPathTextBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.LoadMoreEpisodesButton);
            this.groupBox4.Controls.Add(this.GetEpisodesButton);
            this.groupBox4.Controls.Add(this.GetEpisodesComboBox);
            this.groupBox4.Location = new System.Drawing.Point(12, 127);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(385, 82);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Get Episodes";
            // 
            // LoadMoreEpisodesButton
            // 
            this.LoadMoreEpisodesButton.Enabled = false;
            this.LoadMoreEpisodesButton.Location = new System.Drawing.Point(282, 52);
            this.LoadMoreEpisodesButton.Name = "LoadMoreEpisodesButton";
            this.LoadMoreEpisodesButton.Size = new System.Drawing.Size(97, 23);
            this.LoadMoreEpisodesButton.TabIndex = 2;
            this.LoadMoreEpisodesButton.Text = "Load More";
            this.LoadMoreEpisodesButton.UseVisualStyleBackColor = true;
            this.LoadMoreEpisodesButton.Click += new System.EventHandler(this.LoadMoreEpisodesButton_Click);
            // 
            // GetEpisodesButton
            // 
            this.GetEpisodesButton.Enabled = false;
            this.GetEpisodesButton.Location = new System.Drawing.Point(6, 52);
            this.GetEpisodesButton.Name = "GetEpisodesButton";
            this.GetEpisodesButton.Size = new System.Drawing.Size(270, 23);
            this.GetEpisodesButton.TabIndex = 1;
            this.GetEpisodesButton.Text = "Get Episodes";
            this.GetEpisodesButton.UseVisualStyleBackColor = true;
            this.GetEpisodesButton.Click += new System.EventHandler(this.GetEpisodesButton_Click);
            // 
            // GetEpisodesComboBox
            // 
            this.GetEpisodesComboBox.DisplayMember = "Title";
            this.GetEpisodesComboBox.Enabled = false;
            this.GetEpisodesComboBox.FormattingEnabled = true;
            this.GetEpisodesComboBox.Location = new System.Drawing.Point(6, 23);
            this.GetEpisodesComboBox.Name = "GetEpisodesComboBox";
            this.GetEpisodesComboBox.Size = new System.Drawing.Size(373, 23);
            this.GetEpisodesComboBox.TabIndex = 0;
            this.GetEpisodesComboBox.Text = "Choose an anime";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.VideoComboBox);
            this.groupBox5.Controls.Add(this.StartStreamButton);
            this.groupBox5.Controls.Add(this.StartDownloadButton);
            this.groupBox5.Controls.Add(this.VideoProgressBar);
            this.groupBox5.Controls.Add(this.progressBar1);
            this.groupBox5.Location = new System.Drawing.Point(12, 215);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(776, 109);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Video";
            // 
            // VideoComboBox
            // 
            this.VideoComboBox.DisplayMember = "EpisodeNumber";
            this.VideoComboBox.Enabled = false;
            this.VideoComboBox.FormattingEnabled = true;
            this.VideoComboBox.Location = new System.Drawing.Point(7, 21);
            this.VideoComboBox.Name = "VideoComboBox";
            this.VideoComboBox.Size = new System.Drawing.Size(372, 23);
            this.VideoComboBox.TabIndex = 3;
            this.VideoComboBox.Text = "Choose an Episode";
            // 
            // StartStreamButton
            // 
            this.StartStreamButton.Enabled = false;
            this.StartStreamButton.Location = new System.Drawing.Point(7, 50);
            this.StartStreamButton.Name = "StartStreamButton";
            this.StartStreamButton.Size = new System.Drawing.Size(763, 23);
            this.StartStreamButton.TabIndex = 2;
            this.StartStreamButton.Text = "Start Stream";
            this.StartStreamButton.UseVisualStyleBackColor = true;
            // 
            // StartDownloadButton
            // 
            this.StartDownloadButton.Enabled = false;
            this.StartDownloadButton.Location = new System.Drawing.Point(399, 20);
            this.StartDownloadButton.Name = "StartDownloadButton";
            this.StartDownloadButton.Size = new System.Drawing.Size(371, 23);
            this.StartDownloadButton.TabIndex = 1;
            this.StartDownloadButton.Text = "Start Download";
            this.StartDownloadButton.UseVisualStyleBackColor = true;
            this.StartDownloadButton.Click += new System.EventHandler(this.StartDownloadButton_Click);
            // 
            // VideoProgressBar
            // 
            this.VideoProgressBar.Location = new System.Drawing.Point(7, 79);
            this.VideoProgressBar.Name = "VideoProgressBar";
            this.VideoProgressBar.Size = new System.Drawing.Size(763, 23);
            this.VideoProgressBar.TabIndex = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 50);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(763, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 442);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Logs";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 460);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(776, 108);
            this.textBox1.TabIndex = 6;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(12, 330);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(770, 100);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Kitsu";
            // 
            // VideoProgressBarUpdater
            // 
            this.VideoProgressBarUpdater.DoWork += new System.ComponentModel.DoWorkEventHandler(this.VideoProgressBarUpdater_DoWork);
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 580);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "App";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private ProgressBar progressBar1;
        private Label label1;
        private TextBox textBox1;
        private TextBox SearchTitleTextBox;
        private ComboBox SearchGenresComboBox;
        private Button SearchButton;
        private ComboBox TenguHostsComboBox;
        private TextBox DownlaMaxPacketSizeTextBox;
        private TextBox DownlaMaxConnectionsTextBox;
        private TextBox DownlaDownloadPathTextBox;
        private ComboBox VideoComboBox;
        private Button StartStreamButton;
        private Button StartDownloadButton;
        private ProgressBar VideoProgressBar;
        private Label label3;
        private Label label2;
        private Label label4;
        private GroupBox groupBox6;
        private Button GetEpisodesButton;
        private ComboBox GetEpisodesComboBox;
        private Label label5;
        private Label label7;
        private Label label6;
        private System.ComponentModel.BackgroundWorker VideoProgressBarUpdater;
        private Button LoadMoreEpisodesButton;
    }
}