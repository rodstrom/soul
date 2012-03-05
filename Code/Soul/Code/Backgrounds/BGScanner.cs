using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class BGScanner
    {
        public delegate void CallDeadEvent();
        public event CallDeadEvent callDead = null;
        private Rectangle rect1, rect2;
        private Vector2 pos = Vector2.Zero;
        private Vector2 pos2 = Vector2.Zero;

        private Rectangle screenSize;
        private Rectangle currentRect;
        private Rectangle nextRect;
        private Vector2 currentPos;
        private Vector2 nextPos;

        private Vector2 dimension;

        private float scrollSpeed = 0.0f;
        private bool startedScrollingLeft = false;
        public bool show = true;
        public bool lateStart = false;

        public BGScanner(Soul game, Vector2 dimension, float scrollSpeed, bool show)
        {
            //screenSize = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            screenSize = new Rectangle(0, 0, Constants.RESOLUTION_VIRTUAL_WIDTH, Constants.RESOLUTION_VIRTUAL_HEIGHT);
            this.dimension = dimension;

            this.scrollSpeed = scrollSpeed;

            if (this.scrollSpeed > 0) // Scroll Right
            {
                rect1 = new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH, 0, Constants.RESOLUTION_VIRTUAL_WIDTH, Constants.RESOLUTION_VIRTUAL_HEIGHT);
                rect2 = new Rectangle((int)dimension.X, 0, 0, Constants.RESOLUTION_VIRTUAL_HEIGHT);
                if (show == false)
                {
                    rect1.Width = 1;
                    rect1.X = (int)dimension.X;
                    lateStart = true;
                }
            }
            else if (this.scrollSpeed < 0)  // Scroll Left
            {
                rect1 = new Rectangle(0, 0, Constants.RESOLUTION_VIRTUAL_WIDTH, Constants.RESOLUTION_VIRTUAL_HEIGHT);
                rect2 = new Rectangle(0, 0, 0, Constants.RESOLUTION_VIRTUAL_HEIGHT);
                pos2.X = (float)Constants.RESOLUTION_VIRTUAL_WIDTH;
                if (show == false)
                {
                    rect1.Width = 1;
                    pos.X = (float)Constants.RESOLUTION_VIRTUAL_WIDTH;
                    lateStart = true;
                }
            }

            currentRect = rect1;
            nextRect = rect2;
            currentPos = pos;
            nextPos = pos2;
            this.show = show;
        }

        public void Update(GameTime gameTime)
        {
            if (lateStart == false)
            {
                if (startedScrollingLeft == false && currentRect.X + currentRect.Width >= (int)dimension.X && scrollSpeed < 0)
                {
                    startedScrollingLeft = true;
                }


                if (currentRect.X < 0)
                {
                    currentPos.X += scrollSpeed;
                    currentRect.Width -= (int)scrollSpeed;
                    nextRect.Width = (int)currentPos.X;
                    nextRect.X -= (int)scrollSpeed;
                }
                else if (startedScrollingLeft == true)
                {
                    currentRect.X -= (int)scrollSpeed;
                    currentRect.Width += (int)scrollSpeed;
                    nextPos.X = (float)currentRect.Width;
                    nextRect.Width = Constants.RESOLUTION_VIRTUAL_WIDTH - (int)nextPos.X;
                }
                else
                {

                    currentRect.X -= (int)scrollSpeed;
                }
            }
            else
            {
                if (scrollSpeed > 0.0f)
                {
                    currentRect.Width += (int)scrollSpeed;
                    currentRect.X -= (int)scrollSpeed;
                    if (currentRect.Width >= Constants.RESOLUTION_VIRTUAL_WIDTH)
                    {
                        currentRect.Width = Constants.RESOLUTION_VIRTUAL_WIDTH;
                        lateStart = false;
                    }
                }
                else if (scrollSpeed < 0.0f)
                {
                    if (currentRect.Width > Constants.RESOLUTION_VIRTUAL_WIDTH)
                    {
                        currentRect.Width -= (int)scrollSpeed;
                    }
                    else
                    {
                        currentRect.Width = Constants.RESOLUTION_VIRTUAL_WIDTH;
                    }

                    if (currentPos.X > 0.0f)
                    {
                        currentPos.X += scrollSpeed;
                    }
                    else
                    {
                        currentPos.X = 0.0f;
                    }
                    
                    if (currentRect.Width >= Constants.RESOLUTION_VIRTUAL_WIDTH && currentPos.X <= 0.0f)
                    {
                        lateStart = false;
                    }
                }
            }

            if (currentRect.Width <= 0 && show == true)
            {
                changeRects();
            }
            else if (show == false && (nextRect.Width <= -50 || nextRect.Width >= ((int)dimension.X * 2) + 50))
            {
                if (callDead != null)
                {
                    callDead();
                }
            }
        }
        private void changeRects()
        {
            Rectangle oldCurrentRect = currentRect;
            Rectangle oldNextRect = nextRect;
            Vector2 oldCurrentPos = currentPos;
            Vector2 oldNextPos = nextPos;

            currentRect = oldNextRect;
            currentPos = oldNextPos;
            nextRect = oldCurrentRect;
            nextPos = oldCurrentPos;


            if (this.scrollSpeed > 0) // Scroll Right
            {
                nextRect.Width = 0;
                nextRect.X = 2560;
                nextPos = Vector2.Zero;
            }
            else if (this.scrollSpeed < 0)  // Scroll Left
            {
                nextRect.Width = 0;
                nextRect.X = 0;
                nextPos = Vector2.Zero;
                startedScrollingLeft = false;
            }

        }

        public Vector2 Position1 { get { return currentPos; } }
        public Vector2 Position2 { get { return nextPos; } }
        public Rectangle Rect1 { get { return currentRect; } }
        public Rectangle Rect2 { get { return nextRect; } }

    }
}
