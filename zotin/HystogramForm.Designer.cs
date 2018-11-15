namespace zotin
{
    partial class HystogramForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartR = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartG = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartB = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartB)).BeginInit();
            this.SuspendLayout();
            // 
            // chartR
            // 
            chartArea1.Name = "ChartArea1";
            this.chartR.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartR.Legends.Add(legend1);
            this.chartR.Location = new System.Drawing.Point(12, 12);
            this.chartR.Name = "chartR";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartR.Series.Add(series1);
            this.chartR.Size = new System.Drawing.Size(726, 244);
            this.chartR.TabIndex = 0;
            this.chartR.Text = "chart1";
            // 
            // chartG
            // 
            chartArea2.Name = "ChartArea1";
            this.chartG.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartG.Legends.Add(legend2);
            this.chartG.Location = new System.Drawing.Point(12, 262);
            this.chartG.Name = "chartG";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartG.Series.Add(series2);
            this.chartG.Size = new System.Drawing.Size(726, 244);
            this.chartG.TabIndex = 1;
            this.chartG.Text = "chart2";
            // 
            // chartB
            // 
            chartArea3.Name = "ChartArea1";
            this.chartB.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartB.Legends.Add(legend3);
            this.chartB.Location = new System.Drawing.Point(12, 512);
            this.chartB.Name = "chartB";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartB.Series.Add(series3);
            this.chartB.Size = new System.Drawing.Size(726, 244);
            this.chartB.TabIndex = 2;
            this.chartB.Text = "chart3";
            // 
            // HystogramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 765);
            this.Controls.Add(this.chartB);
            this.Controls.Add(this.chartG);
            this.Controls.Add(this.chartR);
            this.Name = "HystogramForm";
            this.Text = "HystogramForm";
            this.Load += new System.EventHandler(this.HystogramForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartR;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartG;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartB;
    }
}