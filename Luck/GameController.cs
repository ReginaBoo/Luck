using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Configuration;
using System.Windows.Forms;

namespace Luck
{
    public class GameController
    {
        private GameModel model;
        private GameView view;
        private Timer gameTimer;
        private bool gameOver = false;
        public GameController(GameModel model, GameView view)
        {
            this.model = model;
            this.view = view;

            gameTimer = new Timer
            {
                Interval = 20
            };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!gameOver)
            {
                UpdateGame();
                model.UpdateEnemy();
                if (model.CheckPlayerEnemyCollision())
                {
                    GameOver();
                }
            }
        }
        private void UpdateGame()
        {
            if (model.GameOver)
            {
                return;
            }

            if (model.CheckPlayerEnemyCollision())
            {
                gameTimer.Stop();
                GameOver();
                return;
            }
            if (model.CheckPlayerCoinCollision())
            {                     
                model.Player.Score++;
                view.UpdateScoreLabel(model.Player.Score);

                int removedCoinIndex = model.coins.IndexOf(model.RemovedCoin);
                if (removedCoinIndex != -1)
                {
                    model.coins.RemoveAt(removedCoinIndex);
                }

                if (removedCoinIndex != -1 && removedCoinIndex < view.coinsPictureBoxes.Count)
                {
                    view.RemoveCoinPictureBox(view.coinsPictureBoxes[removedCoinIndex]);

                    view.coinsPictureBoxes.RemoveAt(removedCoinIndex);
                }

            }

            if (model.Player.Score == 13)
            {
                WinGame();
                return;
            }

            if (model.Enemy.GoLeft)
            {
                model.MoveEnemyLeft(model.Enemy.Speed);
            }
            if (model.Enemy.GoRight) 
            {
                model.MoveEnemyRight(model.Enemy.Speed) ;
            }
            if (model.PlayerIsOnLadder() && model.Player.GoDown && !model.CheckCollision())
            {
                model.MovePlayerDown(model.Player.Speed) ;
            }
            if (model.PlayerIsOnLadder() && model.Player.GoUp)
            {
                model.MovePlayerUp(model.Player.Speed-1);
            }
            if (model.Enemy.GoUp)
            {
                model.MoveEnemyUp(model.Enemy.Speed+1);
            }
            model.CheckTop();
            model.CheckTopEnemy();
            model.CheckCollisionEnemyPlatform();

            model.WrapPlayer();


            if (model.Player.GoLeft && model.CheckCollision() || (model.Player.GoLeft && model.PlayerIsOnLadder()))
            {
                model.MovePlayerLeft(model.Player.Speed);
            }


            if (model.Player.GoRight && model.CheckCollision() || (model.Player.GoRight && model.PlayerIsOnLadder()))
            {
                model.MovePlayerRight(model.Player.Speed) ;
            }

            if (!model.CheckCollision() && !model.PlayerIsOnLadder())
            {

                model.Player.Y += 10;
                if (model.Player.GoLeft) model.MovePlayerLeft(model.Player.Speed-2);
                if(model.Player.GoRight) model.MovePlayerRight(model.Player.Speed-2);

            }
            view.UpdateEnemyPosition(model.Enemy.X, model.Enemy.Y);
            view.UpdatePlayerPosition(model.Player.X, model.Player.Y);
        }

        private void WinGame()
        {
            if (model.GameOver) return;

            model.GameOver = true;
            gameTimer.Stop();
            view.DrawWinMessageBox();
            CloseAllForms();
        }

        private void GameOver()
        {
            if (model.GameOver) return;

            model.GameOver = true;
            gameTimer.Stop();
            view.DrawMassageBox();
            CloseAllForms();
        }
        private void CloseAllForms()
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                Application.OpenForms[i].Close();
            }
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
