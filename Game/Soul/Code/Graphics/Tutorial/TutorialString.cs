using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class TutorialString : TutorialWidget
    {
        private string key = "";
        private Vector2 keyOffset = Vector2.Zero;
        private SpriteFont spriteFont = null;
        
        public TutorialString(SpriteBatch spriteBatch, Soul game, string id, SpriteFont spriteFont, string key, string frameFilename, Vector2 positionOffset)
            : base(spriteBatch, game, id, frameFilename, positionOffset) 
        {
            this.spriteFont = spriteFont;
            this.key = key;
            this.keyOffset = spriteFont.MeasureString(key) / 2;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition)
        {
            this.position = playerPosition;
            glow.Update();
            base.Update(gameTime, playerPosition);
        }

        public override void Draw()
        {
            frame.Draw(position - positionOffset, new Color(alpha, alpha, alpha, alpha), 0f, frameOffset, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, key, position - positionOffset, new Color(alpha, alpha, alpha, alpha), 0f, keyOffset, 1f, SpriteEffects.None, 0f);
        }
    }
}
