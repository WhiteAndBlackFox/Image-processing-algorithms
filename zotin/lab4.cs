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
    public partial class lab4 : Form
    {
        private readonly ImageProcessor _imageProcessor;
        private Image _resultImage;
        private Bitmap orig;
        private PictureBox resBox;
        public lab4(Bitmap orig, PictureBox pb)
        {
            InitializeComponent();
            this.orig = orig;
            _imageProcessor = new ImageProcessor();
            resBox = pb;
        }

        private void lab4_Shown(object sender, EventArgs e)
        {
            pictureBox1.Image = orig;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = orig;
        }

        private void buttonRegionsGrowing_Click(object sender, EventArgs e)
        {
            _imageProcessor.Open((Bitmap)pictureBox1.Image, true);
            var threshold = trackBarSegmentationRegionsThreshold.Value;
            var result = _imageProcessor.SegmentationRegionsGrowing(threshold);
            SetResultImage(result);
            
        }

        private void SetResultImage(Image image)
        {
            _resultImage = image;
            pictureBox1.Image = image;
        }

        private void trackBarSegmentationRegionsThreshold_Scroll(object sender, EventArgs e)
        {
            label35.Text = "Порого " + trackBarSegmentationRegionsThreshold.Value;
        }

        private void buttonRegionsAdaptiveThreshold_Click(object sender, EventArgs e)
        {
            _imageProcessor.Open((Bitmap)pictureBox1.Image, true);
            var result = _imageProcessor.SegmentationAdapitiveThreshold();
            SetResultImage(result);
        }

        private void вернутьИзображениеНаГлавнуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resBox.Image = pictureBox1.Image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SegmentationForm sf = new SegmentationForm(pictureBox1);
            sf.Show();
        }

    }
}
