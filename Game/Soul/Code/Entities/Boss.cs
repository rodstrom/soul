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
        private DarkThoughtWeapon weapon;
        EntityManager entityManager;

        private double timeSinceLastBullet = 0;
        //private double timeSinceLastSpawn = 0;
        private HitFX hitFx = null;

        public Boss(SpriteBatch spriteBatch, Soul game, GameTime gameTime, string alias, EntityManager entityManager)
            : base(spriteBatch, game, Constants.BOSS_FILENAME, new Vector2(Constants.BOSS_WIDTH, Constants.BOSS_HEIGHT), alias, EntityType.BOSS)
        {
            this.entityManager = entityManager;
            weapon = new DarkThoughtWeapon(spriteBatch, game, sprite.Y);
            weapon.Damage = damage;
            offset = Vector2.Zero;
            this.health = Constants.BOSS_MAX_HEALTH;
            //this.animation.MaxFrames = 1;
            //hitFx = new HitFX(game);
            timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            velocity.X = 5;
            //timeSinceLastSpawn = gameTime.TotalGameTime.TotalMilliseconds;
        }

        public override void Draw()
        {
            /*if (hitFx.IsHit == true)
            {
                Rectangle rect = new Rectangle(animation.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
                sprite.Draw(position, rect, Color.White, rotation, offset, scale, SpriteEffects.None, layer, hitFx.Effect);
            }
            else
            {*/
                base.Draw();
            //}
        }

        public override void Update(GameTime gameTime)
        {
            //hitFx.Update();

            if (gameTime.TotalGameTime.TotalMilliseconds - timeSinceLastBullet > fireRate)
            {
                entityManager.addBullet(weapon.Shoot(position));
                timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            }

            Move(gameTime);
            //animation.Animate(gameTime);
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
                //hitFx.Hit();
                if (health <= 0)
                {
                    KillMe = true;
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

                killMe = true;
            }
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

    }
}
