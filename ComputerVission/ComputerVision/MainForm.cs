using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Linq;

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

        private void histogramBtn_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Color color;
            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);
            int[] histogram = new int[256];
            int[] cumulativeHistogram = new int[256];
            int[] histogramTransform = new int[256];

            //Initialize histogram
            for (int k = 0; k < 256; k++)
            {
                histogram[k] = 0;
            }

            workImage.Lock();
            // Get histogram values
            for (int i = 0; i < workImage.Width; i++)
            {
                for (int j = 0; j < workImage.Height; j++)
                {
                    color = workImage.GetPixel(i, j);
                    byte R = color.R;
                    byte G = color.G;
                    byte B = color.B;

                    byte average = (byte)((R + G + B) / 3);
                    histogram[average]++;
                }
            }

            //Compute cumulative histogram
            cumulativeHistogram[0] = histogram[0];
            for (int i = 1; i < 255; i++)
                cumulativeHistogram[i] = cumulativeHistogram[i - 1] + histogram[i];

            //Compute histogram transform
            for (int i = 0; i < 255; i++)
                histogramTransform[i] = (cumulativeHistogram[i] * 255) / (workImage.Width * workImage.Height);

            for (int i = 0; i < workImage.Width; i++)
            {
                for (int j = 0; j < workImage.Height; j++)
                {
                    //Debug.Print(i.ToString()+" "+j.ToString());
                    //Debug.Print(workImage.Height.ToString());
                    color = workImage.GetPixel(i, j);
                    byte R = color.R;
                    byte G = color.G;
                    byte B = color.B;

                    byte average = (byte)((R + G + B) / 3);
                    color = Color.FromArgb(histogramTransform[average], histogramTransform[average], histogramTransform[average]);
                    workImage.SetPixel(i, j, color);

                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private void rotateBar_Scroll(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Color color;
            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);

            double angleRadian = this.rotateBar.Value * Math.PI / 180.0;

            for (int i = 0; i < workImage.Width; i++)
            {
                for (int j = 0; j < workImage.Height; j++)
                {

                }
            }


            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();

        }

        private void ftjBtn_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);
            Color color;

            int matrixNum = Int32.Parse(ftjTxtBox.Text);
            int matrixDivide = (matrixNum + 2) * (matrixNum + 2);
            int[][] filterMatrix = new int[3][];

            for (int i = 0; i < 3; i++)
                filterMatrix[i] = new int[3];

            filterMatrix[0][0] = 1;
            filterMatrix[0][1] = matrixNum;
            filterMatrix[0][2] = 1;
            filterMatrix[1][0] = matrixNum;
            filterMatrix[1][1] = matrixNum * matrixNum;
            filterMatrix[1][2] = matrixNum;
            filterMatrix[2][0] = 1;
            filterMatrix[2][1] = matrixNum;
            filterMatrix[2][2] = 1;

            workImage.Lock();
            for (int i = 1; i < workImage.Width - 1; i++)
            {
                for (int j = 1; j < workImage.Height - 1; j++)
                {
                    double sumRed = 0;
                    double sumGreen = 0;
                    double sumBlue = 0;
                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {

                            color = workImage.GetPixel(row, col);
                            byte R = color.R;
                            byte G = color.G;
                            byte B = color.B;

                            sumRed += R * filterMatrix[row - (i - 1)][col - (j - 1)];
                            sumGreen += G * filterMatrix[row - (i - 1)][col - (j - 1)];
                            sumBlue += B * filterMatrix[row - (i - 1)][col - (j - 1)];
                        }
                    }
                    sumRed = sumRed / matrixDivide;
                    sumGreen = sumGreen / matrixDivide;
                    sumBlue = sumBlue / matrixDivide;
                    color = Color.FromArgb((int)sumRed, (int)sumGreen, (int)sumBlue);
                    workImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }
        private int _computeMedian(int[] pixels)
        {
            int a = pixels[0];
            int b = pixels[1];
            int c = pixels[2];
            int d = pixels[3];
            int e = pixels[4];

            int med = 0;

            int min0 = Math.Min(a, Math.Min(b, c));
            int min1 = Math.Min(a, Math.Min(b, d));
            int min2 = Math.Min(a, Math.Min(b, e));
            int min3 = Math.Min(a, Math.Min(c, d));
            int min4 = Math.Min(a, Math.Min(c, e));
            int min5 = Math.Min(a, Math.Min(d, e));
            int min6 = Math.Min(b, Math.Min(c, d));
            int min7 = Math.Min(b, Math.Min(c, e));
            int min8 = Math.Min(b, Math.Min(d, e));
            int min9 = Math.Min(c, Math.Min(d, e));

            int max0 = Math.Max(min0, min1);
            int max1 = Math.Max(min2, min3);
            int max2 = Math.Max(min4, min5);
            int max3 = Math.Max(min6, min7);
            int max4 = Math.Max(min8, min9);
            med = Math.Max(max0, Math.Max(max1, Math.Max(max2, Math.Max(max3, max4))));
            return med;

        }
        private void medianBtn_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Color color;
            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);
            workImage.Lock();
            int[] redPixels = new int[5];
            int[] greenPixels = new int[5];
            int[] bluePixels = new int[5];

            for (int i = 0; i < workImage.Width - 1; i++)
            {
                for (int j = 2; j < workImage.Height - 1;)
                {



                    redPixels[0] = workImage.GetPixel(i, j - 2).R;
                    redPixels[1] = workImage.GetPixel(i, j - 1).R;
                    redPixels[2] = workImage.GetPixel(i, j).R;
                    redPixels[3] = workImage.GetPixel(i, j + 1).R;
                    redPixels[4] = workImage.GetPixel(i, j + 2).R;

                    greenPixels[0] = workImage.GetPixel(i, j - 2).G;
                    greenPixels[1] = workImage.GetPixel(i, j - 1).G;
                    greenPixels[2] = workImage.GetPixel(i, j).G;
                    greenPixels[3] = workImage.GetPixel(i, j + 1).G;
                    greenPixels[4] = workImage.GetPixel(i, j + 2).G;

                    bluePixels[0] = workImage.GetPixel(i, j - 2).B;
                    bluePixels[1] = workImage.GetPixel(i, j - 1).B;
                    bluePixels[2] = workImage.GetPixel(i, j).B;
                    bluePixels[3] = workImage.GetPixel(i, j + 1).B;
                    bluePixels[4] = workImage.GetPixel(i, j + 2).B;

                    int redMed = _computeMedian(redPixels);
                    int greenMed = _computeMedian(greenPixels);
                    int blueMed = _computeMedian(bluePixels);
                    color = Color.FromArgb(redMed, greenMed, blueMed);
                    workImage.SetPixel(i, j, color);

                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }
        private bool Salt_Pepper(int x, int y)
        {
            bool is_white_black = true;
            //Debug.Print("SALT_PEPPER");
            //Debug.Print(x.ToString());
            //Debug.Print(y.ToString());

            Color color = workImage.GetPixel(x, y);
            ;
            byte red = color.R;
            byte green = color.G;
            byte blue = color.B;

            int sum = red + green + blue;
            if (sum == 0 || sum == 765)
            {
                is_white_black = true;
            }
            else
            {
                is_white_black = false;
            }

            return is_white_black;
        }
        private int sum_absolute_differences(int x1, int x2, int y1, int y2, int context_size)
        {
            int imageWidth = workImage.Width;
            int imageheight = workImage.Height;
            int sum = 0;
            for (int i = -context_size / 2; i < context_size / 2 && (i + x1 < imageWidth) && (i + x2 < imageWidth); i++)
            {
                for (int j = -context_size / 2; j < context_size / 2 && (j + y1 < imageheight) && (j + y2 < imageheight); j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    Color c0 = workImage.GetPixel(x1 + i, y1 + j);
                    Color c1 = workImage.GetPixel(x2 + i, y2 + j);

                    sum += c0.R - c1.R; // NOTE: the function is intended only for greyscale images
                }
            }
            return sum;
        }
        private int cpb(int x, int y, int context_size, int search_radius, int thresshold, int[] histogram)
        {

            for (int i = x - search_radius; i <= x + search_radius; i++)
            {
                for (int j = y - search_radius; j <= y + search_radius; j++)
                {
                    if (i == x || j == y)
                        continue;
                    if (sum_absolute_differences(x, y, i, j, context_size) < thresshold && !Salt_Pepper(i, j))
                    {
                        //Debug.Print("CPB");
                        int colorValue = workImage.GetPixel(i, j).R; // NOTE: for grayscale only
                        //Debug.Print("GOOD");
                        histogram[colorValue]++;
                    }
                }
            }
            int maxValue = -99999;
            int maxValueIndex = 0;
            for (int i = 0; i < histogram.Length; i++)
                if (histogram[i] > maxValue)
                {
                    maxValue = histogram[i];
                    maxValueIndex = i;
                }

            return maxValueIndex;
        }
        private void markovBtn_Click(object sender, EventArgs e) // cbpf
        {
            int[] histogram = new int[255];

            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Color color;
            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);
            workImage.Lock();

            int contexSize = 3;
            int searchRadiu = 4;
            int thresshold = 500;

            for (int i = 1; i < workImage.Width - 1; i++)
            {
                for (int j = 1; j < workImage.Height - 1; j++)
                {
                    // make greyscale image
                    color = workImage.GetPixel(i, j);
                    byte red = color.R;
                    byte green = color.G;
                    byte blue = color.B;

                    int mean = (red + blue + green) / 3;
                    color = Color.FromArgb(mean, mean, mean);
                    workImage.SetPixel(i, j, color);

                    if (Salt_Pepper(i, j))
                    {
                        int newValue = cpb(i, j, contexSize, searchRadiu, thresshold, histogram);
                        Color newColor = Color.FromArgb(newValue, newValue, newValue);
                        workImage.SetPixel(i, j, newColor);
                    }
                }

                panelDestination.BackgroundImage = null;
                panelDestination.BackgroundImage = workImage.GetBitMap();
                workImage.Unlock();
            }
        }

        private void median2Btn_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Color color;

            workImage.Lock();
            int neighbourNum = 9;
            byte[] redValues = new byte[neighbourNum];
            byte[] greenValues = new byte[neighbourNum];
            byte[] blueValues = new byte[neighbourNum];

            for (int i = 2; i < workImage.Width - 2; i++)
            {
                for (int j = 2; j < workImage.Height - 2; j++)
                {
                    color = workImage.GetPixel(i - 1, j - 1);
                    redValues[0] = color.R;
                    greenValues[0] = color.G;
                    blueValues[0] = color.B;

                    color = workImage.GetPixel(i - 1, j);
                    redValues[1] = color.R;
                    greenValues[1] = color.G;
                    blueValues[1] = color.B;

                    color = workImage.GetPixel(i - 1, j + 1);
                    redValues[2] = color.R;
                    greenValues[2] = color.G;
                    blueValues[2] = color.B;

                    color = workImage.GetPixel(i, j - 1);
                    redValues[3] = color.R;
                    greenValues[3] = color.G;
                    blueValues[3] = color.B;

                    color = workImage.GetPixel(i, j);
                    redValues[4] = color.R;
                    greenValues[4] = color.G;
                    blueValues[4] = color.B;

                    color = workImage.GetPixel(i, j + 1);
                    redValues[5] = color.R;
                    greenValues[5] = color.G;
                    blueValues[5] = color.B;

                    color = workImage.GetPixel(i + 1, j - 1);
                    redValues[6] = color.R;
                    greenValues[6] = color.G;
                    blueValues[6] = color.B;

                    color = workImage.GetPixel(i + 1, j);
                    redValues[7] = color.R;
                    greenValues[7] = color.G;
                    blueValues[7] = color.B;

                    color = workImage.GetPixel(i + 1, j + 1);
                    redValues[8] = color.R;
                    greenValues[8] = color.G;
                    blueValues[8] = color.B;

                    Array.Sort(redValues);
                    Array.Sort(greenValues);
                    Array.Sort(blueValues);

                    color = Color.FromArgb(redValues[4], greenValues[4], blueValues[4]);
                    workImage.SetPixel(i, j, color);
                }
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
        }

        private void ftsBtn_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);

            FastImage originalImage = new FastImage(new Bitmap(sSourceFileName));
            Color color;


            int[][] filterMatrix = new int[3][];

            for (int i = 0; i < 3; i++)
                filterMatrix[i] = new int[3];

            filterMatrix[0][0] = -1;
            filterMatrix[0][1] = -1;
            filterMatrix[0][2] = -1;
            filterMatrix[1][0] = -1;
            filterMatrix[1][1] = 9;
            filterMatrix[1][2] = -1;
            filterMatrix[2][0] = -1;
            filterMatrix[2][1] = -1;
            filterMatrix[2][2] = -1;

            workImage.Lock();
            originalImage.Lock();
            color = originalImage.GetPixel(1, 1);

            for (int i = 1; i < workImage.Width - 1; i++)
            {
                for (int j = 1; j < workImage.Height - 1; j++)
                {
                    double sumRed = 0;
                    double sumGreen = 0;
                    double sumBlue = 0;
                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {

                            color = originalImage.GetPixel(row, col);
                            byte R = color.R;
                            byte G = color.G;
                            byte B = color.B;

                            sumRed += R * filterMatrix[row - (i - 1)][col - (j - 1)];
                            sumGreen += G * filterMatrix[row - (i - 1)][col - (j - 1)];
                            sumBlue += B * filterMatrix[row - (i - 1)][col - (j - 1)];
                        }
                    }


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

                    color = Color.FromArgb((int)sumRed, (int)sumGreen, (int)sumBlue);
                    workImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
            originalImage.Unlock();
        }

        private void unsharpBtn_Click(object sender, EventArgs e)
        {
            double c = 0.6;
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);
            FastImage originalImage = new FastImage(new Bitmap(sSourceFileName));
            Color color;

            //int matrixNum = Int32.Parse(ftjTxtBox.Text);
            int matrixNum = 1;
            int matrixDivide = (matrixNum + 2) * (matrixNum + 2);
            int[][] filterMatrix = new int[3][];

            for (int i = 0; i < 3; i++)
                filterMatrix[i] = new int[3];

            filterMatrix[0][0] = 1;
            filterMatrix[0][1] = matrixNum;
            filterMatrix[0][2] = 1;
            filterMatrix[1][0] = matrixNum;
            filterMatrix[1][1] = matrixNum * matrixNum;
            filterMatrix[1][2] = matrixNum;
            filterMatrix[2][0] = 1;
            filterMatrix[2][1] = matrixNum;
            filterMatrix[2][2] = 1;

            workImage.Lock();
            originalImage.Lock();
            for (int i = 1; i < workImage.Width - 1; i++)
            {
                for (int j = 1; j < workImage.Height - 1; j++)
                {
                    double sumRed = 0;
                    double sumGreen = 0;
                    double sumBlue = 0;
                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {

                            color = workImage.GetPixel(row, col);
                            byte R = color.R;
                            byte G = color.G;
                            byte B = color.B;

                            sumRed += R * filterMatrix[row - (i - 1)][col - (j - 1)];
                            sumGreen += G * filterMatrix[row - (i - 1)][col - (j - 1)];
                            sumBlue += B * filterMatrix[row - (i - 1)][col - (j - 1)];
                        }
                    }
                    sumRed = sumRed / matrixDivide;
                    sumGreen = sumGreen / matrixDivide;
                    sumBlue = sumBlue / matrixDivide;

                    double newRed = c * workImage.GetPixel(i, j).R / (2 * c - 1) - (1 - c) * sumRed / (2 * c - 1);
                    double newGreen = c * workImage.GetPixel(i, j).G / (2 * c - 1) - (1 - c) * sumGreen / (2 * c - 1);
                    double newBlue = c * workImage.GetPixel(i, j).B / (2 * c - 1) - (1 - c) * sumBlue / (2 * c - 1);

                    color = Color.FromArgb((int)newRed, (int)newGreen, (int)newBlue);

                    originalImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            workImage.Unlock();
            workImage.Unlock();
        }

        private void Kirsch_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);

            FastImage originalImage = new FastImage(new Bitmap(sSourceFileName));
            Color color;
            workImage.Lock();
            originalImage.Lock();

            int[][] kirsch1 = new int[3][];
            int[][] kirsch2 = new int[3][];
            int[][] kirsch3 = new int[3][];
            int[][] kirsch4 = new int[3][];

            for (int i = 0; i < 3; i++)
            {
                kirsch1[i] = new int[3];
                kirsch2[i] = new int[3];
                kirsch3[i] = new int[3];
                kirsch4[i] = new int[3];
            }
           
            kirsch1[0][0] = -1;
            kirsch1[0][1] = 0;
            kirsch1[0][2] = 1;
            kirsch1[1][0] = -1;
            kirsch1[1][1] = 0;
            kirsch1[1][2] = 1;
            kirsch1[2][0] = -1;
            kirsch1[2][1] = 0;
            kirsch1[2][2] = 1;

            
            kirsch2[0][0] = 1;
            kirsch2[0][1] = 1;
            kirsch2[0][2] = 1;
            kirsch2[1][0] = 0;
            kirsch2[1][1] = 0;
            kirsch2[1][2] = 0;
            kirsch2[2][0] = -1;
            kirsch2[2][1] = -1;
            kirsch2[2][2] = -1;

            
            kirsch3[0][0] = 0;
            kirsch3[0][1] = 1;
            kirsch3[0][2] = 1;
            kirsch3[1][0] = -1;
            kirsch3[1][1] = 0;
            kirsch3[1][2] = 1;
            kirsch3[2][0] = -1;
            kirsch3[2][1] = -1;
            kirsch3[2][2] = 0;

            
            kirsch4[0][0] = 1;
            kirsch4[0][1] = 1;
            kirsch4[0][2] = 0;
            kirsch4[1][0] = 1;
            kirsch4[1][1] = 0;
            kirsch4[1][2] = -1;
            kirsch4[2][0] = 0;
            kirsch4[2][1] = -1;
            kirsch4[2][2] = -1;


            for (int i = 2; i < workImage.Width - 1; i++)
            {
                for (int j = 2; j < workImage.Height - 1; j++)
                {
                    int[] sumRed = new int[4];
                    int[] sumGreen = new int[4];
                    int[] sumBlue = new int[4];

                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {

                            color = originalImage.GetPixel(row, col);
                            byte R = color.R;
                            byte G = color.G;
                            byte B = color.B;

                            sumRed[0] += R * kirsch1[row - (i - 1)][col - (j - 1)];
                            sumRed[1] += R * kirsch2[row - (i - 1)][col - (j - 1)];
                            sumRed[2] += R * kirsch2[row - (i - 1)][col - (j - 1)];
                            sumRed[3] += R * kirsch3[row - (i - 1)][col - (j - 1)];

                            sumGreen[0] += G * kirsch1[row - (i - 1)][col - (j - 1)];
                            sumGreen[1] += G * kirsch2[row - (i - 1)][col - (j - 1)];
                            sumGreen[2] += G * kirsch3[row - (i - 1)][col - (j - 1)];
                            sumGreen[3] += G * kirsch4[row - (i - 1)][col - (j - 1)];


                            sumBlue[0] += B * kirsch1[row - (i - 1)][col - (j - 1)];
                            sumBlue[1] += B * kirsch2[row - (i - 1)][col - (j - 1)];
                            sumBlue[2] += B * kirsch3[row - (i - 1)][col - (j - 1)];
                            sumBlue[3] += B * kirsch4[row - (i - 1)][col - (j - 1)];
                        }
                  

                    }
                    int maxRed = sumRed.Max();
                    int maxGreen = sumGreen.Max();
                    int maxBlue = sumBlue.Max();

                    if (maxRed > 255)
                        maxRed = 255;
                    else if (maxRed < 0)
                        maxRed = 0;

                    if (maxBlue > 255)
                        maxBlue = 255;
                    else if (maxBlue < 0)
                        maxBlue = 0;

                    if (maxGreen > 255)
                        maxGreen = 255;
                    else if (maxGreen < 0)
                        maxGreen = 0;

                    color = Color.FromArgb(maxRed, maxGreen, maxBlue);
                    workImage.SetPixel(i, j, color);
                }
             
                
            }
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            originalImage.Unlock();
            workImage.Unlock();
        }

        private void laplaceBtn_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);

            FastImage originalImage = new FastImage(new Bitmap(sSourceFileName));
            Color color;
            workImage.Lock();
            originalImage.Lock();

            int[][] laplaceMatrix = new int[3][];
            int[][] h1 = new int[3][];

            for (int i = 0; i < 3; i++)
            {
                laplaceMatrix[i] = new int[3];
                h1[i] = new int[3];
            }

            h1[0][0] = 0;
            h1[0][1] = 1;
            h1[0][2] = 0;
            h1[1][0] = 1;
            h1[1][1] = -4;
            h1[1][2] = 1;
            h1[2][0] = 0;
            h1[2][1] = 1;
            h1[2][2] = 0;

            laplaceMatrix[0][0] = -1;
            laplaceMatrix[0][1] = -1;
            laplaceMatrix[0][2] = -1;
            laplaceMatrix[1][0] = -1;
            laplaceMatrix[1][1] = 8;
            laplaceMatrix[1][2] = -1;
            laplaceMatrix[2][0] = -1;
            laplaceMatrix[2][1] = -1;
            laplaceMatrix[2][2] = -1;

            


            for (int i=1;i<originalImage.Width-1;i++)
            {
                for(int j=0; j < originalImage.Height-1; j++)
                {
                    int[] sumRed = new int[2];
                    int[] sumGreen = new int[2];
                    int[] sumBlue = new int[2];
                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {

                            color = originalImage.GetPixel(row, col);
                            byte R = color.R;
                            byte G = color.G;
                            byte B = color.B;

                            sumRed[0] += R * h1[row - (i - 1)][col - (j - 1)];
                            sumRed[1] += R * laplaceMatrix[row - (i - 1)][col - (j - 1)];
                            sumGreen[0] += G * h1[row - (i - 1)][col - (j - 1)];
                            sumGreen[1] += G * laplaceMatrix[row - (i - 1)][col - (j - 1)];
                            sumBlue[0] += B * h1[row - (i - 1)][col - (j - 1)];
                            sumBlue[1] += B * laplaceMatrix[row - (i - 1)][col - (j - 1)];
                        }
                    }

                    int maxRed = sumRed.Max();
                    int maxGreen = sumGreen.Max();
                    int maxBlue = sumBlue.Max();

                    if (maxRed > 255)
                        maxRed = 255;
                    else if (maxRed < 0)
                        maxRed = 0;

                    if (maxBlue > 255)
                        maxBlue = 255;
                    else if (maxBlue < 0)
                        maxBlue = 0;

                    if (maxGreen > 255)
                        maxGreen = 255;
                    else if (maxGreen < 0)
                        maxGreen = 0;

                    color = Color.FromArgb(maxRed, maxGreen, maxBlue);
                    workImage.SetPixel(i, j, color);
                }
            }


            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            originalImage.Unlock();
            workImage.Unlock();

        }

        private void freiChenBtn_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);

            FastImage originalImage = new FastImage(new Bitmap(sSourceFileName));
            Color color;

            double[,] F1 = {
            { 1,  Math.Sqrt(2),  1 },
            { 0,  0,             0 },
            { -1, -Math.Sqrt(2), -1 }
        };

            double[,] F2 = {
            { 1, 0, -1 },
            { Math.Sqrt(2), 0, -Math.Sqrt(2) },
            { 1, 0, -1 }
        };

            double[,] F3 = {
            { 0, -1, Math.Sqrt(2) },
            { 1,  0, -1 },
            { -Math.Sqrt(2), 1, 0 }
        };

            double[,] F4 = {
            { Math.Sqrt(2), -1, 0 },
            { -1, 0, 1 },
            { 0, 1, -Math.Sqrt(2) }
        };

            double[,] F5 = {
            { 0, 1, 0 },
            { -1, 0, -1 },
            { 0, 1, 0 }
        };

            double[,] F6 = {
            { -1, 0, 1 },
            {  0, 0, 0 },
            {  1, 0, -1 }
        };

            double[,] F7 = {
            { 1, -2, 1 },
            { -2, 4, -2 },
            { 1, -2, 1 }
        };

            double[,] F8 = {
            { -2, 1, -2 },
            {  1, 4, 1 },
            { -2, 1, -2 }
        };

            double[,] F9 = {
            { 1.0/9, 1.0/9, 1.0/9 },
            { 1.0/9, 1.0/9, 1.0/9 },
            { 1.0/9, 1.0/9, 1.0/9 }
};
            workImage.Lock();
            originalImage.Lock();

            for (int i = 1; i < originalImage.Width - 1; i++)
            {
                for (int j = 0; j < originalImage.Height - 1; j++)
                {
                    double[] sumRed = new double[9];
                    double[] sumGreen = new double[9];
                    double[] sumBlue = new double[9];
                    for (int row = i - 1; row <= i + 1; row++)
                    {
                        for (int col = j - 1; col <= j + 1; col++)
                        {

                            color = originalImage.GetPixel(row, col);
                            byte R = color.R;
                            byte G = color.G;
                            byte B = color.B;
                            sumRed[0] += R * F1[row - (i - 1),col - (j - 1)];
                            sumRed[1] += R * F2[row - (i - 1), col - (j - 1)];
                            sumRed[2] += R * F3[row - (i - 1), col - (j - 1)];
                            sumRed[3] += R * F4[row - (i - 1), col - (j - 1)];
                            sumRed[4] += R * F5[row - (i - 1), col - (j - 1)];
                            sumRed[5] += R * F6[row - (i - 1), col - (j - 1)];
                            sumRed[6] += R * F7[row - (i - 1), col - (j - 1)];
                            sumRed[7] += R * F8[row - (i - 1), col - (j - 1)];
                            sumRed[8] += R * F9[row - (i - 1), col - (j - 1)];

                            sumBlue[0] += R * F1[row - (i - 1), col - (j - 1)];
                            sumBlue[1] += R * F2[row - (i - 1), col - (j - 1)];
                            sumBlue[2] += R * F3[row - (i - 1), col - (j - 1)];
                            sumBlue[3] += R * F4[row - (i - 1), col - (j - 1)];
                            sumBlue[4] += R * F5[row - (i - 1), col - (j - 1)];
                            sumBlue[5] += R * F6[row - (i - 1), col - (j - 1)];
                            sumBlue[6] += R * F7[row - (i - 1), col - (j - 1)];
                            sumBlue[7] += R * F8[row - (i - 1), col - (j - 1)];
                            sumBlue[8] += R * F9[row - (i - 1), col - (j - 1)];

                            sumGreen[0] += R * F1[row - (i - 1), col - (j - 1)];
                            sumGreen[1] += R * F2[row - (i - 1), col - (j - 1)];
                            sumGreen[2] += R * F3[row - (i - 1), col - (j - 1)];
                            sumGreen[3] += R * F4[row - (i - 1), col - (j - 1)];
                            sumGreen[4] += R * F5[row - (i - 1), col - (j - 1)];
                            sumGreen[5] += R * F6[row - (i - 1), col - (j - 1)];
                            sumGreen[6] += R * F7[row - (i - 1), col - (j - 1)];
                            sumGreen[7] += R * F8[row - (i - 1), col - (j - 1)];
                            sumGreen[8] += R * F9[row - (i - 1), col - (j - 1)];
                        }
                    }

                    double rRed = 0;
                    double rGreen = 0;
                    double rBlue = 0;

                    double sumRedUpper = 0;
                    double sumRedLower = 0;
                    double sumGreenUpper = 0;
                    double sumGreenLower = 0;
                    double sumBlueUpper = 0;
                    double sumBlueLower = 0;

                    sumRedUpper = Math.Pow(sumRed[0], 2) + Math.Pow(sumRed[1], 2) + Math.Pow(sumRed[2], 2) + Math.Pow(sumRed[3], 2);
                    sumRedLower = sumRedUpper + Math.Pow(sumRed[4], 2) + Math.Pow(sumRed[5], 2) + Math.Pow(sumRed[6], 2) + Math.Pow(sumRed[7], 2);
                    rRed = Math.Sqrt(sumRedUpper / sumRedLower) * 255;

                    sumGreenUpper = Math.Pow(sumGreen[0], 2) + Math.Pow(sumGreen[1], 2) + Math.Pow(sumGreen[2], 2) + Math.Pow(sumGreen[3], 2);
                    sumGreenLower = sumBlueUpper + Math.Pow(sumGreen[4], 2) + Math.Pow(sumGreen[5], 2) + Math.Pow(sumGreen[6], 2) + Math.Pow(sumGreen[7], 2);
                    rGreen = Math.Sqrt(sumGreenUpper / sumGreenLower) * 255;
                    sumBlueUpper = Math.Pow(sumRed[0], 2) + Math.Pow(sumRed[1], 2) + Math.Pow(sumRed[2], 2) + Math.Pow(sumRed[3], 2);
                    sumBlueLower = sumBlueUpper + Math.Pow(sumRed[4], 2) + Math.Pow(sumRed[5], 2) + Math.Pow(sumRed[6], 2) + Math.Pow(sumRed[7], 2);
                    rGreen = Math.Sqrt(sumBlueUpper / sumBlueLower) * 255;

                    if (rRed > 255)
                        rRed = 255;
                    else if (rRed < 0)
                        rRed = 0;

                    if (rGreen > 255)
                        rGreen = 255;
                    else if (rGreen < 0)
                        rGreen = 0;

                    if (rBlue > 255)
                        rBlue = 255;
                    else if (rBlue < 0)
                        rBlue = 0;
                        color = Color.FromArgb((int)rRed, (int)rGreen, (int)rBlue);
                    workImage.SetPixel(i, j, color);
                }
            }

            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            originalImage.Unlock();
            workImage.Unlock();

        }
        private int getMeanIntensity(int x0,int y0, int x1, int y1)
        {
            int meanIntensity = 0;
            int pixelCount = 0;
            Color pixelColor;
            //Debug.Print(x1.ToString());
            //Debug.Print(y1.ToString());
            for(int column = x0; column <= x1; column++)
            {
                for(int row = y0; row <= y1; row++)
                {
                    pixelColor = workImage.GetPixel(column, row);

                    int red = pixelColor.R;
                    int green = pixelColor.G;
                    int blue = pixelColor.B;

                    int greyIntensity = (red + green + blue) / 3;
                    pixelCount++;
                    meanIntensity += greyIntensity;
                }
            }
            if (pixelCount == 0)
                return 0;

            return meanIntensity/pixelCount;
        }

        private void splitMergeBtn_Click(object sender, EventArgs e)
        {
            if (workImage == null)
            {
                MessageBox.Show("No image loaded. Please load an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            image = new Bitmap(sSourceFileName);
            workImage = new FastImage(image);

            FastImage originalImage = new FastImage(new Bitmap(sSourceFileName));
            Color color;
            workImage.Lock();
            originalImage.Lock();

            int threshold = Int32.Parse(splitThresholdBox.Text);
            Queue<ImageBlock> ProcessList = new Queue<ImageBlock>();
            List<ImageBlock> Regions = new List<ImageBlock>();
            ImageBlock originalImageBlock = new ImageBlock(0, 0, workImage.Width - 1, workImage.Height - 1);

            ProcessList.Enqueue(originalImageBlock);

            do
            {
                ImageBlock block = ProcessList.Dequeue();
                int x0 = block.startPoint.x;
                int y0 = block.startPoint.y;

                int x1 = block.endPoint.x;
                int y1 = block.endPoint.y;

                int meanIntensity = getMeanIntensity(x0, y0, x1, y1);
                int numBlockPixels = Math.Abs((x1 - x0) * (y1 - y0));
                int squareError = 0;
                bool exit = false;
                for(int row = x0; row < x1; row++)
                {
                    for(int column = y0; column < y1; column++)
                    {
                        Color regionPixelColor = workImage.GetPixel(row, column);
                        int regionPixelAverage = (regionPixelColor.R + regionPixelColor.G + regionPixelColor.B) / 3;

                        squareError = (regionPixelAverage - meanIntensity) * (regionPixelAverage - meanIntensity);
                    }
                }
                //Debug.Print("Amin");
                float sigma;
                if (1 - numBlockPixels == 0)
                    sigma = 0;
                else
                    sigma = squareError / (1 - numBlockPixels);

                if(sigma > threshold)
                {
                    int halfRow = Math.Abs((x1 - x0) / 2);
                    int halfColumn = Math.Abs((y1 - y0) / 2);

                    if ((x1 - x0 == 0) && (y1 - y0 == 1))
                        exit = true;
                    ProcessList.Enqueue(new ImageBlock(x0, y0, halfRow, halfColumn));
                    ProcessList.Enqueue(new ImageBlock(halfRow, halfColumn, x1, y1));
                    ProcessList.Enqueue(new ImageBlock(x0, halfColumn, halfRow, y1));
                    ProcessList.Enqueue(new ImageBlock(halfRow, y0, halfRow, halfColumn));
                }

                Debug.Print(ProcessList.Count().ToString());
            } while (ProcessList.Count() > 0 && !ext);
            
            panelDestination.BackgroundImage = null;
            panelDestination.BackgroundImage = workImage.GetBitMap();
            originalImage.Unlock();
            workImage.Unlock();
        }
    }
}