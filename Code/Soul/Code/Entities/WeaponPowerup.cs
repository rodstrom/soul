using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class WeaponPowerup : Entity
    {

        public WeaponPowerup(SpriteBatch spriteBatch, Soul game, string alias, Vector2 position)
            : base(spriteBatch, game, Constants.WEAPON_POWERUP_FILENAME, new Vector2(Constants.WEAPON_DIMENSION), alias, EntityType.WEAPON_POWERUP)
        {
            this.position = position;
            velocity.X = 1.0f;
            this.hitRadius = Constants.WEAPON_POWERUP_RADIUS;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity; 
        }

        public override void Draw()
        {
            sprite.Draw(position, Color.White, 0.0f, offset, 1.0f, SpriteEffects.None, 0.0f);
        }


        public override void onCollision(Entity entity)
        {
            if (entity.Type == EntityType.PLAYER)
            {
                killMe = true;
            }
        }
        public override void takeDamage(int value) { }
        public override int getDamage() { return 0; }
    }
}
