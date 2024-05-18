﻿using System;
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
                    // Скрываем PictureBox монеты
                    view.RemoveCoinPictureBox(view.coinsPictureBoxes[removedCoinIndex]);

                    // Удаляем скрытый PictureBox монеты из списка в представлении
                    view.coinsPictureBoxes.RemoveAt(removedCoinIndex);
                }

            }
            if (model.Enemy.GoLeft)
            {
                model.MoveEnemyLeft(4);
            }
            if (model.Enemy.GoRight) 
            {
                model.MoveEnemyRight(4);
            }
            if (model.PlayerIsOnLadder() && model.Player.GoDown && !model.CheckCollision())
            {
                model.MovePlayerDown(6);
            }
            if (model.PlayerIsOnLadder() && model.Player.GoUp)
            {
                model.MovePlayerUp(4);
            }
            if (model.Enemy.GoUp)
            {
                model.MoveEnemyUp(4);
            }
            model.CheckTop();
            model.CheckTopEnemy();
            model.CheckCollisionEnemyPlatform();


            if (model.Player.GoLeft && model.CheckCollision() || (model.Player.GoLeft && model.PlayerIsOnLadder()))
            {
                model.MovePlayerLeft(6);
            }


            if (model.Player.GoRight && model.CheckCollision() || (model.Player.GoRight && model.PlayerIsOnLadder()))
            {
                    model.MovePlayerRight(6);
            }

            if (!model.CheckCollision() && !model.PlayerIsOnLadder())
            {

                model.Player.Y += 10;
                if (model.Player.GoLeft) model.MovePlayerLeft(4);
                if(model.Player.GoRight) model.MovePlayerRight(4);

            }
            view.UpdateEnemyPosition(model.Enemy.X, model.Enemy.Y);
            view.UpdatePlayerPosition(model.Player.X, model.Player.Y);
        }

        private void GameOver()
        {
            // Set gameOver flag to true
            gameOver = true;

            // Stop the timer to prevent multiple message boxes
            gameTimer.Stop();

            // Show the game over message box
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
