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

        public abstract void Update(GameTime gameTime);
        public abstract void Draw();
    }
}
