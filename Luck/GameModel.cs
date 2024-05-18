using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Luck
{
    public class GameModel
    {
        public PlayerModel Player { get; private set; }
        public List<PlatformModel> Platforms { get; private set; }
        public List<LadderModel> Ladders { get; private set; }
        Dictionary<int, List<int>> PositionX = new Dictionary<int, List<int>>();
        public List<CoinModel> coins = new List<CoinModel>();
        public CoinModel RemovedCoin;
        public EnemyModel Enemy { get; private set; }

        public GameModel()
        {
            InitializePlayer();
            InitializePlatforms();
            InitializeLadders();
            InitializeEnemy();
            coins = CreateCoins();
        }

        public void InitializePlatforms()
        {
            Platforms = CreatePlatforms();
        }
        public void InitializeLadders()
        {
            Ladders = CreateLadders();
        }


        private List<PlatformModel> CreatePlatforms()
        {
            List<PlatformModel> platforms = new List<PlatformModel>();

            int platformWidth = 128;
            int platformHeight = 30;
            int startY = 960 - 300;
            int spacingY = -170;
            int numberOfPlatforms = 9;
            int numberOfRows = 4;
            Random rnd = new Random();

            for (int j = 0; j < numberOfRows; j++)
            {
                List<int> positions = CreateListOfPositionForPlatforms();
                for (int i = 0; i < numberOfPlatforms; i++)
                {
                    int randomIndex = rnd.Next(0, positions.Count);

                    int platformX = positions[randomIndex];
                    if (!PositionX.ContainsKey(j))
                    {
                        PositionX[j] = new List<int>
                        {
                            platformX + 45
                        };
                    }
                    else
                    {
                        PositionX[j].Add(platformX + 45);
                    }
                    positions.RemoveAt(randomIndex);
                    int platformY = startY + j * (platformHeight + spacingY);

                    platforms.Add(new PlatformModel(platformX, platformY, platformWidth, platformHeight));
                }
            }
            return platforms;
        }

        private List<CoinModel> CreateCoins()
        {
            List<CoinModel> coins = new List<CoinModel>();
            Random rnd = new Random();

            foreach (var platform in Platforms)
            {
                int minX = platform.X;
                int maxX = platform.X + platform.Width - 30; // Уменьшаем на ширину монеты


                // Генерируем случайные координаты для монеты в пределах границ платформы
                int coinX = platform.X + 50;
                int coinY = platform.Y - 45;

                // Создаем монету и добавляем ее в список
                CoinModel coin = new CoinModel(coinX, coinY);
                bool intersectsWithLadder = false;
                foreach (var ladder in Ladders)
                {
                    if (CoinIntersectsLadder(coin, ladder))
                    {
                        intersectsWithLadder = true;
                        break;
                    }
                }
                if (!intersectsWithLadder)
                {
                    coins.Add(coin);
                }

            }

            return coins;
        }

        private bool CoinIntersectsLadder(CoinModel coin, LadderModel ladder)
        {
            // Проверяем пересечение по осям X и Y
            bool intersectsX = coin.X < ladder.X + ladder.Width && coin.X + coin.Width > ladder.X;
            bool intersectsY = coin.Y < ladder.Y + ladder.Height && coin.Y + coin.Height > ladder.Y;
            // Если есть пересечение по обеим осям, возвращаем true
            return intersectsX && intersectsY;
        }

        private List<LadderModel> CreateLadders()
        {
            List<LadderModel> ladders = new List<LadderModel>();
            Dictionary<int, List<int>> copyPositionX = new Dictionary<int, List<int>>(PositionX);

            int ladderHeight = 150;
            int startY = 660;
            int spacingY = -290;
            int numberOfRows = 4;
            int numberOfColumns = 5;

            Random rnd = new Random();

            for (int i = 0; i < numberOfRows; i++)
            {
                List<int> tempPositionX = new List<int>(PositionX[i]);
                for (int j = 0; j < numberOfColumns; j++)
                {
                    int ladderX;
                    int randomIndex;
                    if (i > 0 && tempPositionX.Count > 0)
                    {
                        randomIndex = rnd.Next(0, tempPositionX.Count);
                        ladderX = tempPositionX[randomIndex];
                        tempPositionX.RemoveAt(randomIndex);
                        while (!copyPositionX[i - 1].Contains(ladderX) && tempPositionX.Count > 0)
                        {
                            randomIndex = rnd.Next(0, tempPositionX.Count);
                            ladderX = tempPositionX[randomIndex];
                            tempPositionX.RemoveAt(randomIndex);
                        }
                    }
                    else if (i == 0 && tempPositionX.Count > 0)
                    {
                        randomIndex = rnd.Next(0, tempPositionX.Count);
                        ladderX = tempPositionX[randomIndex];
                        tempPositionX.RemoveAt(randomIndex);
                    }
                    else
                    {
                        break;
                    }
                    int ladderY = startY + i * (ladderHeight + spacingY);

                    LadderModel ladder = new LadderModel(ladderX, ladderY);
                    ladders.Add(ladder);
                }
            }

            return ladders;
        }

        private List<int> CreateListOfPositionForPlatforms()
        {
            List<int> positionPlatforms = new List<int>();

            for (int i = 0; i < 1536; i += 128)
            {
                positionPlatforms.Add(i);
            }

            return positionPlatforms;
        }

        public void InitializePlayer()
        {
            Player = new PlayerModel()
            {
                Width = 50,
                Height = 50,
                X = 200,
                Y = 960 - 200,
            };

        }

        public void InitializeEnemy()
        {
            Enemy = new EnemyModel()
            {
                Width = 40,
                Height = 40,
                X = 50,
                Y = 960 - 190
            };
        }

        public void MovePlayerLeft(int speed)
        {
            Player.X -= speed;
        }

        public void MovePlayerRight(int speed)
        {
            Player.X += speed;
        }

        public void MovePlayerDown(int speed)
        {
            Player.Y += speed;
        }
        public void MovePlayerUp(int speed)
        {
            Player.Y -= speed;
        }


        public bool PlayerIsOnLadder()
        {

            foreach (var ladder in Ladders)
            {
                if (Player.X + Player.Width >= ladder.X &&
                   Player.X <= ladder.X + ladder.Width &&
                   Player.Y + Player.Height >= ladder.Y &&
                   Player.Y <= ladder.Y + ladder.Height)
                {
                    return true;
                }
            }
            return false;

        }

        public void CheckTop()
        {
            foreach (var platform in Platforms)
            {
                if (Player.X + Player.Width >= platform.X &&
                    Player.X <= platform.X + platform.Width &&
                    Player.Y <= platform.Y + platform.Height &&
                    Player.Y >= platform.Y)
                {
                    Player.Y = platform.Y - Player.Height;
                    break;
                }
            }
        }

        public void CheckTopEnemy()
        {
            foreach (var platform in Platforms)
            {
                if (Enemy.X + Enemy.Width >= platform.X &&
                    Enemy.X <= platform.X + platform.Width &&
                    Enemy.Y <= platform.Y + platform.Height &&
                    Enemy.Y >= platform.Y)
                {
                    Enemy.Y = platform.Y - Enemy.Height;
                    break;
                }
            }
        }

        public bool CheckCollisionEnemyPlatform()
        {
            foreach (var platform in Platforms)
            {
                if (Enemy.X + Enemy.Width >= platform.X &&
                Enemy.X <= platform.X + platform.Width &&
                Enemy.Y + Enemy.Height >= platform.Y &&
                Enemy.Y <= platform.Y + platform.Height)
                {
                    Enemy.Y = platform.Y - Enemy.Height;
                    return true;
                }
            }
            return false;
        }
        public bool CheckCollision()
        {
            foreach (var platform in Platforms)
            {
                if (Player.X + Player.Width >= platform.X &&
                    Player.X <= platform.X + platform.Width &&
                    Player.Y + Player.Height >= platform.Y &&
                    Player.Y <= platform.Y + platform.Height)
                {
                    Player.Y = platform.Y - Player.Height;
                    return true;

                }
            }
            return false;
        }

        public void UpdateEnemy()
        {
            bool enemyOnLadder = false;
            bool enemyOnPlatform = CheckCollisionEnemyPlatform();

            if (Player.Y < (Enemy.Y - 20))
            {
                LadderModel nearestLadder = null;
                int minDistance = int.MaxValue;

                foreach (var ladder in Ladders)
                {
                    if ((Enemy.Y - ladder.Y) >= (ladder.Height - Enemy.Height))
                    {
                        int distance = Math.Abs(ladder.X - Player.X);
                        if (distance < minDistance)
                        {
                            nearestLadder = ladder;
                            minDistance = distance;
                        }
                    }

                }

                if (nearestLadder != null)
                {
                    if (Enemy.X < nearestLadder.X)
                    {
                        Enemy.GoRight = true;
                        Enemy.GoLeft = false;
                    }
                    else if (Enemy.X > nearestLadder.X)
                    {
                        Enemy.GoLeft = true;
                        Enemy.GoRight = false;
                    }
                    else if (Enemy.X == nearestLadder.X)
                    {
                        enemyOnLadder = true;
                        if (Player.Y < Enemy.Y - 20)
                        {
                            Enemy.GoUp = true;
                        }
                    }
                }
            }

            if (!enemyOnPlatform && !Enemy.GoUp) MoveEnemyDown(4);


            if (CheckCollisionEnemyPlatform() && !enemyOnLadder && Player.Y >= Enemy.Y - 20)
            {
                if (Enemy.X < Player.X)
                {
                    Enemy.GoRight = true;
                    Enemy.GoLeft = false;
                }
                else if (Enemy.X > Player.X)
                {
                    Enemy.GoLeft = true;
                    Enemy.GoRight = false;
                }
            }

            foreach (var ladder in Ladders)
            {
                if (CheckLadderCollision(ladder))
                {
                    if (Player.Y < Enemy.Y - 20)
                    {
                        Enemy.GoUp = true;
                        Enemy.GoLeft = false;
                        Enemy.GoRight = false;
                    }
                    else if (Player.Y > Enemy.Y - 20)
                    {
                        Enemy.GoUp = false;
                        Enemy.Y += 3; // Move enemy down
                    }
                    enemyOnLadder = true;
                    break;
                }
            }

            foreach (var ladder in Ladders)
            {
                if (CheckLadderCollision(ladder))
                {
                    if ((Enemy.Y - 20) > Player.Y)
                    {
                        Enemy.GoUp = true;
                    }
                    else if ((Enemy.Y - 20) < Player.Y)
                    {
                        Enemy.GoUp = false;
                    }
                    break;
                }
                else
                {
                    Enemy.GoUp = false;
                }
            }
        }


        public void MoveEnemyDown(int x)
        {
            Enemy.Y += x;
        }
        public void MoveEnemyRight(int x)
        {
            Enemy.X += x;
        }

        public void MoveEnemyLeft(int x)
        {
            Enemy.X -= x;
        }

        public void MoveEnemyUp(int y)
        {
            Enemy.Y -= y;
        }

        public bool CheckLadderCollision(LadderModel ladder)
        {
            if (Enemy.X >= ladder.X &&
                 Enemy.X <= ladder.X + ladder.Width &&
                 Enemy.Y + Enemy.Height >= ladder.Y &&
                 Enemy.Y <= ladder.Y + ladder.Height)
            {
                Enemy.GoUp = true;
                return true;
            }
            Enemy.GoUp = false;
            return false;
        }


        public bool CheckPlayerEnemyCollision()
        {
            if (Player.X + Player.Width >= Enemy.X &&
                Player.X <= Enemy.X + Enemy.Width &&
                Player.Y + Player.Height >= Enemy.Y &&
                Player.Y <= Enemy.Y + Enemy.Height)
            {
                return true;
            }
            return false;
        }

        public bool CheckPlayerCoinCollision()
        {
            foreach (var coin in coins)
            {
                if (Player.X + Player.Width >= coin.X &&
                    Player.X <= coin.X + coin.Width &&
                    Player.Y + Player.Height >= coin.Y &&
                    Player.Y <= coin.Y + coin.Height)
                {
                    RemovedCoin = coin;
                    return true;
                }

            }
            return false;
        }
    }
}
