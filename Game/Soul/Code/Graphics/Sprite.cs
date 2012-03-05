using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class Sprite : DrawableGameComponent
    {
        private Texture2D texture;
        private SpriteBatch spriteBatch;

        public Sprite(SpriteBatch spriteBatch, Soul game, string filename) : base(game)
        {
            this.spriteBatch = spriteBatch;
            texture = Game.Content.Load<Texture2D>(filename);
        }

        public void Draw(Vector2 position, Rectangle rect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layer)
        {
            spriteBatch.Draw(texture, position, rect, color, rotation, origin, scale, spriteEffects, layer);
        }

        public void Draw(Vector2 position, Rectangle rect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layer, Effect effect)
        {
            spriteBatch.End();
            spriteBatch.Begin(0, null, null, null, null, effect, Resolution.getTransformationMatrix());
            spriteBatch.Draw(texture, position, rect, color, rotation, origin, scale, spriteEffects, layer);
            spriteBatch.End();
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
        }


        public void Draw(Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layer)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, spriteEffects, layer);
        }

        public void Draw(Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layer, Effect effect)
        {
            spriteBatch.End();
            spriteBatch.Begin(0, null, null, null, null, effect, Resolution.getTransformationMatrix());
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, spriteEffects, layer);
            spriteBatch.End();
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
        }

        public int X
        {
            get { return texture.Width; }
        }

        public int Y
        {
            get { return texture.Height; }
        }

        public Vector2 Dimension
        {
            get
            {
                return new Vector2((float)texture.Width, (float)texture.Height);
            }
        }
    }
}
