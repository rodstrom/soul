using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Soul.Debug
{
    class SpawnEnemies
    {
        private bool one = false;
        private bool two = false;
        private bool three = false;
        private bool four = false;
        private bool five = false;
        KeyboardState keyState;
        KeyboardState oldKeyState;
        

        public void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyUp(Keys.D1) == true && oldKeyState.IsKeyDown(Keys.D1) == true)
            {
                one = true;
            }
            else
            {
                one = false;
            }

            if (keyState.IsKeyUp(Keys.D2) == true && oldKeyState.IsKeyDown(Keys.D2) == true)
            {
                two = true;
            }
            else
            {
                two = false;
            }

            if (keyState.IsKeyUp(Keys.D3) == true && oldKeyState.IsKeyDown(Keys.D3) == true)
            {
                three = true;
            }
            else
            {
                three = false;
            }

            if (keyState.IsKeyUp(Keys.D4) == true && oldKeyState.IsKeyDown(Keys.D4) == true)
            {
                four = true;
            }
            else
            {
                four = false;
            }

            if (keyState.IsKeyUp(Keys.D5) == true && oldKeyState.IsKeyDown(Keys.D5) == true)
            {
                five = true;
            }
            else
            {
                five = false;
            }

            oldKeyState = keyState;

        }

        public bool One
        {
            get { return one; }
        }

        public bool Two
        {
            get { return two; }
        }

        public bool Three
        {
            get { return three; }
        }

        public bool Four
        {
            get { return four; }
        }

        public bool Five
        {
            get { return five; }
        }
    }
}
