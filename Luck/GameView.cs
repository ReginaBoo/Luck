using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Luck
{
    public partial class GameView : Form
    {
        private GameModel model;
        public PictureBox firstPlayer;
        public PictureBox floor;
        private GameController controller;
        public List<PictureBox> platformPictureBoxes;
        private List<PictureBox> ladderPictureBoxes;

        public GameView(GameModel model)
        {
            this.model = model;

            this.KeyDown += KeyIsDown;
            this.KeyUp += KeyIsUp;

            controller = new GameController(model, this);

            this.KeyPreview = true;
            InitializeUI();
        }


        private void InitializeUI()
        {
            DrawForm();
            DrawPlayer();
            DrawPlatforms();
            DrawLadders();
        }
        private void DrawForm() 
        {
            ClientSize = new Size(1536, 960);

            this.BackgroundImage = Image.FromFile(@"..\..\Sprites\Background.png");
            this.BackColor = ColorTranslator.FromHtml("#75c1ff");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        private void DrawPlayer()
        {

            firstPlayer = new PictureBox
            {
                Image = Image.FromFile(@"..\..\Sprites\berry.png"),
                Size = new Size(model.Player.Width, model.Player.Height),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(model.Player.X,model.Player.Y),
                Tag = "player"
            };

            this.Controls.Add(firstPlayer);
        }

        private void DrawPlatforms()
        {
            floor = new PictureBox
            {
                BackgroundImage = Image.FromFile(@"..\..\Sprites\floor.png"),
                BackgroundImageLayout = ImageLayout.Tile,
                Size = new Size(ClientSize.Width, 150),
                Location = new Point(0, ClientSize.Height - 150),
                Tag = "platform"
            };
            this.Controls.Add(floor);

            model.Platforms.Add(new PlatformModel(floor.Location.X, floor.Location.Y, floor.Width, floor.Height));

            platformPictureBoxes = new List<PictureBox>();

            foreach (var platform in model.Platforms)
            {
                PictureBox platformPictureBox = new PictureBox()
                {
                    BackgroundImage = Image.FromFile(@"..\..\Sprites\platform.png"),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Size = new Size(platform.Width, platform.Height),
                    Location = new Point(platform.X, platform.Y),
                    Tag = "platform"
                };

                platformPictureBoxes.Add(platformPictureBox);
                this.Controls.Add(platformPictureBox);
            }

        }

        private void DrawLadders()
        {
            ladderPictureBoxes = new List<PictureBox>();

            foreach (var ladder in model.Ladders)
            {
                PictureBox ladderPictureBox = new PictureBox
                {
                    Image = Image.FromFile(@"..\..\Sprites\Ladder.png"),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Size = new Size(40, 150),
                    Location = new Point(ladder.X, ladder.Y),
                    Tag = "ladder"
                };
                ladderPictureBoxes.Add(ladderPictureBox);
                this.Controls.Add(ladderPictureBox);
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            controller.KeyIsDown(e.KeyCode);
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            controller.KeyIsUp(e.KeyCode);
        }

        public void UpdatePlayerPosition(int X, int Y) 
        {
            firstPlayer.Location = new Point(X, Y);
        }


    }
}
