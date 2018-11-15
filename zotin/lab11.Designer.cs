namespace zotin
{
    partial class lab11
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
            this.ColorModel = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxRgbB = new System.Windows.Forms.CheckBox();
            this.checkBoxRgbG = new System.Windows.Forms.CheckBox();
            this.checkBoxRgbR = new System.Windows.Forms.CheckBox();
            this.trackBarRgbR = new System.Windows.Forms.TrackBar();
            this.trackBarRgbB = new System.Windows.Forms.TrackBar();
            this.trackBarRgbG = new System.Windows.Forms.TrackBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxYuvV = new System.Windows.Forms.CheckBox();
            this.checkBoxYuvU = new System.Windows.Forms.CheckBox();
            this.checkBoxYuvY = new System.Windows.Forms.CheckBox();
            this.trackBarYuvY = new System.Windows.Forms.TrackBar();
            this.trackBarYuvV = new System.Windows.Forms.TrackBar();
            this.trackBarYuvU = new System.Windows.Forms.TrackBar();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBoxHsvV = new System.Windows.Forms.CheckBox();
            this.checkBoxHsvS = new System.Windows.Forms.CheckBox();
            this.checkBoxHsvH = new System.Windows.Forms.CheckBox();
            this.trackBarHsvH = new System.Windows.Forms.TrackBar();
            this.trackBarHsvV = new System.Windows.Forms.TrackBar();
            this.trackBarHsvS = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ColorModel.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRgbR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRgbB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRgbG)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYuvY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYuvV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYuvU)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHsvH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHsvV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHsvS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ColorModel
            // 
            this.ColorModel.Controls.Add(this.tabPage1);
            this.ColorModel.Controls.Add(this.tabPage2);
            this.ColorModel.Controls.Add(this.tabPage3);
            this.ColorModel.Location = new System.Drawing.Point(12, 27);
            this.ColorModel.Name = "ColorModel";
            this.ColorModel.SelectedIndex = 0;
            this.ColorModel.Size = new System.Drawing.Size(225, 186);
            this.ColorModel.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxRgbB);
            this.tabPage1.Controls.Add(this.checkBoxRgbG);
            this.tabPage1.Controls.Add(this.checkBoxRgbR);
            this.tabPage1.Controls.Add(this.trackBarRgbR);
            this.tabPage1.Controls.Add(this.trackBarRgbB);
            this.tabPage1.Controls.Add(this.trackBarRgbG);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(217, 160);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "RGB";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxRgbB
            // 
            this.checkBoxRgbB.AutoSize = true;
            this.checkBoxRgbB.Checked = true;
            this.checkBoxRgbB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRgbB.Location = new System.Drawing.Point(3, 113);
            this.checkBoxRgbB.Name = "checkBoxRgbB";
            this.checkBoxRgbB.Size = new System.Drawing.Size(33, 17);
            this.checkBoxRgbB.TabIndex = 5;
            this.checkBoxRgbB.Text = "B";
            this.checkBoxRgbB.UseVisualStyleBackColor = true;
            this.checkBoxRgbB.CheckedChanged += new System.EventHandler(this.VisualizationRgbModel);
            // 
            // checkBoxRgbG
            // 
            this.checkBoxRgbG.AutoSize = true;
            this.checkBoxRgbG.Checked = true;
            this.checkBoxRgbG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRgbG.Location = new System.Drawing.Point(3, 66);
            this.checkBoxRgbG.Name = "checkBoxRgbG";
            this.checkBoxRgbG.Size = new System.Drawing.Size(34, 17);
            this.checkBoxRgbG.TabIndex = 4;
            this.checkBoxRgbG.Text = "G";
            this.checkBoxRgbG.UseVisualStyleBackColor = true;
            this.checkBoxRgbG.CheckedChanged += new System.EventHandler(this.VisualizationRgbModel);
            // 
            // checkBoxRgbR
            // 
            this.checkBoxRgbR.AutoSize = true;
            this.checkBoxRgbR.Checked = true;
            this.checkBoxRgbR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRgbR.Location = new System.Drawing.Point(3, 19);
            this.checkBoxRgbR.Name = "checkBoxRgbR";
            this.checkBoxRgbR.Size = new System.Drawing.Size(34, 17);
            this.checkBoxRgbR.TabIndex = 3;
            this.checkBoxRgbR.Text = "R";
            this.checkBoxRgbR.UseVisualStyleBackColor = true;
            this.checkBoxRgbR.CheckedChanged += new System.EventHandler(this.VisualizationRgbModel);
            // 
            // trackBarRgbR
            // 
            this.trackBarRgbR.Location = new System.Drawing.Point(41, 6);
            this.trackBarRgbR.Maximum = 255;
            this.trackBarRgbR.Name = "trackBarRgbR";
            this.trackBarRgbR.Size = new System.Drawing.Size(170, 45);
            this.trackBarRgbR.TabIndex = 2;
            this.trackBarRgbR.Value = 255;
            this.trackBarRgbR.Scroll += new System.EventHandler(this.VisualizationRgbModel);
            // 
            // trackBarRgbB
            // 
            this.trackBarRgbB.Location = new System.Drawing.Point(41, 107);
            this.trackBarRgbB.Maximum = 255;
            this.trackBarRgbB.Name = "trackBarRgbB";
            this.trackBarRgbB.Size = new System.Drawing.Size(170, 45);
            this.trackBarRgbB.TabIndex = 1;
            this.trackBarRgbB.Value = 255;
            this.trackBarRgbB.Scroll += new System.EventHandler(this.VisualizationRgbModel);
            // 
            // trackBarRgbG
            // 
            this.trackBarRgbG.Location = new System.Drawing.Point(41, 58);
            this.trackBarRgbG.Maximum = 255;
            this.trackBarRgbG.Name = "trackBarRgbG";
            this.trackBarRgbG.Size = new System.Drawing.Size(170, 45);
            this.trackBarRgbG.TabIndex = 0;
            this.trackBarRgbG.Value = 255;
            this.trackBarRgbG.Scroll += new System.EventHandler(this.VisualizationRgbModel);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxYuvV);
            this.tabPage2.Controls.Add(this.checkBoxYuvU);
            this.tabPage2.Controls.Add(this.checkBoxYuvY);
            this.tabPage2.Controls.Add(this.trackBarYuvY);
            this.tabPage2.Controls.Add(this.trackBarYuvV);
            this.tabPage2.Controls.Add(this.trackBarYuvU);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(217, 160);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "YUV";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxYuvV
            // 
            this.checkBoxYuvV.AutoSize = true;
            this.checkBoxYuvV.Checked = true;
            this.checkBoxYuvV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxYuvV.Location = new System.Drawing.Point(3, 107);
            this.checkBoxYuvV.Name = "checkBoxYuvV";
            this.checkBoxYuvV.Size = new System.Drawing.Size(33, 17);
            this.checkBoxYuvV.TabIndex = 11;
            this.checkBoxYuvV.Text = "V";
            this.checkBoxYuvV.UseVisualStyleBackColor = true;
            // 
            // checkBoxYuvU
            // 
            this.checkBoxYuvU.AutoSize = true;
            this.checkBoxYuvU.Checked = true;
            this.checkBoxYuvU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxYuvU.Location = new System.Drawing.Point(3, 66);
            this.checkBoxYuvU.Name = "checkBoxYuvU";
            this.checkBoxYuvU.Size = new System.Drawing.Size(34, 17);
            this.checkBoxYuvU.TabIndex = 10;
            this.checkBoxYuvU.Text = "U";
            this.checkBoxYuvU.UseVisualStyleBackColor = true;
            // 
            // checkBoxYuvY
            // 
            this.checkBoxYuvY.AutoSize = true;
            this.checkBoxYuvY.Checked = true;
            this.checkBoxYuvY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxYuvY.Location = new System.Drawing.Point(3, 19);
            this.checkBoxYuvY.Name = "checkBoxYuvY";
            this.checkBoxYuvY.Size = new System.Drawing.Size(33, 17);
            this.checkBoxYuvY.TabIndex = 9;
            this.checkBoxYuvY.Text = "Y";
            this.checkBoxYuvY.UseVisualStyleBackColor = true;
            // 
            // trackBarYuvY
            // 
            this.trackBarYuvY.Location = new System.Drawing.Point(40, 6);
            this.trackBarYuvY.Maximum = 255;
            this.trackBarYuvY.Name = "trackBarYuvY";
            this.trackBarYuvY.Size = new System.Drawing.Size(170, 45);
            this.trackBarYuvY.TabIndex = 8;
            this.trackBarYuvY.Value = 127;
            this.trackBarYuvY.Scroll += new System.EventHandler(this.VisualizationYuvModel);
            // 
            // trackBarYuvV
            // 
            this.trackBarYuvV.Location = new System.Drawing.Point(39, 107);
            this.trackBarYuvV.Maximum = 157;
            this.trackBarYuvV.Minimum = -157;
            this.trackBarYuvV.Name = "trackBarYuvV";
            this.trackBarYuvV.Size = new System.Drawing.Size(170, 45);
            this.trackBarYuvV.TabIndex = 7;
            this.trackBarYuvV.Scroll += new System.EventHandler(this.VisualizationYuvModel);
            // 
            // trackBarYuvU
            // 
            this.trackBarYuvU.Location = new System.Drawing.Point(40, 57);
            this.trackBarYuvU.Maximum = 112;
            this.trackBarYuvU.Minimum = -112;
            this.trackBarYuvU.Name = "trackBarYuvU";
            this.trackBarYuvU.Size = new System.Drawing.Size(170, 45);
            this.trackBarYuvU.TabIndex = 6;
            this.trackBarYuvU.Scroll += new System.EventHandler(this.VisualizationYuvModel);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBoxHsvV);
            this.tabPage3.Controls.Add(this.checkBoxHsvS);
            this.tabPage3.Controls.Add(this.checkBoxHsvH);
            this.tabPage3.Controls.Add(this.trackBarHsvH);
            this.tabPage3.Controls.Add(this.trackBarHsvV);
            this.tabPage3.Controls.Add(this.trackBarHsvS);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(217, 160);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "HSV";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBoxHsvV
            // 
            this.checkBoxHsvV.AutoSize = true;
            this.checkBoxHsvV.Checked = true;
            this.checkBoxHsvV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHsvV.Location = new System.Drawing.Point(6, 106);
            this.checkBoxHsvV.Name = "checkBoxHsvV";
            this.checkBoxHsvV.Size = new System.Drawing.Size(33, 17);
            this.checkBoxHsvV.TabIndex = 14;
            this.checkBoxHsvV.Text = "V";
            this.checkBoxHsvV.UseVisualStyleBackColor = true;
            this.checkBoxHsvV.CheckedChanged += new System.EventHandler(this.VisualizationHsvModel);
            // 
            // checkBoxHsvS
            // 
            this.checkBoxHsvS.AutoSize = true;
            this.checkBoxHsvS.Checked = true;
            this.checkBoxHsvS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHsvS.Location = new System.Drawing.Point(6, 65);
            this.checkBoxHsvS.Name = "checkBoxHsvS";
            this.checkBoxHsvS.Size = new System.Drawing.Size(33, 17);
            this.checkBoxHsvS.TabIndex = 13;
            this.checkBoxHsvS.Text = "S";
            this.checkBoxHsvS.UseVisualStyleBackColor = true;
            this.checkBoxHsvS.CheckedChanged += new System.EventHandler(this.VisualizationHsvModel);
            // 
            // checkBoxHsvH
            // 
            this.checkBoxHsvH.AutoSize = true;
            this.checkBoxHsvH.Checked = true;
            this.checkBoxHsvH.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHsvH.Location = new System.Drawing.Point(6, 18);
            this.checkBoxHsvH.Name = "checkBoxHsvH";
            this.checkBoxHsvH.Size = new System.Drawing.Size(34, 17);
            this.checkBoxHsvH.TabIndex = 12;
            this.checkBoxHsvH.Text = "H";
            this.checkBoxHsvH.UseVisualStyleBackColor = true;
            this.checkBoxHsvH.CheckedChanged += new System.EventHandler(this.VisualizationHsvModel);
            // 
            // trackBarHsvH
            // 
            this.trackBarHsvH.Location = new System.Drawing.Point(41, 6);
            this.trackBarHsvH.Maximum = 360;
            this.trackBarHsvH.Name = "trackBarHsvH";
            this.trackBarHsvH.Size = new System.Drawing.Size(170, 45);
            this.trackBarHsvH.TabIndex = 8;
            this.trackBarHsvH.Value = 180;
            this.trackBarHsvH.Scroll += new System.EventHandler(this.VisualizationHsvModel);
            // 
            // trackBarHsvV
            // 
            this.trackBarHsvV.Location = new System.Drawing.Point(41, 109);
            this.trackBarHsvV.Maximum = 100;
            this.trackBarHsvV.Name = "trackBarHsvV";
            this.trackBarHsvV.Size = new System.Drawing.Size(170, 45);
            this.trackBarHsvV.TabIndex = 7;
            this.trackBarHsvV.Value = 50;
            this.trackBarHsvV.Scroll += new System.EventHandler(this.VisualizationHsvModel);
            // 
            // trackBarHsvS
            // 
            this.trackBarHsvS.Location = new System.Drawing.Point(41, 58);
            this.trackBarHsvS.Maximum = 100;
            this.trackBarHsvS.Name = "trackBarHsvS";
            this.trackBarHsvS.Size = new System.Drawing.Size(170, 45);
            this.trackBarHsvS.TabIndex = 6;
            this.trackBarHsvS.Value = 50;
            this.trackBarHsvS.Scroll += new System.EventHandler(this.VisualizationHsvModel);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(7, 219);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(338, 405);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(239, 121);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 35);
            this.button6.TabIndex = 12;
            this.button6.Text = "Коррекции изображени";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(239, 189);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(111, 24);
            this.button4.TabIndex = 10;
            this.button4.Text = "Доп. задание";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(239, 162);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 27);
            this.button3.TabIndex = 9;
            this.button3.Text = "Шумы";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(239, 85);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 30);
            this.button2.TabIndex = 8;
            this.button2.Text = "Гистограммы";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(239, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 30);
            this.button1.TabIndex = 13;
            this.button1.Text = "GrayScale";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(239, 29);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(50, 20);
            this.button5.TabIndex = 14;
            this.button5.Text = "SP";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(295, 29);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(50, 20);
            this.button7.TabIndex = 15;
            this.button7.Text = "BP";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(357, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // вернутьИзображениеНаГлавнуюToolStripMenuItem
            // 
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Name = "вернутьИзображениеНаГлавнуюToolStripMenuItem";
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Size = new System.Drawing.Size(206, 20);
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Text = "Вернуть изображение на главную";
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Click += new System.EventHandler(this.вернутьИзображениеНаГлавнуюToolStripMenuItem_Click);
            // 
            // lab11
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 632);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ColorModel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "lab11";
            this.Text = "lab1";
            this.Shown += new System.EventHandler(this.lab11_Shown);
            this.ColorModel.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRgbR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRgbB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRgbG)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYuvY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYuvV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarYuvU)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHsvH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHsvV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHsvS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl ColorModel;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TrackBar trackBarRgbR;
        private System.Windows.Forms.TrackBar trackBarRgbB;
        private System.Windows.Forms.TrackBar trackBarRgbG;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar trackBarYuvY;
        private System.Windows.Forms.TrackBar trackBarYuvV;
        private System.Windows.Forms.TrackBar trackBarYuvU;
        private System.Windows.Forms.TrackBar trackBarHsvH;
        private System.Windows.Forms.TrackBar trackBarHsvV;
        private System.Windows.Forms.TrackBar trackBarHsvS;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxRgbB;
        private System.Windows.Forms.CheckBox checkBoxRgbG;
        private System.Windows.Forms.CheckBox checkBoxRgbR;
        private System.Windows.Forms.CheckBox checkBoxYuvV;
        private System.Windows.Forms.CheckBox checkBoxYuvU;
        private System.Windows.Forms.CheckBox checkBoxYuvY;
        private System.Windows.Forms.CheckBox checkBoxHsvV;
        private System.Windows.Forms.CheckBox checkBoxHsvS;
        private System.Windows.Forms.CheckBox checkBoxHsvH;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem вернутьИзображениеНаГлавнуюToolStripMenuItem;
    }
}