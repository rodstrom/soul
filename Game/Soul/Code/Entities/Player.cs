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
        public event NightmareHit nightmareHit = null; 

        private List<Entity> lesserDemonList = new List<Entity>();

        private PlayerWeapon weapon;
        private InputManager controls;
        private bool waitingtoDie = false;
        private Sprite glow = null;
        private Sprite shootSprite = null;
        private HitFX hitFx = null;

        private float glowScale = 1.0f;
        private float glowScalePercentage = 0.005f;
        private bool decrease = true;
        private PlayerShootAnimation playerAnimShoot = null;

        private bool dieMin = false;
        private float dieScaleMin = 0.2f;
        private float dieScaleMax = 1.5f;
        private float dieScalePercentage = 0.08f;

        private bool movingForward = false;
        private bool movingBackwards = false;
        private bool lesserDemonStuck = false;
        private double timeSinceLastShot = 0;

        private EntityManager entityManager;

        public Player(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, string alias, EntityManager entityManager, InputManager controls)
            : base(spriteBatch, game, Constants.PLAYER_FILENAME, new Vector2(Constants.PLAYER_WIDTH, Constants.PLAYER_HEIGHT), alias, EntityType.PLAYER)
        {
            
            audio = audioManager;
            this.entityManager = entityManager;
            this.controls = controls;
            //this.health = Constants.PLAYER_MAX_HEALTH;
            weapon = new PlayerWeapon(spriteBatch, game, (int)dimension.Y);
            //weapon.Damage = Constants.PLAYER_WEAPON_DAMAGE;
            weapon.Damage = damage;
            //maxVelocity = new Vector2 (Constants.PLAYER_MAX_SPEED, Constants.PLAYER_MAX_SPEED);
            acceleration = new Vector2 (Constants.PLAYER_ACCELERATION, Constants.PLAYER_ACCELERATION);
            animationState = (int)PlayerAnimationState.IDLE;
            this.glow = new Sprite(spriteBatch, game, Constants.PLAYER_GLOW_FILENAME);
            this.shootSprite = new Sprite(spriteBatch, game, Constants.PLAYER_SHOOT_ANIM);
            hitFx = new HitFX(game);
            //this.hitRadius = Constants.PLAYER_RADIUS;
            this.animation.FrameRate = 30;

            playerAnimShoot = new PlayerShootAnimation(spriteBatch, game);
            
            pointLight = new PointLight()
            {
                Color = new Vector4(0f, 0f, 0f, 1f),
                Power = 1f,
                LightDecay = 300,
                Position = new Vector3(0f, 0f, 100f),
                IsEnabled = true
            }; 
        }

        public override void Draw()
        {
            if (hitFx.IsHit == true)
            {
                glow.Draw(GlowPosition, Color.White, rotation, origin, glowScale, SpriteEffects.None, layer, hitFx.Effect);
            }
            else
            {
                glow.Draw(GlowPosition, Color.White, rotation, origin, glowScale, SpriteEffects.None, layer);
            }
            playerAnimShoot.Draw(ShootAnimPosition);
            base.Draw();
        }

        public override void Update(GameTime gameTime)
        {
            weapon.Update(gameTime);
            hitFx.Update();
            playerAnimShoot.Update(gameTime, controls);
            if (waitingtoDie == false)
            {
                checkHealthStatus();
                if (controls.Shooting == true && lesserDemonStuck == false)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds - timeSinceLastShot > fireRate)
                    {
                        Shoot();
                        playerAnimShoot.Shoot();
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

                if (decrease == true)
                {
                    glowScale -= glowScalePercentage;
                    pointLight.LightDecay = pointLight.LightDecay - 1;
                    if (glowScale <= 0.7f)
                    {
                        decrease = false;
                    }
                }
                else
                {
                    glowScale += glowScalePercentage;
                    pointLight.LightDecay = pointLight.LightDecay + 1;
                    if (glowScale >= 1.0f)
                    {
                        decrease = true;
                    }
                }

                animation.Animate(gameTime);
                Move(gameTime);
                /*pointLight.Position = new Vector3(
                ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) * 200) + 450,
                ((float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 2) * 200) + 350,
                pointLight.Position.Z);*/
                pointLight.Position = new Vector3(position.X, position.Y, pointLight.Position.Z);
                
            }
            else
            {
                if (glowScale >= dieScaleMin && dieMin == false)
                {
                    glowScale -= dieScalePercentage;
                    if (glowScale <= dieScaleMin)
                    {
                        dieMin = true;
                    }
                }
                else if (glowScale <= dieScaleMax && dieMin == true)
                {
                    glowScale += dieScalePercentage;
                    if (glowScale >= dieScaleMax)
                    {
                        GlowParticle glowParticle;
                        Random random = new Random();
                        for (int i = 0; i < Constants.PLAYER_NUMBER_OF_DEATH_GLOWS; i++)
                        {
                            glowParticle = new GlowParticle(spriteBatch, game, "glow_death" + i.ToString(), position, random);
                            entityManager.addEntity(glowParticle);
                        }
                            killMe = true;
                    }
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
            audio.playSound(Constants.AUDIO_PLAYER_FIRE);
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

            if (entity.Type == EntityType.DARK_THOUGHT_BULLET || entity.Type == EntityType.BLUE_BLOOD_VESSEL || entity.Type == EntityType.NIGHTMARE || entity.Type == EntityType.DARK_WHISPER || entity.Type == EntityType.DARK_WHISPER_SPIKE)
            {
                health -= entity.getDamage();
                hitFx.Hit();
                if (entity.Type == EntityType.NIGHTMARE)
                {
                    if (nightmareHit != null)
                    {
                        nightmareHit();
                    }
                }
            }

            if (entity.Type == EntityType.HEALTH_POWERUP)
            {
                health += 10;
                if (health > Constants.PLAYER_MAX_HEALTH)
                {
                    health = Constants.PLAYER_MAX_HEALTH;
                }
            }

            if (entity.Type == EntityType.WEAPON_POWERUP)
            {
                weapon.IncreaseWeaponLevel();
            }

            if (waitingtoDie == false && health <= 0)
            {
                waitingtoDie = true;
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
                killMe = true;
            }
        }

        public Vector2 GlowPosition
        {
            get
            {
                Vector2 newPosition = position;
                newPosition.X -= ((float)glow.X * 0.5f) * glowScale;
                newPosition.Y -= ((float)glow.Y * 0.5f) * glowScale;
                return newPosition;
            }
        }

        private void checkHealthStatus()
        {
            if (health > 80)
            {
                glowScalePercentage = 0.005f;
            }
            else if (health < 80 && health > 50)
            {
                glowScalePercentage = 0.015f;
            }
            else if (health < 50 && health > 20)
            {
                glowScalePercentage = 0.03f;
            }
            else if (health < 20)
            {
                glowScalePercentage = 0.05f;
            }
        }

        private Vector2 ShootAnimPosition
        {
            get
            {
                Vector2 tmpVector2 = Position;
                tmpVector2.X -= 50;
                return tmpVector2;
            }
        }

    }
}
