using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class PlayerShootAnimation : Animation
    {

        enum AnimState
        {
            APPEARING = 0,
            DISAPPEARING,
            SHOOTING
        };

        private Sprite sprite = null;
        private Vector2 dimension = Vector2.Zero;
        private AnimState animState = AnimState.APPEARING;
        private bool shooting = false;

        public PlayerShootAnimation(SpriteBatch spriteBatch, Soul game)
        {
            sprite = new Sprite(spriteBatch, game, Constants.PLAYER_SHOOT_ANIM);
            dimension = new Vector2(Constants.PLAYER_SHOOT_DIMENSION);
            FrameRate = 50;
        }

        public void Update(GameTime gameTime, InputManager controls)
        {

            if (shooting == true)
            {
                AnimationControl(controls);
            }
            Animate(gameTime);
        }

        public void Draw(Vector2 position)
        {
            if (shooting == true)
            {
                Rectangle rect = new Rectangle(CurrentFrame * (int)dimension.X, (int)animState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
                sprite.Draw(position, rect, Color.White, 0f, new Vector2(0f), 1f, SpriteEffects.None, 0f);
            }
        }

        private void AnimationControl(InputManager controls)
        {
            if (animState == AnimState.APPEARING && CurrentFrame == MaxFrames)
            {
                MaxFrames = 0;
                CurrentFrame = 0;
                animState = AnimState.SHOOTING;
            }
            else if (animState == AnimState.SHOOTING && controls.Shooting == false)
            {
                animState = AnimState.DISAPPEARING;
                MaxFrames = 9;
            }
            else if (animState == AnimState.DISAPPEARING && CurrentFrame == MaxFrames)
            {
                shooting = false;
            }
        }

        public void Shoot()
        {
            if (shooting == false)
            {
                MaxFrames = 8;
                shooting = true;
                animState = AnimState.APPEARING;
            }
        }
    }
}
