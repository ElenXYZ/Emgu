using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
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
    public partial class FormCamera : Form
    {
        public FormCamera()
        {
            InitializeComponent();
        }


        private Capture capture;  //takes images from camera as image frames
        private bool captureInProgress;

        private void ProcessFrame(object sender, EventArgs arg)
        {
            ImageViewer viewer = new ImageViewer(); //create an image viewer
            //Capture capture = new Capture(); //create a camera captue
            viewer.Image = capture.QueryFrame();  //line 1
            CamImageBox.Image = viewer.Image.Bitmap;  //line 2
        }

        private void FormCamera_Load(object sender, EventArgs e)
        {
            if (capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }

            if (capture != null)
            {
                if (captureInProgress)
                {  //if camera is getting frames then stop the capture and set button Text
                   // "Start" for resuming capture
                    btnStart.Text = "Start!"; //
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    //if camera is NOT getting frames then start the capture and set button
                    // Text to "Stop" for pausing capture
                    btnStart.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }
                captureInProgress = !captureInProgress;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                Application.Idle -= ProcessFrame;
                capture.Stop();
                capture.Dispose();
            }
        }
    }
}
