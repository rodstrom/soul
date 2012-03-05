using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class GlowFX
    {
        private Effect glow = null;
        public float glowScalar = 0.003f;
        public float glowMax = 0.6f;
        public float glowMin = .1f;
        public float glowFx = .1f;
        public bool glowState = false;

        public GlowFX(Game game)
        {
            glow = game.Content.Load<Effect>(Constants.FLASH_EFFECT_FILENAME);
        }

        public GlowFX(Game game, float glowScalar, float min, float max)
        {
            glow = game.Content.Load<Effect>(Constants.FLASH_EFFECT_FILENAME);
            this.glowScalar = glowScalar;
            this.glowMin = min;
            this.glowMax = max;
        }

        public GlowFX(Game game, string filename, float glowScalar, float min, float max)
        {
            glow = game.Content.Load<Effect>(filename);
            this.glowScalar = glowScalar;
            this.glowMin = min;
            this.glowMax = max;
        }

        public void Update()
        {
            if (glowState == false)
            {
                glowFx += glowScalar;
                if (glowFx >= glowMax)
                {
                    glowState = !glowState;
                    glowFx = glowMax;
                }
                glow.Parameters["Percentage"].SetValue(glowFx);
            }
            else
            {
                glowFx -= glowScalar;
                if (glowFx <= glowMin)
                {
                    glowState = !glowState;
                    glowFx = glowMin;
                }
                glow.Parameters["Percentage"].SetValue(glowFx);
            }
        }

        public Effect Effect { get { return glow; } }



    }
}
