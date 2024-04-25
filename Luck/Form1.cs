using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luck
{
    public partial class Form1 : Form
    {
        private PictureBox firstPlayer;
        private PictureBox floor;
        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {

            ClientSize = new Size(1536, 960);

            this.BackgroundImage = Image.FromFile("C:\\C#\\Luck\\Luck\\Sprites\\Background.png");
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

            PictureBox firstPlayer = new PictureBox
            {
                Image = Image.FromFile("C:\\C#\\Luck\\Luck\\Sprites\\berry.png"),
                Size = new Size(80,80),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(150, ClientSize.Height - 160),
                Tag = "player"
            };

            this.Controls.Add(firstPlayer);

            PictureBox floor = new PictureBox
            {
                BackgroundImage = Image.FromFile("C:\\C#\\Luck\\Luck\\Sprites\\floor.png"),
                BackgroundImageLayout = ImageLayout.Tile,
                Size = new Size(ClientSize.Width, 80),
                Location = new Point(0, ClientSize.Height - 80)
            };
            this.Controls.Add(floor);
        }

    private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close(); // Закрываем окно
        }
    }

}
