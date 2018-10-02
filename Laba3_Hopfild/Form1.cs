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
        private const int Size = 30;

        private Image[] origins;
        private Image[] testImages;
        private readonly ImageProcessing processor;
        private NeuralNetwork neuralNetwork;
        private IList<bool[,]> sourceImages;
        private IList<bool[,]> customImages;

        public Form1()
        {
            InitializeComponent();
            this.TestImagesButton.Visible = false;
            this.TestImagesButton.Enabled = false;
            this.IsCustomImages.CheckedChanged += this.IsCustomImage_Checked;

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

            if (!this.IsCustomImages.Checked)
            {
                this.SetChangedImages(objects);
            }
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
            TestImage2.Image = this.processor.AddNoise((Bitmap)origins[0].Image, 50);
            TestImage7.Image = this.processor.AddNoise((Bitmap)origins[0].Image, 75);
            TestImage8.Image = this.processor.AddNoise((Bitmap)origins[0].Image, 100);

            TestImage3.Image = this.processor.AddNoise((Bitmap)origins[1].Image, 0);
            TestImage4.Image = this.processor.AddNoise((Bitmap)origins[1].Image, 25);
            TestImage9.Image = this.processor.AddNoise((Bitmap)origins[1].Image, 50);
            TestImage10.Image = this.processor.AddNoise((Bitmap)origins[1].Image, 75);

            TestImage5.Image = this.processor.AddNoise((Bitmap)origins[2].Image, 0);
            TestImage6.Image = this.processor.AddNoise((Bitmap)origins[2].Image, 40);
            TestImage11.Image = this.processor.AddNoise((Bitmap)origins[2].Image, 79);
            TestImage12.Image = this.processor.AddNoise((Bitmap)origins[2].Image, 110);
        }

        private void TrainButton_Click(object sender, EventArgs e)
        {
            if (this.FirstOriginImage.Image == null || this.SecondOriginImage.Image == null || this.ThirdOriginImage.Image == null)
            {
                MessageBox.Show("Add images!");
                return;
            }

            this.StatusLabel.Text = "Network training started.";

            this.neuralNetwork = new NeuralNetwork(Size);

            this.sourceImages = new List<bool[,]>()
            {
                this.processor.ConvertToBoolMatrix(this.processor.Resize((Bitmap)FirstOriginImage.Image, Size, Size)),
                this.processor.ConvertToBoolMatrix(this.processor.Resize((Bitmap)SecondOriginImage.Image, Size, Size)),
                this.processor.ConvertToBoolMatrix(this.processor.Resize((Bitmap)ThirdOriginImage.Image, Size, Size))
            };

            this.neuralNetwork.PrepareNetwork(this.sourceImages);

            this.StatusLabel.Text = "Network training completed.";
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            if (this.FirstOriginImage.Image == null || this.SecondOriginImage.Image == null || this.ThirdOriginImage.Image == null)
            {
                MessageBox.Show("Add images!");
                return;
            }

            this.StatusLabel.Text = "Network is procesing your data.";
            var firstCount = 0;
            var secondCount = 0;
            var thirdCount = 0;
            var labels = this.GetLabels();
            

            if (this.customImages.Any())
            {
                for (var i = 0; i < 3; ++i)
                {
                    foreach (var item in this.customImages)
                    {
                        if (this.neuralNetwork.Analyze(this.sourceImages[i], item))
                        {
                            switch (i)
                            {
                                case 0: ++firstCount; break;
                                case 1: ++secondCount; break;
                                case 2: ++thirdCount; break;
                            }
                        }
                    }
                }
            } else
            {
                var changedImages = this.GetChangedImages();

                for (var i = 0; i < 3; ++i)
                {
                    foreach (var item in changedImages)
                    {
                        if (this.neuralNetwork.Analyze(this.sourceImages[i], this.processor.ConvertToBoolMatrix(item)))
                        {
                            switch (i)
                            {
                                case 0: ++firstCount; break;
                                case 1: ++secondCount; break;
                                case 2: ++thirdCount; break;
                            }
                        }
                    }
                }
            }
            
            labels[0].Text = $"Count of images, similar to the first Image: {firstCount}";
            labels[1].Text = $"Count of images, similar to the second Image: {secondCount}";
            labels[2].Text = $"Count of images, similar to the third Image: {thirdCount}";

            this.StatusLabel.Text = "Network have finished.";
        }

        private void IsCustomImage_Checked(object sender, EventArgs e)
        {
            this.TestImagesButton.Visible = this.IsCustomImages.Checked;
            this.TestImagesButton.Enabled = this.IsCustomImages.Checked;
            if (!this.IsCustomImages.Checked)
            {
                this.customImages.Clear();
            }
        }

        private void TestImagesButton_Click(object sender, EventArgs e)
        {
            var paths = this.OpenFileDialogAndChooseImages();
            if (paths == null || !paths.Any())
            {
                MessageBox.Show("You have to choose the Images!");
            }
            else
            {
                this.customImages = new List<bool[,]>();

                for (var i = 0; i < paths.Length; ++i)
                {
                    this.SetCustomImage(paths[i], i + 1);
                }
            }
        }

        private Label[] GetLabels()
        {
            return new Label[]
            {
                this.FirstImageCount,
                this.SecondImageCount,
                this.ThirdImageCount
            };
        }

        private Bitmap[] GetChangedImages()
        {
            return new Bitmap[]
            {
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.FirstOriginImage.Image, Size, Size), 0),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.FirstOriginImage.Image, Size, Size), 50),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.FirstOriginImage.Image, Size, Size), 75),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.FirstOriginImage.Image, Size, Size), 100),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.SecondOriginImage.Image, Size, Size), 0),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.SecondOriginImage.Image, Size, Size), 25),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.SecondOriginImage.Image, Size, Size), 50),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.SecondOriginImage.Image, Size, Size), 75),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.ThirdOriginImage.Image, Size, Size), 0),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.ThirdOriginImage.Image, Size, Size), 40),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.ThirdOriginImage.Image, Size, Size), 79),
                this.processor.AddNoise(this.processor.Resize((Bitmap)this.ThirdOriginImage.Image, Size, Size), 110)
            };
        }

        private void SetCustomImage(string path, int number)
        {
            switch (number)
            {
                case 1:
                    this.TestImage1.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage1.Width, this.TestImage1.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage1.Image));
                    break;
                case 2:
                    this.TestImage2.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage2.Width, this.TestImage2.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage2.Image));
                    break;
                case 3:
                    this.TestImage3.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage3.Width, this.TestImage3.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage3.Image));
                    break;
                case 4:
                    this.TestImage4.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage4.Width, this.TestImage4.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage4.Image));
                    break;
                case 5:
                    this.TestImage5.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage5.Width, this.TestImage5.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage5.Image));
                    break;
                case 6:
                    this.TestImage6.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage6.Width, this.TestImage6.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage6.Image));
                    break;
                case 7:
                    this.TestImage7.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage7.Width, this.TestImage7.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage7.Image));
                    break;
                case 8:
                    this.TestImage8.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage8.Width, this.TestImage8.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage8.Image));
                    break;
                case 9:
                    this.TestImage9.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage9.Width, this.TestImage9.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage9.Image));
                    break;
                case 10:
                    this.TestImage10.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage10.Width, this.TestImage10.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage10.Image));
                    break;
                case 11:
                    this.TestImage11.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage11.Width, this.TestImage11.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage11.Image));
                    break;
                case 12:
                    this.TestImage12.Image = this.processor.Resize((Bitmap)Image.FromFile(path), this.TestImage12.Width, this.TestImage12.Height);
                    this.customImages.Add(this.PrepareCustomImage((Bitmap)this.TestImage12.Image));
                    break;
            }
        }

        private bool[,] PrepareCustomImage(Bitmap image)
        {
            var tempImg = this.processor.CreateBitImage(
                    this.processor.MinMaxFilter(
                        this.processor.ConvertToShadowsOfGray(
                            this.processor.Resize(image, Size, Size)), 1));
            this.processor.InversColors(ref tempImg);

            return this.processor.ConvertToBoolMatrix(tempImg); 
        }
    }
}
