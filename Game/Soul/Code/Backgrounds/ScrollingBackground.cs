using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class ScrollingBackground : Background
    {
        private string id = "";
        private uint startTime = 0;
        private uint deleteTime = 0;
        private uint timer = 0;
        private bool wait = false;
        private Sprite sprite;
        private Sprite normal; 

        public ScrollingBackground(SpriteBatch spriteBatch, Soul game, string filename, string id, uint startTime, uint deleteTime, float scrollSpeed, float layer, bool persistScroll)
        {
            this.startTime = startTime;
            if (deleteTime == 0)
            {
                this.deleteTime = uint.MaxValue;
            }
            else
            {
                this.deleteTime = deleteTime;
            }
            this.id = id;
            this.layer = layer;
            this.persistScroll = persistScroll;
            sprite = new Sprite(spriteBatch, game, filename);
            filename += "_depth";
            normal = new Sprite(spriteBatch, game, filename);
            if (startTime != 0)
            {
                scanner = new BGScanner(game, new Vector2(sprite.X, sprite.Y), scrollSpeed, false);
                wait = true;
                visible = false;
            }
            else
            {
                scanner = new BGScanner(game, new Vector2(sprite.X, sprite.Y), scrollSpeed, true);
            }
            scanner.callDead += new BGScanner.CallDeadEvent(nowDead);
        }

        public override void Update(GameTime gameTime)
        {
            timer += (uint)gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= startTime && wait == true)
            {
                visible = true;
                scanner.show = true;
                wait = false;
            }

            if (timer >= deleteTime)
            {
                scanner.show = false;
            }

            if (visible == true)
            {
                scanner.Update(gameTime);
            }
        }

        public override void Draw()
        {
            if (visible == true)
            {
                sprite.Draw(scanner.Position2, scanner.Rect2, Color.White, 0f, new Vector2(0f), 1f, SpriteEffects.None, layer);
                sprite.Draw(scanner.Position1, scanner.Rect1, Color.White, 0f, new Vector2(0f), 1f, SpriteEffects.None, layer);
            }
        }

        public override void DrawNormalMap()
        {
            if (visible == true)
            {
                normal.Draw(scanner.Position2, scanner.Rect2, Color.White, 0f, new Vector2(0f), 1f, SpriteEffects.None, layer);
                normal.Draw(scanner.Position1, scanner.Rect1, Color.White, 0f, new Vector2(0f), 1f, SpriteEffects.None, layer);
            }
        }

        private void nowDead()
        {
            this.dead = true;
        }
    }
}
