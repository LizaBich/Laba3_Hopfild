using Laba2_Klaster.ImageProcessing;
using Laba3_Hopfild.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Laba3_Hopfild
{
    public partial class Form1 : Form
    {
        private Image[] origins;
        private Image[] testImages;
        private readonly ImageProcessing processor;
        private NeuralNetwork neuralNetwork;

        public Form1()
        {
            InitializeComponent();
            this.origins = new Image[3];
            this.testImages = new Image[12];
            this.processor = new ImageProcessing();
        }

        private void BrowseOriginsButton_Click(object sender, EventArgs e)
        {
            var objects = new PictureBox[]
            {
                FirstOriginImage,
                SecondOriginImage,
                ThirdOriginImage
            };

            this.SetImages(objects);

            Bitmap img = (Bitmap)FirstOriginImage.Image;
            this.processor.InversColors(ref img);
            FirstOriginImage.Image = this.processor.CreateBitImage(img);

            img = (Bitmap)SecondOriginImage.Image;
            this.processor.InversColors(ref img);
            SecondOriginImage.Image = this.processor.CreateBitImage(img);

            img = (Bitmap)ThirdOriginImage.Image;
            this.processor.InversColors(ref img);
            ThirdOriginImage.Image = this.processor.CreateBitImage(img);

            this.SetChangedImages(objects);
        }

        private string[] OpenFileDialogAndChooseImages()
        {
            string[] fileNames = null;

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Images (*.PNG;*.JPG)|*.PNG;*.JPG";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileNames = openFileDialog.FileNames;
                }
            }

            return fileNames;
        }

        private void SetImages(PictureBox[] objects)
        {
            var paths = this.OpenFileDialogAndChooseImages();
            if (paths == null || !paths.Any())
            {
                MessageBox.Show("You have to choose the Images!");
            }
            else
            {
                for (var i = 0; i < objects.Length && i < paths.Length; ++i)
                {
                    objects[i].Image = this.processor.Resize((Bitmap)Image.FromFile(paths[i]), objects[i].Width, objects[i].Height);
                }
            }
        }

        private void SetChangedImages(PictureBox[] origins)
        {
            TestImage1.Image = this.processor.AddNoise((Bitmap)origins[0].Image, 0);
            TestImage2.Image = this.processor.AddNoise((Bitmap)origins[0].Image, 25);
            TestImage7.Image = this.processor.AddNoise((Bitmap)origins[0].Image, 50);
            TestImage8.Image = this.processor.AddNoise((Bitmap)origins[0].Image, 75);

            TestImage3.Image = this.processor.AddNoise((Bitmap)origins[1].Image, 0);
            TestImage4.Image = this.processor.AddNoise((Bitmap)origins[1].Image, 25);
            TestImage9.Image = this.processor.AddNoise((Bitmap)origins[1].Image, 50);
            TestImage10.Image = this.processor.AddNoise((Bitmap)origins[1].Image, 75);

            TestImage5.Image = this.processor.AddNoise((Bitmap)origins[2].Image, 0);
            TestImage6.Image = this.processor.AddNoise((Bitmap)origins[2].Image, 25);
            TestImage11.Image = this.processor.AddNoise((Bitmap)origins[2].Image, 50);
            TestImage12.Image = this.processor.AddNoise((Bitmap)origins[2].Image, 75);
        }

        private void TrainButton_Click(object sender, EventArgs e)
        {
            if (this.FirstOriginImage.Image == null || this.SecondOriginImage.Image == null || this.ThirdOriginImage.Image == null)
            {
                MessageBox.Show("Add images!");
                return;
            }

            this.StatusLabel.Text = "Network training started.";

            this.neuralNetwork = new NeuralNetwork(this.FirstOriginImage.Image.Width);

            var list = new List<bool[,]>()
            {
                this.processor.ConvertToBoolMatrix((Bitmap)FirstOriginImage.Image),
                this.processor.ConvertToBoolMatrix((Bitmap)SecondOriginImage.Image),
                this.processor.ConvertToBoolMatrix((Bitmap)ThirdOriginImage.Image)
            };

            this.neuralNetwork.PrepareNetwork(list);

            this.StatusLabel.Text = "Network training completed.";
        }
    }
}
