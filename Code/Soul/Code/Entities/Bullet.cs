using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class Bullet : Entity
    {
        private float scalePercentage = 0.03f;
        private bool decrease = true;

        public Bullet(SpriteBatch spriteBatch, Soul game, Vector2 position, Vector2 velocity, string filename, string alias, EntityType type, int damage) : base(spriteBatch, game, filename, new Vector2(20.0f, 20.0f), alias, type)
        {
            this.position = position;
            this.velocity = velocity;
            this.damage = damage;
            this.hitRadius = Constants.BULLET_RADIUS;
        }

        public override void Draw()
        {
            sprite.Draw(position, Color.White, rotation, offset, scale, SpriteEffects.None, layer);
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;

            if (decrease == true)
            {
                scale -= scalePercentage;
                if (scale <= 0.6f)
                {
                    decrease = false;
                }
            }
            else
            {
                scale += scalePercentage;
                if (scale >= 1.0f)
                {
                    decrease = true;
                }
            }
        }



        public override void onCollision(Entity entity)
        {
            return;
        }

        public override int getDamage()
        {
            return damage;
        }

        public override void takeDamage(int value)
        {
            return;
        }

        public Vector2 ActualPosition
        {
            get
            {
                Vector2 newPosition = position;
                newPosition.X -= ((float)sprite.X * 0.5f) * scale;
                newPosition.Y -= ((float)sprite.Y * 0.5f) * scale;
                return newPosition;
            }
        }
    }
}
