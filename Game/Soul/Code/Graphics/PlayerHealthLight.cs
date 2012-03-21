using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class PlayerHealthLight
    {
        enum FadeToColor
        {
            NONE = 0,
            HEALTH,
            SPREAD,
            RAPID
        }

        private PointLight pointLight = null;
        private Vector4 healthColor = Vector4.Zero;
        private Vector4 spreadColor = Vector4.Zero;
        private Vector4 rapidColor = Vector4.Zero;
        private Soul game = null;
        private int maxRadius = 0;
        private int minRadius = 0;
        private int flashScalar = 0;
        private int playerMaxHealth = 0;
        private int currentLightRadius = 0;
        private int currentHealth = 0;
        private int colorScalar = 0;
        private int flashScaleTarget = 0;
        private float oldHealthPercentage = 0f;
        private bool repeatFlicker = false;
        private bool enemyHit = false;
        private bool powerUpActive = false;
        private int enemyHitScaleState = 0;
        private int powerUpHitScaleState = 0;
        private bool powerUpHit = false;
        private double timer = 0.0;
        private FadeToColor fadeToColor = FadeToColor.NONE;
        private EntityType pickedUpPowerUp = EntityType.NONE;
        

        public PlayerHealthLight(Soul game, int health)
        {
            this.game = game;
            this.flashScalar = int.Parse(game.lighting.getValue("PlayerHealthLight", "FlashScalar"));
            this.maxRadius = int.Parse(game.lighting.getValue("PlayerHealthLight", "MaxRadius"));
            this.minRadius = int.Parse(game.lighting.getValue("PlayerHealthLight", "MinRadius"));
            this.playerMaxHealth = int.Parse(game.constants.getValue("PLAYER", "MAXHEALTH"));
            this.healthColor = new Vector4(float.Parse(game.lighting.getValue("PlayerHealthLight", "ColorR")), float.Parse(game.lighting.getValue("PlayerHealthLight", "ColorG")), float.Parse(game.lighting.getValue("PlayerHealthLight", "ColorB")), float.Parse(game.lighting.getValue("PlayerHealthLight", "ColorA")));
            this.spreadColor = new Vector4(float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorR")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorG")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorB")), float.Parse(game.lighting.getValue("SpreadPowerUp", "ColorA")));
            this.rapidColor = new Vector4(float.Parse(game.lighting.getValue("RapidPowerUp", "ColorR")), float.Parse(game.lighting.getValue("RapidPowerUp", "ColorG")), float.Parse(game.lighting.getValue("RapidPowerUp", "ColorB")), float.Parse(game.lighting.getValue("RapidPowerUp", "ColorA")));
            this.colorScalar = 8;
            this.currentLightRadius = maxRadius;
            this.currentHealth = health;
            pointLight = new PointLight()
            {
                Color = healthColor,
                Power = float.Parse(game.lighting.getValue("PlayerHealthLight", "Power")),
                LightDecay = int.Parse(game.lighting.getValue("PlayerHealthLight", "MaxRadius")),
                Position = new Vector3(0f, 0f, float.Parse(game.lighting.getValue("PlayerHealthLight", "ZPosition"))),
                IsEnabled = true,
                renderSpecular = bool.Parse(game.lighting.getValue("PlayerHealthLight", "Specular"))
            };
            updateHealthStatus();
        }

        public PointLight PointLight { get { return pointLight; } }

        public void Update(GameTime gameTime, int currentHealth, Vector2 playerPosition)
        {
            this.pointLight.Position = new Vector3(playerPosition.X, playerPosition.Y, pointLight.Position.Z);
            this.currentHealth = currentHealth;

            if (powerUpActive == true)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer >= 10000.0 && timer < 14999.0)
                {
                    if (timer >= 13.000)
                        colorScalar = 12;
                    repeatFlicker = true;
                }
                else if (timer >= 15000.0)
                {
                    repeatFlicker = false;
                    fadeToColor = FadeToColor.HEALTH;
                    colorScalar = 8;
                }
            }

            if (fadeToColor != FadeToColor.NONE)
            {
                FadeToLight();
            }

            if (enemyHit == true)
            {
                EnemyHitScale();
            }
            else if (powerUpHit == true)
            {
                PowerUpHitScale();
            }

        }

        public void updateHealthStatus()
        {
            float percentage = (float)currentHealth / (float)playerMaxHealth;

            if (percentage > 9.0f)
            {
                if (oldHealthPercentage != 1.0f)
                {
                    oldHealthPercentage = 1.0f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.9f && percentage > 0.8f)
            {
                if (oldHealthPercentage != 0.9f)
                {
                    oldHealthPercentage = 0.9f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.8f && percentage > 0.7f)
            {
                if (oldHealthPercentage != 0.8f)
                {
                    oldHealthPercentage = 0.8f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.7f && percentage > 0.6f)
            {
                if (oldHealthPercentage != 0.7f)
                {
                    oldHealthPercentage = 0.7f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.6f && percentage > 0.5f)
            {
                if (oldHealthPercentage != 0.6f)
                {
                    oldHealthPercentage = 0.6f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.5f && percentage > 0.4f)
            {
                if (oldHealthPercentage != 0.5f)
                {
                    oldHealthPercentage = 0.5f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.4f && percentage > 0.3f)
            {
                if (oldHealthPercentage != 0.4f)
                {
                    oldHealthPercentage = 0.4f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.3f && percentage > 0.2f)
            {
                if (oldHealthPercentage != 0.3f)
                {
                    oldHealthPercentage = 0.3f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.2f && percentage > 0.1f)
            {
                if (oldHealthPercentage != 0.2f)
                {
                    oldHealthPercentage = 0.2f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            else if (percentage < 0.1f && percentage > 0.0f)
            {
                if (oldHealthPercentage != 0.1f)
                {
                    oldHealthPercentage = 0.1f;
                    float newLightRadie = (float)maxRadius * oldHealthPercentage;
                    currentLightRadius = (int)newLightRadie;
                }
            }
            if (powerUpHit == true)
            {
                powerUpHitScaleState = 0;
                powerUpHit = false;
            }

            enemyHit = true;

            pointLight.LightDecay = currentLightRadius;
            float newRadiusTarget = (float)currentLightRadius * 0.8f;
            flashScaleTarget = (int)newRadiusTarget;
        }

        public void PowerUpHit(Entity entity)
        {
            if (entity.Type == EntityType.WEAPON_POWERUP_SPREAD)
            {
                fadeToColor = FadeToColor.SPREAD;
                pickedUpPowerUp = EntityType.WEAPON_POWERUP_SPREAD;
                powerUpActive = true;
                timer = 0.0;
                if (repeatFlicker == true)
                    repeatFlicker = false;

            }
            else if (entity.Type == EntityType.WEAPON_POWERUP_RAPID)
            {
                fadeToColor = FadeToColor.RAPID;
                pickedUpPowerUp = EntityType.WEAPON_POWERUP_RAPID;
                powerUpActive = true;
                timer = 0.0;
                if (repeatFlicker == true)
                    repeatFlicker = false;
            }
            if (enemyHit == true)
            {
                enemyHitScaleState = 0;
                enemyHit = false;
            }

            pointLight.LightDecay = currentLightRadius;
            powerUpHit = true;
            float newRadiusTarget = (float)currentLightRadius * 1.4f;
            flashScaleTarget = (int)newRadiusTarget;
        }

        private void EnemyHitScale()
        {
            switch (enemyHitScaleState)
            {
                case 0:
                    {
                        pointLight.LightDecay -= flashScalar;
                        if (pointLight.LightDecay <= flashScaleTarget)
                        {
                            pointLight.LightDecay = flashScaleTarget;
                            enemyHitScaleState++;
                        }
                        break;
                    }
                case 1:
                    {
                        pointLight.LightDecay += flashScalar;
                        if (pointLight.LightDecay >= currentLightRadius)
                        {
                            pointLight.LightDecay = currentLightRadius;
                            enemyHitScaleState++;
                        }
                        break;
                    }
                case 2:
                    {
                        enemyHit = false;
                        enemyHitScaleState = 0;
                        break;
                    }
            }
        }

        private void PowerUpHitScale()
        {
            switch (powerUpHitScaleState)
            {
                case 0:
                    {
                        pointLight.LightDecay += flashScalar;
                        if (pointLight.LightDecay >= flashScaleTarget)
                        {
                            pointLight.LightDecay = flashScaleTarget;
                            powerUpHitScaleState++;
                        }
                        break;
                    }
                case 1:
                    {
                        pointLight.LightDecay -= flashScalar;
                        if (pointLight.LightDecay <= currentLightRadius)
                        {
                            pointLight.LightDecay = currentLightRadius;
                            powerUpHitScaleState++;
                        }
                        break;
                    }
                case 2:
                    {
                        powerUpHitScaleState = 0;
                        powerUpHit = false;
                        break;
                    }
            }
        }

        private void FadeToLight()
        {
            switch (fadeToColor)
            {
                case FadeToColor.HEALTH:
                    {
                        if (LightFade(healthColor) == true && repeatFlicker == false)
                            fadeToColor = FadeToColor.NONE;

                        break;
                    }
                case FadeToColor.RAPID:
                    {
                        if (LightFade(rapidColor) == true && false)
                            fadeToColor = FadeToColor.NONE;
                        break;
                    }
                case FadeToColor.SPREAD:
                    {
                        if (LightFade(spreadColor) == true && false)
                            fadeToColor = FadeToColor.NONE;
                        break;
                    }
            }
        }

        private bool LightFade(Vector4 color)
        {
            if (pointLight.Color.X < color.X)
            {
                pointLight.Color.X += colorScalar;
                if (pointLight.Color.X > color.X)
                    pointLight.Color.X = color.X;
            }
            else if (pointLight.Color.X > color.X)
            {
                pointLight.Color.X -= colorScalar;
                if (pointLight.Color.X < color.X)
                    pointLight.Color.X = color.X;
            }

            if (pointLight.Color.Y < color.Y)
            {
                pointLight.Color.Y += colorScalar;
                if (pointLight.Color.Y > color.Y)
                    pointLight.Color.Y = color.Y;
            }
            else if (pointLight.Color.Y > color.Y)
            {
                pointLight.Color.Y -= colorScalar;
                if (pointLight.Color.Y < color.Y)
                    pointLight.Color.Y = color.Y;
            }

            if (pointLight.Color.Z < color.Z)
            {
                pointLight.Color.Z += colorScalar;
                if (pointLight.Color.Z > color.Z)
                    pointLight.Color.Z = color.Z;
            }
            else if (pointLight.Color.Z > color.Z)
            {
                pointLight.Color.Z -= colorScalar;
                if (pointLight.Color.Z < color.Z)
                    pointLight.Color.Z = color.Z;
            }

            if (pointLight.Color.W < color.W)
            {
                pointLight.Color.W += colorScalar;
                if (pointLight.Color.W > color.W)
                    pointLight.Color.W = color.W;
            }
            else if (pointLight.Color.W > color.W)
            {
                pointLight.Color.W -= colorScalar;
                if (pointLight.Color.W < color.W)
                    pointLight.Color.W = color.W;
            }



            if (color == pointLight.Color)
            {
                if (repeatFlicker == true)
                {
                    if (fadeToColor == FadeToColor.HEALTH)
                    {
                        if (pickedUpPowerUp == EntityType.WEAPON_POWERUP_RAPID)
                        {
                            fadeToColor = FadeToColor.RAPID;
                            return false;
                        }
                        else if (pickedUpPowerUp == EntityType.WEAPON_POWERUP_SPREAD)
                        {
                            fadeToColor = FadeToColor.SPREAD;
                            return false;
                        }
                    }
                    else if (fadeToColor == FadeToColor.RAPID || fadeToColor == FadeToColor.SPREAD)
                    {
                        fadeToColor = FadeToColor.HEALTH;
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
