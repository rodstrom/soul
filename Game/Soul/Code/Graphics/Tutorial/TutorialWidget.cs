using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    abstract class TutorialWidget
    {
        protected SpriteBatch spriteBatch = null;
        protected Soul game = null;
        protected Sprite frame = null;
        protected Vector2 frameOffset = Vector2.Zero;
        public Vector2 position = Vector2.Zero;
        protected Vector2 positionOffset = Vector2.Zero;
        protected int alpha = 0;
        protected int alphaScaler = 10;
        protected string id = "";
        protected bool fadeIn = false;
        protected bool fadeOut = false;
        protected GlowFX glow = null;
        public bool done = false;

        public TutorialWidget(SpriteBatch spriteBatch, Soul game, string id, string filename, Vector2 positionOffset)
        {
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.frame = new Sprite(spriteBatch, game, filename);
            this.frameOffset = frame.Dimension * 0.5f;
            this.positionOffset = positionOffset;
            this.glow = new GlowFX(game, 0.01f, 0.4f, 0.9f);
            this.id = id;
        }

        public virtual void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (fadeIn == true || fadeOut == true)
            {
                Fading();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (fadeIn == true || fadeOut == true)
            {
                Fading();
            }
        }

        public abstract void Draw();

        public string ID { get { return id; } }

        private void Fading()
        {
            if (fadeIn == true)
            {
                alpha += alphaScaler;
                if (alpha >= 255)
                {
                    fadeIn = false;
                    alpha = 255;
                }
            }
            else if (fadeOut == true)
            {
                alpha -= alphaScaler;
                if (alpha <= 0)
                {
                    fadeOut = false;
                    alpha = 0;
                    glow.stop = true;
                    done = true;
                }
            }
        }

        public void FadeIn()
        {
            if (fadeOut == true)
            {
                fadeOut = false;
            }
            fadeIn = true;
        }

        public void FadeOut()
        {
            if (fadeIn == true)
            {
                fadeIn = false;
            }
            fadeOut = true;
        }

        public bool IsAlphaZero
        {
            get
            {
                if (alpha <= 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsAlphaMax
        {
            get
            {
                if (alpha >= 255)
                {
                    return true;
                }
                return false;
            }
        }



    }
}
