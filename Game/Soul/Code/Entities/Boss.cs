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

        private int burstTimer = 0;
        private bool attack = true;
		private int currentAttack = -1;
		private int currentSpawn = 0;
        private int burstPause;

		private enum AttackPattern{
			HOMING = 0,
			SHOTGUN,
			SPRAY
		};

		private enum SpawnPattern{
			NIGHTMARE = 0,
			LESSER_DEMON,
			DARK_WHISPER
		};

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

            fireRate = burst;
            burstPause = int.Parse(game.constants.getValue("BOSS", "BURSTPAUSE"));
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
                burstTimer += gameTime.ElapsedGameTime.Milliseconds;
				
				switch(currentAttack)
				{
					case (int)AttackPattern.HOMING:
				        if (attack && fireTimer >= fireRate)
				        {
				            directionToPlayer = (target - bulletOrigin);
				            directionToPlayer.Normalize();

				            shootAnimate();

				            entityManager.addBullet(weapon.Shoot(bulletOrigin, directionToPlayer.Y));
				            fireTimer = 0;
				        }
						break;
                    case (int)AttackPattern.SHOTGUN:
						if (attack && fireTimer >= fireRate)
				        {
				            shootAnimate();
							shootSpread();
				            fireTimer = 0;
				        }
						break;
                    case (int)AttackPattern.SPRAY:
                        if (attack && fireTimer >= fireRate)
                        {
                            shootAnimate();
                            bool playerIsNorth = false;
                            if (target.Y < 360f)
                            {
                                playerIsNorth = true;
                            }
                            entityManager.addBullet(weapon.Shoot(bulletOrigin, burstTimer, burst, playerIsNorth));
                            fireTimer = 0;
                        }
						break;
				}

                if (attack && burstTimer > burst)
                {
                    attack = false;
                    burstTimer = 0;
                    changeAttack();
                }
                else if (!attack && burstTimer > burstPause)
                {
                    attack = true;
                    burstTimer = 0;
                }

				spawnTimer += gameTime.ElapsedGameTime.Milliseconds;
		        if (spawnTimer >= spawnRate && !attack)
		        {
		            spawnAnimate();     //spawns an enemy when mouth is completely open

		            //SpawnEntity();
		            spawnRate = random.Next(minSpawn, maxSpawn);

                    spawnTimer = 0;
                    fireTimer = -int.Parse(game.constants.getValue("BOSS", "SPAWNSHOOTDELAY"));
                    burstTimer = -int.Parse(game.constants.getValue("BOSS", "SPAWNSHOOTDELAY"));
		        }

        		if (health <= 0)
                {
                    die();
                }
            }

            if (waitingToDie && !killMe)
            {
                pointLight.LightDecay = pointLight.LightDecay + 10;
                pointLight.Power = pointLight.Power - 0.0012f;
                
                fireTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (fireTimer > 3000)
                {
                    entityManager.killAllEntities();
                    entityManager.cleansedLevel();
                }

                //if (animation.CurrentFrame >= animation.MaxFrames)
                //{
                //    if (animationState <= 2)
                //    {
                //        animationState++;
                //        animation.CurrentFrame = 0;
                //    }
                //    else if (animationState == 3)
                //    {
                //        animationState++;
                //        animation.CurrentFrame = 0;
                //        animation.MaxFrames = 2;
                //    }
                //    else if (animationState == 4)
                //    {
                //        //animationState = 0;           //loop for debug
                //        //animation.CurrentFrame = 0;
                //        //animation.MaxFrames = 6;
                //        entityManager.killAllEntities();
                //        entityManager.cleansedLevel();
                //    }
                //} 
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
            changeSpawn();

            switch (currentSpawn)
			{
                case (int)SpawnPattern.NIGHTMARE:
					Nightmare nightmare = new Nightmare(spriteBatch, game, audio, entityManager, "nightmare" + spawnNumber.ToString());
					while (entityManager.addEntity(nightmare) == false)
					{
						spawnNumber++;
						nightmare.Alias = "nightmare" + spawnNumber.ToString();
					}
					nightmare.position = spawnOrigin;
					break;
                case (int)SpawnPattern.LESSER_DEMON:
					LesserDemon lesserDemon = new LesserDemon(spriteBatch, game, audio, "lesserdemon" + spawnNumber.ToString(), target);
                    while (entityManager.addEntity(lesserDemon) == false)
                    {
                        spawnNumber++;
                        lesserDemon.Alias = "lesserdemon" + spawnNumber.ToString();
                    }
                    lesserDemon.position = spawnOrigin;
                    audio.playSound("lesser_demon_spawn");
					break;
                case (int)SpawnPattern.DARK_WHISPER:
					DarkWhisper darkWhisper = new DarkWhisper(spriteBatch, game, audio, "dark_whisper" + spawnNumber.ToString(), entityManager, null);
                    while (entityManager.addEntity(darkWhisper) == false)
                    {
                        spawnNumber++;
                        darkWhisper.Alias = "dark_whisper" + spawnNumber.ToString();
                    }
                    darkWhisper.position = spawnOrigin;
					break;
			}
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
            //dimension.Y = Constants.BOSS_DEATH_HEIGHT;
            //dimension.X = Constants.BOSS_DEATH_WIDTH;
            //sprite = spriteDeath;
            //animation.MaxFrames = 5;
            //animation.CurrentFrame = 0;
            //animationState = 0;
            //position.Y -= Constants.BOSS_DEATH_OFFSET;
            shooting = false;
            spawning = false;
            entityManager.ghostAll();
            entityManager.brightenScreen();

            fireTimer = 0;

            pointLight = new PointLight()
            {
                Color = new Vector4(240, 240, 0, 255),
                Power = 0.2f,
                LightDecay = 200,
                Position = new Vector3(0f, 360f, 50f),
                IsEnabled = true
            };
            entityManager.addLight(pointLight);
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

        private void shootSpread()
        {
            Bullet bullet;

            for (int i = 0; i < 5; i++)
            {
                bullet = weapon.ShootSpread(bulletOrigin, i);
                if (bullet != null)
                {
                    entityManager.addBullet(bullet);
                }
            }

            //audio.playSound("player_shoot");
        }

		private void changeAttack()
		{
			currentAttack = random.Next(3);

			switch(currentAttack)
			{
                case (int)AttackPattern.HOMING:
                    damage = int.Parse(game.constants.getValue("BOSS", "HOMINGDAMAGE"));
                    fireRate = burst / int.Parse(game.constants.getValue("BOSS", "HOMINGBULLETS"));
                    weapon.Damage = damage;
					break;
                case (int)AttackPattern.SHOTGUN:
                    damage = int.Parse(game.constants.getValue("BOSS", "SHOTGUNDAMAGE"));
                    fireRate = burst / int.Parse(game.constants.getValue("BOSS", "SHOTGUNFIRES"));
                    weapon.Damage = damage;
					break;
                case (int)AttackPattern.SPRAY:
                    damage = int.Parse(game.constants.getValue("BOSS", "SPRAYDAMAGE"));
                    fireRate = burst / int.Parse(game.constants.getValue("BOSS", "SPRAYBULLETS"));
                    weapon.Damage = damage;
					break;
			}
		}

		private void changeSpawn()
		{
            currentSpawn = random.Next(3);

			switch(currentSpawn)
			{
                case (int)SpawnPattern.NIGHTMARE:
                    currentSpawn = random.Next(3);
					break;
                case (int)SpawnPattern.LESSER_DEMON:
					break;
                case (int)SpawnPattern.DARK_WHISPER:
					break;
			}
		}
    }
}
