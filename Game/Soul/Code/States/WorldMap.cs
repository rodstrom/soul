using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class WorldMapState : State
    {
        private BrainMapManager mapManager = null;
        private FadeInOut fadeInOut = null;
        private SaveData saveData = null;
        private TutorialSprite leftArrow = null;
        private TutorialSprite rightArrow = null;
        private TutorialSprite enterButton = null;
        private bool tutorial = false;

        public WorldMapState(SpriteBatch spriteBatch, Soul game, AudioManager audio, InputManager controls, string id) : base(spriteBatch, game, audio, controls, id)
        {
            fadeInOut = new FadeInOut(spriteBatch, game);
        }

        public override void initialize(string data)
        {
            saveData = new SaveData(Constants.SAVE_DATA_FILENAME);
            saveData.LoadDataFile();
            changeState = false;
            if (data != "")
            {
                saveData.SaveDataFile(data, true);
            }

            Vector2 position = new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f);
            mapManager = new BrainMapManager(spriteBatch, game, audio, controls, position);
            BrainMapMarker mapMarker = new BrainMapMarker(spriteBatch, game, controls, position, "BrainMap\\level01", "level01", saveData.LevelStatus("level01"));
            mapManager.addBrainMap(mapMarker);
            mapMarker = new BrainMapMarker(spriteBatch, game, controls, position, "BrainMap\\level02", "level02", saveData.LevelStatus("level02"));
            mapManager.addBrainMap(mapMarker);
            mapMarker = new BrainMapMarker(spriteBatch, game, controls, position, "BrainMap\\level03", "level03", saveData.LevelStatus("level03"));
            mapManager.addBrainMap(mapMarker);

            audio.playMusic("map_music");
            mapManager.initialize();
            this.tutorial = bool.Parse(game.config.getValue("General", "Tutorial"));
            leftArrow = new TutorialSprite(spriteBatch, game, "arrow_left", Constants.TUTORIAL_ARROW, Constants.TUTORIAL_BUTTON_FRAME, Vector2.Zero, -(float)Math.PI * 0.5f);
            leftArrow.position = new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f - 450f, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f);
            rightArrow = new TutorialSprite(spriteBatch, game, "arrow_right", Constants.TUTORIAL_ARROW, Constants.TUTORIAL_BUTTON_FRAME, Vector2.Zero, (float)Math.PI * 0.5f);
            rightArrow.position = new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 450f, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f);
            enterButton = new TutorialSprite(spriteBatch, game, "enter_button", Constants.TUTORIAL_BUTTON_ENTER, Constants.TUTORIAL_BUTTON_FRAME_ENTER, Vector2.Zero);
            enterButton.position = new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f - 30f, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 210f);

            if (tutorial == true)
            {
                leftArrow.FadeIn();
                rightArrow.FadeIn();
                enterButton.FadeIn();
            }

            fadeInOut.Reset();
            fadeInOut.FadeIn();
            
        }

        public override void shutdown()
        {
            changeState = false;
        }

        public override bool Update(GameTime gameTime)
        {
            int value = mapManager.Update(gameTime);

            if (tutorial == true)
            {
                if (controls.MoveLeftOnce == true)
                {
                    leftArrow.FadeOut();
                    rightArrow.FadeOut();
                }
                else if (controls.MoveRightOnce == true)
                {
                    leftArrow.FadeOut();
                    rightArrow.FadeOut();
                }

                if (controls.ShootingOnce == true)
                {
                    enterButton.FadeOut();
                }

                leftArrow.Update(gameTime);
                rightArrow.Update(gameTime);
                enterButton.Update(gameTime);
                if (leftArrow.IsAlphaZero == true && rightArrow.IsAlphaZero == true && enterButton.IsAlphaZero == true)
                {
                    tutorial = false;
                }
            }


            if (value == 1)
            {
                nextState = "PlayState";
                fadeInOut.FadeOut();
            }
            else if (value == -1)
            {
                nextState = "MenuState";
                fadeInOut.FadeOut();
            }

            if (fadeInOut.FadeOutDone == true)
            {
                changeState = true;
                return changeState;
            }
            else
            {
                fadeInOut.Update(gameTime);
            }
            return changeState;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            mapManager.Draw();
            if (tutorial == true)
            {
                leftArrow.Draw();
                rightArrow.Draw();
                enterButton.Draw();
            }
            fadeInOut.Draw();
            spriteBatch.End();
        }

        public override string getNextState()
        {
            return nextState;
        }

        public override string StateData()
        {
            return mapManager.CurrentLevel;
        }
    }
}
