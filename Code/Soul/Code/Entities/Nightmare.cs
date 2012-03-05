using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class Nightmare : Entity
    {
        public delegate void PowerupReleaseHandle(Entity entity);
        public event PowerupReleaseHandle onDie = null;
        private EntityManager entityManager;
        private Vector2 directionToPlayer = Vector2.Zero;
        private HitFX hitFx = null;

        public Nightmare(SpriteBatch spriteBatch, Soul game, EntityManager entityManager, string alias)
            : base(spriteBatch, game, Constants.NIGHTMARE_FILENAME, new Vector2(Constants.NIGHTMARE_WIDTH, Constants.NIGHTMARE_HEIGHT), alias, EntityType.NIGHTMARE)
        {
            this.entityManager = entityManager;
            //this.maxVelocity = new Vector2 (Constants.NIGHTMARE_MAX_SPEED, Constants.NIGHTMARE_MAX_SPEED);
            this.acceleration = new Vector2 (Constants.NIGHTMARE_ACCELERATION, Constants.NIGHTMARE_ACCELERATION);
            //this.health = Constants.NIGHTMARE_MAX_HEALTH;
            //this.damage = Constants.NIGHTMARE_DAMAGE;
            //this.animation.MaxFrames = 13;
            hitFx = new HitFX(game);
            //this.hitRadius = Constants.NIGHTMARE_RADIUS;
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
            Move(gameTime);
            animation.Animate(gameTime);
            base.Update(gameTime);
        }

        #region move
        public void Move(GameTime gameTime)
        {
            if (target == Vector2.Zero)
            {
                velocity = Vector2.Zero;
            }
            else
            {
                directionToPlayer = (target - position);
                directionToPlayer.Normalize();
                if (directionToPlayer.X > 0.0f)
                {
                    velocity.X += acceleration.X;
                    if (velocity.X > maxVelocity.X)
                    {
                        velocity.X = maxVelocity.X;
                    }
                }
                else if (directionToPlayer.X < 0.0f)
                {
                    velocity.X -= acceleration.X;
                    if (velocity.X < -maxVelocity.X)
                    {
                        velocity.X = -maxVelocity.X;
                    }
                }

                if (directionToPlayer.Y > 0.0f)
                {
                    velocity.Y += acceleration.Y;
                    if (velocity.Y > maxVelocity.Y)
                    {
                        velocity.Y = maxVelocity.Y;
                    }
                }
                else if (directionToPlayer.Y < 0.0f)
                {
                    velocity.Y -= acceleration.Y;
                    if (velocity.Y < -maxVelocity.Y)
                    {
                        velocity.Y = -maxVelocity.Y;
                    }
                }
            }
            position += velocity;
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

