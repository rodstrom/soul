﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class DarkThought : Entity
    {
        public delegate void PowerupReleaseHandle(Entity entity);
        private DarkThoughtWeapon weapon;
        EntityManager entityManager;
        Path path;

        private bool attack = true;
        private double timeSinceLastBullet = 0;
        private double timeSinceLastBurst = 0;
        private Vector2 directionToWaypoint = Vector2.Zero;
        public event PowerupReleaseHandle onDie = null;
        private HitFX hitFx = null;
        private bool waitingToDie = false;

        public DarkThought(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, GameTime gameTime, string alias, EntityManager entityManager, Path path)
            : base(spriteBatch, game, Constants.DARK_THOUGHT_FILENAME, new Vector2(Constants.DARK_THOUGHT_WIDTH, Constants.DARK_THOUGHT_HEIGHT), alias, EntityType.DARK_THOUGHT)
        {
            this.audio = audioManager;
            this.entityManager = entityManager;
            weapon = new DarkThoughtWeapon(spriteBatch, game, sprite.Y);
            weapon.Damage = damage;
            //maxVelocity = new Vector2(Constants.DARK_THOUGHT_MAX_SPEED, Constants.DARK_THOUGHT_MAX_SPEED);
            acceleration = new Vector2(Constants.DARK_THOUGHT_ACCELERATION, Constants.DARK_THOUGHT_ACCELERATION);
            this.path = path;
            //this.health = Constants.DARK_THOUGHT_MAX_HEALTH;
            this.animation.MaxFrames = 11;
            hitFx = new HitFX(game);
            //this.hitRadius = Constants.DARK_THOUGHT_RADIUS;
            timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            timeSinceLastBurst = gameTime.TotalGameTime.TotalMilliseconds;
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

            if (waitingToDie == false)
            {
                path.Update(gameTime, Position);

                if (attack && timeSinceLastBullet > fireRate)
                {
                    entityManager.addBullet(weapon.Shoot(position));
                    audio.playSound("dark_thought_shoot");
                    timeSinceLastBullet = 0;
                }

                if (attack && timeSinceLastBurst > burst)
                {
                    attack = false;
                    timeSinceLastBurst = 0;
                }
                else if (!attack && timeSinceLastBurst > burst * 2)
                {
                    attack = true;
                    timeSinceLastBurst = 0;
                }

                timeSinceLastBullet += gameTime.ElapsedGameTime.TotalMilliseconds;
                timeSinceLastBurst += gameTime.ElapsedGameTime.TotalMilliseconds;

                
            }
            else
            {
                if (animation.CurrentFrame >= animation.MaxFrames)
                {
                    killMe = true;
                }
            }
            Move(gameTime);
            animation.Animate(gameTime);
        }

        public void Move(GameTime gameTime)
        {

                directionToWaypoint = (path.CurrentPath - Position);
                directionToWaypoint.Normalize();

                velocity = velocity + directionToWaypoint * acceleration;
                if (velocity.Length() > maxVelocity.Length())
                {
                    velocity.Normalize();
                    velocity *= maxVelocity;
                }

                position += velocity;
            
        }

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
                    OnDeath();
                    audio.playSound("dark_thought_die");
                }
            }
        }

        public override int getDamage()
        {
            return weapon.Damage;
        }

        public override void takeDamage(int value)
        {
            health -= value;
            if (health <= 0)
            {

                OnDeath();
            }
        }

        private bool checkWaypoint()
        {
            float difference = Vector2.Distance(Position, path.CurrentPath);

            if (difference <= 20.0f)
            {
                return true;
            }
            return false;
        }

        public override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing == true)
                {
                    animation.Dispose();
                    weapon.Dispose();
                }
            }
            disposed = true;
        }

        private void OnDeath()
        {
            waitingToDie = true;
            ghost = true;
            animation.MaxFrames = 16;
            animation.CurrentFrame = 0;
            animationState = 1;
            animation.FrameRate = 30;
        }

    }
}
