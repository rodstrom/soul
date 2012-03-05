using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class BackgroundPillar : Background
    {
        
        private Sprite sprite = null;
        private uint timer = 0;
        private uint appearTime = 0;
        private Rectangle screenSize;

        public BackgroundPillar(SpriteBatch spriteBatch, Soul game, string filename, uint appearTime, float scrollSpeed, float layer)
        {
            sprite = new Sprite(spriteBatch, game, filename);
            this.layer = layer;
            this.scrollSpeed = scrollSpeed;
            if (scrollSpeed > 0.0f)
            {
                position.X = 0.0f - (float)sprite.X;
            }
            else if (scrollSpeed < 0.0f)
            {
                position.X = (float)game.Window.ClientBounds.Width;
            }
            screenSize = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            spriteWidth = (float)sprite.X;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (uint)gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= appearTime && visible == false)
            {
                visible = true;
            }

            if (visible == true)
            {
                position.X += scrollSpeed; 
            }

            if (scrollSpeed > 0.0f)
            {
                if (position.X >= screenSize.Width)
                {
                    this.dead = true;
                }
            }
            else if (scrollSpeed < 0.0f)
            {
                if (position.X + (float)sprite.X <= 0.0f)
                {
                    this.dead = true;
                }
            }
        }

        public override void Draw()
        {
            if (visible == true)
            {
                sprite.Draw(position, Color.White, 0f, new Vector2(0f), 1f, SpriteEffects.None, layer);
            }
        }

    }
}
