using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Luck
{
    public partial class Form1 : Form
    {
        private PictureBox firstPlayer;
        private PictureBox floor;
        private PictureBox ladder;
        List<int> positionX = new List<int> {};


        bool goLeft, goRight, goUp, jumping;
        int jumpSpeed;
        int force;
        int playerSpeed = 7;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
            InitializeTimer();
        }

        private void MainGameTimer(object sender, EventArgs e)
        {
            firstPlayer.Top += jumpSpeed;

            if (goLeft == true)
            {
                firstPlayer.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                firstPlayer.Left += playerSpeed;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed -= 7;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }
            if (goUp == true)
            { 
                firstPlayer.Top -= playerSpeed; 
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "platform") 
                    {
                        if (firstPlayer.Bounds.IntersectsWith(x.Bounds)) 
                        {
                            force = 1;
                            firstPlayer.Top = x.Top - firstPlayer.Height;
                        }
                        x.BringToFront();
                    }

                    if ((string)x.Tag == "ladder" && goUp == true && firstPlayer.Bounds.IntersectsWith(x.Bounds)) // Проверяем, находится ли игрок на лестнице и нажата ли клавиша вверх
                    {
                        firstPlayer.Top -= playerSpeed; 
                    }

                }

                
            }
        }
        private void InitializeTimer()
        {
            Timer gameTimer = new Timer()
            {
                Interval = 20,
                Enabled = true
            };

            gameTimer.Tick += MainGameTimer;
        }

        private void InitializeGame()
        {
            ClientSize = new Size(1536, 960);

            this.BackgroundImage = Image.FromFile(@"..\..\Sprites\Background.png");
            this.BackColor = ColorTranslator.FromHtml("#75c1ff");
            this.BackgroundImageLayout = ImageLayout.Stretch; 
            this.FormBorderStyle = FormBorderStyle.None;

            Button closeButton = new Button
            {
                Text = "Закрыть",
                Location = new Point(ClientSize.Width - 90, 10),
            };

            closeButton.Click += CloseButton_Click;
            this.Controls.Add(closeButton);

            firstPlayer = new PictureBox
            {
                Image = Image.FromFile("C:\\C#\\Luck\\Luck\\Sprites\\berry.png"),
                Size = new Size(60,60),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(150, ClientSize.Height - 160),
                Tag = "player"
            };

            this.Controls.Add(firstPlayer);

            floor = new PictureBox
            {
                BackgroundImage = Image.FromFile("C:\\C#\\Luck\\Luck\\Sprites\\floor.png"),
                BackgroundImageLayout = ImageLayout.Tile,
                Size = new Size(ClientSize.Width, 150),
                Location = new Point(0, ClientSize.Height - 150),
                Tag = "platform"
            };
            this.Controls.Add(floor);

            CreatePlatforms();

            CreateListOfPositionForLadders(positionX);

            CreateLadders(positionX);

            
            this.Controls.Add(ladder);
            this.KeyPreview = true; 
        }

        private List<int> CreateListOfPositionForLadders(List<int> positionX)
        {
            for (int i = 45; i < ClientSize.Width; i += 125)
            { 
                positionX.Add(i);
            }
            return positionX;
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.D)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.W && jumping == false)
            {
                jumping = true;
            }

            if (e.KeyCode == Keys.S && IsLadder())
            {
                goUp = true;
            }
            else
            {
                goUp = false;
            }
        }

        private bool IsLadder()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "ladder" && firstPlayer.Bounds.IntersectsWith(x.Bounds)) // Проверяем, находится ли игрок на лестнице и нажата ли клавиша вверх
                    {
                        return true;
                    }

                }
            }
            return false;
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.D)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }
        }

        private void CreatePlatforms() 
        {
            int platformWidth = 128;
            int platformHeight = 30;
            int startX = 0;
            int startY = ClientSize.Height - 300;
            int spacingY = -170;
            int numberOfPlatforms = 12;
            int numberOfRows = 4;

            for (int j = 0; j < numberOfRows; j++) 
            {
                for (int i = 0; i < numberOfPlatforms; i++)
                {
                    int platformX = startX + platformWidth * i;
                    int platformY = startY + j * (platformHeight + spacingY);
                    PictureBox platformPictureBox = new PictureBox()
                    {
                        Image = Image.FromFile("C:\\C#\\Luck\\Luck\\Sprites\\platform.png"),
                        Size = new Size(platformWidth, platformHeight),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = new Point(platformX, platformY),
                        Tag = "platform"
                    };
                    this.Controls.Add(platformPictureBox);
                }
            }
            
        }

        private void CreateLadders(List<int> positionsX)
        {
            int ladderHeight = 150;
            int startY = ClientSize.Height - 300;
            int spacingY = -290;
            int numberOfRows = 4;
            int numberOfColumns = 6;

            Random rnd = new Random();

            for (int i = 0; i < numberOfRows; i++)
            {
                List<int> positions = new List<int>(positionsX);
                
                for (int j = 0; j < numberOfColumns; j++)
                {
                    int randomIndex = rnd.Next(0, positions.Count);

                    int ladderX = positions[randomIndex];

                    positions.RemoveAt(randomIndex);

                    int ladderY = startY + i * (ladderHeight + spacingY);

                    PictureBox ladder = new PictureBox
                    {
                        Image = Image.FromFile("C:\\C#\\Luck\\Luck\\Sprites\\Ladder.png"),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = new Size(40, 150),
                        Location = new Point(ladderX, ladderY),
                        Tag = "ladder"
                    };
                    this.Controls.Add(ladder);
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
            {
                this.Close();
            }


    }

}
