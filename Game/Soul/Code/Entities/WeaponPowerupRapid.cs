using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Debug;

namespace Soul
{
    class WeaponPowerupRapid : Entity
    {

        public WeaponPowerupRapid(SpriteBatch spriteBatch, Soul game, string alias, Vector2 position)
            : base(spriteBatch, game, Constants.WEAPON_POWERUP_RAPID_FILENAME, new Vector2(Constants.WEAPON_POWERUP_RAPID_DIMENSION), alias, EntityType.WEAPON_POWERUP_RAPID)
        {
            animation.MaxFrames = 4;
            this.position = position;
            velocity.X = 1.0f;
            this.hitRadius = Constants.WEAPON_POWERUP_RAPID_RADIUS;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            animation.Animate(gameTime);
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

            Rectangle rect = new Rectangle(animation.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
            sprite.Draw(position, rect, Color.White, rotation, offset, scale, SpriteEffects.None, layer);
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
