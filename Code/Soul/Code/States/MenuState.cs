using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class MenuState : State
    {
        MenuManager menuManager;
        Sprite bg;
        Sprite logo;
        FadeInOut fade;
        private bool quit = false;

        public MenuState(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, string id) : base(spriteBatch, game, audioManager, controls, id) { }

        public override void initialize(string data)
        {
            nextState = "";
            bg = new Sprite(spriteBatch, game, Constants.MENU_BG_FILENAME);
            logo = new Sprite(spriteBatch, game, "GUI\\logo_SOUL");
            fade = new FadeInOut(spriteBatch, game);
            menuManager = new MenuManager(controls);
            ImageButton button = new ImageButton(spriteBatch, game, controls, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), "GUI\\menu_Start", "start");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, controls, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), "GUI\\menu_Options", "options");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, controls, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), "GUI\\menu_Credits", "credits");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, controls, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), "GUI\\menu_Quit", "quit");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            audio.playMusic(Constants.AUDIO_MENU);
            fade.FadeIn();
        }

        public override void shutdown()
        {
            audio.stopMusic();
            bg.Dispose();
            changeState = false;
        }

        public override string getNextState()
        {
            return nextState;
        }

        public override bool Update(GameTime gameTime)
        {
            if (controls.Pause)
            {
                fade.FadeOut();
                quit = true;
            } 
            
            menuManager.Update(gameTime);
            if (controls.MoveDownOnce == true)
            {
                menuManager.increment();
            }
            else if (controls.MoveUpOnce == true)
            {
                menuManager.decrement();
            }

            if (fade.FadeOutDone == true)
            {
                if (quit == true)
                {
                    game.Exit();
                }
                else
                {
                    changeState = true;
                }
            }
            else if (fade.IsFading == true)
            {
                fade.Update(gameTime);
            }

            return changeState;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            bg.Draw(new Vector2(0f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            logo.Draw(new Vector2(50f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            menuManager.Draw();
            fade.Draw();
            spriteBatch.End();
        }

        private void OnButtonPress(ImageButton button)
        {
            if (button.ID == "start")
            {
                nextState = "WorldMapState";
                fade.FadeOut();
            }
            else if (button.ID == "options")
            {
                nextState = "OptionsState";
                changeState = true;
            }
            else if (button.ID == "credits")
            {
                nextState = "CreditsState";
                changeState = true;
            }
            else if (button.ID == "quit")
            {
                fade.FadeOut();
                quit = true;
            }
        }

        public override string StateData()
        {
            return "";
        }
    }
}
