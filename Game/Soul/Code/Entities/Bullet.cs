using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class Bullet : Entity
    {
        private float scalePercentage = 0.03f;
        private bool decrease = true;

        private Vector2 originalPosition = Vector2.Zero;
        private bool useRange = false;

        public bool bigBullet = false;
        public bool spike = false;

        public bool pulsate = true;

        public Bullet(SpriteBatch spriteBatch, Soul game, Vector2 position, Vector2 velocity, string filename, string alias, EntityType type, int damage, int colorType = 0)
            : base(spriteBatch, game, filename, new Vector2(20.0f, 20.0f), alias, type)
        {
            this.position = position;
            this.velocity = velocity;
            this.damage = damage;
            this.hitRadius = Constants.BULLET_RADIUS;
            if (type == EntityType.PLAYER_BULLET)
            {
                pointLight = new PointLight()
                {
                    Color = new Vector4(float.Parse(game.lighting.getValue("PlayerBullet", "ColorR")), float.Parse(game.lighting.getValue("PlayerBullet", "ColorG")), float.Parse(game.lighting.getValue("PlayerBullet", "ColorB")), float.Parse(game.lighting.getValue("PlayerBullet", "ColorA"))),
                    Power = float.Parse(game.lighting.getValue("PlayerBullet", "Power")),
                    LightDecay = int.Parse(game.lighting.getValue("PlayerBullet", "LightDecay")),
                    Position = new Vector3(0f, 0f, float.Parse(game.lighting.getValue("PlayerBullet", "ZPosition"))),
                    IsEnabled = true,
                    renderSpecular = bool.Parse(game.lighting.getValue("PlayerBullet", "Specular"))
                };
                if (colorType == 1)
                {
                    pointLight.Color = new Vector4(float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorR")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorG")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorB")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorA")));
                }
                else if (colorType == 2)
                {
                    pointLight.Color = new Vector4(float.Parse(game.lighting.getValue("RapidPowerUp", "ColorR")), float.Parse(game.lighting.getValue("RapidPowerUp", "ColorG")), float.Parse(game.lighting.getValue("RapidPowerUp", "ColorB")), float.Parse(game.lighting.getValue("RapidPowerUp", "ColorA")));
                }
            }
            else if (type == EntityType.DARK_THOUGHT_BULLET)
            {
                pointLight = new PointLight()
                {
                    Color = new Vector4(float.Parse(game.lighting.getValue("DarkThoughtBullet", "ColorR")), float.Parse(game.lighting.getValue("DarkThoughtBullet", "ColorG")), float.Parse(game.lighting.getValue("DarkThoughtBullet", "ColorB")), float.Parse(game.lighting.getValue("DarkThoughtBullet", "ColorA"))),
                    Power = float.Parse(game.lighting.getValue("DarkThoughtBullet", "Power")),
                    LightDecay = int.Parse(game.lighting.getValue("DarkThoughtBullet", "LightDecay")),
                    Position = new Vector3(0f, 0f, float.Parse(game.lighting.getValue("DarkThoughtBullet", "ZPosition"))),
                    IsEnabled = true,
                    renderSpecular = bool.Parse(game.lighting.getValue("DarkThoughtBullet", "Specular"))
                };
            }
            else if (type == EntityType.BOSS_BULLET)
            {
                pointLight = new PointLight()
                {
                    Color = new Vector4(float.Parse(game.lighting.getValue("BossBullet", "ColorR")), float.Parse(game.lighting.getValue("BossBullet", "ColorG")), float.Parse(game.lighting.getValue("BossBullet", "ColorB")), float.Parse(game.lighting.getValue("BossBullet", "ColorA"))),
                    Power = float.Parse(game.lighting.getValue("BossBullet", "Power")),
                    LightDecay = int.Parse(game.lighting.getValue("BossBullet", "LightDecay")),
                    Position = new Vector3(0f, 0f, float.Parse(game.lighting.getValue("BossBullet", "ZPosition"))),
                    IsEnabled = true,
                    renderSpecular = bool.Parse(game.lighting.getValue("BossBullet", "Specular"))
                };
            }


        }

        public Bullet(Random r, int range, SpriteBatch spriteBatch, Soul game, Vector2 position, Vector2 velocity, string filename, string alias, EntityType type, int damage)
            : base(spriteBatch, game, filename, new Vector2(Constants.DARK_WHISPER_SPIKE_WIDTH, Constants.DARK_WHISPER_SPIKE_HEIGHT), alias, type)
        {
            spikeRange = range;
            useRange = true;
            originalPosition = position;

            spike = true;

            animation.CurrentFrame = r.Next(3);

            this.position = position;
            this.velocity = velocity;
            this.damage = damage;
            this.hitRadius = Constants.BULLET_RADIUS;
            /*if (type == EntityType.PLAYER_BULLET)
            {
                pointLight = new PointLight()
                {
                    Color = new Vector4(1f, 1f, 1f, 1f),
                    Power = 2f,
                    LightDecay = 100,
                    Position = new Vector3(0f, 0f, 70f),
                    IsEnabled = true
                };
            }*/
        }

        public override void Draw()
        {
            if (spike)
            {
                Rectangle rect = new Rectangle(animation.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
                sprite.Draw(position, rect, Color.White, rotation, offset, scale, SpriteEffects.None, layer);
            }
            else
            {
                float newScale = scale;
                if (bigBullet)
                {
                    newScale = scale * 1.2f;
                }
                sprite.Draw(position, Color.White, rotation, offset, newScale, SpriteEffects.None, layer);
            }
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            if (pointLight != null)
            {
                pointLight.Position = new Vector3(position.X, position.Y, pointLight.Position.Z);
            }

            pulsate = !pulsate;

            if (pulsate)
            {
                if (decrease == true)
                {
                    scale -= scalePercentage;

                    if (pointLight != null)
                    {
                        pointLight.LightDecay = pointLight.LightDecay - 1;
                    }

                    if (scale <= 0.8f)
                    {
                        decrease = false;
                    }
                }
                else
                {
                    scale += scalePercentage;

                    if (pointLight != null)
                    {
                        pointLight.LightDecay = pointLight.LightDecay + 1;
                    }

                    if (scale >= 1.0f)
                    {
                        decrease = true;
                    }
                }
            }
        }

        public bool rangeExceded()
        {
            if (useRange)
            {
                if (Math.Abs(originalPosition.X - position.X) > spikeRange)
                {
                    return true;
                }
                if (Math.Abs(originalPosition.Y - position.Y) > spikeRange)
                {
                    return true;
                }
            }
            return false;
        }

        public override void onCollision(Entity entity)
        {
            return;
        }

        public override int getDamage()
        {
            return damage;
        }

        public override void takeDamage(int value)
        {
            return;
        }

        public Vector2 ActualPosition
        {
            get
            {
                Vector2 newPosition = position;
                newPosition.X -= ((float)sprite.X * 0.5f) * scale;
                newPosition.Y -= ((float)sprite.Y * 0.5f) * scale;
                return newPosition;
            }
        }

        public void setRotation(float r)
        {
            rotation = r;
        }
    }
}
