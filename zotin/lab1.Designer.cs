namespace zotin
{
    partial class lab1
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.toHSV = new System.Windows.Forms.Button();
            this.toYUV = new System.Windows.Forms.Button();
            this.toHSL = new System.Windows.Forms.Button();
            this.toRGB = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.toHSV);
            this.panel2.Controls.Add(this.toYUV);
            this.panel2.Controls.Add(this.toHSL);
            this.panel2.Controls.Add(this.toRGB);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(167, 83);
            this.panel2.TabIndex = 0;
            // 
            // toHSV
            // 
            this.toHSV.Location = new System.Drawing.Point(85, 21);
            this.toHSV.Name = "toHSV";
            this.toHSV.Size = new System.Drawing.Size(75, 23);
            this.toHSV.TabIndex = 4;
            this.toHSV.Text = "HSV";
            this.toHSV.UseVisualStyleBackColor = true;
            this.toHSV.Click += new System.EventHandler(this.toHSV_Click);
            // 
            // toYUV
            // 
            this.toYUV.Location = new System.Drawing.Point(85, 50);
            this.toYUV.Name = "toYUV";
            this.toYUV.Size = new System.Drawing.Size(75, 23);
            this.toYUV.TabIndex = 3;
            this.toYUV.Text = "YUV";
            this.toYUV.UseVisualStyleBackColor = true;
            this.toYUV.Click += new System.EventHandler(this.toYUV_Click);
            // 
            // toHSL
            // 
            this.toHSL.Location = new System.Drawing.Point(4, 50);
            this.toHSL.Name = "toHSL";
            this.toHSL.Size = new System.Drawing.Size(75, 23);
            this.toHSL.TabIndex = 2;
            this.toHSL.Text = "HSL";
            this.toHSL.UseVisualStyleBackColor = true;
            this.toHSL.Click += new System.EventHandler(this.toHSL_Click);
            // 
            // toRGB
            // 
            this.toRGB.Location = new System.Drawing.Point(4, 21);
            this.toRGB.Name = "toRGB";
            this.toRGB.Size = new System.Drawing.Size(75, 23);
            this.toRGB.TabIndex = 1;
            this.toRGB.Text = "RGB";
            this.toRGB.UseVisualStyleBackColor = true;
            this.toRGB.Click += new System.EventHandler(this.toRGB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Цветовая модель";
            // 
            // lab1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(193, 117);
            this.Controls.Add(this.panel2);
            this.Name = "lab1";
            this.Text = "lab1";
            this.Shown += new System.EventHandler(this.lab1_Shown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button toHSV;
        private System.Windows.Forms.Button toYUV;
        private System.Windows.Forms.Button toHSL;
        private System.Windows.Forms.Button toRGB;
    }
}