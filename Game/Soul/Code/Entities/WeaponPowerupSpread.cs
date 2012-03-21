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
            : base(spriteBatch, game, Constants.WEAPON_POWERUP_SPREAD_FILENAME, new Vector2(Constants.WEAPON_POWERUP_SPREAD_DIMENSION), alias, EntityType.WEAPON_POWERUP_SPREAD)
        {
            animation.MaxFrames = 6;
            this.position = position;
            velocity.X = 1.0f;
            this.hitRadius = Constants.WEAPON_POWERUP_SPREAD_RADIUS;

            pointLight = new PointLight()
            {
                Color = new Vector4(float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorR")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorG")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorB")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorA"))),
                Power = float.Parse(game.lighting.getValue("SpreadPowerUp", "Power")),
                LightDecay = int.Parse(game.lighting.getValue("SpreadPowerUp", "LightDecay")),
                Position = new Vector3(0f, 0f, float.Parse(game.lighting.getValue("SpreadPowerUp", "ZPosition"))),
                IsEnabled = true,
                renderSpecular = bool.Parse(game.lighting.getValue("SpreadPowerUp", "Specular"))
            };
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            animation.Animate(gameTime);
            pointLight.Position = new Vector3(position.X, position.Y, pointLight.Position.Z);
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
