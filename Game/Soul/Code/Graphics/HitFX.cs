using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class HitFX
    {
        private Effect hit;
        private float fx = 0.0f;
        private float fxScale = 0.05f;

        public HitFX(Soul game)
        {
            hit = game.Content.Load<Effect>(Constants.FLASH_EFFECT_FILENAME);
        }

        public HitFX(Soul game, float fxScale)
        {
            hit = game.Content.Load<Effect>(Constants.FLASH_EFFECT_FILENAME);
            this.fxScale = fxScale;
        }

        public void Update()
        {
            if (fx > 0.0f)
            {
                fx -= fxScale;
                hit.Parameters["Percentage"].SetValue(fx);
            }
        }

        public void Hit() { fx = 1.0f; }

        public void Hit(float f) { fx = f; }

        public bool IsHit
        {
            get 
            {
                if (fx > 0.0f)
                {
                    return true;
                }
                return false;
            }
        }

        public Effect Effect { get { return hit;}}
    }
}
