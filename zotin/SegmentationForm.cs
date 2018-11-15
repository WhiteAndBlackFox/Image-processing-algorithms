using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zotin
{
    public partial class SegmentationForm : Form
    {
        private Thread _backgroundThread;
        private readonly PictureBox _pictureBox;
        private readonly SegmentsComparer _segmentsComparer1;
        private readonly SegmentsComparer _segmentsComparer2;
        private Image _image1;
        private Image _image2;
        private Bitmap img;
        private IDictionary<int, List<Point>> _regions1;
        private IDictionary<int, List<Point>> _regions2;
        private readonly SegmentsComparerChartForm _segmentsComparerChartForm;

        public SegmentationForm(PictureBox pb)
        {
            InitializeComponent();
            _pictureBox = pb;
            _segmentsComparer1 = new SegmentsComparer();
            _segmentsComparer2 = new SegmentsComparer();
            _segmentsComparerChartForm = new SegmentsComparerChartForm();
            _segmentsComparerChartForm.Opacity = 0;
            _segmentsComparerChartForm.Show();
            _segmentsComparerChartForm.Hide();
            _segmentsComparerChartForm.Opacity = 1;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var position = pictureBox1.GetMousePosition(e.Location);
            if (position != null)
            {
                button3.Visible = true;
                SelectRegion(position.Value);
            }
        }

        private void SelectRegion(Point position)
        {
            var segmentInfo1 = SelectRegion(_regions1, pictureBox1, _segmentsComparer1, position);
            var segmentInfo2 = SelectRegion(_regions2, pictureBox2, _segmentsComparer2, position);
            
            listBox1.Items.Clear();
            listBox1.Items.Add("Изображение 1:");
            listBox1.Items.Add("Площадь сегмента: " + segmentInfo1.Area.ToString());
            listBox1.Items.Add("Периметр сегмента: "+segmentInfo1.Perimeter.ToString());
            listBox1.Items.Add("Центр массы по x:"+ segmentInfo1.CenterOfMass.X.ToString());
            listBox1.Items.Add("Центр массы по y:" + segmentInfo1.CenterOfMass.Y );
            listBox1.Items.Add("Плотность: " + segmentInfo1.Density.ToString());
            listBox1.Items.Add("Код: " + segmentInfo1.ChainCode8.ToString());

            listBox2.Items.Clear();
            listBox2.Items.Add("Изображение 2:");
            listBox2.Items.Add("Площадь сегмента: " + segmentInfo2.Area.ToString());
            listBox2.Items.Add("Периметр сегмента: " + segmentInfo2.Perimeter.ToString());
            listBox2.Items.Add("Центр массы по x:" + segmentInfo2.CenterOfMass.X.ToString());
            listBox2.Items.Add("Центр массы по y:" + segmentInfo2.CenterOfMass.Y);
            listBox2.Items.Add("Плотность: " + segmentInfo2.Density.ToString());
            listBox2.Items.Add("Код: " + segmentInfo2.ChainCode8.ToString());

            _segmentsComparerChartForm.AddData(string.Format("Изображение 1 ({0})", position), segmentInfo1);
            _segmentsComparerChartForm.AddData(string.Format("Изображение 2 ({0})", position), segmentInfo2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_regions1 == null || _regions2 == null) return;
            if (_backgroundThread != null && _backgroundThread.IsAlive) return;
            button2.Enabled = false;
            ResulListBox.Items.Clear();
            ResulListBox.Items.Add(string.Format("Изображение 1: количество регионов: {0}", _regions1.Count));
            ResulListBox.Items.Add(string.Format("Изображение 2: количество регионов: {0}", _regions2.Count));
            _backgroundThread = new Thread(Compare);
            _backgroundThread.Start();
        }

        private void Compare()
        {
            Action<string> newListItem = item =>
            {
                ResulListBox.Invoke((MethodInvoker)delegate
                {
                    ResulListBox.Items.Add(item);
                });
            };

            var firstBigger = _regions1.Count > _regions2.Count;
            var region1 = firstBigger ? _regions2 : _regions1;
            var region2 = firstBigger ? _regions1 : _regions2;
            int match = 0;
            int simular = 0;
            foreach (var item1 in region1)
            {
                foreach (var item2 in region2)
                {
                    var pointList1 = item1.Value;
                    var pointList2 = item2.Value;
                    int count = pointList1.Intersect(pointList2).Count();

                    var d1 = (float)pointList1.Count / pointList2.Count;
                    var d2 = (float)count / pointList1.Count;
                    if (d1 >= 0.97 && d1 <= 1.03 && d2 >= 0.97)
                    {
                        match++;
                        newListItem(string.Format("Найдена область совпадения: {0}", d2));
                    }
                    else if (d1 >= 0.70 && d1 <= 1.3 && d2 >= 0.70)
                    {
                        simular++;
                        newListItem(string.Format("Найдена область совпадения: {0}", d2));
                    }
                }
            }
            newListItem(string.Format("Алгоритм разрастания регионов: {0}", match));
            newListItem(string.Format("Алгоритм адаптивной пороговой сегментации: {0}", simular));
            button2.Invoke((MethodInvoker)delegate
            {
                button2.Enabled = true;
            });
            MessageBox.Show(string.Format("Номер региона совпадения: {0}\nКоличество похожих регионов: {1}", match, simular));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = _pictureBox.Image;
            _segmentsComparer1.Open(pictureBox1.Image, true);
            _image1 = _segmentsComparer1.SegmentationSelectSegments();
            pictureBox1.Image = _image1;
            _regions1 = _segmentsComparer1.Regions;
            ResulListBox.Items.Add(string.Format("Изображение 1: количество регионов: {0}", _regions1.Count));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_image1 != null) pictureBox1.Image = _image1;
            if (_image2 != null) pictureBox2.Image = _image2;
        }

        private SegmentInfo SelectRegion(IDictionary<int, List<Point>> regions, PictureBox pictureBox, SegmentsComparer segmentsComparer, Point location)
        {
            if (pictureBox.Image == null) return null;
            pictureBox.Image = segmentsComparer.AlgorithmGrayScale(GrayScale.FromRgb);
            SegmentInfo segmentInfo = null;
            List<Point> pointList = regions.Values.FirstOrDefault(points => points.Contains(location));
            if (pointList != null)
            {
                var points = pointList.ToArray();
                segmentInfo = new SegmentInfo(points);
                using (var g = Graphics.FromImage(pictureBox.Image))
                {
                    Pen pen = new Pen(Color.Fuchsia);
                    if (points.Length < 3)
                    {
                        var point = points.First();
                        g.DrawEllipse(pen, point.X - 3, point.Y - 3, 6, 6);
                    }
                    else
                    {
                        g.DrawPolygon(pen, points);
                    }
                }
            }
            return segmentInfo;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = _pictureBox.Image;
            _segmentsComparer2.Open(pictureBox2.Image, true);
            _image2 = _segmentsComparer2.SegmentationSelectSegments();
            pictureBox2.Image = _image2;
            _regions2 = _segmentsComparer2.Regions;
            ResulListBox.Items.Add(string.Format("Изображение 2: количество регионов: {0}", _regions2.Count));
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            var position = pictureBox2.GetMousePosition(e.Location);
            if (position != null)
            {
                button3.Visible = true;
                SelectRegion(position.Value);
            }
        }

        private void SegmentationForm_Shown(object sender, EventArgs e)
        {
            /*
            pictureBox1.Image = _pictureBox.Image;
            _segmentsComparer1.Open(pictureBox1.Image, true);
            _image1 = _segmentsComparer1.SegmentationSelectSegments();
            pictureBox1.Image = _image1;
            _regions1 = _segmentsComparer1.Regions;
            ResulListBox.Items.Add(string.Format("Изображение 1: количество регионов: {0}", _regions1.Count));
            
            pictureBox2.Image = _pictureBox.Image;
            _segmentsComparer2.Open(pictureBox2.Image, true);
            _image2 = _segmentsComparer2.SegmentationSelectSegments();
            pictureBox2.Image = _image2;
            _regions2 = _segmentsComparer2.Regions;
            ResulListBox.Items.Add(string.Format("Изображение 2: количество регионов: {0}", _regions2.Count));
             * */
        }
    }
}
