﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class HealthPowerup : Entity
    {
        private float glowScale = 1.0f;
        private float glowScalePercentage = 0.005f;
        private bool decrease = true;

        public HealthPowerup(SpriteBatch spriteBatch, Soul game, string alias, Vector2 position)
            : base(spriteBatch, game, Constants.HEALTH_POWERUP_FILENAME, new Vector2(Constants.HEALTH_POWERUP_DIMENSION), alias, EntityType.HEALTH_POWERUP)
        {
            this.position = position;
            velocity.X = 1.0f;
            this.hitRadius = Constants.HEALTH_POWERUP_RADIUS;
        }

        public override void Update(GameTime gameTime)
        {
            if (decrease == true)
            {
                glowScale -= glowScalePercentage;
                if (glowScale <= 0.6f)
                {
                    decrease = false;
                }
            }
            else
            {
                glowScale += glowScalePercentage;
                if (glowScale >= 1.0f)
                {
                    decrease = true;
                }
            }
            position += velocity;  
        }

        public override void Draw()
        {
            sprite.Draw(position,Color.White, rotation, offset, glowScale, SpriteEffects.None, layer);
        }

        public override void onCollision(Entity entity)
        {
            if (entity.Type == EntityType.PLAYER)
            {
                killMe = true;
            }
        }

        public Vector2 GlowPosition
        {
            get
            {
                Vector2 newPosition = position;
                newPosition.X -= ((float)sprite.X * 0.5f) * glowScale;
                newPosition.Y -= ((float)sprite.Y * 0.5f) * glowScale;
                return newPosition;
            }
        }

        public override void takeDamage(int value) { }
        public override int getDamage() { return 0; }
    }
}