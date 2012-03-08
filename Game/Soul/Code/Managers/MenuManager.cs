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
        private List<KeyValuePair<ImageButton, Widget>> buttons;
        private int currentPosition = 0;
        private InputManager controls;
        private string id = "";

        public MenuManager(InputManager controls, string id = "")
        {
            buttons = new List<KeyValuePair<ImageButton, Widget>>();
            this.controls = controls;
            this.id = id;
        }


        public void initialize()
        {
            buttons[currentPosition].Key.Focus = true;
            buttons[currentPosition].Key.glowScale = 1.0f;
            buttons[currentPosition].Key.show = true;
        }

        public void AddButton(ImageButton button)
        {
            KeyValuePair<ImageButton, Widget> tmpPair = new KeyValuePair<ImageButton, Widget>(button, null);
            buttons.Add(tmpPair);
        }

        public void AddButton(ImageButton button, Widget widget)
        {
            KeyValuePair<ImageButton, Widget> tmpPair = new KeyValuePair<ImageButton, Widget>(button, widget);
            buttons.Add(tmpPair);
        }

        public void increment()
        {
            buttons[currentPosition].Key.Focus = false;
            buttons[currentPosition].Key.fullSize = false;
            currentPosition++;
            if (currentPosition > buttons.Count - 1)
            {
                currentPosition = 0;
            }
            buttons[currentPosition].Key.Focus = true;
            buttons[currentPosition].Key.show = true;
        }

        public void decrement()
        {
            buttons[currentPosition].Key.Focus = false;
            buttons[currentPosition].Key.fullSize = false;
            currentPosition--;
            if (currentPosition < 0)
            {
                currentPosition = buttons.Count - 1;
            }
            buttons[currentPosition].Key.Focus = true;
            buttons[currentPosition].Key.show = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Key.Update(gameTime);
                if (buttons[i].Value != null)
                {
                    buttons[i].Value.Update(gameTime);
                }
            }
        }

        public virtual void Draw()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Key.Draw();
                if (buttons[i].Value != null)
                {
                    buttons[i].Value.Draw();
                }
            }
        }

        public void Reset()
        {
            buttons[currentPosition].Key.Focus = false;
            buttons[currentPosition].Key.show = false;
            buttons[currentPosition].Key.fullSize = false;
            buttons[currentPosition].Key.glowScale = 0f;
            currentPosition = 0;
            buttons[currentPosition].Key.Focus = true;
            buttons[currentPosition].Key.show = true;
            buttons[currentPosition].Key.glowScale = 1f;
        }

        public void SetCurrentSelection(string text)
        {
            buttons[currentPosition].Value.Selection = text;
        }

        public void FadeIn()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Key.FadeIn();
                if (buttons[i].Value != null)
                {
                    buttons[i].Value.FadeIn();
                }
            }
        }

        public void FadeOut()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Key.FadeOut();
                if (buttons[i].Value != null)
                {
                    buttons[i].Value.FadeOut();
                }
            }
        }

        public bool FadeOutDone()
        {
            if (buttons[0].Key.IsAlphaZero == true)
            {
                return true;
            }
            return false;
        }

        public bool FadeInDone()
        {
            if (buttons[0].Key.IsAlphaMax == true)
            {
                return true;
            }
            return false;
        }

        public string SelectionID()
        {
            return buttons[currentPosition].Key.ID;
        }

        public void SetSelection(string text)
        {
            if (buttons[currentPosition].Value != null)
            {
                buttons[currentPosition].Value.Selection = text;
            }
        }

        public string ID { get { return id; } }

    }
}
