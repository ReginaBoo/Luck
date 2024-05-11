using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Luck
{
    public class GameModel
    {
        public PlayerModel Player { get; private set; }
        public List<PlatformModel> Platforms { get; private set; }
        public List<LadderModel> Ladders { get; private set; }
        Dictionary<int,List<int>> PositionX = new Dictionary<int, List<int>>();
       

        public GameModel()
        {
            InitializePlayer();
            InitializePlatforms();
            InitializeLadders();
        }

        void InitializePlatforms()
        {
            Platforms = CreatePlatforms();
        }
        void InitializeLadders()
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
                        PositionX[j].Add(platformX+45);
                    }
                    positions.RemoveAt(randomIndex);
                    int platformY = startY + j * (platformHeight + spacingY);

                    platforms.Add(new PlatformModel(platformX, platformY, platformWidth, platformHeight));
                }
            }
            return platforms;
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
                    int ladderX = 0;
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
    
        private void InitializePlayer()
        {
            Player = new PlayerModel()
            {
                Width = 50,
                Height = 50,
                X = 200,
                Y = 960 - 500,
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
                    Player.Y <= platform.Y + platform.Height && // Касается ли верхняя часть персонажа платформы
                    Player.Y >= platform.Y)
                {
                    // Перемещаем персонажа на верхнюю часть платформы
                    Player.Y = platform.Y - Player.Height;
                    break; // Выходим из цикла, если найдена платформа
                }
            }
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
                    return true;

                }
            }
            return false;
        }

    }
}
