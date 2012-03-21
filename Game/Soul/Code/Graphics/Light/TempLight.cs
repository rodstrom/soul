using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class TempLight
    {
        public PointLight pointLight = null;
        private bool scalePowerUp = true;
        private bool scalePowerDown = false;
        private float powerScalar = 0f;
        private float minPower = 0f;
        private float maxPower = 0f;

        public TempLight(Soul game, Vector3 position, float powerScalar, Vector4 colorValue, int lightDecay, float power, bool specular, float minPower, float maxPower)
        {
            this.minPower = minPower;
            this.maxPower = maxPower;
            this.powerScalar = powerScalar;
            pointLight = new PointLight()
            {
                Color = colorValue,
                LightDecay = lightDecay,
                Power = power,
                Position = position,
                IsEnabled = true,
                renderSpecular = specular
            };
        }

        public void Update(GameTime gameTime)
        {
            if (pointLight.Power <= -0.1f)
            {
                pointLight.dead = true;
                return;
            }

            if (scalePowerUp == true)
            {
                pointLight.Power += powerScalar;
                if (pointLight.Power >= 0.1f)
                {
                    scalePowerUp = false;
                    scalePowerDown = true;
                }
            }
            else if (scalePowerDown == true)
            {
                pointLight.Power -= powerScalar;
            }
        }
    }
}
