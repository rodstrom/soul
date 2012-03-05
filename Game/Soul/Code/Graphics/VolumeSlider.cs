using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Soul.Manager;

namespace Soul
{
    class VolumeSlider : Widget
    {
        private Sprite slider = null;
        private Sprite marker = null;
        private Vector2 sliderOffset = Vector2.Zero;
        private Vector2 markerOffset = Vector2.Zero;
        private AudioManager audioManager = null;


        public VolumeSlider(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, Vector2 position, string id)
            : base(id)
        {
            if (audioManager != null)
            {
                this.audioManager = audioManager;
            }
            slider = new Sprite(spriteBatch, game, Constants.GUI_SLIDER);
            marker = new Sprite(spriteBatch, game, Constants.GUI_SLIDER_MARKER);
            sliderOffset = slider.Dimension * 0.5f;
            markerOffset = marker.Dimension * 0.5f;
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            Vector2 newPos = position;
            newPos.X -= slider.X * 0.35f;

            if (audioManager != null)
            {
                newPos.X += (float)Math.Round(audioManager.getFXVolume() * 210f);
            }
            else
            {
                newPos.X += (float)Math.Round(MediaPlayer.Volume * 210f);
            }
            marker.Draw(newPos, new Color(alpha, alpha, alpha, alpha), 0f, markerOffset, 1f, SpriteEffects.None, 0f);
            slider.Draw(position, new Color(alpha, alpha, alpha, alpha), 0f, sliderOffset, 1f, SpriteEffects.None, 0f);
        }
    }

/*spriteBatch.Draw(marker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 210, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 150 - 43, 420, 100), Color.White);
spriteBatch.Draw(slider, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + 150, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 150 - 15, 320, 48), Color.White);
spriteBatch.Draw(sliderMarker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + 184 + (int)Math.Round(audio.getFXVolume() * 210), Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 150 - 6, 30, 30), Color.White);*/
}
