namespace WFEmgu
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblImagePath = new System.Windows.Forms.Label();
            this.originalImageBox = new System.Windows.Forms.PictureBox();
            this.triangleRectangleImageBox = new System.Windows.Forms.PictureBox();
            this.circleImageBox = new System.Windows.Forms.PictureBox();
            this.lineImageBox = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triangleRectangleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Open Image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblImagePath
            // 
            this.lblImagePath.AutoSize = true;
            this.lblImagePath.Location = new System.Drawing.Point(93, 17);
            this.lblImagePath.Name = "lblImagePath";
            this.lblImagePath.Size = new System.Drawing.Size(0, 13);
            this.lblImagePath.TabIndex = 1;
            // 
            // originalImageBox
            // 
            this.originalImageBox.Location = new System.Drawing.Point(12, 41);
            this.originalImageBox.Name = "originalImageBox";
            this.originalImageBox.Size = new System.Drawing.Size(400, 400);
            this.originalImageBox.TabIndex = 2;
            this.originalImageBox.TabStop = false;
            // 
            // triangleRectangleImageBox
            // 
            this.triangleRectangleImageBox.Location = new System.Drawing.Point(418, 41);
            this.triangleRectangleImageBox.Name = "triangleRectangleImageBox";
            this.triangleRectangleImageBox.Size = new System.Drawing.Size(400, 400);
            this.triangleRectangleImageBox.TabIndex = 3;
            this.triangleRectangleImageBox.TabStop = false;
            // 
            // circleImageBox
            // 
            this.circleImageBox.Location = new System.Drawing.Point(12, 447);
            this.circleImageBox.Name = "circleImageBox";
            this.circleImageBox.Size = new System.Drawing.Size(400, 400);
            this.circleImageBox.TabIndex = 4;
            this.circleImageBox.TabStop = false;
            // 
            // lineImageBox
            // 
            this.lineImageBox.Location = new System.Drawing.Point(418, 447);
            this.lineImageBox.Name = "lineImageBox";
            this.lineImageBox.Size = new System.Drawing.Size(400, 400);
            this.lineImageBox.TabIndex = 5;
            this.lineImageBox.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(845, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Capture";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 687);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lineImageBox);
            this.Controls.Add(this.circleImageBox);
            this.Controls.Add(this.triangleRectangleImageBox);
            this.Controls.Add(this.originalImageBox);
            this.Controls.Add(this.lblImagePath);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Распознование фигур";
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triangleRectangleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.circleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblImagePath;
        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.PictureBox triangleRectangleImageBox;
        private System.Windows.Forms.PictureBox circleImageBox;
        private System.Windows.Forms.PictureBox lineImageBox;
        private System.Windows.Forms.Button button2;
    }
}

