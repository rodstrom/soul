using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class MenuImage : Widget
    {
        private Sprite image = null;
        private Vector2 offset = Vector2.Zero;

        public MenuImage(SpriteBatch spriteBatch, Soul game, Vector2 position, string filename, string id)
            : base(id)
        {
            image = new Sprite(spriteBatch, game, filename);
            offset = image.Dimension * 0.5f;
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            image.Draw(position, new Color(alpha, alpha, alpha, alpha), 0f, offset, 1f, SpriteEffects.None, 0f);
        }
    }
}
