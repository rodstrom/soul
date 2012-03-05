using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class ImageButton : Widget
    {
        public delegate void ButtonEventHandler(ImageButton button);

        private Sprite button;
        private Sprite hover;
        private InputManager controls;
        public bool fullSize = false;
        private bool decrease = false;
        public bool show = false;
        public float glowScale = 0.0f;
        private float glowScalePercentage = 0.05f;
        private float glowScaleMinimal = 0.002f;
        private Vector2 offset = Vector2.Zero;
        private Vector2 glowOffset = Vector2.Zero;
        public event ButtonEventHandler onClick = null;

        public ImageButton(SpriteBatch spriteBatch, Soul game, InputManager controls, Vector2 position, string filename, string id)
            : base(id)
        {
            this.controls = controls;
            this.position = position;
            button = new Sprite(spriteBatch, game, filename);
            hover = new Sprite(spriteBatch, game, Constants.MENU_HOVER);
            offset = button.Dimension * 0.5f;
            glowOffset = hover.Dimension * 0.5f;
            glowOffset.Y -= 6f;
        }

        public override void Update(GameTime gameTime)
        {
            if (Focus == true)
            {
                if (glowScale <= 1.0f && fullSize == false)
                {
                    glowScale += glowScalePercentage;
                }
                else if (glowScale >= 1.0f && fullSize == false)
                {
                    fullSize = true;
                }

                if (fullSize == true)
                {
                    if (decrease == true)
                    {
                        glowScale -= glowScaleMinimal;
                        if (glowScale <= 0.85f)
                        {
                            decrease = false;
                        }
                    }
                    else
                    {
                        glowScale += glowScaleMinimal;
                        if (glowScale >= 1.0f)
                        {
                            decrease = true;
                        }
                    }
                }

            }
            else
            {
                if (glowScale >= 0.0f)
                {
                    glowScale -= glowScalePercentage;
                }
                else if (show == true)
                {
                    show = false;
                }
            }

            if (controls.ShootingOnce == true && Focus == true)
            {
                if (onClick != null)
                {
                    onClick(this);
                }
            }
        }

        public override void Draw()
        {
            if (show == true)
            {
                hover.Draw(position, Color.White, 0.0f, glowOffset, glowScale, SpriteEffects.None, 0.0f);
            }
            button.Draw(position, Color.White, 0.0f, offset, 1.0f, SpriteEffects.None, 0.0f);
        }

        public Vector2 ActualPosition
        {
            get
            {
                Vector2 newPosition = position;
                newPosition.X -= ((float)hover.X * 0.5f) * glowScale;
                newPosition.Y -= ((float)hover.Y * 0.5f) * glowScale;
                return newPosition;
            }
        }

        public Vector2 Position
        {
            get
            {
                Vector2 newPosition = position;
                newPosition.X -= (float)button.X * 0.5f;
                newPosition.Y -= (float)button.Y * 0.5f;
                return newPosition;
            }
        }
    }
}
