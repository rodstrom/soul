﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    abstract class Background : IDisposable
    {
        protected bool dead = false;
        protected bool visible = true;
        protected float layer = 0.0f;
        protected float spriteWidth = 0f;
        protected float scrollSpeed = 0f;
        protected bool persistScroll = false;
        protected Vector2 position = Vector2.Zero;
        protected bool disposed = false;
        protected BGScanner scanner = new BGScanner();

        public Background()
        {
        
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void slowDown()
        {
            if ((scrollSpeed >= 1f || scanner.scrollSpeed >= 1f) && persistScroll)
            {
                scrollSpeed -= 1f;
                scrollSpeed *= 0.5f;
                scrollSpeed += 1f;
                scanner.scrollSpeed -= 1f;
                scanner.scrollSpeed *= 0.5f;
                scanner.scrollSpeed += 1f;
            }
            else
            {
                scrollSpeed = 0f;
                scanner.scrollSpeed = 0f;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing == true)
                {

                }
            }
            disposed = true;
        }

        public virtual void initialize()
        {

        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw();
        public abstract void DrawNormalMap();

        public float Layer { get { return layer; } }
        public bool Visible { get { return visible; } }
        public bool Dead { get { return dead; } }
        public Vector2 Position { get { return position; } }
        public float SpriteWidth { get { return spriteWidth; } }
        public float ScrollSpeed { get { return scrollSpeed; } }
    }
}


