namespace ComputerVision
{
    partial class MainForm
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
            this.panelSource = new System.Windows.Forms.Panel();
            this.panelDestination = new System.Windows.Forms.Panel();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.unsharpBtn = new System.Windows.Forms.Button();
            this.ftsBtn = new System.Windows.Forms.Button();
            this.median2Btn = new System.Windows.Forms.Button();
            this.markovBtn = new System.Windows.Forms.Button();
            this.medianBtn = new System.Windows.Forms.Button();
            this.ftjTxtBox = new System.Windows.Forms.TextBox();
            this.ftjBtn = new System.Windows.Forms.Button();
            this.rotateBar = new System.Windows.Forms.TrackBar();
            this.histogramBtn = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.lumenBar = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonGrayscale = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Kirsch = new System.Windows.Forms.Button();
            this.laplaceBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rotateBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lumenBar)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSource
            // 
            this.panelSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSource.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panelSource.Location = new System.Drawing.Point(16, 15);
            this.panelSource.Margin = new System.Windows.Forms.Padding(4);
            this.panelSource.Name = "panelSource";
            this.panelSource.Size = new System.Drawing.Size(425, 294);
            this.panelSource.TabIndex = 0;
            // 
            // panelDestination
            // 
            this.panelDestination.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDestination.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panelDestination.Location = new System.Drawing.Point(464, 15);
            this.panelDestination.Margin = new System.Windows.Forms.Padding(4);
            this.panelDestination.Name = "panelDestination";
            this.panelDestination.Size = new System.Drawing.Size(425, 294);
            this.panelDestination.TabIndex = 1;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(16, 540);
            this.buttonLoad.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(100, 28);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.laplaceBtn);
            this.panel1.Controls.Add(this.Kirsch);
            this.panel1.Controls.Add(this.unsharpBtn);
            this.panel1.Controls.Add(this.ftsBtn);
            this.panel1.Controls.Add(this.median2Btn);
            this.panel1.Controls.Add(this.markovBtn);
            this.panel1.Controls.Add(this.medianBtn);
            this.panel1.Controls.Add(this.ftjTxtBox);
            this.panel1.Controls.Add(this.ftjBtn);
            this.panel1.Controls.Add(this.rotateBar);
            this.panel1.Controls.Add(this.histogramBtn);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.lumenBar);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.buttonGrayscale);
            this.panel1.Location = new System.Drawing.Point(464, 334);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 233);
            this.panel1.TabIndex = 3;
            // 
            // unsharpBtn
            // 
            this.unsharpBtn.Location = new System.Drawing.Point(165, 4);
            this.unsharpBtn.Name = "unsharpBtn";
            this.unsharpBtn.Size = new System.Drawing.Size(75, 23);
            this.unsharpBtn.TabIndex = 26;
            this.unsharpBtn.Text = "Unsharp Masking";
            this.unsharpBtn.UseVisualStyleBackColor = true;
            this.unsharpBtn.Click += new System.EventHandler(this.unsharpBtn_Click);
            // 
            // ftsBtn
            // 
            this.ftsBtn.Location = new System.Drawing.Point(346, 26);
            this.ftsBtn.Name = "ftsBtn";
            this.ftsBtn.Size = new System.Drawing.Size(75, 23);
            this.ftsBtn.TabIndex = 25;
            this.ftsBtn.Text = "FTS";
            this.ftsBtn.UseVisualStyleBackColor = true;
            this.ftsBtn.Click += new System.EventHandler(this.ftsBtn_Click);
            // 
            // median2Btn
            // 
            this.median2Btn.Location = new System.Drawing.Point(154, 51);
            this.median2Btn.Name = "median2Btn";
            this.median2Btn.Size = new System.Drawing.Size(75, 23);
            this.median2Btn.TabIndex = 24;
            this.median2Btn.Text = "Median2";
            this.median2Btn.UseVisualStyleBackColor = true;
            this.median2Btn.Click += new System.EventHandler(this.median2Btn_Click);
            // 
            // markovBtn
            // 
            this.markovBtn.Location = new System.Drawing.Point(271, 61);
            this.markovBtn.Name = "markovBtn";
            this.markovBtn.Size = new System.Drawing.Size(75, 23);
            this.markovBtn.TabIndex = 23;
            this.markovBtn.Text = "Markov";
            this.markovBtn.UseVisualStyleBackColor = true;
            this.markovBtn.Click += new System.EventHandler(this.markovBtn_Click);
            // 
            // medianBtn
            // 
            this.medianBtn.Location = new System.Drawing.Point(154, 89);
            this.medianBtn.Name = "medianBtn";
            this.medianBtn.Size = new System.Drawing.Size(75, 23);
            this.medianBtn.TabIndex = 22;
            this.medianBtn.Text = "Median";
            this.medianBtn.UseVisualStyleBackColor = true;
            this.medianBtn.Click += new System.EventHandler(this.medianBtn_Click);
            // 
            // ftjTxtBox
            // 
            this.ftjTxtBox.Location = new System.Drawing.Point(228, 26);
            this.ftjTxtBox.Name = "ftjTxtBox";
            this.ftjTxtBox.Size = new System.Drawing.Size(100, 22);
            this.ftjTxtBox.TabIndex = 21;
            // 
            // ftjBtn
            // 
            this.ftjBtn.Location = new System.Drawing.Point(346, 61);
            this.ftjBtn.Name = "ftjBtn";
            this.ftjBtn.Size = new System.Drawing.Size(75, 23);
            this.ftjBtn.TabIndex = 20;
            this.ftjBtn.Text = "FTJ";
            this.ftjBtn.UseVisualStyleBackColor = true;
            this.ftjBtn.Click += new System.EventHandler(this.ftjBtn_Click);
            // 
            // rotateBar
            // 
            this.rotateBar.Location = new System.Drawing.Point(19, 3);
            this.rotateBar.Name = "rotateBar";
            this.rotateBar.Size = new System.Drawing.Size(104, 56);
            this.rotateBar.TabIndex = 19;
            this.rotateBar.Scroll += new System.EventHandler(this.rotateBar_Scroll);
            // 
            // histogramBtn
            // 
            this.histogramBtn.Location = new System.Drawing.Point(19, 61);
            this.histogramBtn.Name = "histogramBtn";
            this.histogramBtn.Size = new System.Drawing.Size(75, 23);
            this.histogramBtn.TabIndex = 18;
            this.histogramBtn.Text = "histogramBtn";
            this.histogramBtn.UseVisualStyleBackColor = true;
            this.histogramBtn.Click += new System.EventHandler(this.histogramBtn_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(242, 85);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 56);
            this.trackBar1.TabIndex = 17;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // lumenBar
            // 
            this.lumenBar.Location = new System.Drawing.Point(265, 172);
            this.lumenBar.Margin = new System.Windows.Forms.Padding(4);
            this.lumenBar.Maximum = 255;
            this.lumenBar.Minimum = -255;
            this.lumenBar.Name = "lumenBar";
            this.lumenBar.Size = new System.Drawing.Size(139, 56);
            this.lumenBar.TabIndex = 16;
            this.lumenBar.Scroll += new System.EventHandler(this.lumenBar_Scroll);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 190);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 14;
            this.button1.Text = "Negative";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonGrayscale
            // 
            this.buttonGrayscale.Location = new System.Drawing.Point(9, 191);
            this.buttonGrayscale.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGrayscale.Name = "buttonGrayscale";
            this.buttonGrayscale.Size = new System.Drawing.Size(100, 28);
            this.buttonGrayscale.TabIndex = 13;
            this.buttonGrayscale.Text = "Grayscale";
            this.buttonGrayscale.UseVisualStyleBackColor = true;
            this.buttonGrayscale.Click += new System.EventHandler(this.buttonGrayscale_Click);
            // 
            // Kirsch
            // 
            this.Kirsch.Location = new System.Drawing.Point(19, 90);
            this.Kirsch.Name = "Kirsch";
            this.Kirsch.Size = new System.Drawing.Size(75, 23);
            this.Kirsch.TabIndex = 27;
            this.Kirsch.Text = "kirschBtn";
            this.Kirsch.UseVisualStyleBackColor = true;
            this.Kirsch.Click += new System.EventHandler(this.Kirsch_Click);
            // 
            // laplaceBtn
            // 
            this.laplaceBtn.Location = new System.Drawing.Point(73, 36);
            this.laplaceBtn.Name = "laplaceBtn";
            this.laplaceBtn.Size = new System.Drawing.Size(75, 23);
            this.laplaceBtn.TabIndex = 28;
            this.laplaceBtn.Text = "Laplace";
            this.laplaceBtn.UseVisualStyleBackColor = true;
            this.laplaceBtn.Click += new System.EventHandler(this.laplaceBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 582);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.panelDestination);
            this.Controls.Add(this.panelSource);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rotateBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lumenBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSource;
        private System.Windows.Forms.Panel panelDestination;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonGrayscale;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TrackBar lumenBar;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button histogramBtn;
        private System.Windows.Forms.TrackBar rotateBar;
        private System.Windows.Forms.TextBox ftjTxtBox;
        private System.Windows.Forms.Button ftjBtn;
        private System.Windows.Forms.Button medianBtn;
        private System.Windows.Forms.Button markovBtn;
        private System.Windows.Forms.Button median2Btn;
        private System.Windows.Forms.Button ftsBtn;
        private System.Windows.Forms.Button unsharpBtn;
        private System.Windows.Forms.Button Kirsch;
        private System.Windows.Forms.Button laplaceBtn;
    }
}

