using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul.Manager
{
    class MenuManager
    {
        private List<ImageButton> buttons;
        private int currentPosition = 0;
        private InputManager controls;

        public MenuManager(InputManager controls)
        {
            buttons = new List<ImageButton>();
            this.controls = controls;
        }

        public void initialize()
        {
            buttons[currentPosition].Focus = true;
            buttons[currentPosition].glowScale = 1.0f;
            buttons[currentPosition].show = true;
        }

        public void AddButton(ImageButton button)
        {
            buttons.Add(button);
        }

        public void increment()
        {
            buttons[currentPosition].Focus = false;
            buttons[currentPosition].fullSize = false;
            currentPosition++;
            if (currentPosition > buttons.Count - 1)
            {
                currentPosition = 0;
            }
            buttons[currentPosition].Focus = true;
            buttons[currentPosition].show = true;
        }

        public void decrement()
        {
            buttons[currentPosition].Focus = false;
            buttons[currentPosition].fullSize = false;
            currentPosition--;
            if (currentPosition < 0)
            {
                currentPosition = buttons.Count - 1;
            }
            buttons[currentPosition].Focus = true;
            buttons[currentPosition].show = true;
        }

        public void Update(GameTime gameTime)
        {
            //controls.Update(gameTime);
            foreach (ImageButton button in buttons)
            {
                button.Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach (ImageButton button in buttons)
            {
                button.Draw();
            }
        }

        public void Reset()
        {
            buttons[currentPosition].Focus = false;
            buttons[currentPosition].show = false;
            buttons[currentPosition].fullSize = false;
            buttons[currentPosition].glowScale = 0f;
            currentPosition = 0;
            buttons[currentPosition].Focus = true;
            buttons[currentPosition].show = true;
            buttons[currentPosition].glowScale = 1f;
        }



    }
}
