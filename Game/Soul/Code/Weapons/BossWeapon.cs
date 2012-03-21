using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class BossWeapon : Weapon
    {
        bool playerIsNorth = false;

        public BossWeapon(SpriteBatch spriteBatch, Soul game, int spriteHeight)
            : base(spriteBatch, game, spriteHeight)
        {
            damage = Constants.BOSS_DAMAGE;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        public override Bullet Shoot(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public Bullet Shoot(Vector2 position, float angle)
        {
            Bullet bullet = new Bullet(spriteBatch, game, position, new Vector2(float.Parse(game.constants.getValue("BOSS", "HOMINGSPEED")), angle * 20f), Constants.BOSS_BULLET_FILENAME, "Boss_bullet", EntityType.BOSS_BULLET, damage);
            return bullet;
        }

        public Bullet Shoot(Vector2 position, int burstTime, int burstMax, bool north)
        {
            float progress = (float)burstTime / (float)burstMax;
            if (burstTime < 20)
            {
                playerIsNorth = north;
            }

            Vector2 direction;
            if (playerIsNorth)
            {
                direction = (new Vector2(1280f, (720f * (progress * 2f))) - position);
                if (progress > 0.5f)
                {
                    direction = (new Vector2(1280f, (720f - (720f * ((progress - 0.5f) * 2f)))) - position);
                }
            }
            else
            {
                direction = (new Vector2(1280f, (720f - (720f * (progress * 2f)))) - position);
                if (progress > 0.5f)
                {
                    direction = (new Vector2(1280f, (720f * ((progress - 0.5f) * 2f))) - position);
                }
            }
            direction.Normalize();
            Bullet bullet = new Bullet(spriteBatch, game, position, new Vector2(float.Parse(game.constants.getValue("BOSS", "SPRAYSPEED")), direction.Y * 20f), Constants.BOSS_BULLET_FILENAME, "Boss_bullet", EntityType.BOSS_BULLET, damage);
            return bullet;
        }

        public Bullet Shoot(Vector2 position, int i)
        {
            int spread = int.Parse(game.constants.getValue("BOSS", "SHOTGUNSPREAD"));
            velocity = getVelocity(i, spread);
            float velocitySpread = (float)spread / 500f;

            switch (i)
            {
                case 0:
                    position.Y -= spread * 2;
                    break;
                case 1:
                    position.Y -= spread;
                    position.X += spread * 2;
                    velocity.X *= (1f + velocitySpread);
                    break;
                case 2:
                    position.X += spread * 3;
                    velocity.X *= (1f + velocitySpread * 2);
                    break;
                case 3:
                    position.Y += spread;
                    position.X += spread * 2;
                    velocity.X *= (1f + velocitySpread);
                    break;
                case 4:
                    position.Y += spread * 2;
                    break;
            }

            Bullet bullet = new Bullet(spriteBatch, game, position, velocity, Constants.BOSS_BULLET_FILENAME, "Boss_bullet", EntityType.BOSS_BULLET, damage);
            bullet.bigBullet = true;
            return bullet;
        }

        public override Vector2 getPosition(Vector2 position)
        {
            throw new NotImplementedException();
        }

        public override Vector2 getVelocity()
        {
            throw new NotImplementedException();
        }

		public Vector2 getVelocity(float i, float s)
        {
            Vector2 newVelocity = new Vector2(float.Parse(game.constants.getValue("BOSS", "SHOTGUNSPEED")), - s / 2f + i * (s / 4f));
            return newVelocity;
        }
    }
}
