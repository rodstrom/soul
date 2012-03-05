using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Soul.Manager;

namespace Soul
{
    class ControlsState : State
    {
        Sprite background;
        Sprite logo;
        Sprite options;
        Sprite control;
        Sprite back;
        Sprite hover;
        SpriteFont font;
        Sprite shoot;
        Sprite up;
        Sprite down;
        Sprite left;
        Sprite right;
        int active = 5;
        String alert = "";
        bool wait = false;
        float moveRight = 0f;
        long waitTime = 0;
        Stopwatch timer = Stopwatch.StartNew();

        public ControlsState(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, string id) : base(spriteBatch, game, audioManager, controls, id) 
        {
            
        }

        public override void initialize(string data)
        {
            background = new Sprite(spriteBatch, game, "Backgrounds\\menubg");
            logo = new Sprite(spriteBatch, game, "GUI\\logo_SOUL");
            options = new Sprite(spriteBatch, game, "GUI\\menu_Options");
            control = new Sprite(spriteBatch, game, "GUI\\menu_Controls");
            back = new Sprite(spriteBatch, game, "GUI\\menu_Back");
            font = game.Content.Load<SpriteFont>("GUI\\Extrafine");
            hover = new Sprite(spriteBatch, game, "GUI\\menu_Marker");
            shoot = new Sprite(spriteBatch, game, "GUI\\controls_Shoot");
            up = new Sprite(spriteBatch, game, "GUI\\controls_Up");
            down = new Sprite(spriteBatch, game, "GUI\\controls_Down");
            left = new Sprite(spriteBatch, game, "GUI\\controls_Left");
            right = new Sprite(spriteBatch, game, "GUI\\controls_Right");
        }

        public override void shutdown()
        {
            
            changeState = false;
        }

        public override string getNextState()
        {
            return "OptionsState";
        }

        public override bool Update(GameTime gameTime)
        {
            if (wait)
            {
                ChangeKey(active);
            }
            else if(timer.ElapsedMilliseconds > waitTime)
            {
                if (controls.Pause)
                {
                    changeState = true;
                }
                if (controls.ShootingOnce)
                {
                    if (active == 5)
                    {
                        changeState = true;
                    }
                    else
                    {
                        ChangeKey(active);
                        wait = true;
                    }
                }
                if (controls.MoveRightOnce && active == 5)
                {
                    active = 0;
                }
                if (controls.MoveDownOnce)
                {
                    active++;
                    if (active > 5)
                    {
                        active = 0;
                    }
                }
                else if (controls.MoveUpOnce)
                {
                    active--;
                    if (active < 0)
                    {
                        active = 5;
                    }
                }
            }
            return changeState;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());

            background.Draw(Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            logo.Draw(new Vector2(50f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            options.Draw(new Vector2(120f, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 - 50f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            control.Draw(new Vector2(145f, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 20f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (active == 5)
            {
                hover.Draw(new Vector2(290f, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 245f), Color.White, 0f, new Vector2(210f, 50f), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                hover.Draw(new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + moveRight, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + active * 50f + 50f), Color.White, 0f, new Vector2(210f, 50f), 1f, SpriteEffects.None, 0f);
            }
            back.Draw(new Vector2(120f, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 190f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            Vector2 origin = new Vector2(128f, 30f);
            shoot.Draw(new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 50f), Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            up.Draw(new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100f), Color.White, 0, origin, 1.0f, SpriteEffects.None, 0.5f);
            down.Draw(new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150f), Color.White, 0, origin, 1.0f, SpriteEffects.None, 0.5f);
            left.Draw(new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200f), Color.White, 0, origin, 1.0f, SpriteEffects.None, 0.5f);
            right.Draw(new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250f), Color.White, 0, origin, 1.0f, SpriteEffects.None, 0.5f);

            string output = controls.shoot.ToString();
            Vector2 FontOrigin = font.MeasureString(output) / 2;
            spriteBatch.DrawString(font, output, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 200f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 50f), Color.Gray, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            output = controls.up.ToString();
            FontOrigin = font.MeasureString(output) / 2;
            spriteBatch.DrawString(font, output, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 200f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100f), Color.Gray, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            output = controls.down.ToString();
            FontOrigin = font.MeasureString(output) / 2;
            spriteBatch.DrawString(font, output, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 200f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150f), Color.Gray, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            output = controls.left.ToString();
            FontOrigin = font.MeasureString(output) / 2;
            spriteBatch.DrawString(font, output, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 200f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200f), Color.Gray, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            output = controls.right.ToString();
            FontOrigin = font.MeasureString(output) / 2;
            spriteBatch.DrawString(font, output, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 200f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250f), Color.Gray, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            output = alert;
            FontOrigin = font.MeasureString(output) / 2;
            spriteBatch.DrawString(font, output, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            
            output = "These changes are applied in-menu and ingame";
            FontOrigin = font.MeasureString(output) / 2;
            spriteBatch.DrawString(font, output, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT - 50f), Color.White, 0, FontOrigin, 0.8f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        void ChangeKey(int a)
        {           
            alert = "Press only one key to continue";
            moveRight = 200f;

            if (controls.keyState.GetPressedKeys().Length > 0)
            {
                wait = false;
                alert = "";
                moveRight = 0f;
                waitTime = 250;
                timer.Reset();
                timer.Start();
                Keys newKey = controls.keyState.GetPressedKeys().First();
                controls.setKey(a, newKey);
            }
        }

        public override string StateData()
        {
            return "";
        }
    }
}
