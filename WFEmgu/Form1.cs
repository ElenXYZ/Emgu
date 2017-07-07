using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Diagnostics;
using Emgu.CV.Util;

namespace WFEmgu
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lblImagePath.Text = openFileDialog1.FileName;
                GetImage(openFileDialog1.FileName);
            }
        }

        List<Triangle2DF> triangleList = new List<Triangle2DF>();
        List<RotatedRect> boxList = new List<RotatedRect>();
        Image<Bgr, Byte> triangleRectImage;
        private void GetImage(string filepath)
        {

            StringBuilder msgBuilder = new StringBuilder("Performance: ");
            Image<Bgr, Byte> img = new Image<Bgr, byte>(lblImagePath.Text).Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);
            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);
            triangleRectImage = img.CopyBlank();

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            //Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

            #region circle detection
            Stopwatch watch = Stopwatch.StartNew();
            double cannyThreshold = 180.0;
            double circleAccumulatorThreshold = 120;
            CircleF[] circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5);

            watch.Stop();
            msgBuilder.Append(String.Format("Hough circles - {0} ms; ", watch.ElapsedMilliseconds));
            #endregion

            #region Canny and edge detection
            watch.Reset(); watch.Start();
            double cannyThresholdLinking = 120.0;
            UMat cannyEdges = new UMat();
            CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking);

            LineSegment2D[] lines = CvInvoke.HoughLinesP(
               cannyEdges,
               1, //Distance resolution in pixel-related units
               Math.PI / 45.0, //Angle resolution measured in radians.
               20, //threshold
               30, //min Line width
               10); //gap between lines

            watch.Stop();
            msgBuilder.Append(String.Format("Canny & Hough lines - {0} ms; ", watch.ElapsedMilliseconds));
            #endregion

            #region Find triangles and rectangles
            watch.Reset(); watch.Start();
            List<Triangle2DF> triangleList = new List<Triangle2DF>();
            List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contours with area greater than 250
                        {
                            if (approxContour.Size == 3) //The contour has 3 vertices, it is a triangle
                            {
                                Point[] pts = approxContour.ToArray();
                                triangleList.Add(new Triangle2DF(
                                   pts[0],
                                   pts[1],
                                   pts[2]
                                   ));
                            }
                            else if (approxContour.Size == 4) //The contour has 4 vertices.
                            {
                                #region determine if all the angles in the contour are within [80, 100] degree
                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle = Math.Abs(
                                       edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 80 || angle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }
                                #endregion

                                if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                            }
                        }
                    }
                }
            }

            watch.Stop();
            msgBuilder.Append(String.Format("Triangles & Rectangles - {0} ms; ", watch.ElapsedMilliseconds));
            #endregion
            
            originalImageBox.Image = img.ToBitmap();
            this.Text = msgBuilder.ToString();

            #region draw triangles and rectangles
            Image<Bgr, Byte> triangleRectangleImage = img.CopyBlank();
            foreach (Triangle2DF triangle in triangleList)
                triangleRectangleImage.Draw(triangle, new Bgr(Color.DarkBlue), 2);
            foreach (RotatedRect box in boxList)
                triangleRectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);
            triangleRectangleImageBox.Image = triangleRectangleImage.ToBitmap();
            #endregion

            #region Находим контуры
            VectorOfVectorOfPoint contoursDetected = new VectorOfVectorOfPoint();

            CvInvoke.FindContours(uimage, contoursDetected, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
            Image<Bgr, Byte> contourImage = img.CopyBlank();
            var contoursArray = new List<VectorOfPoint>();
            int countDetected = contoursDetected.Size;
          
            for (int i = 0; i < countDetected; i++)
            { using (VectorOfPoint currContour = contoursDetected[i])
                {
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(currContour, approxContour, CvInvoke.ArcLength(currContour, true) * 0.02, true);

                        
                        if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contour with area > 250
                        {
                            if (approxContour.Size == 3) //The contour has 3 vertices, is a triangle
                            {
                                Point[] pts = approxContour.ToArray();
                                triangleList.Add(new Triangle2DF(pts[0], pts[1], pts[2]));
                            }
                            else if (approxContour.Size == 4) // The contour has 4 vertices
                            {
                                #region Determine if all the angles in the contours are within [80,100] degree
                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);
                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double dAngle = Math.Abs(edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (dAngle < 80 || dAngle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }
                                #endregion
                                if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                            }
                        }
                    }
                }

            }
            foreach (Triangle2DF triangle in triangleList)
            {
                triangleRectImage.Draw(triangle, new Bgr(Color.DarkBlue), 2);
            }
            foreach (RotatedRect box in boxList)
                triangleRectImage.Draw(box, new Bgr(Color.Red), 2);
            circleImageBox.Image = triangleRectImage.ToBitmap();

        
               
            #endregion


            //#region draw circles
            //Image<Bgr, Byte> circleImage = img.CopyBlank();
            //foreach (CircleF circle in circles)
            //    circleImage.Draw(circle, new Bgr(Color.Brown), 2);
            //circleImageBox.Image = circleImage.ToBitmap();
            //#endregion

            //#region draw lines
            //Image<Bgr, Byte> lineImage = img.CopyBlank();
            //foreach (LineSegment2D line in lines)
            //    lineImage.Draw(line, new Bgr(Color.Green), 2);
            //lineImageBox.Image = lineImage.ToBitmap();
            //#endregion

        }
        [STAThread]
        private void button2_Click(object sender, EventArgs e)
        {

            FormCamera formCamera = new FormCamera();
            formCamera.Show();


            // Снимок с камеры
            //ImageViewer viewer = new ImageViewer(); //create an image viewer
            //Capture capture = new Capture(); //create a camera captue
            //viewer.Image = capture.QueryFrame(); //draw the image obtained from camera
            //Image<Bgr, Byte> ImageFrame = capture.QueryFrame().ToImage<Bgr, Byte>();
            //originalImageBox.Image = ImageFrame.ToBitmap();
        }

        
    }
}
