using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class Boss : Entity
    {
        private BossWeapon weapon;
        EntityManager entityManager;
        private Vector2 bulletOrigin = new Vector2(50, 345);
        private Vector2 spawnOrigin = new Vector2(90, 555);
        private Vector2 directionToPlayer = Vector2.Zero;

        private int fireTimer = 0;
        private int spawnNumber = 0;
        private int spawnRate = 0;
        private int spawnTimer = 0;
        private Random random;
        private HitFX hitFx = null;
        private bool waitingToDie = false;

        private Sprite spriteIdle;
        private Sprite spriteShoot;
        private Sprite spriteSpawn;
        private Sprite spriteDeath;

        public Boss(SpriteBatch spriteBatch, Soul game, GameTime gameTime, string alias, EntityManager entityManager)
            : base(spriteBatch, game, Constants.BOSS_IDLE_FILENAME, new Vector2(Constants.BOSS_WIDTH, Constants.BOSS_HEIGHT), alias, EntityType.BOSS)
        {
            spriteIdle = new Sprite(spriteBatch, game, Constants.BOSS_IDLE_FILENAME);
            //spriteShoot = new Sprite(spriteBatch, game, Constants.BOSS_SHOOT_FILENAME);
            //spriteSpawn = new Sprite(spriteBatch, game, Constants.BOSS_SPAWN_FILENAME);
            spriteDeath = new Sprite(spriteBatch, game, Constants.BOSS_DEATH_FILENAME);

            this.sprite = spriteIdle;
            this.animation.MaxFrames = 0;

            this.entityManager = entityManager;
            weapon = new BossWeapon(spriteBatch, game, sprite.Y);
            weapon.Damage = damage;
            offset = Vector2.Zero;

            this.random = new Random();
            hitFx = new HitFX(game);
            velocity.X = 5;
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

            if (!waitingToDie && !killMe)
            {
                Move(gameTime);

                fireTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (fireTimer >= fireRate)
                {
                    directionToPlayer = (target - bulletOrigin);
                    directionToPlayer.Normalize();

                    entityManager.addBullet(weapon.Shoot(bulletOrigin, directionToPlayer.Y));
                    fireTimer = 0;
                }

                spawnTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (spawnTimer >= spawnRate)
                {
                    SpawnEntity();
                    spawnTimer = 0;
                    spawnRate = random.Next(minSpawn, maxSpawn);
                }

                if (health <= 0)
                {
                    die();
                }
            }

            if (waitingToDie && !killMe)
            {
                if (animation.CurrentFrame >= animation.MaxFrames)
                {
                    if (animationState <= 2)
                    {
                        animationState++;
                        animation.CurrentFrame = 0;
                    }
                    else if (animationState == 3)
                    {
                        animationState++;
                        animation.CurrentFrame = 0;
                        animation.MaxFrames = 2;
                    }
                    else if (animationState == 4)
                    {
                        //animationState = 0;           //loop for debug
                        //animation.CurrentFrame = 0;
                        //animation.MaxFrames = 6;
                        killMe = true;
                        entityManager.killAllEntities();
                        entityManager.cleansedLevel();
                    }
                }
            }

            animation.Animate(gameTime);
        }

        public void SpawnEntity()
        {
            Nightmare nightmare = new Nightmare(spriteBatch, game, entityManager, "nightmare" + spawnNumber.ToString());
            while (entityManager.addEntity(nightmare) == false)
            {
                spawnNumber++;
                nightmare.Alias = "nightmare" + spawnNumber.ToString();
            }
            nightmare.position = spawnOrigin;
        }

        public void Move(GameTime gameTime)
        {
            if (position.X < 0)
            {
                position.X += velocity.X;
            }
            
        }

        public override void onCollision(Entity entity)
        {
            if (entity.Type == EntityType.PLAYER_BULLET || entity.Type == EntityType.DARK_WHISPER || entity.Type == EntityType.DARK_WHISPER_SPIKE)
            {
                health -= entity.getDamage();
                hitFx.Hit(0.5f);
            }
        }

        public override int getDamage()
        {
            return weapon.Damage;
        }

        public override void takeDamage(int value)
        {
            health -= value;
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

        private void die()
        {
            waitingToDie = true;
            dimension.X = Constants.BOSS_DEATH_WIDTH;
            sprite = spriteDeath;
            animation.MaxFrames = 5;
            animation.CurrentFrame = 0;
            animationState = 0;
        }
    }
}
