using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class PillarBatch : Background
    {
        private List<BackgroundPillar> pillarList = null;
        private List<BackgroundPillar> killList = null;
        private Random random;
        private int lowestSpawn = 0;
        private int highestSpawn = 0;
        private int spawnRate = 0;
        private int deadTime = 0;
        private bool randomDirection = false;
        private bool randomSpeed = false;
        private List<string> fileNameList = null;
        private uint timer = 0;
        private SpriteBatch spriteBatch;
        private Soul game;

        public PillarBatch(SpriteBatch spriteBatch, Soul game, int lowestSpawn, int highestSpawn, int deadTime, float direction, bool randomDirection, bool randomSpeed, float layer)
        {
            this.randomSpeed = randomSpeed;
            this.fileNameList = new List<string>();
            CreateFileNameList();
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.layer = layer;
            this.scrollSpeed = direction;
            this.randomDirection = randomDirection;
            this.pillarList = new List<BackgroundPillar>();
            this.killList = new List<BackgroundPillar>();
            this.random = new Random();
            this.lowestSpawn = lowestSpawn;
            this.highestSpawn = highestSpawn;
            SetNewSpawnRate();
            this.deadTime = deadTime;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (uint)gameTime.ElapsedGameTime.Milliseconds;
            if (spawnRate <= timer)
            {
                AddPillar();
                SetNewSpawnRate();
                timer = 0;
            }

            if (pillarList.Count > 0)
            {
                for (int i = 0; i < pillarList.Count; i++)
                {
                    pillarList[i].Update(gameTime);
                    if (checkDeadStatus(pillarList[i].Position.X, pillarList[i].ScrollSpeed, pillarList[i].SpriteWidth))
                    {
                        killList.Add(pillarList[i]);
                        pillarList.RemoveAt(i);
                        i--;
                    }

                }
            }
        }

        public override void Draw()
        {
            if (pillarList.Count > 0)
            {
                foreach (BackgroundPillar bg in pillarList)
                {
                    bg.Draw();
                }
            }
        }

        private void SetNewSpawnRate()
        {
            spawnRate = random.Next(lowestSpawn, highestSpawn);
        }

        private void AddPillar()
        {
            BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, getRandomFileName(), 0, ScrollDirection(), layer);
            pillarList.Add(bg);
        }

        private string getRandomFileName()
        {
            return fileNameList[random.Next(0, fileNameList.Count - 1)];
        }

        private float ScrollDirection()
        {
            float newDirection = scrollSpeed;

            if (randomDirection == true)
            {
                int results = random.Next(1, 3);
                if (results == 1)
                {
                    newDirection = scrollSpeed * -1.0f;
                }
            }

            if (randomSpeed == true)
            {
                if (newDirection > 1f)
                {
                    int value = random.Next(1, (int)newDirection + 1);
                    newDirection = (float)value;
                }
                else if (newDirection < -1f)
                {
                    int value = random.Next(-1, (int)-newDirection - 1);
                    while (value == 0)
                    {
                        value = random.Next(-1, (int)-newDirection - 1);
                    }
                    newDirection = (float)value;
                    
                }
            }
            return newDirection;
        }

        private bool checkDeadStatus(float position, float direction, float width)
        {
            if (direction < 0f)
            {
                if ((position + width) < 0f)
                {
                    return true;
                }
            }
            else if (direction > 0f)
            {
                if (position > game.Window.ClientBounds.Width)
                {
                    return true;
                }
            }
            return false;
        }

        public override void DrawNormalMap()
        {
            if (pillarList.Count > 0)
            {
                foreach (BackgroundPillar bg in pillarList)
                {
                    bg.DrawNormalMap();
                }
            }
        }

        private void CreateFileNameList()
        {
            fileNameList.Add(Constants.PILLAR_1);
            fileNameList.Add(Constants.PILLAR_2);
            fileNameList.Add(Constants.PILLAR_3);
            fileNameList.Add(Constants.PILLAR_4);
            fileNameList.Add(Constants.PILLAR_5);
            fileNameList.Add(Constants.PILLAR_6);
            fileNameList.Add(Constants.PILLAR_7);
            fileNameList.Add(Constants.PILLAR_8);
            fileNameList.Add(Constants.PILLAR_9);
            fileNameList.Add(Constants.PILLAR_10);
            fileNameList.Add(Constants.PILLAR_11);
            fileNameList.Add(Constants.PILLAR_12);
            fileNameList.Add(Constants.PILLAR_13);
            fileNameList.Add(Constants.PILLAR_14);
            fileNameList.Add(Constants.PILLAR_15);
        }

    }
}
