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
        private Vector2 bulletOrigin = new Vector2(75, 245);
        private Vector2 spawnOrigin = new Vector2(80, 500);
        private Vector2 directionToPlayer = Vector2.Zero;

        private int fireTimer = 0;
        private int spawnNumber = 0;
        private int spawnRate = 0;
        private int spawnTimer = 0;
        private Random random;
        private HitFX hitFx = null;
        private bool waitingToDie = false;
        private bool inPlace = false;

        private bool shooting = false;
        private bool spawning = false;

        private Sprite spriteIdle;
        private Sprite spriteShoot;
        private Sprite spriteSpawn;
        private Sprite spriteDeath;

        private Animation animation2;

        public Boss(SpriteBatch spriteBatch, Soul game, AudioManager audio, GameTime gameTime, string alias, EntityManager entityManager)
            : base(spriteBatch, game, Constants.BOSS_IDLE_FILENAME, new Vector2(Constants.BOSS_WIDTH, Constants.BOSS_HEIGHT), alias, EntityType.BOSS)
        {
            spriteIdle = new Sprite(spriteBatch, game, Constants.BOSS_IDLE_FILENAME);
            spriteShoot = new Sprite(spriteBatch, game, Constants.BOSS_SHOOT_FILENAME);
            spriteSpawn = new Sprite(spriteBatch, game, Constants.BOSS_SPAWN_FILENAME);
            spriteDeath = new Sprite(spriteBatch, game, Constants.BOSS_DEATH_FILENAME);

            this.sprite = spriteSpawn;
            this.animation.MaxFrames = 0;

            animation2 = new Animation(((int)(spriteShoot.X / dimension.X)) - 1);

            this.entityManager = entityManager;
            weapon = new BossWeapon(spriteBatch, game, sprite.Y);
            weapon.Damage = damage;
            offset = Vector2.Zero;

            this.random = new Random();
            hitFx = new HitFX(game);
            velocity.X = 5;
            this.audio = audio;
        }

        public override void Draw()
        {
            Rectangle rect = new Rectangle(animation.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
            Rectangle rect2 = new Rectangle(animation2.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y / 2);
            if (hitFx.IsHit == true)
            {
                sprite.Draw(position, rect, Color.White, rotation, offset, scale, SpriteEffects.None, layer, hitFx.Effect);
                if (shooting) spriteShoot.Draw(position, rect2, Color.White, rotation, offset, scale, SpriteEffects.None, layer, hitFx.Effect);
            }
            else
            {
                base.Draw();
                if (shooting) spriteShoot.Draw(position, rect2, Color.White, rotation, offset, scale, SpriteEffects.None, layer);
            }
        }

        public override void Update(GameTime gameTime)
        {
            hitFx.Update();

            if (!inPlace)
            {
                Move(gameTime);
            }

            if (!waitingToDie && !killMe && inPlace)
            {
                fireTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (fireTimer >= fireRate)
                {
                    directionToPlayer = (target - bulletOrigin);
                    directionToPlayer.Normalize();

                    shootAnimate();

                    entityManager.addBullet(weapon.Shoot(bulletOrigin, directionToPlayer.Y));
                    fireTimer = 0;
                }

                spawnTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (spawnTimer >= spawnRate)
                {
                    spawnAnimate();     //spawns an enemy when mouth is completely open

                    //SpawnEntity();
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
                        entityManager.killAllEntities();
                        entityManager.cleansedLevel();
                    }
                } 
            }

            if (spawning)
            {
                if (animation.CurrentFrame == 4)
                {
                    SpawnEntity();
                    animation.CurrentFrame++;
                }

                if (animation.CurrentFrame >= animation.MaxFrames)
                {
                    spawning = false;

                    animation.MaxFrames = 0;
                    animation.CurrentFrame = 0;
                }
            }

            if (shooting)
            {
                if (animation2.CurrentFrame >= animation2.MaxFrames)
                {
                    shooting = false;
                    animation2.CurrentFrame = 0;
                }

                animation2.Animate(gameTime);
            }

            animation.Animate(gameTime);
        }

        public void SpawnEntity()
        {
            Nightmare nightmare = new Nightmare(spriteBatch, game, audio, entityManager, "nightmare" + spawnNumber.ToString());
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
            else
            {
                inPlace = true;
                spawnOrigin += position;
                bulletOrigin += position;
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
            dimension.Y = Constants.BOSS_DEATH_HEIGHT;
            dimension.X = Constants.BOSS_DEATH_WIDTH;
            sprite = spriteDeath;
            animation.MaxFrames = 5;
            animation.CurrentFrame = 0;
            animationState = 0;
            shooting = false;
            spawning = false;
        }

        private void shootAnimate()
        {
            //sprite = spriteShoot;
            //animation2.MaxFrames = 12;
            animation2.CurrentFrame = 0;
            //animationState = 0;
            shooting = true;
        }

        private void spawnAnimate()
        {
            sprite = spriteSpawn;
            animation.MaxFrames = 7;
            animation.CurrentFrame = 0;
            //animationState = 0;
            spawning = true;
        }
    }
}
