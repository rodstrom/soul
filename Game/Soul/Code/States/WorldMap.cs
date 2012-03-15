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
            mapManager.addBrainMap(mapMarker, 0);
            mapMarker = new BrainMapMarker(spriteBatch, game, controls, position, "BrainMap\\level02", "level02", saveData.LevelStatus("level02"));
            mapManager.addBrainMap(mapMarker, 0);
            mapMarker = new BrainMapMarker(spriteBatch, game, controls, position, "BrainMap\\level06", "level06", saveData.LevelStatus("level06"));
            mapManager.addBrainMap(mapMarker, 1);
            mapMarker = new BrainMapMarker(spriteBatch, game, controls, position, "BrainMap\\level03", "level03", saveData.LevelStatus("level03"));
            mapManager.addBrainMap(mapMarker, 1);
            mapMarker = new BrainMapMarker(spriteBatch, game, controls, position, "BrainMap\\level05", "level05", saveData.LevelStatus("level05"));
            mapManager.addBrainMap(mapMarker, 2);
            mapMarker = new BrainMapMarker(spriteBatch, game, controls, position, "BrainMap\\level04", "level04", saveData.LevelStatus("level04"));
            mapManager.addBrainMap(mapMarker, 2);
            audio.playMusic("map_music");
            mapManager.initialize();
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

            /*if (controls.Pause == true)
            {
                nextState = "MenuState";
                fadeInOut.FadeOut();
            }*/

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
