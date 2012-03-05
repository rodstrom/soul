using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class FadeInOut
    {
        private Sprite sprite;
        private Rectangle screenSize;
        private bool fadeIn = false;
        private bool fadeInDone = false;
        private bool fadeOut = false;
        private bool fadeOutDone = false;
        private int alphaValue = 1;
        private int fadeIncrement = 3;
        private int maxAlpha = 0;

        public FadeInOut(SpriteBatch spriteBatch, Soul game)
        {
            sprite = new Sprite(spriteBatch, game, Constants.BLACK);
            screenSize = new Rectangle(0, 0, Constants.RESOLUTION_VIRTUAL_WIDTH, Constants.RESOLUTION_VIRTUAL_HEIGHT);
            maxAlpha = 254;
            alphaValue = 254;
        }

        public FadeInOut(SpriteBatch spriteBatch, Soul game, int maxAlpha)
        {
            sprite = new Sprite(spriteBatch, game, Constants.BLACK);
            screenSize = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            this.maxAlpha = maxAlpha;
            alphaValue = 254;
        }

        public void Update(GameTime gameTime)
        {
            if (fadeOut == true)
            {
                alphaValue += fadeIncrement;
                if (alphaValue >= maxAlpha)
                {
                    alphaValue = maxAlpha;
                    fadeOut = false;
                    fadeOutDone = true;
                }
            }

            if (fadeIn == true)
            {
                alphaValue -= fadeIncrement;
                if (alphaValue <= 0)
                {
                    fadeIn = false;
                    fadeInDone = true;
                    alphaValue = 0;
                }
            }
        }

        public void Draw()
        {
            if (IsFading == true || fadeOutDone == true)
            {
                sprite.Draw(new Vector2(0.0f, 0.0f), screenSize, new Color(255, 255, 255, alphaValue), 0.0f, new Vector2(0.0f, 0.0f), 1.0f, SpriteEffects.None, 0.0f);
            }
        }

        public void FadeIn()
        {
            /*if (IsFading == false && fadeInDone == false)
            {
                fadeIn = true;
                alphaValue = maxAlpha - 1;
            }*/
            fadeIn = true;
            if (fadeOut == true)
            {
                fadeOut = false;
            }
            /*else
            {
                alphaValue = 254;
            }*/
        }

        public void FadeOut()
        {
            /*if (IsFading == false && fadeOutDone == false)
            {
                fadeOut = true;
                alphaValue = 1;
            }*/

            if (fadeOutDone == false)
            {
                fadeOut = true;
                if (fadeIn == true)
                {
                    fadeIn = false;
                }
                /*else
                {
                    alphaValue = 1;
                }*/
            }
        }

        public bool FadeInDone { get { return fadeInDone; } }
        public bool FadeOutDone { get { return fadeOutDone; } }

        public bool IsFading
        {
            get
            {
                if (fadeIn == true || fadeOut == true)
                {
                    return true;
                }
                return false;
            }
        }

        public void Reset()
        {
            fadeOutDone = false;
            fadeInDone = false;
        }
    }
}