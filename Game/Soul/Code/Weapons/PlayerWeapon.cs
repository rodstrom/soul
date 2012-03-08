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
        private float weaponSpreadModifier = 40f;
        private float minYVel = 0f;
        private float maxYVel = 0f;

        //private int shootDelay = 50;
        //private uint timer = 0;
        private Random random = new Random();

        public PlayerWeapon(SpriteBatch spriteBatch, Soul game, int spriteHeight)
            : base(spriteBatch, game, spriteHeight)
        {
            damage = Constants.PLAYER_WEAPON_DAMAGE;
        }

        public override Bullet Shoot(Vector2 position)
        {
            /*if (timer <= shootDelay)
            {
                return null;
            }
            else
            {
                timer = 0;
            }*/
            
            velocity = getVelocity();
            position = getPosition(position);
            Bullet bullet = new Bullet(spriteBatch, game, position, velocity, Constants.PLAYER_BULLET_FILENAME, "bullet", EntityType.PLAYER_BULLET, damage);
            return bullet;
        }

        public override void Update(GameTime gameTime)
        {
            //timer += (uint)gameTime.ElapsedGameTime.Milliseconds;
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
            Vector2 newVelocity = new Vector2(-Constants.BULLET_VELOCITY, NextFloat(random, minYVel, maxYVel));
            /*if (weaponLevel >= 0 && weaponLevel <= 1)
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
            }*/
            return newVelocity;
        }

        public override Vector2 getPosition(Vector2 position)
        {
            Vector2 newPosition = position;
            newPosition.Y = random.Next((int)newPosition.Y - (spriteHeight / 2) + (int)weaponSpreadModifier, (int)newPosition.Y + (spriteHeight / 2) - (int)weaponSpreadModifier);
            return newPosition;
        }

        public void IncreaseWeaponLevel()
        {
            minYVel -= 0.5f;
            maxYVel += 0.5f;
        }

        public void DecreaseWeaponLevel()
        {
            if (weaponSpreadModifier >= 40.0f) weaponSpreadModifier -= weaponSpreadModifier * 0.05f;
            
            if (minYVel < 0.0f) minYVel += 0.05f;

            if (maxYVel > 0.0f)maxYVel -= 0.05f;
        }

        private float NextFloat(Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }



        public int WeaponLevel
        {
            get { return weaponLevel; }
            set { weaponLevel = value; }
        }
    }
}
