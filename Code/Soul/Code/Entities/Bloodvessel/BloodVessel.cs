using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class BloodVessel : Entity
    {
        public delegate void PowerupReleaseHandle(Entity entity);

        protected EntityManager entityManager;
        protected bool moveRight = false;
        protected bool moveLeft = false;
        protected bool moveUp = false;
        protected bool moveDown = false;
        public event PowerupReleaseHandle onDie = null;
        protected float rotationValue = 0.05f;
        protected HitFX hitFx = null;

         public BloodVessel(SpriteBatch spriteBatch, Soul game, EntityManager entityManager, Vector2 dimension, EntityType entityType, string alias, string filename)
            : base(spriteBatch, game, filename, dimension, alias, entityType)
        {
            this.entityManager = entityManager;
            this.animation.MaxFrames = 0;
            hitFx = new HitFX(game);
        }

        public override void Draw()
        {
            if (hitFx.IsHit == true)
            {
                Rectangle rect = new Rectangle(animation.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
                sprite.Draw(position, rect, Color.White, rotation, offset, scale, SpriteEffects.None, layer, hitFx.Effect);
            }
            else
            {
                base.Draw();
            }
        }

        public override void Update(GameTime gameTime)
        {
            hitFx.Update();
            rotation += rotationValue;
            Move(gameTime);
            animation.Animate(gameTime);
            base.Update(gameTime);
        }

        #region move
        public void Move(GameTime gameTime)
        {
            if (moveLeft == true)
            {
                velocity.X -= acceleration.X;
                if (velocity.X < -maxVelocity.X)
                {
                    velocity.X = -maxVelocity.X;
                }
            }
            else if (moveRight == true)
            {
                velocity.X += acceleration.X;
                if (velocity.X > maxVelocity.X)
                {
                    velocity.X = maxVelocity.X;
                }
            }

            if (moveUp == true)
            {
                velocity.Y -= acceleration.Y;
                if (velocity.Y < -maxVelocity.Y)
                {
                    velocity.Y = -maxVelocity.Y;
                }
            }
            else if (moveDown == true)
            {
                velocity.Y += acceleration.Y;
                if (velocity.Y > maxVelocity.Y)
                {
                    velocity.Y = maxVelocity.Y;
                }
            }
            position += velocity;
            

            if (position.X > screenBoundaries.Width)
            {
                killMe = true;
            }
        }
        #endregion move
        public override void onCollision(Entity entity)
        {
            if (entity.Type == EntityType.PLAYER_BULLET || entity.Type == EntityType.DARK_WHISPER || entity.Type == EntityType.DARK_WHISPER_SPIKE)
            {
                health -= entity.getDamage();
                hitFx.Hit();
                if (health <= 0)
                {
                    if (onDie != null)
                    {
                        onDie(this);
                    }
                    killMe = true;
                }
            }
            else if (entity.Type == EntityType.PLAYER)
            {
                killMe = true;
            }
        }

        public override int getDamage()
        {
            return damage;
        }

        public override void takeDamage(int value)
        {
            health -= value;
            if (health <= 0)
            {
                killMe = true;
            }
        }
    }
}
