namespace FacebookAutoLikeConsoleSelenium
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_Run = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.timer_MainSchedulerTimer = new System.Windows.Forms.Timer(this.components);
            this.timer_FormStatusUpdater = new System.Windows.Forms.Timer(this.components);
            this.timer_Scrayping = new System.Windows.Forms.Timer(this.components);
            this.textBox_BrowserURL = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Run
            // 
            this.button_Run.BackColor = System.Drawing.Color.Transparent;
            this.button_Run.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Run.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.button_Run.ForeColor = System.Drawing.Color.Transparent;
            this.button_Run.Location = new System.Drawing.Point(2, 3);
            this.button_Run.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Run.Name = "button_Run";
            this.button_Run.Size = new System.Drawing.Size(59, 22);
            this.button_Run.TabIndex = 0;
            this.button_Run.Text = "RUN";
            this.button_Run.UseVisualStyleBackColor = false;
            this.button_Run.Click += new System.EventHandler(this.button_RunnStop_MianScheduler_Click);
            // 
            // button_Stop
            // 
            this.button_Stop.BackColor = System.Drawing.Color.Transparent;
            this.button_Stop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Stop.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.button_Stop.ForeColor = System.Drawing.Color.Transparent;
            this.button_Stop.Location = new System.Drawing.Point(3, 31);
            this.button_Stop.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(58, 22);
            this.button_Stop.TabIndex = 1;
            this.button_Stop.Text = "STOP";
            this.button_Stop.UseVisualStyleBackColor = false;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(719, 554);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // timer_MainSchedulerTimer
            // 
            //this.timer_MainSchedulerTimer.Tick += new System.EventHandler(this.timer_MainSchedulerTimer_Tick);
            // 
            // timer_FormStatusUpdater
            // 
            this.timer_FormStatusUpdater.Enabled = true;
            this.timer_FormStatusUpdater.Tick += new System.EventHandler(this.timer_FormStatusUpdater_Tick);
            // 
            // timer_Scrayping
            // 
            this.timer_Scrayping.Interval = 500;
            this.timer_Scrayping.Tick += new System.EventHandler(this.timer_Scrayping_Tick);
            // 
            // textBox_BrowserURL
            // 
            this.textBox_BrowserURL.Location = new System.Drawing.Point(3, 59);
            this.textBox_BrowserURL.Name = "textBox_BrowserURL";
            this.textBox_BrowserURL.Size = new System.Drawing.Size(107, 19);
            this.textBox_BrowserURL.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(868, 586);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(860, 560);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_Run);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_BrowserURL);
            this.splitContainer1.Panel1.Controls.Add(this.button_Stop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer1.Size = new System.Drawing.Size(854, 554);
            this.splitContainer1.SplitterDistance = 131;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(860, 560);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            this.ClientSize = new System.Drawing.Size(868, 586);
            this.Controls.Add(this.tabControl1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.Text = "Form1 アタッカー";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Run;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Timer timer_MainSchedulerTimer;
        private System.Windows.Forms.Timer timer_FormStatusUpdater;
        private System.Windows.Forms.Timer timer_Scrayping;
        private System.Windows.Forms.TextBox textBox_BrowserURL;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}

