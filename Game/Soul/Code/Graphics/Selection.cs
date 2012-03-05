using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class Selection : Widget
    {
        private Dictionary<string, KeyValuePair<Sprite, Vector2>> selectionList = null;
        private Sprite leftArrow = null;
        private Sprite rightArrow = null;
        private Vector2 leftArrowOffset = Vector2.Zero;
        private Vector2 rightArrowOffset = Vector2.Zero;
        private SpriteBatch spriteBatch = null;
        private Soul game = null;
        

        public Selection(SpriteBatch spriteBatch, Soul game, Vector2 position, string id)
            : base(id)
        {
            this.spriteBatch = spriteBatch;
            this.game = game;
            selectionList = new Dictionary<string, KeyValuePair<Sprite, Vector2>>();
            this.position = position;
            leftArrow = new Sprite(spriteBatch, game, Constants.GUI_ARROW_LEFT);
            rightArrow = new Sprite(spriteBatch, game, Constants.GUI_ARROW_RIGHT);
            leftArrowOffset = leftArrow.Dimension * 0.5f;
            rightArrowOffset = rightArrow.Dimension * 0.5f;
            this.IsSelection = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            selectionList[selection].Key.Draw(position, new Color(alpha, alpha, alpha, alpha), 0f, selectionList[selection].Value, 1f, SpriteEffects.None, 0f);
            leftArrow.Draw(new Vector2(position.X - 100f, position.Y), new Color(alpha, alpha, alpha, alpha), 0f, leftArrowOffset, 1f, SpriteEffects.None, 0f);
            rightArrow.Draw(new Vector2(position.X + 100f, position.Y), new Color(alpha, alpha, alpha, alpha), 0f, rightArrowOffset, 1f, SpriteEffects.None, 0f);
        }

        public void AddSelection(string selection, string filename)
        {
            Sprite sprite = new Sprite(spriteBatch, game, filename);
            Vector2 offset = sprite.Dimension * 0.5f;
            selectionList[selection] = new KeyValuePair<Sprite, Vector2>(sprite, offset);
        }
    }
}
