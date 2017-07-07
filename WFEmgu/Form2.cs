using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFEmgu
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GetImage(openFileDialog1.FileName);

            }
        }
        UMat uimage = new UMat();
        Image<Bgr, Byte> img;
        private void GetImage(string filepath)
        {

            StringBuilder msgBuilder = new StringBuilder("Performance: ");
            img = new Image<Bgr, byte>(filepath).Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);
            
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);
            originalImageBox.Image = uimage.Bitmap;
            fnEdgeDetection();


        }
        double dCannyThres = 180.0;
        #region Canny and edge detection
        UMat cannyEdges = new UMat();
        LineSegment2D[] lines;
        Image<Bgr, Byte> lineImage;
        private void fnEdgeDetection()
        {
            double dCannyThreLinking = 120.0;
            CvInvoke.Canny(uimage, cannyEdges, dCannyThres, dCannyThreLinking);
            lines = CvInvoke.HoughLinesP(cannyEdges,
            1, //Distance resolution in pixel-related units
            Math.PI / 45.0, //Angle resolution measured in radians.
            20, //threshold
            30, //min Line width
            10); //gap between lines
            lineImage = img.CopyBlank();
            foreach (LineSegment2D line in lines)
                lineImage.Draw(line, new Bgr(Color.Green), 2);
            pictureBox1.Image = lineImage.Bitmap;
        }
        #endregion
    }
}
