using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class TutorialSprite : TutorialWidget
    {
        private Sprite key = null;
        private Vector2 keyOffset = Vector2.Zero;
        private float rotation = 0f;

        public TutorialSprite(SpriteBatch spriteBatch, Soul game, string id, string keyFilename, string frameFilename, Vector2 positionOffset, float rotation = 0f)
            : base(spriteBatch, game, id, frameFilename, positionOffset) 
        {
            this.key = new Sprite(spriteBatch, game, keyFilename);
            this.keyOffset = key.Dimension * 0.5f;
            this.rotation = rotation;
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
            key.Draw(position - positionOffset, new Color(alpha, alpha, alpha, alpha), rotation, keyOffset, 1f, SpriteEffects.None, 0f);

        }
    }
}
