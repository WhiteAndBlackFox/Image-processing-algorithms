namespace zotin
{
    partial class lab3
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxSegmentationMorphologizationStructureElement = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.radioButtonMorphologizationErosion = new System.Windows.Forms.RadioButton();
            this.radioButtonSegmentationMorphologizationDelation = new System.Windows.Forms.RadioButton();
            this.label22 = new System.Windows.Forms.Label();
            this.trackBarSegmentationMorphologizationRadius = new System.Windows.Forms.TrackBar();
            this.buttonMorphologization = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownSigma = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMask = new System.Windows.Forms.NumericUpDown();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownAmp = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRobertsThreshold = new System.Windows.Forms.NumericUpDown();
            this.button9 = new System.Windows.Forms.Button();
            this.radioButtonGrayscale = new System.Windows.Forms.RadioButton();
            this.radioButtonB = new System.Windows.Forms.RadioButton();
            this.radioButtonG = new System.Windows.Forms.RadioButton();
            this.radioButtonR = new System.Windows.Forms.RadioButton();
            this.radioButtonRGB = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSegmentationMorphologizationRadius)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSigma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAmp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRobertsThreshold)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(349, 457);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownY);
            this.groupBox1.Controls.Add(this.numericUpDownX);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(368, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(392, 163);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Бинаризация";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(244, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 50);
            this.button2.TabIndex = 11;
            this.button2.Text = "Локальная бинаризация Ниблэк";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(125, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 50);
            this.button3.TabIndex = 10;
            this.button3.Text = "Локальная бинаризация Бернсен";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(200, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "X";
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.Location = new System.Drawing.Point(220, 128);
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(43, 20);
            this.numericUpDownY.TabIndex = 7;
            this.numericUpDownY.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.Location = new System.Drawing.Point(220, 101);
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(43, 20);
            this.numericUpDownX.TabIndex = 6;
            this.numericUpDownX.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(6, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(133, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Преобразовать в ч/б";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(26, 105);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(43, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "Глобальная бинаризация Отсу";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(766, 17);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(181, 467);
            this.button4.TabIndex = 2;
            this.button4.Text = "Исходное изображение";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxSegmentationMorphologizationStructureElement);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.radioButtonMorphologizationErosion);
            this.groupBox2.Controls.Add(this.radioButtonSegmentationMorphologizationDelation);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.trackBarSegmentationMorphologizationRadius);
            this.groupBox2.Controls.Add(this.buttonMorphologization);
            this.groupBox2.Location = new System.Drawing.Point(368, 183);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 144);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Морфологическая обработка";
            // 
            // comboBoxSegmentationMorphologizationStructureElement
            // 
            this.comboBoxSegmentationMorphologizationStructureElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSegmentationMorphologizationStructureElement.FormattingEnabled = true;
            this.comboBoxSegmentationMorphologizationStructureElement.Items.AddRange(new object[] {
            "Квадрат",
            "Круг"});
            this.comboBoxSegmentationMorphologizationStructureElement.Location = new System.Drawing.Point(10, 117);
            this.comboBoxSegmentationMorphologizationStructureElement.Name = "comboBoxSegmentationMorphologizationStructureElement";
            this.comboBoxSegmentationMorphologizationStructureElement.Size = new System.Drawing.Size(232, 21);
            this.comboBoxSegmentationMorphologizationStructureElement.TabIndex = 51;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(7, 100);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(116, 13);
            this.label23.TabIndex = 50;
            this.label23.Text = "Структура элементов";
            // 
            // radioButtonMorphologizationErosion
            // 
            this.radioButtonMorphologizationErosion.AutoSize = true;
            this.radioButtonMorphologizationErosion.Location = new System.Drawing.Point(101, 78);
            this.radioButtonMorphologizationErosion.Name = "radioButtonMorphologizationErosion";
            this.radioButtonMorphologizationErosion.Size = new System.Drawing.Size(68, 17);
            this.radioButtonMorphologizationErosion.TabIndex = 49;
            this.radioButtonMorphologizationErosion.Text = "Эррозия";
            this.radioButtonMorphologizationErosion.UseVisualStyleBackColor = true;
            // 
            // radioButtonSegmentationMorphologizationDelation
            // 
            this.radioButtonSegmentationMorphologizationDelation.AutoSize = true;
            this.radioButtonSegmentationMorphologizationDelation.Checked = true;
            this.radioButtonSegmentationMorphologizationDelation.Location = new System.Drawing.Point(10, 78);
            this.radioButtonSegmentationMorphologizationDelation.Name = "radioButtonSegmentationMorphologizationDelation";
            this.radioButtonSegmentationMorphologizationDelation.Size = new System.Drawing.Size(70, 17);
            this.radioButtonSegmentationMorphologizationDelation.TabIndex = 48;
            this.radioButtonSegmentationMorphologizationDelation.TabStop = true;
            this.radioButtonSegmentationMorphologizationDelation.Text = "Диляция";
            this.radioButtonSegmentationMorphologizationDelation.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(121, 19);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(43, 13);
            this.label22.TabIndex = 47;
            this.label22.Text = "Радиус";
            // 
            // trackBarSegmentationMorphologizationRadius
            // 
            this.trackBarSegmentationMorphologizationRadius.AutoSize = false;
            this.trackBarSegmentationMorphologizationRadius.Location = new System.Drawing.Point(125, 35);
            this.trackBarSegmentationMorphologizationRadius.Maximum = 20;
            this.trackBarSegmentationMorphologizationRadius.Minimum = 1;
            this.trackBarSegmentationMorphologizationRadius.Name = "trackBarSegmentationMorphologizationRadius";
            this.trackBarSegmentationMorphologizationRadius.Size = new System.Drawing.Size(178, 25);
            this.trackBarSegmentationMorphologizationRadius.TabIndex = 46;
            this.trackBarSegmentationMorphologizationRadius.Value = 1;
            // 
            // buttonMorphologization
            // 
            this.buttonMorphologization.Location = new System.Drawing.Point(6, 19);
            this.buttonMorphologization.Name = "buttonMorphologization";
            this.buttonMorphologization.Size = new System.Drawing.Size(109, 47);
            this.buttonMorphologization.TabIndex = 45;
            this.buttonMorphologization.Text = "Морфологическая обработка";
            this.buttonMorphologization.UseVisualStyleBackColor = true;
            this.buttonMorphologization.Click += new System.EventHandler(this.SegmentationMorphologization);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.numericUpDownSigma);
            this.panel3.Controls.Add(this.numericUpDownMask);
            this.panel3.Controls.Add(this.button11);
            this.panel3.Controls.Add(this.button10);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.numericUpDownAmp);
            this.panel3.Controls.Add(this.numericUpDownRobertsThreshold);
            this.panel3.Controls.Add(this.button9);
            this.panel3.Controls.Add(this.radioButtonGrayscale);
            this.panel3.Controls.Add(this.radioButtonB);
            this.panel3.Controls.Add(this.radioButtonG);
            this.panel3.Controls.Add(this.radioButtonR);
            this.panel3.Controls.Add(this.radioButtonRGB);
            this.panel3.Location = new System.Drawing.Point(374, 333);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(386, 151);
            this.panel3.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(177, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Сигма";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(177, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Маска";
            // 
            // numericUpDownSigma
            // 
            this.numericUpDownSigma.DecimalPlaces = 2;
            this.numericUpDownSigma.Location = new System.Drawing.Point(232, 67);
            this.numericUpDownSigma.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSigma.Name = "numericUpDownSigma";
            this.numericUpDownSigma.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownSigma.TabIndex = 28;
            this.numericUpDownSigma.Value = new decimal(new int[] {
            22,
            0,
            0,
            65536});
            // 
            // numericUpDownMask
            // 
            this.numericUpDownMask.Location = new System.Drawing.Point(232, 41);
            this.numericUpDownMask.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownMask.Name = "numericUpDownMask";
            this.numericUpDownMask.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownMask.TabIndex = 27;
            this.numericUpDownMask.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(161, 9);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(143, 23);
            this.button11.TabIndex = 26;
            this.button11.Text = "Лаплас";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(12, 92);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(143, 23);
            this.button10.TabIndex = 25;
            this.button10.Text = "Собель";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "К";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Граница";
            // 
            // numericUpDownAmp
            // 
            this.numericUpDownAmp.DecimalPlaces = 2;
            this.numericUpDownAmp.Location = new System.Drawing.Point(83, 64);
            this.numericUpDownAmp.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownAmp.Name = "numericUpDownAmp";
            this.numericUpDownAmp.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownAmp.TabIndex = 22;
            this.numericUpDownAmp.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // numericUpDownRobertsThreshold
            // 
            this.numericUpDownRobertsThreshold.Location = new System.Drawing.Point(83, 38);
            this.numericUpDownRobertsThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownRobertsThreshold.Name = "numericUpDownRobertsThreshold";
            this.numericUpDownRobertsThreshold.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownRobertsThreshold.TabIndex = 21;
            this.numericUpDownRobertsThreshold.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(12, 9);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(143, 23);
            this.button9.TabIndex = 20;
            this.button9.Text = "Робертс";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // radioButtonGrayscale
            // 
            this.radioButtonGrayscale.AutoSize = true;
            this.radioButtonGrayscale.Location = new System.Drawing.Point(310, 79);
            this.radioButtonGrayscale.Name = "radioButtonGrayscale";
            this.radioButtonGrayscale.Size = new System.Drawing.Size(72, 17);
            this.radioButtonGrayscale.TabIndex = 4;
            this.radioButtonGrayscale.Text = "Grayscale";
            this.radioButtonGrayscale.UseVisualStyleBackColor = true;
            this.radioButtonGrayscale.CheckedChanged += new System.EventHandler(this.radioButtonGrayscale_CheckedChanged);
            // 
            // radioButtonB
            // 
            this.radioButtonB.AutoSize = true;
            this.radioButtonB.Location = new System.Drawing.Point(310, 60);
            this.radioButtonB.Name = "radioButtonB";
            this.radioButtonB.Size = new System.Drawing.Size(32, 17);
            this.radioButtonB.TabIndex = 3;
            this.radioButtonB.Text = "B";
            this.radioButtonB.UseVisualStyleBackColor = true;
            this.radioButtonB.CheckedChanged += new System.EventHandler(this.radioButtonB_CheckedChanged);
            // 
            // radioButtonG
            // 
            this.radioButtonG.AutoSize = true;
            this.radioButtonG.Location = new System.Drawing.Point(310, 41);
            this.radioButtonG.Name = "radioButtonG";
            this.radioButtonG.Size = new System.Drawing.Size(33, 17);
            this.radioButtonG.TabIndex = 2;
            this.radioButtonG.Text = "G";
            this.radioButtonG.UseVisualStyleBackColor = true;
            this.radioButtonG.CheckedChanged += new System.EventHandler(this.radioButtonG_CheckedChanged);
            // 
            // radioButtonR
            // 
            this.radioButtonR.AutoSize = true;
            this.radioButtonR.Location = new System.Drawing.Point(310, 22);
            this.radioButtonR.Name = "radioButtonR";
            this.radioButtonR.Size = new System.Drawing.Size(33, 17);
            this.radioButtonR.TabIndex = 1;
            this.radioButtonR.Text = "R";
            this.radioButtonR.UseVisualStyleBackColor = true;
            this.radioButtonR.CheckedChanged += new System.EventHandler(this.radioButtonR_CheckedChanged);
            // 
            // radioButtonRGB
            // 
            this.radioButtonRGB.AutoSize = true;
            this.radioButtonRGB.Checked = true;
            this.radioButtonRGB.Location = new System.Drawing.Point(310, 3);
            this.radioButtonRGB.Name = "radioButtonRGB";
            this.radioButtonRGB.Size = new System.Drawing.Size(48, 17);
            this.radioButtonRGB.TabIndex = 0;
            this.radioButtonRGB.TabStop = true;
            this.radioButtonRGB.Text = "RGB";
            this.radioButtonRGB.UseVisualStyleBackColor = true;
            this.radioButtonRGB.CheckedChanged += new System.EventHandler(this.radioButtonRGB_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(959, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // вернутьИзображениеНаГлавнуюToolStripMenuItem
            // 
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Name = "вернутьИзображениеНаГлавнуюToolStripMenuItem";
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Size = new System.Drawing.Size(206, 20);
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Text = "Вернуть изображение на главную";
            this.вернутьИзображениеНаГлавнуюToolStripMenuItem.Click += new System.EventHandler(this.вернутьИзображениеНаГлавнуюToolStripMenuItem_Click);
            // 
            // lab3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 500);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "lab3";
            this.Text = "lab3";
            this.Shown += new System.EventHandler(this.lab3_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSegmentationMorphologizationRadius)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSigma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAmp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRobertsThreshold)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxSegmentationMorphologizationStructureElement;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.RadioButton radioButtonMorphologizationErosion;
        private System.Windows.Forms.RadioButton radioButtonSegmentationMorphologizationDelation;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TrackBar trackBarSegmentationMorphologizationRadius;
        private System.Windows.Forms.Button buttonMorphologization;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownSigma;
        private System.Windows.Forms.NumericUpDown numericUpDownMask;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownAmp;
        private System.Windows.Forms.NumericUpDown numericUpDownRobertsThreshold;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.RadioButton radioButtonGrayscale;
        private System.Windows.Forms.RadioButton radioButtonB;
        private System.Windows.Forms.RadioButton radioButtonG;
        private System.Windows.Forms.RadioButton radioButtonR;
        private System.Windows.Forms.RadioButton radioButtonRGB;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem вернутьИзображениеНаГлавнуюToolStripMenuItem;
    }
}