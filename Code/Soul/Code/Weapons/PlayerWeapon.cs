using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class PlayerWeapon : Weapon
    {
        private int weaponLevel = 0;
        private int weaponSpreadModifier = 40;
        private int shootDelay = 50;
        private uint timer = 0;
        private Random random = new Random();

        public PlayerWeapon(SpriteBatch spriteBatch, Soul game, int spriteHeight)
            : base(spriteBatch, game, spriteHeight)
        {
            damage = Constants.PLAYER_WEAPON_DAMAGE;
        }

        public override Bullet Shoot(Vector2 position)
        {
            if (timer <= shootDelay)
            {
                return null;
            }
            else
            {
                timer = 0;
            }
            
            velocity = getVelocity();
            position = getPosition(position);
            Bullet bullet = new Bullet(spriteBatch, game, position, velocity, Constants.PLAYER_BULLET_FILENAME, "bullet", EntityType.PLAYER_BULLET, damage);
            return bullet;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (uint)gameTime.ElapsedGameTime.Milliseconds;
            if (weaponLevel < Constants.WEAPON_LEVEL_LOWEST)
            {
                weaponLevel = Constants.WEAPON_LEVEL_LOWEST;
            }
            else if (weaponLevel > Constants.WEAPON_LEVEL_HIGHEST)
            {
                weaponLevel = Constants.WEAPON_LEVEL_HIGHEST;
            }
        }

        public override Vector2 getVelocity()
        {
            Vector2 newVelocity = Vector2.Zero;
            if (weaponLevel >= 0 && weaponLevel <= 1)
            {
                newVelocity = new Vector2(-Constants.BULLET_VELOCITY, 0.0f);
            }
            else if (weaponLevel == 2)
            {
                float newY = NextFloat(random, -1.0f, 1.0f);
                newVelocity = new Vector2(-Constants.BULLET_VELOCITY, newY);
            }
            else if (weaponLevel >= 3 && weaponLevel <= 4)
            {
                float newY = NextFloat(random, -2.0f, 2.0f);
                newVelocity = new Vector2(-Constants.BULLET_VELOCITY, newY);
            }
            else if (weaponLevel == 5)
            {
                float newY = NextFloat(random, -3.0f, 3.0f);
                newVelocity = new Vector2(-Constants.BULLET_VELOCITY, newY);
            }
            return newVelocity;
        }

        public override Vector2 getPosition(Vector2 position)
        {
            Vector2 newPosition = position;
            newPosition.Y = random.Next((int)newPosition.Y - (spriteHeight / 2) + weaponSpreadModifier, (int)newPosition.Y + (spriteHeight / 2) - weaponSpreadModifier);
            return newPosition;
        }

        public void IncreaseWeaponLevel()
        {
            weaponLevel++;
            if (weaponLevel > Constants.WEAPON_LEVEL_HIGHEST)
            {
                weaponLevel = Constants.WEAPON_LEVEL_HIGHEST;
            }
            ModifyWeaponStats();
        }

        public void DecreaseWeaponLevel()
        {
            weaponLevel--;
            if (weaponLevel < Constants.WEAPON_LEVEL_LOWEST)
            {
                weaponLevel = Constants.WEAPON_LEVEL_LOWEST;
            }
            ModifyWeaponStats();
        }

        private float NextFloat(Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        private void ModifyWeaponStats()
        {
            if (weaponLevel == 0)
            {
                weaponSpreadModifier = 40;
                shootDelay = 60;
            }
            else if (weaponLevel == 1)
            {
                weaponSpreadModifier = 35;
                shootDelay = 50;
            }
            else if (weaponLevel == 2)
            {
                weaponSpreadModifier = 30;
                shootDelay = 40;
            }
            else if (weaponLevel == 3)
            {
                weaponSpreadModifier = 25;
                shootDelay = 30;
            }
            else if (weaponLevel == 4)
            {
                weaponSpreadModifier = 20;
                shootDelay = 20;
            }
            else if (weaponLevel == 5)
            {
                weaponSpreadModifier = 15;
                shootDelay = 10;
            }

        }

        public int WeaponLevel
        {
            get { return weaponLevel; }
            set { weaponLevel = value; }
        }
    }
}
