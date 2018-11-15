using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zotin
{
    public partial class SegmentsComparerChartForm : Form
    {
        public SegmentsComparerChartForm()
        {
            InitializeComponent();

            SegmentChart.SetCheckBoxes(new[]
            {
                checkBox1, checkBox2, checkBox3, checkBox4, checkBox5    
            });
        }

        public void AddData(string title, SegmentInfo segmentInfo)
        {
            if (segmentInfo == null) return;
            Invoke((MethodInvoker)delegate
            {
                SegmentChart.Series[0].Points.AddXY(title, segmentInfo.Area);
                SegmentChart.Series[1].Points.AddXY(title, segmentInfo.Perimeter);
                SegmentChart.Series[2].Points.AddXY(title, segmentInfo.CenterOfMass.X);
                SegmentChart.Series[3].Points.AddXY(title, segmentInfo.CenterOfMass.Y);
                SegmentChart.Series[4].Points.AddXY(title, segmentInfo.Density);
                textBoxLog.Text += String.Format("{0}. {1}\r\n\r\n", title, segmentInfo);
            });
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            foreach (var series in SegmentChart.Series)
            {
                series.Points.Clear();
            }
            textBoxLog.Text = "";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = true;
            Hide();
        }
    }
}
