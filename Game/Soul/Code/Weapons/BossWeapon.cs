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
        public BossWeapon(SpriteBatch spriteBatch, Soul game, int spriteHeight)
            : base(spriteBatch, game, spriteHeight)
        {
            damage = Constants.BOSS_DAMAGE;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        public override Bullet Shoot(Vector2 position, int colorValue = 0)
        {
            Bullet bullet = new Bullet(spriteBatch, game, position, new Vector2(15.0f, 0.0f), Constants.BOSS_BULLET_FILENAME, "Boss_bullet", EntityType.BOSS_BULLET, damage);
            return bullet;
        }

        public Bullet Shoot(Vector2 position, float angle)
        {
            Bullet bullet = new Bullet(spriteBatch, game, position, new Vector2(15.0f, angle * 20f), Constants.BOSS_BULLET_FILENAME, "Boss_bullet", EntityType.BOSS_BULLET, damage);
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
    }
}
