using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul.Manager
{
    class BackgroundManager
    {
        private SpriteBatch spriteBatch = null;
        private Soul game = null;
        private List<Background> backgrounds = null;
        private List<Background> killList = null;
        private List<BackgroundData> queueList = null;
        private uint timer = 0;

        public BackgroundManager(SpriteBatch spriteBatch, Soul game)
        {
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.backgrounds = new List<Background>();
            this.killList = new List<Background>();
            this.queueList = new List<BackgroundData>();
        }

        public void addBackground(Background background)
        {
            backgrounds.Add(background);
        }

        public void addToQueue(BackgroundData bgData)
        {
            queueList.Add(bgData);
        }


        public void Update(GameTime gameTime)
        {
            timer += (uint)gameTime.ElapsedGameTime.Milliseconds;
            checkQueueList();
            for (int i = 0; i < backgrounds.Count; i++ )
            {
                if (backgrounds[i].Dead == true)
                {
                    killList.Add(backgrounds[i]);
                    backgrounds.RemoveAt(i);
                    i--;
                    continue;
                }
                backgrounds[i].Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach (Background background in backgrounds)
            {
                background.Draw();
            }
        }

        public void DrawNormalMap()
        {
            foreach (Background background in backgrounds)
            {
                background.DrawNormalMap();
            }
        }

        private void checkQueueList()
        {
            if (queueList.Count > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (queueList[i].SpawnTime >= timer)
                    {
                        if (queueList[i].Type == "Scrolling")
                        {
                            ScrollingBackground bg = new ScrollingBackground(spriteBatch, game, queueList[i].Filename, "bg" + i.ToString(), queueList[i].SpawnTime, queueList[i].DeleteTime, queueList[i].Direction, queueList[i].Layer);
                            addBackground(bg);
                        }
                        else if (queueList[i].Type == "Pillar")
                        {
                            BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, getPillarFileName(queueList[i].Filename), queueList[i].SpawnTime, queueList[i].Direction, queueList[i].Layer);
                            addBackground(bg);
                        }
                        else if (queueList[i].Type == "Batch")
                        {
                            PillarBatch bg = new PillarBatch(spriteBatch, game, queueList[i].LowestSpawnRate, queueList[i].HighestSpawnRate, (int)queueList[i].DeleteTime, queueList[i].Direction, queueList[i].RandomDirection, queueList[i].RandomSpeed, queueList[i].Layer);
                            addBackground(bg);
                        }
                        queueList.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < queueList.Count; i++)
                {
                    if (queueList[i].SpawnTime >= timer)
                    {
                        if (queueList[i].Type == "Scrolling")
                        {
                            ScrollingBackground bg = new ScrollingBackground(spriteBatch, game, queueList[i].Filename, "bg" + i.ToString(), queueList[i].SpawnTime, queueList[i].DeleteTime, queueList[i].Direction, queueList[i].Layer);
                            addBackground(bg);
                        }
                        else if (queueList[i].Type == "Pillar")
                        {
                            BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, getPillarFileName(queueList[i].Filename), queueList[i].SpawnTime, queueList[i].Direction, queueList[i].Layer);
                            addBackground(bg);
                        }
                        queueList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public int ListSize { get { return backgrounds.Count; } }

        public string getPillarFileName(string filename)
        {
            if (filename == "PILLAR_1")
            {
                return Constants.PILLAR_1;
            }
            else if (filename == "PILLAR_2")
            {
                return Constants.PILLAR_2;
            }
            else if (filename == "PILLAR_3")
            {
                return Constants.PILLAR_3;
            }
            else if (filename == "PILLAR_4")
            {
                return Constants.PILLAR_4;
            }
            else if (filename == "PILLAR_5")
            {
                return Constants.PILLAR_5;
            }
            else if (filename == "PILLAR_6")
            {
                return Constants.PILLAR_6;
            }
            else if (filename == "PILLAR_7")
            {
                return Constants.PILLAR_7;
            }
            else if (filename == "PILLAR_8")
            {
                return Constants.PILLAR_8;
            }
            else if (filename == "PILLAR_9")
            {
                return Constants.PILLAR_9;
            }
            else if (filename == "PILLAR_10")
            {
                return Constants.PILLAR_10;
            }
            else if (filename == "PILLAR_11")
            {
                return Constants.PILLAR_11;
            }
            else if (filename == "PILLAR_12")
            {
                return Constants.PILLAR_12;
            }
            else if (filename == "PILLAR_13")
            {
                return Constants.PILLAR_13;
            }
            else if (filename == "PILLAR_14")
            {
                return Constants.PILLAR_14;
            }
            else if (filename == "PILLAR_15")
            {
                return Constants.PILLAR_15;
            }
            return "";
        }
    }
}
