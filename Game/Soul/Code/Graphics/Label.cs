using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class Label : Widget
    {
        private SpriteFont spriteFont = null;
        private SpriteBatch spriteBatch = null;
        private Sprite leftArrow = null;
        private Sprite rightArrow = null;
        private Vector2 leftArrowOffset = Vector2.Zero;
        private Vector2 rightArrowOffset = Vector2.Zero;

        public Label(SpriteBatch spriteBatch, Soul game, Vector2 position, string id, string text)
            : base(id)
        {
            this.spriteBatch = spriteBatch;
            spriteFont = game.Content.Load<SpriteFont>(Constants.GUI_FONT);
            leftArrow = new Sprite(spriteBatch, game, Constants.GUI_ARROW_LEFT);
            rightArrow = new Sprite(spriteBatch, game, Constants.GUI_ARROW_RIGHT);
            leftArrowOffset = leftArrow.Dimension * 0.5f;
            rightArrowOffset = rightArrow.Dimension * 0.5f;
            this.selection = text;
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            Vector2 FontOrigin = spriteFont.MeasureString(selection) / 2;
            spriteBatch.DrawString(spriteFont, selection, position, new Color(alpha, alpha, alpha, alpha), 0f, FontOrigin, 1f, SpriteEffects.None, 0f);
            leftArrow.Draw(new Vector2(position.X - 100f, position.Y), new Color(alpha, alpha, alpha, alpha), 0f, leftArrowOffset, 1f, SpriteEffects.None, 0f);
            rightArrow.Draw(new Vector2(position.X + 100f, position.Y), new Color(alpha, alpha, alpha, alpha), 0f, rightArrowOffset, 1f, SpriteEffects.None, 0f);
        }


    }
}
