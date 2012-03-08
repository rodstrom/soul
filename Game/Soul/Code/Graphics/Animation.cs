using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Soul
{
    class Animation : IDisposable
    {
        private bool oscillate = false;
        private bool reverse = false;
        private int maxFrames = 0;
        private int currentFrame = 0;
        private int oldTime = 0;
        private int frameInc = 1;
        private int frameRate = 100;
        public bool playOnce = false;

        public Animation(int value)
        {
            this.maxFrames = value;
        }

        public Animation() {}

        public void Animate(GameTime gameTime)
        {
            if (oldTime + frameRate > gameTime.TotalGameTime.TotalMilliseconds)
            {
                return;
            }

            oldTime = (int)gameTime.TotalGameTime.TotalMilliseconds;

            if (reverse == false)
            {
                currentFrame += frameInc;
            }
            else
            {
                currentFrame -= frameInc;
            }

            if (oscillate == true)
            {
                if (frameInc > 0)
                {
                    if (currentFrame >= maxFrames - 1)
                    {
                        frameInc = -frameInc;
                    }
                }
                else
                {
                    if (currentFrame <= 0)
                    {
                        frameInc = -frameInc;
                    }
                }
            }
            else if (currentFrame > maxFrames && reverse == false)
            {
                currentFrame = 0;
                if (playOnce == true)
                {
                    currentFrame = maxFrames;
                }
            }
            else if (currentFrame < 0 && reverse == true)
            {
                currentFrame = maxFrames;
                if (playOnce == true)
                {
                    currentFrame = 0;
                }
            }

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool Oscillate
        {
            get { return oscillate; }
            set { oscillate = value; }
        }

        public int MaxFrames
        {
            get { return maxFrames; }
            set { maxFrames = value; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public int FrameRate
        {
            get { return frameRate; }
            set { frameRate = value; }
        }

        public bool Reverse
        {
            get { return reverse; }
            set { reverse = value; }
        }
    }
}
