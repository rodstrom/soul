using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class InnerDemon : Entity
    {
        public delegate void PowerupReleaseHandle(Entity entity);
        public event PowerupReleaseHandle onDie = null;
        private EntityManager entityManager;
        private Path path = null;
        private HitFX hitFx = null;

        private int spawnNumber = 0;

        private int spawnTimer = 0;
        private int timer = 0;
        private bool entitySpawned = true;
        private Vector2 directionToWaypoint = Vector2.Zero;
        private Random random;

        public InnerDemon(SpriteBatch spriteBatch, Soul game, string alias, EntityManager entityManager, Path path)
            : base(spriteBatch, game, Constants.INNER_DEMON_FILENAME, new Vector2(Constants.INNER_DEMON_WIDTH, Constants.INNER_DEMON_HEIGHT), alias, EntityType.INNER_DEMON)
        {
            this.path = path;
            this.path.Repeat = true;
            //maxVelocity = new Vector2(Constants.INNER_DEMON_MAX_SPEED, Constants.INNER_DEMON_MAX_SPEED);
            acceleration = new Vector2(Constants.INNER_DEMON_ACCELERATION, Constants.INNER_DEMON_ACCELERATION);
            //this.health = Constants.INNER_DEMON_MAX_HEALTH;
            this.entityManager = entityManager;
            this.random = new Random();
            hitFx = new HitFX(game);
            //this.hitRadius = Constants.INNER_DEMON_RADIUS;
        }

        public void SpawnLesserDemon()
        {
            LesserDemon lesserDemon = new LesserDemon(spriteBatch, game, "lesserdemon" + spawnNumber.ToString(), target);
            while (entityManager.addEntity(lesserDemon) == false)
            {
                spawnNumber++;
                lesserDemon.Alias = "lesserdemon" + spawnNumber.ToString();
            }
            lesserDemon.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            hitFx.Update();
            path.Update(gameTime, Position);
            if (entitySpawned == true)
            {
                spawnTimer = random.Next(minSpawn, maxSpawn);
                entitySpawned = false;
            }

            if (entitySpawned == false)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer >= spawnTimer)
                {
                    SpawnLesserDemon();
                    timer = 0;
                    entitySpawned = true;
                }
            }

            base.Update(gameTime);
            Move(gameTime);
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
        }

        public override int getDamage()
        {
            return 0;
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
