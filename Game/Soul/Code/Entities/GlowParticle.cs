using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class GlowParticle : Entity
    {
        private float glowScale = 1.0f;
        private float glowScalePercentage = 0.05f;
        private bool decrease = true;

        public GlowParticle(SpriteBatch spriteBatch, Soul game, string alias, Vector2 position, Random random)
            : base(spriteBatch, game, Constants.GLOW_PARTICLE_FILENAME, new Vector2(Constants.GLOW_PARTICLE_DIMENSION), alias, EntityType.DIE_PARTICLE)
        {
            maxVelocity = new Vector2(Constants.GLOW_PARTICLE_MAX_SPEED);
            acceleration = new Vector2(Constants.GLOW_PARTICLE_ACCELERATION);
            this.ghost = true;
            this.position = new Vector2(position.X, position.Y);
            float x = (float)random.Next(-150, 150);
            float y = (float)random.Next(-150, 150);
            this.position.X += x;
            this.position.Y += y;
            
            if (x > 0.0f)
            {
                velocity.X = NextFloat(random, 0.0f, 1.0f);
            }
            else if (x < 0.0f)
            {
                velocity.X = NextFloat(random, -1.0f, 0.0f);
            }

            if (y > 0.0f)
            {
                velocity.Y = NextFloat(random, 0.0f, 1.0f);
            }
            else if (y < 0.0f)
            {
                velocity.Y = NextFloat(random, -1.0f, 0.0f);
            }


            pointLight = new PointLight()
            {
                Color = new Vector4(0f, 0f, 0f, 1f),
                Power = 1f,
                LightDecay = 90,
                Position = new Vector3(0f, 0f, 50f),
                IsEnabled = true
            };
        }

        public GlowParticle(SpriteBatch spriteBatch, Soul game, string alias, Vector2 position)
            : base(spriteBatch, game, Constants.GLOW_PARTICLE_FILENAME, new Vector2(Constants.GLOW_PARTICLE_DIMENSION), alias, EntityType.DIE_PARTICLE)
        {
            maxVelocity = new Vector2(Constants.GLOW_PARTICLE_MAX_SPEED);
            acceleration = new Vector2(Constants.GLOW_PARTICLE_ACCELERATION);
            this.ghost = true;
        }

        public GlowParticle(SpriteBatch spriteBatch, Soul game, string alias, Vector2 position, Vector2 velocity)
            : base(spriteBatch, game, Constants.GLOW_PARTICLE_FILENAME, new Vector2(Constants.GLOW_PARTICLE_DIMENSION), alias, EntityType.DIE_PARTICLE)
        {
            this.velocity = velocity;
            this.position = position;
            pointLight = new PointLight()
            {
                Color = new Vector4(float.Parse(game.lighting.getValue("MainMenuLight", "ColorR")), float.Parse(game.lighting.getValue("MainMenuLight", "ColorG")), float.Parse(game.lighting.getValue("MainMenuLight", "ColorB")), float.Parse(game.lighting.getValue("MainMenuLight", "ColorA"))),
                Power = float.Parse(game.lighting.getValue("MainMenuLight", "Power")),
                LightDecay = int.Parse(game.lighting.getValue("MainMenuLight", "LightDecay")),
                Position = new Vector3(0f, 0f, float.Parse(game.lighting.getValue("MainMenuLight", "ZPosition"))),
                IsEnabled = true,
                renderSpecular = bool.Parse(game.lighting.getValue("MainMenuLight", "Specular"))
            };
        }
        private float NextFloat(Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        public override void Update(GameTime gameTime)
        {
            if (pointLight != null)
            {
                pointLight.Position = new Vector3(position.X, position.Y, pointLight.Position.Z);
            }

            if (decrease == true)
            {
                glowScale -= glowScalePercentage;
                pointLight.LightDecay = pointLight.LightDecay - 1;
                if (glowScale <= 0.6f)
                {
                    decrease = false;
                }
            }
            else
            {
                glowScale += glowScalePercentage;
                pointLight.LightDecay = pointLight.LightDecay + 1;
                if (glowScale >= 1.0f)
                {
                    decrease = true;
                }
            }
            position += velocity;
        }

        public override void Draw()
        {
            sprite.Draw(position, Color.White, rotation, offset, glowScale, SpriteEffects.None, layer);
        }

        public override void onCollision(Entity entity){}
        public override void takeDamage(int value) {}
        public override int getDamage(){return 0;}

        

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
    }
}
