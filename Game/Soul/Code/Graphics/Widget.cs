using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    abstract class Widget
    {
        public Vector2 position = Vector2.Zero;
        public bool hasFocus = false;
        protected string id;
        protected string selection = null;
        protected int alpha = 0;
        protected int alphaScaler = 5;
        protected bool fadeIn = false;
        protected bool fadeOut = false;
        protected bool IsSelection = false;

        public Widget(string id)
        {
            this.id = id;
        }

        public bool Focus
        {
            get { return hasFocus; }
            set { hasFocus = value; }
        }

        public virtual bool HandleInput()
        {
            return false;
        }

        public string ID { get { return id; } }

        public virtual void Update(GameTime gameTime)
        {
            if (fadeIn == true || fadeOut == true)
            {
                Fading();
            }
        }

        public abstract void Draw();

        public string Selection { set { selection = value; } }

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
    }
}
