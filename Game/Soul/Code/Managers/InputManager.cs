using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Soul.Manager
{
    class InputManager
    {
        public KeyboardState keyState;
        KeyboardState oldKeyState;
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool moveUp = false;
        private bool moveDown = false;
        private bool shooting = false;
        private bool moveLeftOnce = false;
        private bool moveRightOnce = false;
        private bool moveUpOnce = false;
        private bool moveDownOnce = false;
        private bool shootingOnce = false;
        private bool pauseOnce = false;
        private bool debugOnce = false;
        //private IniFile config;

        public Keys up;
        public Keys down;
        public Keys right;
        public Keys left;
        public Keys shoot;
        public Keys pause;
        public Keys debug;

        Soul game;

        public InputManager(Soul game)
        {
            //config = new IniFile("Content\\Config\\config.ini");
            //game.config.parse();
            this.game = game;
            up = setKey(game.config.getValue("Controls", "Up"));
            down = setKey(game.config.getValue("Controls", "Down"));
            right = setKey(game.config.getValue("Controls", "Right"));
            left = setKey(game.config.getValue("Controls", "Left"));
            shoot = setKey(game.config.getValue("Controls", "Shoot"));
            pause = setKey(game.config.getValue("Controls", "Pause"));
            debug = Keys.F20;
            if(bool.Parse(game.config.getValue("Debug", "KeyEnabled")))
            {
                debug = Keys.P;
            }
        }

        private Keys setKey(String newKey)
        {
            try
            {
                return (Keys)Enum.Parse(typeof(Keys), newKey);
            }
            catch (Exception)
            {
               return Keys.F24;
            }
        }

        public void setKey(int key, Keys name)
        {
            String changedBinding = "";
            bool save = false;
            switch (key)
            {
                case 0:
                    shoot = name;
                    changedBinding = "Shoot";
                    save = true;
                    break;
                case 1:
                    up = name;
                    changedBinding = "Up";
                    save = true;
                    break;
                case 2:
                    down = name;
                    changedBinding = "Down";
                    save = true;
                    break;
                case 3:
                    left = name;
                    changedBinding = "Left";
                    save = true;
                    break;
                case 4:
                    right = name;
                    changedBinding = "Right";
                    save = true;
                    break;
                case 5:
                    pause = name;
                    changedBinding = "Pause";
                    save = true;
                    break;
                default:
                    break;
            }
            if (save)
            {
                //config.parse();
                game.config.addModify("Controls", changedBinding, name.ToString());
                game.config.save();
            }
        }

        public void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(up) == true || keyState.IsKeyDown(Keys.Up) == true)
            {
                moveUp = true;
            }
            
            if (keyState.IsKeyDown(down) == true || keyState.IsKeyDown(Keys.Down) == true)
            {
                moveDown = true;
            }

            if (keyState.IsKeyDown(right) == true || keyState.IsKeyDown(Keys.Right) == true)
            {
                moveRight = true;
            }

            if (keyState.IsKeyDown(left) == true || keyState.IsKeyDown(Keys.Left) == true)
            {
                moveLeft = true;
            }

            if (keyState.IsKeyDown(shoot) == true)
            {
                shooting = true;
            }

            //////////////////////////////////////////////
            //////////////////////////////////////////////

            if (keyState.IsKeyDown(up) == false && keyState.IsKeyDown(Keys.Up) == false)
            {
                moveUp = false;
            }

            if (keyState.IsKeyDown(down) == false && keyState.IsKeyDown(Keys.Down) == false)
            {
                moveDown = false;
            }

            if (keyState.IsKeyDown(right) == false && keyState.IsKeyDown(Keys.Right) == false)
            {
                moveRight = false;
            }

            if (keyState.IsKeyDown(left) == false && keyState.IsKeyDown(Keys.Left) == false)
            {
                moveLeft = false;
            }

            if (keyState.IsKeyDown(shoot) == false)
            {
                shooting = false;
            }

            //////////////////////////////////////////////
            //////////////////////////////////////////////

            if (keyState.IsKeyUp(shoot) == true && oldKeyState.IsKeyDown(shoot) == true || keyState.IsKeyUp(Keys.Enter) == true && oldKeyState.IsKeyDown(Keys.Enter) == true)
            {
                shootingOnce = true;
            }
            else
            {
                shootingOnce = false;
            }

            if (keyState.IsKeyUp(up) == true && oldKeyState.IsKeyDown(up) == true || (keyState.IsKeyUp(Keys.Up) == true && oldKeyState.IsKeyDown(Keys.Up)))
            {
                moveUpOnce = true;
            }
            else
            {
                moveUpOnce = false;
            }

            if (keyState.IsKeyUp(down) == true && oldKeyState.IsKeyDown(down) == true || keyState.IsKeyUp(Keys.Down) == true && oldKeyState.IsKeyDown(Keys.Down) == true)
            {
                moveDownOnce = true;
            }
            else
            {
                moveDownOnce = false;
            }

            if (keyState.IsKeyUp(left) == true && oldKeyState.IsKeyDown(left) == true || keyState.IsKeyUp(Keys.Left) == true && oldKeyState.IsKeyDown(Keys.Left) == true)
            {
                moveLeftOnce = true;
            }
            else
            {
                moveLeftOnce = false;
            }

            if (keyState.IsKeyUp(right) == true && oldKeyState.IsKeyDown(right) == true || keyState.IsKeyUp(Keys.Right) == true && oldKeyState.IsKeyDown(Keys.Right) == true)
            {
                moveRightOnce = true;
            }
            else
            {
                moveRightOnce = false;
            }

            if (keyState.IsKeyUp(pause) == true && oldKeyState.IsKeyDown(pause) == true)
            {
                pauseOnce = true;
            }
            else
            {
                pauseOnce = false;
            }

            if (keyState.IsKeyUp(debug) == true && oldKeyState.IsKeyDown(debug) == true)
            {
                debugOnce = true;
            }
            else
            {
                debugOnce = false;
            }



            keyProblems();

            oldKeyState = keyState;
        }

        private void keyProblems()
        {
            String changedBinding = "";
            Keys changedKey = Keys.F24;
            bool save = false;
            if (shoot == Keys.F24)
            {
                shoot = Keys.X;
                changedBinding = "Shoot";
                changedKey = shoot;
                save = true;
            }
            if (up == Keys.F24)
            {
                up = Keys.Up;
                changedBinding = "Up";
                changedKey = up;
                save = true;
            }
            if (down == Keys.F24)
            {
                down = Keys.Down;
                changedBinding = "Down";
                changedKey = down;
                save = true;
            }
            if (left == Keys.F24)
            {
                left = Keys.Left;
                changedBinding = "Left";
                changedKey = left;
                save = true;
            }
            if (right == Keys.F24)
            {
                right = Keys.Right;
                changedBinding = "Right";
                changedKey = right;
                save = true;
            }
            if (pause == Keys.F24)
            {
                pause = Keys.Escape;
                changedBinding = "Pause";
                changedKey = pause;
                save = true;
            }

            if (save)
            {
                //config.parse();
                game.config.addModify("Controls", changedBinding, changedKey.ToString());
                game.config.save();
            }
        }

        #region getKeys
        public bool MoveLeft
        {
            get { return moveLeft; }
        }

        public bool MoveRight
        {
            get { return moveRight; }
        }

        public bool MoveUp
        {
            get { return moveUp; }
        }

        public bool MoveDown
        {
            get { return moveDown; }
        }

        public bool Shooting
        {
            get { return shooting; }
        }

        public bool ShootingOnce
        {
            get { return shootingOnce; }
        }

        public bool MoveUpOnce
        {
            get { return moveUpOnce; }
        }

        public bool MoveDownOnce
        {
            get { return moveDownOnce; }
        }

        public bool MoveLeftOnce
        {
            get { return moveLeftOnce; }
        }

        public bool MoveRightOnce
        {
            get { return moveRightOnce; }
        }

        public bool Pause
        {
            get { return pauseOnce; }
        }

        public bool Debug
        {
            get { return debugOnce; }
        }
        #endregion getKeys
    }
}
