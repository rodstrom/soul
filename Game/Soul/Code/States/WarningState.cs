using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class WarningState : State
    {
        Sprite bg;
        FadeInOut fade;
        int timer = 0;

        public WarningState(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, string id) : base(spriteBatch, game, audioManager, controls, id) { }

        public override void initialize(string data)
        {
            bg = new Sprite(spriteBatch, game, Constants.WARNING_IMAGE_FILENAME);
            fade = new FadeInOut(spriteBatch, game);
            //audio.playMusic(Constants.AUDIO_INTRO);
            //audio.playSound("intro_startup");
            fade.FadeIn();
        }

        public override void shutdown()
        {
            audio.stopMusic();
            bg.Dispose();
            timer = 0;
            changeState = false;
        }

        public override string getNextState()
        {
            return "MenuState";
        }

        public override bool Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (controls.Pause || controls.ShootingOnce)
            {
                changeState = true;
            }
            else
            {
                if (fade.FadeOutDone)
                {
                    changeState = true;
                }
                else if (fade.IsFading)
                {
                    fade.Update(gameTime);
                }

                if (timer >= 10000 && !fade.IsFading)
                {
                    fade.FadeOut();
                }
            }

            return changeState;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            bg.Draw(new Vector2(0f, 0f), new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, new Vector2(0f, 0f), 1.0f, SpriteEffects.None, 0f);
            fade.Draw();
            spriteBatch.End();
        }

        public override string StateData()
        {
            return "";
        }
    }
}
