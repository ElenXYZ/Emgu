using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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
            fnCircleDetection();

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

        #region Circle detection
        CircleF[] circles;
        Image<Bgr, Byte> CircleImage;
        private void fnCircleDetection()
        {
            dCannyThres = 180.0;
            CircleImage = img.CopyBlank();
            double dCircleAccumulatorThres = 120.0;
            circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 2.0, 3.0, dCannyThres, dCircleAccumulatorThres);
            foreach (CircleF circle in circles)
                CircleImage.Draw(circle, new Bgr(Color.Brown), 2);
            CircleBox.Image = CircleImage.Bitmap;
        }
        #endregion

        static double angle(Point pt1, Point pt2, Point pt0)
        {
            double dx1 = pt1.X - pt0.X;
            double dy1 = pt1.Y - pt0.Y;
            double dx2 = pt2.X - pt0.X;
            double dy2 = pt2.Y - pt0.Y;
            return (dx1 * dx2 + dy1 * dy2) / Math.Sqrt((dx1 * dx1 + dy1 * dy1) * (dx2 * dx2 + dy2 * dy2) + 1e-10);
        }

        /**
 * Helper function to display text in the center of a contour
 */
        void setLabel(Mat im, string label, VectorOfPoint contour)
        { 
        //int fontface = cv::FONT_HERSHEY_SIMPLEX;
        int fontface =(int) FontFace.HersheySimplex;
        double scale = 0.4;
        int thickness = 1;
        int baseline = 0;
        

       //cv::Size text = cv::getTextSize(label, fontface, scale, thickness, &baseline);
        
        //cv::Rect r = cv::boundingRect(contour);
        Rectangle r = CvInvoke.BoundingRectangle(contour);

            //cv::Point pt(r.x + ((r.width - text.width) / 2), r.y + ((r.height + text.height) / 2));
            Point pt = new Point(r.X + ((r.Width - 10) / 2), r.Y + ((r.Height + 5) / 2));
         
       // cv::rectangle(im, pt + cv::Point(0, baseline), pt + cv::Point(text.width, -text.height), CV_RGB(255,255,255), CV_FILLED);
            CvInvoke.Rectangle(im, r, new MCvScalar(255, 255, 255));
        //cv::putText(im, label, pt, fontface, scale, CV_RGB(0,0,0), thickness, 8);
            CvInvoke.PutText(im, label, pt, FontFace.HersheySimplex, scale, new MCvScalar(0, 0, 0));
}

    public Capture capture;
        public Mat src;
        public Mat gray = new Mat();
        public Mat bw = new Mat();
        public Mat dst = new Mat();
        public VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint ();
        public VectorOfPoint approx = new VectorOfPoint ();
        private void btnCamera_Click(object sender, EventArgs e)
        {
            capture = new Capture();
            while (CvInvoke.WaitKey(30) != 'q')
            {
                src = capture.QueryFrame();
            if (true)
            {
                CvInvoke.CvtColor(src, gray, ColorConversion.Bgr2Gray);
                CvInvoke.Blur(gray, bw, new Size(3, 3), new Point(-1,-1));
                CvInvoke.Canny(gray, bw, 80, 240, 3);
                CvInvoke.Imshow("bw", bw);

                // Find contours

                CvInvoke.FindContours(bw.Clone(), contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                src.CopyTo(dst);
                for (int i = 0; i < contours.Size; i++)
                {


                    // Approximate contour with accuracy proportional
                    // to the contour perimeter

                    CvInvoke.ApproxPolyDP(contours[i], approx, CvInvoke.ArcLength(contours[i], true) * 0.02, true);

                    // Skip small or non-convex objects
                    if (Math.Abs (CvInvoke.ContourArea (contours[i])) < 100 || !CvInvoke.IsContourConvex (approx))
                        continue;
                    if (CvInvoke.ContourArea(contours[i]) < 100 || !CvInvoke.IsContourConvex(approx))
                        continue;

                    if (approx.Size == 3)
                    {
                        setLabel(dst, "TRI", contours[i]);    // Triangles
                    }
                    else if (approx.Size >= 4 && approx.Size <= 6)
                    {

                        // Number of vertices of polygonal curve
                        int vtc = approx.Size;

                        // Get the cosines of all corners
                        VectorOfDouble cos = new VectorOfDouble();
                        List<double> angles = new List<double>();
                        for (int j = 2; j < vtc + 1; j++)
                            angles.Add(angle(approx[j % vtc], approx[j - 2], approx[j - 1]));
                        cos.Push(angles.ToArray());


                            // Sort ascending the cosine values
                         Array.Sort(cos.ToArray());
                            //std::sort(cos.begin(), cos.end());

                            // Get the lowest and the highest cosine
                            double mincos = cos[0];
                            double maxcos = cos[cos.Size - 1];

                        // Use the degrees obtained above and the number of vertices
                        // to determine the shape of the contour
                        if (vtc == 4)
                            setLabel(dst, "RECT", contours[i]);
                        else if (vtc == 5)
                            setLabel(dst, "PENTA", contours[i]);
                        else if (vtc == 6)
                            setLabel(dst, "HEXA", contours[i]);
                    }
                    else
                    {
                        // Detect and label circles
                        double area = CvInvoke.ContourArea (contours[i]);
                        //cv::Rect r = cv::boundingRect(contours[i]);
                        Rectangle r = CvInvoke.BoundingRectangle(contours[i]);
                        int radius = r.Width / 2;

                        if (Math.Abs(1 - ((double)r.Width / r.Height)) <= 0.2 &&
                                Math.Abs(1 - (area / (Math.PI * (radius * radius)))) <= 0.2)
                            setLabel(dst, "CIR", contours[i]);
                    }
                }
                CvInvoke.Imshow("src", src);
                CvInvoke.Imshow("dst", dst);

            }
            else
            {
                break;
            }
        }
 

                }
            }
    }

