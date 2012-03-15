using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Soul.Manager;

namespace Soul
{
    class Player : Entity
    {
        enum PlayerAnimationState
        {
            MOVE_BACK = 0,
            MOVE_FORWARD,
            MOVING_FORWARD,
            IDLE,
            MOVING_BACKWARDS
        };

        public delegate void NightmareHit();
        public delegate void HitFlash();
        public event NightmareHit nightmareHit = null;
        public event HitFlash hitFlash = null;
        

        private List<Entity> lesserDemonList = new List<Entity>();
        private PointLight healthLight;
        private int healthLightMaxRadius = 0;
        private int healthLightMinRadius = 0;
        private GlowFX warningGlow = null;

        private PlayerWeapon weapon;
        private InputManager controls;
        private bool waitingtoDie = false;
        private Sprite glow = null;
        private Sprite shootSprite = null;
        private HitFX hitFx = null;

        private float glowScale = 1.0f;
        private float glowScalePercentage = 0.005f;
        private int dynamicLightScalar = 1;
        private bool decrease = true;

        private float maxDeathPower = 0.0f;
        private float deathPowerScaleUp = 0.0f;
        private float deathPowerScaleDown = 0.0f;

        private int maxDeathLightDecay = 0;
        private int deathDecayScaleUp = 0;
        private int deathDecayScaleDown = 0;
        private float secondExplosionPower = 0.0f;
        private float secondExplosionPwrScalar = 0.0f;
        private int secondExplosionLight = 0;
        private int secondExplosionLightScalar = 0;
        private int fadeOutLight = 0;
        private float fadeOutPower = 0;

        private int deathPass = 1;

        private bool movingForward = false;
        private bool movingBackwards = false;
        private bool lesserDemonStuck = false;
        private double timeSinceLastShot = 0;
        private bool showPlayer = true;
        private bool healthWarning = false;

        private double powerupTime = 0;
        private bool powerupActive = false;
        private bool spreadPowerupActive = false;

        private EntityManager entityManager;

        public Player(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, string alias, EntityManager entityManager, InputManager controls)
            : base(spriteBatch, game, Constants.PLAYER_FILENAME, new Vector2(Constants.PLAYER_WIDTH, Constants.PLAYER_HEIGHT), alias, EntityType.PLAYER)
        {
            
            audio = audioManager;
            this.entityManager = entityManager;
            this.controls = controls;
            weapon = new PlayerWeapon(spriteBatch, game, (int)dimension.Y);
            weapon.Damage = damage;
            acceleration = new Vector2 (Constants.PLAYER_ACCELERATION, Constants.PLAYER_ACCELERATION);
            animationState = (int)PlayerAnimationState.IDLE;
            this.glow = new Sprite(spriteBatch, game, Constants.PLAYER_GLOW_FILENAME);
            this.shootSprite = new Sprite(spriteBatch, game, Constants.PLAYER_SHOOT_ANIM);
            hitFx = new HitFX(game);
            warningGlow = new GlowFX(game, Constants.FLASH_EFFECT_RED_FILENAME, 0.05f, 0.1f, 0.9f);
            this.animation.FrameRate = 30;
            this.maxDeathLightDecay = int.Parse(game.lighting.getValue("PlayerDeath", "MaxDecay"));
            this.maxDeathPower = float.Parse(game.lighting.getValue("PlayerDeath", "MaxPower"));
            this.deathDecayScaleUp = int.Parse(game.lighting.getValue("PlayerDeath", "DecayScaleUp"));
            this.deathDecayScaleDown = int.Parse(game.lighting.getValue("PlayerDeath", "DecayScaleDown"));
            this.deathPowerScaleUp = float.Parse(game.lighting.getValue("PlayerDeath", "PowerScaleUp"));
            this.deathPowerScaleDown = float.Parse(game.lighting.getValue("PlayerDeath", "PowerScaleDown"));
            this.secondExplosionLight = int.Parse(game.lighting.getValue("PlayerDeath", "SecondExplosionLightSize"));
            this.secondExplosionLightScalar = int.Parse(game.lighting.getValue("PlayerDeath", "SecondExplosionLightScalar"));
            this.secondExplosionPower = float.Parse(game.lighting.getValue("PlayerDeath", "SecondExplosionPower"));
            this.secondExplosionPwrScalar = float.Parse(game.lighting.getValue("PlayerDeath", "SecondExplosionPowerScalar"));
            this.fadeOutLight = int.Parse(game.lighting.getValue("PlayerDeath", "FadeOutLightScalar"));
            this.fadeOutPower = float.Parse(game.lighting.getValue("PlayerDeath", "FadeOutPowerScalar"));
            this.healthLightMaxRadius = int.Parse(game.lighting.getValue("PlayerHealthLight", "MaxRadius"));
            this.healthLightMinRadius = int.Parse(game.lighting.getValue("PlayerHealthLight", "MinRadius"));
            
            pointLight = new PointLight()
            {
                Color = new Vector4(float.Parse(game.lighting.getValue("PlayerLight", "ColorR")), float.Parse(game.lighting.getValue("PlayerLight", "ColorG")), float.Parse(game.lighting.getValue("PlayerLight", "ColorB")), float.Parse(game.lighting.getValue("PlayerLight", "ColorA"))),
                Power = float.Parse(game.lighting.getValue("PlayerLight", "Power")),
                LightDecay = int.Parse(game.lighting.getValue("PlayerLight", "LightDecay")),
                Position = new Vector3(0f, 0f, float.Parse(game.lighting.getValue("PlayerLight", "ZPosition"))),
                IsEnabled = true,
                renderSpecular = bool.Parse(game.lighting.getValue("PlayerLight", "Specular"))
            };

            healthLight = new PointLight()
            {
                Color = new Vector4(float.Parse(game.lighting.getValue("PlayerHealthLight", "ColorR")), float.Parse(game.lighting.getValue("PlayerHealthLight", "ColorG")), float.Parse(game.lighting.getValue("PlayerHealthLight", "ColorB")), float.Parse(game.lighting.getValue("PlayerHealthLight", "ColorA"))),
                Power = float.Parse(game.lighting.getValue("PlayerHealthLight", "Power")),
                LightDecay = int.Parse(game.lighting.getValue("PlayerHealthLight", "LightDecay")),
                Position = new Vector3(0f, 0f, float.Parse(game.lighting.getValue("PlayerHealthLight", "ZPosition"))),
                IsEnabled = true,
                renderSpecular = bool.Parse(game.lighting.getValue("PlayerHealthLight", "Specular"))
            };
        }

        public override void Draw()
        {
            /*if (hitFx.IsHit == true)
            {
                sprite.Draw(GlowPosition, Color.White, rotation, origin, glowScale, SpriteEffects.None, layer, hitFx.Effect);
            }
            else
            {
                glow.Draw(GlowPosition, Color.White, rotation, origin, glowScale, SpriteEffects.None, layer);
            }*/

            //base.Draw();

            if (showPlayer == true)
            {
                if (healthWarning == true)
                {
                    Rectangle rect = new Rectangle(animation.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
                    sprite.Draw(position, rect, Color.White, rotation, offset, scale, SpriteEffects.None, layer, warningGlow.Effect);
                }
                else
                {
                    base.Draw();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            weapon.Update(gameTime);


            if (waitingtoDie == false)
            {
                checkHealthStatus();
                if (powerupActive)
                {
                    powerupTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (powerupTime >= 15000)
                    {
                        clearPowerup(false);
                    }
                }
                
                if (controls.Shooting == true && lesserDemonStuck == false)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds - timeSinceLastShot > fireRate)
                    {
                        if (spreadPowerupActive)
                        {
                            ShootSpread();
                        }
                        else
                        {
                            Shoot();
                        }

                        timeSinceLastShot = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                else if (controls.ShootingOnce == true && lesserDemonStuck == true)
                {
                    for (int i = 0; i < lesserDemonList.Count; i++)
                    {
                        lesserDemonList[i].takeDamage(weapon.Damage);
                        if (lesserDemonList[i].KillMe == true)
                        {
                            lesserDemonList.RemoveAt(i);
                            float total = Constants.PLAYER_MAX_SPEED - Constants.PLAYER_LOWEST_SPEED;
                            float currentDeacceleration = (float)lesserDemonList.Count * Constants.PLAYER_DEACCELERATION;
                            if (currentDeacceleration < total)
                            {
                                maxVelocity.X += Constants.PLAYER_DEACCELERATION;
                                maxVelocity.Y += Constants.PLAYER_DEACCELERATION;
                            }
                            i--;
                        }
                    }
                    if (lesserDemonList.Count <= 0)
                    {
                        lesserDemonStuck = false;
                        maxVelocity = new Vector2(Constants.PLAYER_MAX_SPEED);
                    }
                }

                animation.Animate(gameTime);
                Move(gameTime);
                pointLight.Position = new Vector3(position.X, position.Y, pointLight.Position.Z);
                healthLight.Position = new Vector3(position.X, position.Y, healthLight.Position.Z);
                
            }
            else
            {
                if (deathPass == 1)
                {
                    FirstExplosion();
                }
                else if (deathPass == 2)
                {
                    Implosion();
                }
                else if (deathPass == 3)
                {
                    SecondExplosion();
                }
                else if (deathPass == 4)
                {
                    FadeOutLight();
                }
            }
        }

        #region move
        public void Move(GameTime gameTime)
        {

            if (controls.MoveUp == true)
            {
                velocity.Y -= acceleration.Y;
                if (velocity.Y < -maxVelocity.Y)
                {
                    velocity.Y = -maxVelocity.Y;
                }
            }

            if (controls.MoveDown == true)
            {
                velocity.Y += acceleration.Y;
                if (velocity.Y > maxVelocity.Y)
                {
                    velocity.Y = maxVelocity.Y;
                }
            }

            if (controls.MoveLeft == true)
            {
                velocity.X -= acceleration.X;
                if (velocity.X < -maxVelocity.X)
                {
                    velocity.X = -maxVelocity.X;
                }
            }

            if (controls.MoveRight == true)
            {
                velocity.X += acceleration.X;
                if (velocity.X > maxVelocity.X)
                {
                    velocity.X = maxVelocity.X;
                }
            }

            if (controls.MoveRight == false && controls.MoveLeft == false)
            {
                StopMovingX();
            }

            if (controls.MoveUp == false && controls.MoveDown == false)
            {
                StopMovingY();
            }
            position += velocity;
            
            if (velocity.X == 0.0f && animation.Reverse == false)
            {
                animationState = (int)PlayerAnimationState.IDLE;
                animation.MaxFrames = 0;
                animation.CurrentFrame = 0;
                movingBackwards = false;
                movingForward = false;
            }
            else if (velocity.X < 0.0f && animation.Reverse == false)
            {
                movingBackwards = false;
                if (movingForward == false)
                {
                    animationState = (int)PlayerAnimationState.MOVE_FORWARD;
                    animation.MaxFrames = 4;
                    
                    if (animation.CurrentFrame == animation.MaxFrames - 1 && animation.Reverse == false)
                    {
                        movingForward = true;
                    }
                }
                else
                {
                    animation.CurrentFrame = 0;
                    animationState = (int)PlayerAnimationState.MOVING_FORWARD;
                    animation.MaxFrames = 0;
                    if (velocity.X > -maxVelocity.X)
                    {
                        animation.Reverse = true;
                        animationState = (int)PlayerAnimationState.MOVE_FORWARD;
                        animation.MaxFrames = 4;
                        animation.CurrentFrame = animation.MaxFrames - 1;
                    }
                }
            }
            else if (velocity.X > 0.0f && animation.Reverse == false)
            {
                movingForward = false;
                if (movingBackwards == false)
                {
                    animationState = (int)PlayerAnimationState.MOVE_BACK;
                    animation.MaxFrames = 4;

                    if (animation.CurrentFrame >= animation.MaxFrames - 1)
                    {
                        movingBackwards = true;
                    }
                }
                else
                {
                    animation.CurrentFrame = 0;
                    animationState = (int)PlayerAnimationState.MOVING_BACKWARDS;
                    animation.MaxFrames = 0;
                    if (velocity.X < maxVelocity.X)
                    {
                        animation.Reverse = true;
                        animationState = (int)PlayerAnimationState.MOVE_BACK;
                        animation.MaxFrames = 4;
                        animation.CurrentFrame = animation.MaxFrames - 1;
                    }
                }
            }
            else if (animation.Reverse == true)
            {
                if (animation.CurrentFrame <= 0)
                {
                    animation.Reverse = false;
                }
            }

            restriction();
        }

        public void restriction()
        {
            if (position.X - hitRadius < screenBoundaries.Left)
            {
                position.X = screenBoundaries.Left + hitRadius;
            }
            else if (position.X + hitRadius > screenBoundaries.Right)
            {
                position.X = screenBoundaries.Right - hitRadius;
            }

            if (position.Y - hitRadius < screenBoundaries.Top)
            {
                position.Y = screenBoundaries.Top + hitRadius;
            }
            else if (position.Y + hitRadius > screenBoundaries.Bottom)
            {
                position.Y = screenBoundaries.Bottom - hitRadius;
            }
        }

        private void StopMovingY()
        {
            if (velocity.Y < 0.0f)
            {
                velocity.Y += acceleration.Y;
                if (velocity.Y > 0.0f)
                {
                    velocity.Y = 0.0f;
                }
            }
            else if (velocity.Y > 0.0f)
            {
                velocity.Y -= acceleration.Y;
                if (velocity.Y < 0.0f)
                {
                    velocity.Y = 0.0f;
                }
            }
        }

        private void StopMovingX()
        {
            
            if (velocity.X < 0.0f)
            {
                velocity.X += acceleration.X;
                if (velocity.X > 0.0f)
                {
                    velocity.X = 0.0f;
                }
            }
            else if (velocity.X > 0.0f)
            {
                velocity.X -= acceleration.X;
                if (velocity.X < 0.0f)
                {
                    velocity.X = 0.0f;
                }
            }
        }

#endregion move

        public void Shoot()
        {
            Bullet bullet = weapon.Shoot(position);
            if (bullet != null)
            {
                entityManager.addBullet(bullet);
            }
            audio.playSound("player_shoot");
        }

        public void ShootSpread()
        {
            Bullet bullet;
            Vector2 newPos = weapon.getPosition(position);

            for (int i = 0; i < 5; i++)
            {
                bullet = weapon.Shoot(newPos, i);
                if (bullet != null)
                {
                    entityManager.addBullet(bullet);
                }
            }

            audio.playSound("player_shoot");
        }

        public override void onCollision(Entity entity)
        {
            if (entity.Type == EntityType.LESSER_DEMON)
            {
                lesserDemonList.Add(entity);
                if (maxVelocity.Length() >= Constants.PLAYER_LOWEST_SPEED)
                {
                    maxVelocity.X -= Constants.PLAYER_DEACCELERATION;
                    maxVelocity.Y -= Constants.PLAYER_DEACCELERATION;
                }
                lesserDemonStuck = true;
            }

            if (entity.Type == EntityType.DARK_THOUGHT_BULLET || entity.Type == EntityType.BLUE_BLOOD_VESSEL || entity.Type == EntityType.PURPLE_BLOOD_VESSEL || entity.Type == EntityType.RED_BLOOD_VESSEL || entity.Type == EntityType.NIGHTMARE || entity.Type == EntityType.DARK_WHISPER || entity.Type == EntityType.DARK_WHISPER_SPIKE || entity.Type == EntityType.BOSS_BULLET)
            {
                health -= entity.getDamage();
                if (health < healthLightMaxRadius)
                    healthWarning = true;
                
                if (hitFlash != null)
                    hitFlash();

                if (entity.Type == EntityType.NIGHTMARE)
                {
                    if (nightmareHit != null)
                        nightmareHit();
                }
            }

            if (entity.Type == EntityType.HEALTH_POWERUP)
            {
                health += 10;
                if (health > healthLightMaxRadius)
                    healthWarning = false;

                if (health > Constants.PLAYER_MAX_HEALTH)
                {
                    health = Constants.PLAYER_MAX_HEALTH;
                }
                audio.playSound("player_powerup_pickup");
            }

            if (entity.Type == EntityType.WEAPON_POWERUP_SPREAD)
            {
                clearPowerup(true);
                weapon.powerupSpread();
                damage = (int)(damage * float.Parse(game.constants.getValue("WEAPON_POWERUP_SPREAD", "DAMAGEMULTIPLIER")));
                weapon.Damage = damage;
                fireRate /= float.Parse(game.constants.getValue("WEAPON_POWERUP_SPREAD", "RATEMULTIPLIER")); 
                spreadPowerupActive = true;
            }

            if (entity.Type == EntityType.WEAPON_POWERUP_RAPID)
            {
                clearPowerup(true);
                damage = (int)(damage * float.Parse(game.constants.getValue("WEAPON_POWERUP_RAPID", "DAMAGEMULTIPLIER")));
                weapon.Damage = damage;
                fireRate /= float.Parse(game.constants.getValue("WEAPON_POWERUP_RAPID", "RATEMULTIPLIER"));
            }

            if (waitingtoDie == false && health <= 0)
            {
                pointLight.Color = new Vector4(float.Parse(game.lighting.getValue("PlayerDeath", "ColorR")), float.Parse(game.lighting.getValue("PlayerDeath", "ColorG")), float.Parse(game.lighting.getValue("PlayerDeath", "ColorB")), float.Parse(game.lighting.getValue("PlayerDeath", "ColorA")));
                waitingtoDie = true;
                audio.playSound("player_die");
            }
        }

        private void clearPowerup(bool setNew)
        {
            powerupTime = 0;
            weapon.clearPowerup();
            fireRate = float.Parse(game.constants.getValue(type.ToString(), "RATE"));
            damage = int.Parse(game.constants.getValue(type.ToString(), "DAMAGE"));
            weapon.Damage = damage; 
            spreadPowerupActive = false;

            powerupActive = setNew;
            if (setNew)
            {
                audio.playSound("player_powerup_pickup");
            }
        }

        public override int getDamage()
        {
            return weapon.Damage;
        }

        public override void takeDamage(int value)
        {
            health -= value;
            if (health < healthLightMaxRadius)
                healthWarning = true;
            if (health <= 0)
            {
                waitingtoDie = true;
            }
        }

        private void checkHealthStatus()
        {
            float percentage = (float)health / (float)maxHealth;

            float newLightRadius = (float)healthLightMaxRadius * percentage;
            if (newLightRadius <= healthLightMinRadius)
                newLightRadius = healthLightMinRadius;

            healthLight.LightDecay = (int)newLightRadius;

        }

        private void SpawnDeathGlow()
        {
            GlowParticle glowParticle;
            Random random = new Random();
            for (int i = 0; i < Constants.PLAYER_NUMBER_OF_DEATH_GLOWS; i++)
            {
                glowParticle = new GlowParticle(spriteBatch, game, "glow_death" + i.ToString(), position, random);
                entityManager.addEntity(glowParticle);
                //entityManager.AddPointLight(glowParticle.PointLight);
            }
        }

        public PointLight HealthLight { get { return healthLight; } }

        #region DeathExplosion

        private void FirstExplosion()
        {
            if (pointLight.LightDecay <= maxDeathLightDecay)
                pointLight.LightDecay = pointLight.LightDecay + deathDecayScaleUp;

            if (pointLight.Power <= maxDeathPower)
                pointLight.Power = pointLight.Power + deathPowerScaleUp;

            if (pointLight.LightDecay >= maxDeathLightDecay && pointLight.Power >= maxDeathPower)
            {
                deathPass++;
            }
        }

        private void Implosion()
        {
            if (pointLight.Power > 0)
                pointLight.Power = pointLight.Power - deathPowerScaleDown;

            pointLight.LightDecay = pointLight.LightDecay - deathDecayScaleDown;
            if (pointLight.Power <= 0.0f)
            {
                SpawnDeathGlow();
                showPlayer = false;
                deathPass++;
            }
        }

        private void SecondExplosion()
        {
            if (pointLight.LightDecay <= secondExplosionLight)
                pointLight.LightDecay = pointLight.LightDecay + secondExplosionLightScalar;

            if (pointLight.Power <= secondExplosionPower)
                pointLight.Power = pointLight.Power + secondExplosionPwrScalar;

            if (pointLight.LightDecay >= secondExplosionLight && pointLight.Power >= secondExplosionPower)
            {
                deathPass++;
            }
        }

        private void FadeOutLight()
        {
            if (pointLight.LightDecay > 0)
                pointLight.LightDecay = pointLight.LightDecay - fadeOutLight;

            if (pointLight.Power > 0.0f)
                pointLight.Power = pointLight.Power - fadeOutPower;

            if (pointLight.Power <= 0.0f)
            {
                killMe = true;
            }
        }

        #endregion DeathExplosion
    }
}
