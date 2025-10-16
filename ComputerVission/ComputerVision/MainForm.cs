using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace ComputerVision
{
    public partial class MainForm : Form
    {
        private string sSourceFileName = "";
        private FastImage workImage;
        private Bitmap image = null;

        public MainForm()
        {
            InitializeComponent();
            this.trackBar1.Value = 0;
            this.trackBar1.Minimum = -120;
            this.trackBar1.Maximum = 120;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (image != null)
                    {
                        image.Dispose();
                    }

                    sSourceFileName = openFileDialog.FileName;
                    image = new Bitmap(sSourceFileName);
                    workImage = new FastImage(image);

                    panelSource.BackgroundImage = new Bitmap(sSourceFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonGrayscale_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Color color;

            workImage.Lock();
            for (int i = 0; i < workImage.Width; i++)
            {
                for (int j = 0; j < workImage.Height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    byte R = color.R;
                    byte G = color.G;
                    byte B = color.B;

                    byte average = (byte)((R + G + B) / 3);

                    color = Color.FromArgb(average, average, average);

                    workImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Color color;

            workImage.Lock();
            for (int i = 0; i < workImage.Width; i++)
            {
                for (int j = 0; j < workImage.Height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    byte R = (byte)(255 - color.R);
                    byte G = (byte)(255 - color.G);
                    byte B = (byte)(255 - color.B);

                    color = Color.FromArgb(R, G, B);

                    workImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private void lumenBar_Scroll(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);
            Color color;

            workImage.Lock();
            for (int i = 0; i < workImage.Width; i++)
            {
                for (int j = 0; j < workImage.Height; j++)
                {
                    int delta = this.lumenBar.Value;
                    color = workImage.GetPixel(i, j);

                    int sumRed = color.R + delta;
                    int sumGreen = color.G + delta;
                    int sumBlue = color.B + delta;

                    if (sumRed > 255)
                        sumRed = 255;
                    else if (sumRed < 0)
                        sumRed = 0;

                    if (sumGreen > 255)
                        sumGreen = 255;
                    else if (sumGreen < 0)
                        sumGreen = 0;

                    if (sumBlue > 255)
                        sumBlue = 255;
                    else if (sumBlue < 0)
                        sumBlue = 0;

                    color = Color.FromArgb(sumRed, sumGreen, sumBlue);

                    workImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //Change contrast level
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);
            Color color;

            workImage.Lock();
            int maxRed = 0;
            int maxGreen = 0;
            int maxBlue = 0;

            int minRed = 255;
            int minGreen = 255;
            int minBlue = 255;
  
            int delta = this.trackBar1.Value;

            //Get max and minimum values of the image
            for (int i = 0; i < workImage.Width; i++)
            {
                for (int j = 0; j < workImage.Height; j++)
                {
                    color = workImage.GetPixel(i, j);

                    if (color.R > maxRed)
                        maxRed = color.R;
                    if (color.G > maxGreen)
                        maxGreen = color.G;
                    if (color.B > maxBlue)
                        maxBlue = color.B;
                    if (color.R < minRed)
                        minRed = color.R;
                    if (color.G < minGreen)
                        minGreen = color.G;
                    if (color.B < minBlue)
                        minBlue = color.B;

                }
            }

            //Compute the new intervals for the histograms
            int newMaxRed = maxRed + delta;
            int newMaxGreen = maxGreen + delta;
            int newMaxBlue = maxBlue + delta;
            int newMinRed = minRed - delta;
            int newMinGreen = minGreen - delta;
            int newMinBlue = minBlue - delta;

            for (int i = 0; i < workImage.Width; i++)
            {
                for (int j = 0; j < workImage.Height; j++)
                {
                    color = workImage.GetPixel(i, j);

                    double newRed = (newMaxRed - newMinRed) * (color.R - minRed) / (maxRed - minRed) + newMinRed;
                    if (newRed > 255)
                        newRed = 255;
                    else if (newRed < 0)
                        newRed = 0;

                    double newGreen = (newMaxGreen - newMinGreen) * (color.G - minGreen) / (maxGreen - minGreen) + newMinGreen;
                    if (newGreen > 255)
                        newGreen = 255;
                    else if (newGreen < 0)
                        newGreen = 0;
                    double newBlue = (newMaxBlue - newMinBlue) * (color.B - minBlue) / (maxBlue - minBlue) + newMinBlue;
                    if (newBlue > 255)
                        newBlue = 255;
                    else if (newBlue < 0)
                        newBlue = 0;
                    color = Color.FromArgb((int)newRed, (int)newGreen, (int)newBlue);
                    workImage.SetPixel(i, j, color);

                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }
    }
}