using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace zotin
{
    public partial class HystogramForm : Form
    {

        double[] r, g, b;
        string rName, gName, bName;
        public HystogramForm(double[] R, double[] G, double[] B, string RName = "Красный", string GName = "Зеленый", string BName = "Синий", string name = "")
        {
            InitializeComponent();

            if (name != "")
            {
                this.Text = "Гистограмма " + name;
            }
            r = R;
            g = G;
            b = B;
            rName = RName;
            gName = GName;
            bName = BName;
        }

        private void HystogramForm_Load(object sender, EventArgs e)
        {
            chartR.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            chartR.Series.Clear();
            Series seriesR = this.chartR.Series.Add(rName);
            seriesR.IsVisibleInLegend = false;
            seriesR.ChartType = SeriesChartType.Column;
            for (int i = 1; i < r.Length; i++)
            {
                seriesR.Points.AddXY(i, r[i]);
            }
            seriesR.Color = Color.Red;

            chartG.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            chartG.Series.Clear();
            Series seriesG = this.chartG.Series.Add(gName);
            seriesG.IsVisibleInLegend = false;
            seriesG.ChartType = SeriesChartType.Column;
            for (int i = 1; i < g.Length; i++)
            {
                seriesG.Points.AddXY(i, g[i]);
            }
            seriesG.Color = Color.Green;

            chartB.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            chartB.Series.Clear();
            Series seriesB = this.chartB.Series.Add(bName);
            seriesB.IsVisibleInLegend = false;
            seriesB.ChartType = SeriesChartType.Column;
            for (int i = 1; i < b.Length; i++)
            {
                seriesB.Points.AddXY(i, b[i]);
            }
            seriesB.Color = Color.Blue;
        }
    }
}
