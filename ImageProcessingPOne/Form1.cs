using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ImageProcessingPOne
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            for(int x = 0; x < loaded.Width; x++)
                for(int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;

                    Color greyscale = Color.FromArgb(grey, grey, grey);
                    processed.SetPixel(x, y, greyscale);          
                }

            pictureBox2.Image = processed;
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }

            pictureBox2.Image = processed;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);

                    processed.SetPixel(x, y, Color.FromArgb(255-pixel.R, 255-pixel.G, 255-pixel.G));
                }

            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color sample;
            Color gray;
            Byte graydata;

            Bitmap a = (Bitmap)pictureBox1.Image;
            Bitmap b = (Bitmap)pictureBox2.Image;
            
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    sample = a.GetPixel(x, y);
                    graydata = (byte)((sample.R + sample.G + sample.B) / 3);
                    gray = Color.FromArgb(graydata, graydata, graydata);
                    a.SetPixel(x, y, gray);
                }
            }

            int[] histdata = new int[256];
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    sample = a.GetPixel(x, y);
                    histdata[sample.R]++;
                }
            }

            b = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 800; y++)
                {
                    b.SetPixel(x, y, Color.White);
                }
            }

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < Math.Min(histdata[x] / 5, b.Height - 1); y++)
                {
                    b.SetPixel(x, (b.Height - 1) - y, Color.Black);
                }
            }
            pictureBox2.Image = b;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    int sepiaRed = Convert.ToInt32(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int sepiaGreen = Convert.ToInt32(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int sepiaBlue = Convert.ToInt32(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    if (sepiaRed > 255)
                        sepiaRed = 255;

                    if (sepiaGreen > 255)
                        sepiaGreen = 255;

                    if (sepiaBlue > 255)
                        sepiaBlue = 255;

                    Color sepia = Color.FromArgb(sepiaRed, sepiaGreen, sepiaBlue);
                    processed.SetPixel(x, y, sepia);
                }

            pictureBox2.Image = processed;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        pictureBox2.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        pictureBox2.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        pictureBox2.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }
    }
}
