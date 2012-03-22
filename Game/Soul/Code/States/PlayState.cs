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
        private FadeInOut secondFade;
        private EntityManager entityManager;
        private LevelManager levelManager = null;
        private Player player;
        private Sprite cleansed = null;
        private Vector2 cleansedOffset = Vector2.Zero;
        private double timer = 0.0;
        private bool levelComplete = false;
        private string returnData = "";
        private int alpha = 0;
        private int alphaScaler = 5;

        public PlayState(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, string id) : base(spriteBatch, game, audioManager, controls, id) { }

        public override void initialize(string data)
        {
            this.alpha = 0;
            this.returnData = "";
            this.timer = 0.0;
            this.cleansed = new Sprite(spriteBatch, game, Constants.GUI_LEVEL_CLEANSED);
            cleansedOffset = cleansed.Dimension * 0.5f;
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

            levelManager.setLevel(data);
            levelComplete = false;
            secondFade = new FadeInOut(spriteBatch, game);
            secondFade.Reset();
            secondFade.alphaValue = 1;
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

            if (alpha >= 255)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }

            if (value == 1 && fade.IsFading == false)
            {
                fade.FadeOut();
            }

            if (fade.FadeOutDone == true && levelComplete == false)
            {
                changeState = true;
            }
            else if (fade.FadeOutDone == true && levelComplete == true)
            {
                LevelComplete();
            }
            else if (fade.IsFading == true)
            {
                fade.Update(gameTime);
            }

            if (entityManager.IsPlayerDead() == true)
            {
                fade.FadeOut();
            }

            if (alpha > 0)
                secondFade.Update(gameTime);

            levelComplete = levelManager.LevelComplete;

            if (levelComplete == true)
            {
                returnData = levelManager.CurrentLevelID;
            }

            

            return changeState;
        }

        public override void Draw(GameTime gameTime)
        {
            levelManager.Draw(gameTime);
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            fade.Draw();
            if (alpha > 0)
            {
                cleansed.Draw(new Vector2((float)game.Window.ClientBounds.Width * 0.5f, (float)game.Window.ClientBounds.Height * 0.5f), new Color(alpha, alpha, alpha, alpha), 0f, cleansedOffset, 1f, SpriteEffects.None, 0f);
                secondFade.Draw();
            }
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

        private void LevelComplete()
        {
            if (alpha < 255)
            {
                alpha += alphaScaler;
                if (alpha > 255)
                    alpha = 255;
            }

            if (timer >= 2000)
                secondFade.FadeOut();

            if (secondFade.FadeOutDone == true)
            {
                changeState = true;
            }
        }
    }
}
