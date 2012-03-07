using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class BrainMapMarker : Widget
    {
        public delegate void ButtonEventHandler(BrainMapMarker button);
        public event ButtonEventHandler onClick = null;
        private Vector2 offset = Vector2.Zero;
        private Sprite marker = null;
        private Sprite status = null;
        private Sprite statusSecond = null;
        private InputManager controls = null;
        private GlowFX glowFX = null;
        private float scale = 1f;
        private float scaleProcent = 0.008f;
        private bool scaleUp = false;
        private bool scaleDown = false;
        public new int alpha = 0;
        public int sAlpha = 1;
        public bool scaleStatusUp = false;
        public bool scaleStatusDown = false;
        private new int alphaScaler = 10;
        private bool selected = false;
        private bool appearing = false;
        private bool disappearing = false;
        private bool cleansed = false;

        public BrainMapMarker(SpriteBatch spriteBatch, Soul game, InputManager controls, Vector2 position, string filename, string id, bool cleansed) : base(id)
        {
            this.cleansed = cleansed;
            marker = new Sprite(spriteBatch, game, filename);
            this.controls = controls;
            this.position = position;
            offset = new Vector2((float)marker.X * 0.5f, (float)marker.Y * 0.5f);
            status = new Sprite(spriteBatch, game, Constants.BRAIN_MAP_STATUS);
            if (cleansed == true)
            {
                statusSecond = new Sprite(spriteBatch, game, Constants.BRAIN_MAP_CLEANSED);
                glowFX = new GlowFX(game, Constants.FLASH_EFFECT_GREEN_FILENAME, 0.01f, .4f, .8f);
            }
            else
            {
                statusSecond = new Sprite(spriteBatch, game, Constants.BRAIN_MAP_INFECTED);
                glowFX = new GlowFX(game, Constants.FLASH_EFFECT_RED_FILENAME, 0.01f, .4f, .8f);
            }

        }

        public override void Update(GameTime gameTime)
        {
            
            if (scaleStatusUp == true)
            {
                sAlpha += (alphaScaler);
                if (sAlpha >= 255)
                {
                    sAlpha = 255;
                    scaleStatusUp = false;
                }
            }
            else if (scaleStatusDown == true)
            {
                sAlpha -= (alphaScaler);
                if (sAlpha <= 0)
                {
                    sAlpha = 0;
                    scaleStatusUp = false;
                }
            }

            if (hasFocus == true && disappearing == false)
            {
                glowFX.Update();
            }

            if (disappearing == true || appearing == true)
            {
                FadeInOut();
            }

            if (hasFocus == true && controls.ShootingOnce == true && selected == false)
            {
                if (onClick != null)
                {
                    onClick(this);
                    selected = true;
                    scaleUp = true;
                    scaleStatusUp = true;
                    scaleStatusDown = false;
                }
            }

            if (scaleUp == true || scaleDown == true)
            {
                Scale();
            }
        }

        public override void Draw()
        {
            if (alpha > 0)
            {
                if (selected == true || disappearing == true || appearing == true)
                {
                    marker.Draw(position, new Color(alpha, alpha, alpha, alpha), 0f, offset, scale, SpriteEffects.None, 0f);
                    if (sAlpha > 0)
                    {
                        status.Draw(new Vector2(50f, 50f), new Color(sAlpha, sAlpha, sAlpha, sAlpha), 0f, new Vector2(0f), 1f, SpriteEffects.None, 0f);
                        statusSecond.Draw(new Vector2(50f + (float)status.X, 50f), new Color(sAlpha, sAlpha, sAlpha, sAlpha), 0f, new Vector2(0f), 1f, SpriteEffects.None, 0f);
                    }
                }
                else
                {
                    marker.Draw(position, new Color(alpha, alpha, alpha, alpha), 0f, offset, scale, SpriteEffects.None, 0f, glowFX.Effect);
                    if (sAlpha > 0)
                    {
                        status.Draw(new Vector2(50f, 50f), new Color(sAlpha, sAlpha, sAlpha, sAlpha), 0f, new Vector2(0f), 1f, SpriteEffects.None, 0f);
                        statusSecond.Draw(new Vector2(50f + (float)status.X, 50f), new Color(sAlpha, sAlpha, sAlpha, sAlpha), 0f, new Vector2(0f), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        public bool Scaling { get { return scaleUp; } }
        public bool Selected { get { return selected; } }
        public void Appear() 
        {
            disappearing = false;
            appearing = true; 
        }
        public void Disappear() 
        {
            appearing = false;    
            disappearing = true; 
        }

        /*private void GlowControl()
        {
            if (glowValue > glowMax)
            {
                glowing = false;
            }
            else if (glowValue <= 0.3f)
            {
                glowing = true;
            }

            if (glowing == true)
            {
                glowValue += glowScaler;
                glow.Parameters["Percentage"].SetValue(glowValue);
            }
            else if (glowing == false)
            {
                glowValue -= glowScaler;
                glow.Parameters["Percentage"].SetValue(glowValue);
            }
        }*/

        private void FadeInOut()
        {
            if (disappearing == true)
            {
                alpha -= alphaScaler;
                if (alpha <= 0)
                {
                    disappearing = false;
                    alpha = 0;
                }
            }
            else if (appearing == true)
            {
                alpha += alphaScaler;
                if (alpha >= 255)
                {
                    appearing = false;
                    alpha = 255;
                }
            }
        }

        private void Scale()
        {
            if (scaleUp == true)
            {
                scale += scaleProcent;
                if (scale >= 1.15f)
                {
                    scaleUp = false;
                    scale = 1.15f;
                }
            }
            else if (scaleDown == true)
            {
                scaleStatusDown = true;
                scale -= scaleProcent;
                if (scale <= 1f)
                {
                    scaleDown = false;
                    scale = 1f;
                    selected = false;
                }
            }
        }

        public void Deselect()
        {
            scaleDown = true;
        }

    }
}
