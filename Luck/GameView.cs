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
        public PictureBox player;
        public PictureBox floor;
        public PictureBox enemy;
        private GameController controller;
        public List<PictureBox> platformPictureBoxes;
        private List<PictureBox> ladderPictureBoxes;
        public List<PictureBox> coinsPictureBoxes;
        public Label scoreLabel;

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
            DrawEnemy();
            DrawPlatforms();
            DrawLadders();
            DrawCoins();
            DrawLabelScore();
        }

        public void DrawMassageBox() {
            var gameOver = MessageBox.Show("Вы проиграли!", "Game Over", MessageBoxButtons.OK);

        }
        private void DrawForm() 
        {
            ClientSize = new Size(1536, 960);

            this.BackgroundImage = Image.FromFile(@"..\..\Sprites\Background.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        private void DrawPlayer()
        {
            player = new PictureBox
            {
                Image = Image.FromFile(@"..\..\Sprites\berry.png"),
                Size = new Size(model.Player.Width, model.Player.Height),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(model.Player.X,model.Player.Y),
                Tag = "player",
                BackColor = Color.Transparent
            };

            this.Controls.Add(player);
        }

        private void DrawEnemy()
        {
            enemy = new PictureBox
            {
                Image = Image.FromFile(@"..\..\Sprites\tomato.png"), 
                Size = new Size(model.Enemy.Width, model.Enemy.Height),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(model.Enemy.X, model.Enemy.Y),
                Tag = "enemy",
                BackColor = Color.Transparent
            };

            
            this.Controls.Add(enemy);
        }

        private void DrawPlatforms()
        {
            floor = new PictureBox
            {
                BackgroundImage = Image.FromFile(@"..\..\Sprites\floor.png"),
                BackgroundImageLayout = ImageLayout.Tile,
                Size = new Size(ClientSize.Width, 150),
                Location = new Point(0, ClientSize.Height - 150),
                Tag = "platform",
                BackColor = Color.Transparent
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
                    Tag = "platform",
                    BackColor = Color.Transparent
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
                    Size = new Size(ladder.Width, ladder.Height),
                    Location = new Point(ladder.X, ladder.Y),
                    Tag = "ladder",
                    BackColor = Color.Transparent
                };
                ladderPictureBoxes.Add(ladderPictureBox);
                
                this.Controls.Add(ladderPictureBox);
            }
        }

        private void DrawCoins() { 
            coinsPictureBoxes = new List<PictureBox>();
            foreach (var coin in model.coins)
            {
                PictureBox coinPictureBox = new PictureBox
                {
                    Image = Image.FromFile(@"..\..\Sprites\coin.png"),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Size = new Size(30, 30),
                    Location = new Point(coin.X, coin.Y),
                    Tag = "ladder",
                    BackColor = Color.Transparent
                };
                coinsPictureBoxes.Add(coinPictureBox);
                this.Controls.Add(coinPictureBox);
            }

        }

        public void DrawLabelScore()
        {

            scoreLabel = new Label
            {
                Text = "Score: " + model.Player.Score.ToString(),
                Location = new Point(30, 30),
                ForeColor = Color.Black,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold),
                AutoSize = true
            };
            this.Controls.Add(scoreLabel);
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
            player.Location = new Point(X, Y);
        }

        public void UpdateEnemyPosition(int X, int Y)
        {
            enemy.Location = new Point(X, Y);
        }

        public void UpdateScoreLabel(int score)
        {
            scoreLabel.Text = "Score: " + score.ToString();
        }

        public void RemoveCoinPictureBox(PictureBox coinPictureBox)
        {
            if (coinPictureBox != null && this.Controls.Contains(coinPictureBox))
            {
                this.Controls.Remove(coinPictureBox);
                coinPictureBox.Dispose();
            }
        }

        public void DrawWinMessageBox()
        {
            _ = MessageBox.Show("Вы выиграли!", "Congratulations", MessageBoxButtons.OK);
        }
    }
}
