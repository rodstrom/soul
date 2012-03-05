using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class Memory : Background
    {
        private Sprite sprite = null;
        private Sprite normal = null;
        private uint startTime = 0;
        private uint stopTime = 0;
        private int alpha = 0;
        private int alphaScaler = 2;
        private bool fadeIn = false;
        private bool fadeOut = false;


        public Memory(SpriteBatch spriteBatch, Soul game, string filename, float scrollSpeed, uint startTime, uint stopTime, float layer)
        {
            sprite = new Sprite(spriteBatch, game, filename);
            filename += "_depth";
            normal = new Sprite(spriteBatch, game, filename);
            this.scrollSpeed = scrollSpeed;
            this.startTime = startTime;
            this.stopTime = stopTime;
            this.layer = layer;
        }

        public override void Update(GameTime gameTime)
        {
            if (fadeIn == true || fadeOut == true)
            {
                Fading();
            }


        }

        public override void Draw()
        {
            sprite.Draw(Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
        }

        public override void DrawNormalMap()
        {
            normal.Draw(Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
        }

        private void Fading()
        {
            if (fadeIn == true)
            {
                alpha += alphaScaler;
                if (alpha >= 255)
                {
                    alpha = 255;
                    fadeIn = false;
                }
            }
            else if (fadeOut == true)
            {
                alpha -= alphaScaler;
                if (alpha <= 0)
                {
                    alpha = 0;
                    fadeOut = false;
                }
            }
        }

        private void FadeOut()
        {
            if (fadeIn == true)
            {
                fadeIn = false;
            }
            fadeOut = true;
        }

        private void FadeIn()
        {
            if (fadeOut == true)
            {
                fadeOut = false;
            }
            fadeIn = true;
        }

        public override void initialize()
        {
            FadeIn();
        }


    }
}
