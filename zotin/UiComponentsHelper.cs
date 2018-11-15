using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zotin
{
    public static class UiComponentsHelper
    {
        public static void OneCheckedAlways(CheckBox checkBox1, CheckBox checkBox2, CheckBox checkBox3)
        {
            OneCheckedAlways(new[] { checkBox1, checkBox2, checkBox3 });
        }

        public static void OneCheckedAlways(CheckBox[] checkBoxes)
        {
            foreach (var item in checkBoxes)
            {
                item.CheckedChanged += (sender, args) =>
                {
                    if (checkBoxes.All(checkBox => !checkBox.Checked))
                    {
                        ((CheckBox)sender).Checked = true;
                    }
                };
            }
        }

        public static void LessThen(this TrackBar trackBar1, TrackBar trackBar2)
        {
            trackBar1.Scroll += (sender, args) =>
            {
                if (trackBar1.Value > trackBar2.Value)
                {
                    trackBar2.Value = trackBar1.Value;
                }
            };

            trackBar2.Scroll += (sender, args) =>
            {
                if (trackBar2.Value < trackBar1.Value)
                {
                    trackBar1.Value = trackBar2.Value;
                }
            };
        }

        public static void SetColorModel(this ComboBox comboBox, CheckBox checkBox1, CheckBox checkBox2, CheckBox checkBox3)
        {
            var selected = (string)comboBox.SelectedItem;
            if (selected == "RGB")
            {
                checkBox1.Text = ColorModel.RgbNames[0];
                checkBox2.Text = ColorModel.RgbNames[1];
                checkBox3.Text = ColorModel.RgbNames[2];
            }
            else if (selected == "HSV")
            {
                checkBox1.Text = ColorModel.HsvNames[0];
                checkBox2.Text = ColorModel.HsvNames[1];
                checkBox3.Text = ColorModel.HsvNames[2];
            }
            else
            {
                checkBox1.Text = ColorModel.YuvNames[0];
                checkBox2.Text = ColorModel.YuvNames[1];
                checkBox3.Text = ColorModel.YuvNames[2];
            }
        }
    }
}
