namespace zotin
{
    partial class lab4
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonRegionsAdaptiveThreshold = new System.Windows.Forms.Button();
            this.buttonRegionsGrowing = new System.Windows.Forms.Button();
            this.trackBarSegmentationRegionsThreshold = new System.Windows.Forms.TrackBar();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.comboBoxSegmentationRegionsColorModel = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSegmentationRegionsThreshold)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(415, 528);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(424, 529);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "Исходное изображение";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonRegionsAdaptiveThreshold
            // 
            this.buttonRegionsAdaptiveThreshold.Location = new System.Drawing.Point(422, 112);
            this.buttonRegionsAdaptiveThreshold.Name = "buttonRegionsAdaptiveThreshold";
            this.buttonRegionsAdaptiveThreshold.Size = new System.Drawing.Size(251, 23);
            this.buttonRegionsAdaptiveThreshold.TabIndex = 31;
            this.buttonRegionsAdaptiveThreshold.Text = "Алгоритм адаптивной пороговой сегментации";
            this.buttonRegionsAdaptiveThreshold.UseVisualStyleBackColor = true;
            this.buttonRegionsAdaptiveThreshold.Click += new System.EventHandler(this.buttonRegionsAdaptiveThreshold_Click);
            // 
            // buttonRegionsGrowing
            // 
            this.buttonRegionsGrowing.Location = new System.Drawing.Point(422, 83);
            this.buttonRegionsGrowing.Name = "buttonRegionsGrowing";
            this.buttonRegionsGrowing.Size = new System.Drawing.Size(251, 23);
            this.buttonRegionsGrowing.TabIndex = 30;
            this.buttonRegionsGrowing.Text = "Алгоритм разрастания регионов";
            this.buttonRegionsGrowing.UseVisualStyleBackColor = true;
            this.buttonRegionsGrowing.Click += new System.EventHandler(this.buttonRegionsGrowing_Click);
            // 
            // trackBarSegmentationRegionsThreshold
            // 
            this.trackBarSegmentationRegionsThreshold.AutoSize = false;
            this.trackBarSegmentationRegionsThreshold.Location = new System.Drawing.Point(516, 39);
            this.trackBarSegmentationRegionsThreshold.Maximum = 255;
            this.trackBarSegmentationRegionsThreshold.Name = "trackBarSegmentationRegionsThreshold";
            this.trackBarSegmentationRegionsThreshold.Size = new System.Drawing.Size(157, 22);
            this.trackBarSegmentationRegionsThreshold.TabIndex = 29;
            this.trackBarSegmentationRegionsThreshold.Value = 25;
            this.trackBarSegmentationRegionsThreshold.Scroll += new System.EventHandler(this.trackBarSegmentationRegionsThreshold_Scroll);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(424, 39);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(38, 13);
            this.label35.TabIndex = 28;
            this.label35.Text = "Порог";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(419, 16);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(97, 13);
            this.label34.TabIndex = 27;
            this.label34.Text = "Цветовая модель";
            // 
            // comboBoxSegmentationRegionsColorModel
            // 
            this.comboBoxSegmentationRegionsColorModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSegmentationRegionsColorModel.FormattingEnabled = true;
            this.comboBoxSegmentationRegionsColorModel.Items.AddRange(new object[] {
            "RGB",
            "HSV",
            "YUV"});
            this.comboBoxSegmentationRegionsColorModel.Location = new System.Drawing.Point(516, 12);
            this.comboBoxSegmentationRegionsColorModel.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.comboBoxSegmentationRegionsColorModel.Name = "comboBoxSegmentationRegionsColorModel";
            this.comboBoxSegmentationRegionsColorModel.Size = new System.Drawing.Size(157, 21);
            this.comboBoxSegmentationRegionsColorModel.TabIndex = 26;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(688, 24);
            this.menuStrip1.TabIndex = 32;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // вернутьИзображениеНаГлавнуюToolStripMenuItem
            // 
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Name = "вернутьИзображениеНаГлавнуюToolStripMenuItem";
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Size = new System.Drawing.Size(206, 20);
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Text = "Вернуть изображение на главную";
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Click += new System.EventHandler(this.вернутьИзображениеНаГлавнуюToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(422, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(251, 23);
            this.button2.TabIndex = 33;
            this.button2.Text = "Сравнить методы :D";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lab4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 571);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonRegionsAdaptiveThreshold);
            this.Controls.Add(this.buttonRegionsGrowing);
            this.Controls.Add(this.trackBarSegmentationRegionsThreshold);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.comboBoxSegmentationRegionsColorModel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "lab4";
            this.Text = "lab4";
            this.Shown += new System.EventHandler(this.lab4_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSegmentationRegionsThreshold)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonRegionsAdaptiveThreshold;
        private System.Windows.Forms.Button buttonRegionsGrowing;
        private System.Windows.Forms.TrackBar trackBarSegmentationRegionsThreshold;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ComboBox comboBoxSegmentationRegionsColorModel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem вернутьИзображениеНаГлавнуюToolStripMenuItem;
        private System.Windows.Forms.Button button2;
    }
}