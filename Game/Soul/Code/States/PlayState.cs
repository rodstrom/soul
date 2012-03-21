using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class PlayState : State
    {
        private FadeInOut fade;
        private EntityManager entityManager;
        private LevelManager levelManager = null;
        private Player player;
        private bool levelComplete = false;
        private string returnData = "";

        public PlayState(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, string id) : base(spriteBatch, game, audioManager, controls, id) { }

        public override void initialize(string data)
        {
            levelManager = new LevelManager();
            entityManager = new EntityManager(spriteBatch, game, audio, levelManager); 
            
            player = new Player(spriteBatch, game, audio, "player", entityManager, controls);
            Vector2 newPlayerPos = new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH - 200f, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f);
            player.Position = newPlayerPos;
            entityManager.addEntity(player);
            entityManager.initialize();

            Level level = new Level(spriteBatch, game, audio, entityManager, player, controls, Constants.LEVEL01, "level01");
            levelManager.AddLevel(level);
            level = new Level(spriteBatch, game, audio, entityManager, player, controls, Constants.LEVEL02, "level02");
            levelManager.AddLevel(level);
            level = new Level(spriteBatch, game, audio, entityManager, player, controls, Constants.LEVEL03, "level03");
            levelManager.AddLevel(level);
            level = new Level(spriteBatch, game, audio, entityManager, player, controls, Constants.LEVEL04, "level04");
            levelManager.AddLevel(level);
            level = new Level(spriteBatch, game, audio, entityManager, player, controls, Constants.LEVEL05, "level05");
            levelManager.AddLevel(level);
            level = new Level(spriteBatch, game, audio, entityManager, player, controls, Constants.LEVEL06, "level06");
            levelManager.AddLevel(level);

            levelManager.setLevel(data);
            levelComplete = false;
            fade = new FadeInOut(spriteBatch, game);
            fade.Reset();
            fade.FadeIn();
            audio.playMusic("main_music");
        }

        public override void shutdown()
        {
            audio.stopMusic();
            entityManager.clearEntities();
            changeState = false;
        }

        public override bool Update(GameTime gameTime)
        {
            int value;
            value = levelManager.Update(gameTime);
            if (value == 1 && fade.IsFading == false)
            {
                fade.FadeOut();
            }

            if (fade.FadeOutDone == true)
            {
                changeState = true;
            }
            else if (fade.IsFading == true)
            {
                fade.Update(gameTime);
            }

            if (entityManager.IsPlayerDead() == true)
            {
                fade.FadeOut();
            }

            if (levelComplete == true)
            {
                returnData = levelManager.CurrentLevelID;
            }

            levelComplete = levelManager.LevelComplete;


            return changeState;
        }

        public override void Draw(GameTime gameTime)
        {
            levelManager.Draw(gameTime);
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            fade.Draw();
            spriteBatch.End();
        }

        public override string getNextState()
        {
            return "WorldMapState";
        }

        public override string StateData()
        {
            return returnData;
        }
    }
}
