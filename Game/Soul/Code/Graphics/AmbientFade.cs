using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class AmbientFade
    {
        private Color startColor;
        private Color fadeToColor;
        private Color currentColor;

        private int fadeToValue = 0;
        private int fadeBackToValue = 0;

        private bool fadeTo = false;
        private bool fadeBack = false;
        private bool done = false;

        private double timeBetweenChange = 0.0;
        private double timer = 0.0;

        private string id = "";

        public AmbientFade(string id, Color fadeToColor, int fadeToValue, int fadeBackToValue, double timeBetweenChange = 0.0)
        {
            this.id = id;
            this.fadeToColor = fadeToColor;
            this.fadeToValue = fadeToValue;
            this.fadeBackToValue = fadeBackToValue;
            this.timeBetweenChange = timeBetweenChange;
        }

        public void Update(GameTime gameTime)
        {
            if (fadeTo == true)
            {
                FadeToAmbientLight();
                if (currentColor == fadeToColor)
                {
                    fadeTo = false;
                    fadeBack = true;
                }
            }

            if (fadeBack == true)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > timeBetweenChange)
                {
                    FadeBackAmbientLight();
                    if (currentColor == startColor)
                    {
                        fadeBack = false;
                        done = true;
                    }
                }
            }
        }

        public void SetTargetColor(Color targetColor)
        {
            startColor = targetColor;
        }

        public void SetCurrentColor(Color currentColor)
        {
            this.currentColor = currentColor;
        }

        public void Reset()
        {
            fadeTo = true;
            fadeBack = false;
            done = false;
            timer = 0.0;
        }

        public bool Done
        {
            get { return done; }
        }

        public Color CurrentColor { get { return currentColor; } }
        public string ID { get { return id; } }




        private void FadeToAmbientLight()
        {
            int r = (int)currentColor.R;
            int g = (int)currentColor.G;
            int b = (int)currentColor.B;

            if (currentColor.R < fadeToColor.R)
            {
                r += fadeToValue;
                if (r > (int)fadeToColor.R)
                    r = (int)fadeToColor.R;
                
                currentColor.R = (byte)r;
            }
            else if (currentColor.R > fadeToColor.R)
            {
                r -= fadeToValue;
                if (r < 0)
                    r = 0;
                currentColor.R = (byte)r;
            }

            if (currentColor.G < fadeToColor.G)
            {
                g += fadeToValue;
                if (g > (int)fadeToColor.G)
                    g = (int)fadeToColor.G;
                currentColor.G = (byte)g;
            }
            else if (currentColor.G > fadeToColor.G)
            {
                g -= fadeToValue;
                if (g < 0)
                    g = 0;
                currentColor.G = (byte)g;
            }

            if (currentColor.B < fadeToColor.B)
            {
                b += fadeToValue;
                if (b > (int)fadeToColor.G)
                    b = (int)fadeToColor.G;
                currentColor.B = (byte)b;
            }
            else if (currentColor.B > fadeToColor.B)
            {
                b -= fadeToValue;
                if (b < 0)
                    b = 0;
                currentColor.B = (byte)b;
            }
        }

        private void FadeBackAmbientLight()
        {
            int r = (int)currentColor.R;
            int g = (int)currentColor.G;
            int b = (int)currentColor.B;

            if (currentColor.R < startColor.R)
            {
                r += fadeBackToValue;
                if (r > (int)startColor.R)
                    r = (int)startColor.R;
                currentColor.R = (byte)r;
            }
            else if (currentColor.R > startColor.R)
            {
                r -= fadeBackToValue;
                if (r < 0)
                    r = 0;
                currentColor.R = (byte)r;
            }

            if (currentColor.G < startColor.G)
            {
                g += fadeBackToValue;
                if (g > (int)startColor.G)
                    g = (int)startColor.G;
                currentColor.G = (byte)g;
            }
            else if (currentColor.G > startColor.G)
            {
                g -= fadeBackToValue;
                if (g < 0)
                    g = 0;
                currentColor.G = (byte)g;
            }

            if (currentColor.B < startColor.B)
            {
                b += fadeBackToValue;
                if (b > (int)startColor.B)
                    b = (int)startColor.B;
                currentColor.B = (byte)b;
            }
            else if (currentColor.B > startColor.B)
            {
                b -= fadeBackToValue;
                if (b < 0)
                    b = 0;
                currentColor.B = (byte)b;
            }
        }
    }
}
