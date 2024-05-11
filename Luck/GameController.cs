using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Luck
{
    public class GameController
    {
        private GameModel model;
        private GameView view;
        private Timer gameTimer;
        public GameController(GameModel model, GameView view)
        {
            this.model = model;
            this.view = view;

            gameTimer = new Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            UpdateGame();
        }
        private void UpdateGame()
        {
            model.CheckTop();

            if (model.PlayerIsOnLadder() && model.Player.GoDown && !model.CheckCollision())
            {
                model.MovePlayerDown(7);
            }
            if (model.PlayerIsOnLadder() && model.Player.GoUp)
            {
                model.MovePlayerUp(7);
            }


            if (model.Player.GoLeft && model.CheckCollision() || (model.Player.GoLeft && model.PlayerIsOnLadder()))
            {
                model.MovePlayerLeft(7);
            }


            if (model.Player.GoRight && model.CheckCollision() || (model.Player.GoRight && model.PlayerIsOnLadder()))
            {
                    model.MovePlayerRight(7);
            }

            if (!model.CheckCollision() && !model.PlayerIsOnLadder())
            {
                model.Player.Y += 10;
            }
            

            view.UpdatePlayerPosition(model.Player.X, model.Player.Y);
        }

        public void KeyIsDown(Keys e)
        {
            if (e == Keys.A)
            {
                model.Player.GoLeft = true;
            }
            if (e == Keys.D)
            {
                model.Player.GoRight = true;
            }
            if (e == Keys.S)
            {
                model.Player.GoDown = true;
            }
            if (e == Keys.W)
            {
                model.Player.GoUp = true;
            }
        }

        public void KeyIsUp(Keys e)
        {
            if (e == Keys.A)
            {
                model.Player.GoLeft = false;
            }
            if (e == Keys.D)
            {
                model.Player.GoRight = false;
            }
            if (e == Keys.S)
            {
                model.Player.GoDown = false;
            }
            if (e == Keys.W)
            {
                model.Player.GoUp = false;
            }
        }
    }
}
