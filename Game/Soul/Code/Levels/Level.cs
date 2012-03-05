using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class Level
    {
        private string id;
        private bool pause = false;
        private bool quit = false;

        EntityManager entityManager;
        BackgroundManager backgroundManager_back;
        BackgroundManager backgroundManager_front;
        InputManager controls;
        LevelReader levelReader;
        MenuManager menuManager;
        SpriteBatch spriteBatch;
        Soul game;
        int timeStarted = 0;
        bool fulhack = true;

        public Level(SpriteBatch spriteBatch, Soul game, EntityManager entityManager, InputManager controls, string filename, string id)
        {
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.entityManager = entityManager;
            this.id = id;
            this.controls = controls;
            levelReader = new LevelReader(filename, game);
        }

        public void initialize()
        {
            levelReader.Parse();
            entityManager.AddEntityDataList(levelReader.EntityDataList);
            entityManager.initialize();
            CreateBackgrounds();
            menuManager = new MenuManager(controls);
            ImageButton button = new ImageButton(spriteBatch, game, controls, new Vector2((float)game.Window.ClientBounds.Width * 0.5f - 150, (float)game.Window.ClientBounds.Height * 0.5f), "GUI\\button_continue", "continue");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, controls, new Vector2((float)game.Window.ClientBounds.Width * 0.5f + 150, (float)game.Window.ClientBounds.Height * 0.5f), "GUI\\button_quit_pause", "quit");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
        }

        public void shutdown()
        {
            fulhack = true;
        }

        public int Update(GameTime gameTime)
        {
            if (fulhack)
            {
                timeStarted = (int)gameTime.TotalGameTime.TotalMilliseconds;
                fulhack = false;
            }

            if (pause == false)
            {
                backgroundManager_back.Update(gameTime);
                entityManager.Update(gameTime);
                backgroundManager_front.Update(gameTime);
            }
            else
            {
                if (controls.MoveLeftOnce == true)
                {
                    menuManager.decrement();
                }
                else if (controls.MoveRightOnce == true)
                {
                    menuManager.increment();
                }
                menuManager.Update(gameTime);
            }

            if (controls.Pause == true)
            {
                pause = true;
            }

            if (quit == true)
            {
                return 1;
            }
            return 0;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
            backgroundManager_back.Draw(gameTime);
            spriteBatch.End();

            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            entityManager.Draw();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
            backgroundManager_front.Draw(gameTime);
            spriteBatch.End();

            if (pause)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
                menuManager.Draw();
                spriteBatch.End();
            }

            if (bool.Parse(game.config.getValue("Debug", "Timestamp")))
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
                SpriteFont font = game.Content.Load<SpriteFont>("GUI\\Extrafine");
                double time = Math.Round((gameTime.TotalGameTime.TotalMilliseconds - timeStarted + double.Parse(game.config.getValue("Debug", "StartingTime"))) / 1000) ;
                string output = time.ToString();
                spriteBatch.DrawString(font, output, new Vector2(10f), Color.Gray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            }
        }

        private void CreateBackgrounds()
        {
            List<BackgroundData> bgDataBack = levelReader.BackgroundDataListBack;
            backgroundManager_back = new BackgroundManager(spriteBatch, game);
            for (int i = 0; i < bgDataBack.Count; i++)
            {
                if (bgDataBack[i].SpawnTime != 0)
                {
                    backgroundManager_back.addToQueue(bgDataBack[i]);
                }
                else if (bgDataBack[i].Type == "Scrolling")
                {
                    ScrollingBackground bg = new ScrollingBackground(spriteBatch, game, bgDataBack[i].Filename, "bg" + i.ToString(), bgDataBack[i].SpawnTime, bgDataBack[i].DeleteTime, bgDataBack[i].Direction, bgDataBack[i].Layer);
                    backgroundManager_back.addBackground(bg);
                }
                else if (bgDataBack[i].Type == "Pillar")
                {
                    BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, backgroundManager_back.getPillarFileName(bgDataBack[i].Filename), bgDataBack[i].SpawnTime, bgDataBack[i].Direction, bgDataBack[i].Layer);
                    backgroundManager_back.addBackground(bg);
                }
                else if (bgDataBack[i].Type == "Batch")
                {
                    PillarBatch bg = new PillarBatch(spriteBatch, game, bgDataBack[i].LowestSpawnRate, bgDataBack[i].HighestSpawnRate, (int)bgDataBack[i].DeleteTime, bgDataBack[i].Direction, bgDataBack[i].RandomDirection, bgDataBack[i].RandomSpeed, bgDataBack[i].Layer);
                    backgroundManager_back.addBackground(bg);
                }
            }

            List<BackgroundData> bgDataFront = levelReader.BackgroundDataListFront;
            backgroundManager_front = new BackgroundManager(spriteBatch, game);
            for (int i = 0; i < bgDataFront.Count; i++)
            {
                if (bgDataFront[i].SpawnTime != 0)
                {
                    backgroundManager_front.addToQueue(bgDataFront[i]);
                }
                else if (bgDataFront[i].Type == "Scrolling")
                {
                    ScrollingBackground bg = new ScrollingBackground(spriteBatch, game, bgDataFront[i].Filename, "bg" + i.ToString(), bgDataFront[i].SpawnTime, bgDataFront[i].DeleteTime, bgDataFront[i].Direction, bgDataFront[i].Layer);
                    backgroundManager_front.addBackground(bg);
                }
                else if (bgDataFront[i].Type == "Pillar")
                {
                    BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, backgroundManager_front.getPillarFileName(bgDataFront[i].Filename), bgDataFront[i].SpawnTime, bgDataFront[i].Direction, bgDataFront[i].Layer);
                    backgroundManager_front.addBackground(bg);
                }
                else if (bgDataFront[i].Type == "Batch")
                {
                    PillarBatch bg = new PillarBatch(spriteBatch, game, bgDataFront[i].LowestSpawnRate, bgDataFront[i].HighestSpawnRate, (int)bgDataFront[i].DeleteTime, bgDataFront[i].Direction, bgDataFront[i].RandomDirection, bgDataFront[i].RandomSpeed, bgDataFront[i].Layer);
                    backgroundManager_front.addBackground(bg);
                }
            }
        }

        public string ID { get { return id; } }

        private void OnButtonPress(ImageButton button)
        {
            if (button.ID == "continue")
            {
                pause = false;
            }
            else if (button.ID == "quit")
            {
                quit = true;
            }
        }
    }
}
