﻿namespace WebLinkCrawler
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnOutputFile = new System.Windows.Forms.Button();
            this.tbOutputCSVFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numTaskCount = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numMaxNumberLinkDiversity = new System.Windows.Forms.NumericUpDown();
            this.numMaxNumberDomainCollected = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblTaskCount = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblProcessedUrls = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.remainingUrls = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblNewUrl = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblnitialUrl = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTotalUrl = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnFix = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTaskCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxNumberLinkDiversity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxNumberDomainCollected)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnOutputFile);
            this.groupBox7.Controls.Add(this.tbOutputCSVFile);
            this.groupBox7.Controls.Add(this.label5);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox7.ForeColor = System.Drawing.Color.White;
            this.groupBox7.Location = new System.Drawing.Point(12, 266);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(715, 70);
            this.groupBox7.TabIndex = 12;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Output File";
            // 
            // btnOutputFile
            // 
            this.btnOutputFile.BackColor = System.Drawing.Color.Green;
            this.btnOutputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOutputFile.Location = new System.Drawing.Point(590, 27);
            this.btnOutputFile.Name = "btnOutputFile";
            this.btnOutputFile.Size = new System.Drawing.Size(110, 29);
            this.btnOutputFile.TabIndex = 3;
            this.btnOutputFile.Text = "Select";
            this.btnOutputFile.UseVisualStyleBackColor = false;
            this.btnOutputFile.Click += new System.EventHandler(this.btnOutputFile_Click);
            // 
            // tbOutputCSVFile
            // 
            this.tbOutputCSVFile.Location = new System.Drawing.Point(81, 27);
            this.tbOutputCSVFile.Name = "tbOutputCSVFile";
            this.tbOutputCSVFile.Size = new System.Drawing.Size(454, 29);
            this.tbOutputCSVFile.TabIndex = 2;
            this.tbOutputCSVFile.Text = "outputs.txt";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 24);
            this.label5.TabIndex = 1;
            this.label5.Text = "Path :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFile);
            this.groupBox2.Controls.Add(this.tbFile);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(715, 70);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Initial Url List";
            // 
            // btnFile
            // 
            this.btnFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnFile.Location = new System.Drawing.Point(590, 25);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(110, 29);
            this.btnFile.TabIndex = 2;
            this.btnFile.Text = "Select";
            this.btnFile.UseVisualStyleBackColor = false;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // tbFile
            // 
            this.tbFile.Location = new System.Drawing.Point(81, 25);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(454, 29);
            this.tbFile.TabIndex = 1;
            this.tbFile.Text = "referenceLinks.txt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path :";
            // 
            // rtbConsole
            // 
            this.rtbConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rtbConsole.Location = new System.Drawing.Point(12, 478);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.Size = new System.Drawing.Size(715, 202);
            this.rtbConsole.TabIndex = 13;
            this.rtbConsole.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numTaskCount);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numTimeout);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numMaxNumberLinkDiversity);
            this.groupBox1.Controls.Add(this.numMaxNumberDomainCollected);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 101);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(715, 159);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Driver Settings";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // numTaskCount
            // 
            this.numTaskCount.Location = new System.Drawing.Point(590, 84);
            this.numTaskCount.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numTaskCount.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numTaskCount.Name = "numTaskCount";
            this.numTaskCount.Size = new System.Drawing.Size(110, 29);
            this.numTaskCount.TabIndex = 12;
            this.numTaskCount.Value = new decimal(new int[] {
            350,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(465, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "Task  Count:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTimeout
            // 
            this.numTimeout.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTimeout.Location = new System.Drawing.Point(308, 84);
            this.numTimeout.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numTimeout.Minimum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(120, 29);
            this.numTimeout.TabIndex = 10;
            this.numTimeout.Value = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "Web Request Timeout :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numMaxNumberLinkDiversity
            // 
            this.numMaxNumberLinkDiversity.Location = new System.Drawing.Point(590, 42);
            this.numMaxNumberLinkDiversity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxNumberLinkDiversity.Name = "numMaxNumberLinkDiversity";
            this.numMaxNumberLinkDiversity.Size = new System.Drawing.Size(110, 29);
            this.numMaxNumberLinkDiversity.TabIndex = 8;
            this.numMaxNumberLinkDiversity.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // numMaxNumberDomainCollected
            // 
            this.numMaxNumberDomainCollected.Location = new System.Drawing.Point(308, 42);
            this.numMaxNumberDomainCollected.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numMaxNumberDomainCollected.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMaxNumberDomainCollected.Name = "numMaxNumberDomainCollected";
            this.numMaxNumberDomainCollected.Size = new System.Drawing.Size(120, 29);
            this.numMaxNumberDomainCollected.TabIndex = 7;
            this.numMaxNumberDomainCollected.Value = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(455, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 24);
            this.label6.TabIndex = 6;
            this.label6.Text = "Sample Size :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(271, 24);
            this.label4.TabIndex = 5;
            this.label4.Text = "Max Unique Web Page Count :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblTaskCount);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.lblProcessedUrls);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.remainingUrls);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblNewUrl);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.lblnitialUrl);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.lblTotalUrl);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(12, 380);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(715, 92);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "From Last Iteration";
            // 
            // lblTaskCount
            // 
            this.lblTaskCount.AutoSize = true;
            this.lblTaskCount.Location = new System.Drawing.Point(591, 27);
            this.lblTaskCount.Name = "lblTaskCount";
            this.lblTaskCount.Size = new System.Drawing.Size(34, 20);
            this.lblTaskCount.TabIndex = 15;
            this.lblTaskCount.Text = "-----";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(487, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 20);
            this.label12.TabIndex = 14;
            this.label12.Text = "Task Count :";
            // 
            // lblProcessedUrls
            // 
            this.lblProcessedUrls.AutoSize = true;
            this.lblProcessedUrls.Location = new System.Drawing.Point(110, 62);
            this.lblProcessedUrls.Name = "lblProcessedUrls";
            this.lblProcessedUrls.Size = new System.Drawing.Size(34, 20);
            this.lblProcessedUrls.TabIndex = 13;
            this.lblProcessedUrls.Text = "-----";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 20);
            this.label10.TabIndex = 12;
            this.label10.Text = "Processed :";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // remainingUrls
            // 
            this.remainingUrls.AutoSize = true;
            this.remainingUrls.Location = new System.Drawing.Point(590, 62);
            this.remainingUrls.Name = "remainingUrls";
            this.remainingUrls.Size = new System.Drawing.Size(34, 20);
            this.remainingUrls.TabIndex = 11;
            this.remainingUrls.Text = "-----";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(504, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 20);
            this.label8.TabIndex = 10;
            this.label8.Text = "Urls Left :";
            // 
            // lblNewUrl
            // 
            this.lblNewUrl.AutoSize = true;
            this.lblNewUrl.Location = new System.Drawing.Point(351, 62);
            this.lblNewUrl.Name = "lblNewUrl";
            this.lblNewUrl.Size = new System.Drawing.Size(34, 20);
            this.lblNewUrl.TabIndex = 8;
            this.lblNewUrl.Text = "-----";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(265, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 20);
            this.label13.TabIndex = 7;
            this.label13.Text = "New Urls :";
            // 
            // lblnitialUrl
            // 
            this.lblnitialUrl.AutoSize = true;
            this.lblnitialUrl.Location = new System.Drawing.Point(351, 27);
            this.lblnitialUrl.Name = "lblnitialUrl";
            this.lblnitialUrl.Size = new System.Drawing.Size(34, 20);
            this.lblnitialUrl.TabIndex = 6;
            this.lblnitialUrl.Text = "-----";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(259, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 20);
            this.label11.TabIndex = 5;
            this.label11.Text = "Initial Urls :";
            // 
            // lblTotalUrl
            // 
            this.lblTotalUrl.AutoSize = true;
            this.lblTotalUrl.Location = new System.Drawing.Point(110, 27);
            this.lblTotalUrl.Name = "lblTotalUrl";
            this.lblTotalUrl.Size = new System.Drawing.Size(34, 20);
            this.lblTotalUrl.TabIndex = 4;
            this.lblTotalUrl.Text = "-----";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 20);
            this.label9.TabIndex = 3;
            this.label9.Text = "Total Urls :";
            // 
            // btnFix
            // 
            this.btnFix.BackColor = System.Drawing.Color.Transparent;
            this.btnFix.BackgroundImage = global::WebLinkCrawler.Properties.Resources.btn_fix_256;
            this.btnFix.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnFix.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFix.Location = new System.Drawing.Point(612, 695);
            this.btnFix.Margin = new System.Windows.Forms.Padding(0);
            this.btnFix.Name = "btnFix";
            this.btnFix.Size = new System.Drawing.Size(25, 25);
            this.btnFix.TabIndex = 17;
            this.toolTip1.SetToolTip(this.btnFix, "Fix Output");
            this.btnFix.UseVisualStyleBackColor = false;
            this.btnFix.Click += new System.EventHandler(this.btnFix_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.BackgroundImage = global::WebLinkCrawler.Properties.Resources.btn_stop_256;
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnStop.Location = new System.Drawing.Point(659, 695);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(25, 25);
            this.btnStop.TabIndex = 15;
            this.toolTip1.SetToolTip(this.btnStop, "Stop");
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.BackgroundImage = global::WebLinkCrawler.Properties.Resources.btn_play_256;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnStart.Location = new System.Drawing.Point(702, 695);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(25, 25);
            this.btnStart.TabIndex = 14;
            this.toolTip1.SetToolTip(this.btnStart, "Start");
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(741, 739);
            this.Controls.Add(this.btnFix);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.rtbConsole);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTaskCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxNumberLinkDiversity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxNumberDomainCollected)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnOutputFile;
        private System.Windows.Forms.TextBox tbOutputCSVFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numMaxNumberLinkDiversity;
        private System.Windows.Forms.NumericUpDown numMaxNumberDomainCollected;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblTotalUrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblnitialUrl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblNewUrl;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label remainingUrls;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblProcessedUrls;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTaskCount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnFix;
        private System.Windows.Forms.NumericUpDown numTaskCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

