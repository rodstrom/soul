using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Debug;

namespace Soul
{
    class WeaponPowerupSpread : Entity
    {

        public WeaponPowerupSpread(SpriteBatch spriteBatch, Soul game, string alias, Vector2 position)
            : base(spriteBatch, game, Constants.WEAPON_POWERUP_SPREAD_FILENAME, new Vector2(Constants.WEAPON_DIMENSION), alias, EntityType.WEAPON_POWERUP_SPREAD)
        {
            this.position = position;
            velocity.X = 1.0f;
            this.hitRadius = Constants.WEAPON_POWERUP_RADIUS;
            this.scale = 0.3f;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity; 
        }

        public override void Draw()
        {
            if (debug)
            {
                DEBUG_circleLine brush = new DEBUG_circleLine(game.GraphicsDevice);
                brush.CreateCircle(hitRadius, 100);
                brush.Position = position;
                brush.Render(spriteBatch);
            }

            sprite.Draw(position, Color.White, 0.0f, offset, scale, SpriteEffects.None, 0.0f);
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
