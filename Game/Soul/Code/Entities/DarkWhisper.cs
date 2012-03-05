using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class DarkWhisper : Entity
    {
        public delegate void PowerupReleaseHandle(Entity entity);
        public event PowerupReleaseHandle onDie = null;
        private List<Bullet> spikeList;
        private Vector2 directionToWaypoint = Vector2.Zero;
        private Path path = null;
        private EntityManager entityManager = null;
        private bool chase = true;
        private bool spikesReleased = false;
        private float rotationValue = 0.05f;
        private bool waitingToDie = false;
        private HitFX hitFx = null;

        public DarkWhisper(SpriteBatch spriteBatch, Soul game, string alias, EntityManager entityManager, Path path) 
            : base(spriteBatch, game, Constants.DARK_WHISPER_FILENAME, new Vector2(Constants.DARK_WHISPER_WIDTH, Constants.DARK_WHISPER_HEIGHT), alias, EntityType.DARK_WHISPER)
        {
            if (path != null)
            {
                this.path = path;
                this.chase = false;
            }
            this.entityManager = entityManager;
            //this.health = Constants.DARK_WHISPER_MAX_HEALTH;
            //this.maxVelocity =  new Vector2(Constants.DARK_WHISPER_MAX_SPEED);
            this.acceleration = new Vector2(Constants.DARK_WHISPER_ACCELERATION);
            spikeList = new List<Bullet>();
            this.animation.MaxFrames = 0;
            this.animation.FrameRate = 50;
            hitFx = new HitFX(game);
            //this.hitRadius = Constants.DARK_WHISPER_RADIUS;
        }

        public override void Update(GameTime gameTime)
        {
            rotation += rotationValue;
            hitFx.Update();
            if (path != null)
            {
                path.Update(gameTime, Position);
            }
            animation.Animate(gameTime);
   
            if (waitingToDie == true && killMe == false)
            {
                if (animation.CurrentFrame >= animation.MaxFrames)
                {
                    ReleaseSpikes();
                    if (onDie != null)
                    {
                        onDie(this);
                    }
                    killMe = true;
                }
            }
            else
            {
                Move(gameTime);
            }
        }

        public void Move(GameTime gameTime)
        {
            if (chase == true)
            {
                Vector2 newVelocity = target - position;
                newVelocity.Normalize();
                velocity.Y = newVelocity.Y;

                velocity = velocity + newVelocity * acceleration;
                if (position.X >= target.X)
                {
                    velocity.X += 1.0f;
                }

                if (velocity.Y > 0.0f)
                {
                    velocity.Y += 2.0f;
                }
                else if (velocity.Y < 0.0f)
                {
                    velocity.Y -= 2.0f;
                }

                if (velocity.Length() > maxVelocity.Length())
                {
                    velocity.Normalize();
                    velocity *= maxVelocity;
                }
            }
            else
            {
                directionToWaypoint = (path.CurrentPath - Position);
                directionToWaypoint.Normalize();

                velocity = velocity + directionToWaypoint * acceleration;
                if (velocity.Length() > maxVelocity.Length())
                {
                    velocity.Normalize();
                    velocity *= maxVelocity;
                }
            }

            position += velocity;
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

        public override void onCollision(Entity entity)
        {
            if (entity.Type == EntityType.PLAYER_BULLET && waitingToDie == false)
            {
                health -= entity.getDamage();
                hitFx.Hit();
                if (health <= 0)
                {
                    waitingToDie = true;
                    animation.MaxFrames = 17;
                }
            }

            if (entity.Type == EntityType.PLAYER && waitingToDie == false)
            {
                waitingToDie = true;
                animation.MaxFrames = 17;
            }

            if (entity.Type == EntityType.DARK_WHISPER_SPIKE && waitingToDie == false)
            {
                waitingToDie = true;
                animation.MaxFrames = 17;
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
                ReleaseSpikes();
                killMe = true;
            }
        }

        private void ReleaseSpikes()
        {
            if (spikesReleased == true)
            {
                return;
            }
            CreateSpikes();
            for (int i = 0; i < spikeList.Count; i++)
            {
                entityManager.addBullet(spikeList[i]);
            }
            spikesReleased = true;
        }

        private void CreateSpikes()
        {
            Vector2 offsetHalf = offset * 0.5f;
            Vector2 pos = position;
            Vector2 velo = Vector2.Zero;
            pos.Y -= offset.Y;
            velo.Y = -spikeSpeed;
            Bullet spike = new Bullet(spriteBatch, game, pos, velo, Constants.DARK_THOUGHT_BULLET_FILENAME, "dark_whisper_spike", EntityType.DARK_WHISPER_SPIKE, spikeDamage);
            spikeList.Add(spike);
            pos.X += offsetHalf.X;
            pos.Y += offsetHalf.Y;
            velo.X = spikeSpeed;
            spike = new Bullet(spriteBatch, game, pos, velo, Constants.DARK_THOUGHT_BULLET_FILENAME, "dark_whisper_spike", EntityType.DARK_WHISPER_SPIKE, spikeDamage);
            spikeList.Add(spike);
            pos.Y += offsetHalf.Y;
            pos.X += offsetHalf.X;
            velo.Y = 0.0f;
            spike = new Bullet(spriteBatch, game, pos, velo, Constants.DARK_THOUGHT_BULLET_FILENAME, "dark_whisper_spike", EntityType.DARK_WHISPER_SPIKE, spikeDamage);
            spikeList.Add(spike);
            pos.Y += offsetHalf.Y;
            pos.X -= offsetHalf.X;
            velo.Y = spikeSpeed;
            spike = new Bullet(spriteBatch, game, pos, velo, Constants.DARK_THOUGHT_BULLET_FILENAME, "dark_whisper_spike", EntityType.DARK_WHISPER_SPIKE, spikeDamage);
            spikeList.Add(spike);
            velo.X = 0.0f;
            pos.X -= offsetHalf.X;
            pos.Y += offsetHalf.Y;
            spike = new Bullet(spriteBatch, game, pos, velo, Constants.DARK_THOUGHT_BULLET_FILENAME, "dark_whisper_spike", EntityType.DARK_WHISPER_SPIKE, spikeDamage);
            spikeList.Add(spike);
            velo.X = -spikeSpeed;
            pos.X -= offsetHalf.X;
            pos.Y -= offsetHalf.Y;
            spike = new Bullet(spriteBatch, game, pos, velo, Constants.DARK_THOUGHT_BULLET_FILENAME, "dark_whisper_spike", EntityType.DARK_WHISPER_SPIKE, spikeDamage);
            spikeList.Add(spike);
            pos.X -= offsetHalf.X;
            pos.Y -= offsetHalf.Y;
            velo.Y = 0.0f;
            spike = new Bullet(spriteBatch, game, pos, velo, Constants.DARK_THOUGHT_BULLET_FILENAME, "dark_whisper_spike", EntityType.DARK_WHISPER_SPIKE, spikeDamage);
            spikeList.Add(spike);
            pos.X += offsetHalf.X;
            pos.Y -= offsetHalf.Y;
            velo.Y = -spikeSpeed;
            spike = new Bullet(spriteBatch, game, pos, velo, Constants.DARK_THOUGHT_BULLET_FILENAME, "dark_whisper_spike", EntityType.DARK_WHISPER_SPIKE, spikeDamage);
            spikeList.Add(spike);
        }
    }
}
