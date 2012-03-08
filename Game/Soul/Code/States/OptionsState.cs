using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using Soul.Manager;

namespace Soul
{
    class OptionsState : State
    {
        GraphicsDeviceManager graphics;
        LinkedList<DisplayMode> displayModes;
        int currentMode = -1;
        Sprite bg;
        FadeInOut fade;
        private bool quit = false;
        List<String> options = new List<String>();
        int activeOption = 0;
        SpriteFont font;
        Texture2D slider;
        Texture2D sliderMarker;
        Texture2D textMusic;
        Texture2D textEffects;
        Texture2D textFullscreen;
        Texture2D textWindowed;
        Texture2D leftArrow;
        Texture2D rightArrow;
        Texture2D textBack;
        Texture2D textOptions;
        Texture2D textSoul;
        Texture2D marker;
        Texture2D textControls;

        VolumeSlider volumeSlider = null;
        Selection selectionTest = null;

        //string fullScreenSel = "Fullscreen";
        //IniFile config;

        public OptionsState(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, GraphicsDeviceManager graphics, LinkedList<DisplayMode> displayModes, InputManager controls, string id) : base(spriteBatch, game, audioManager, controls, id) 
        {
            this.graphics = graphics;
            this.displayModes = displayModes;
        }

        public override void initialize(string data)
        {
            nextState = "MenuState";
            font = game.Content.Load<SpriteFont>("GUI\\Extrafine");
            slider = game.Content.Load<Texture2D>("GUI\\options_Slider");
            sliderMarker = game.Content.Load<Texture2D>("GUI\\options_SliderMarker");
            textEffects = game.Content.Load<Texture2D>("GUI\\options_EffectsVolume");
            textMusic = game.Content.Load<Texture2D>("GUI\\options_MusicVolume");
            textFullscreen = game.Content.Load<Texture2D>("GUI\\options_Fullscreen");
            textWindowed = game.Content.Load<Texture2D>("GUI\\options_Windowed");
            leftArrow = game.Content.Load<Texture2D>("GUI\\arrow_Left");
            rightArrow = game.Content.Load<Texture2D>("GUI\\arrow_Right");
            textBack = game.Content.Load<Texture2D>("GUI\\menu_Back");
            textOptions = game.Content.Load<Texture2D>("GUI\\menu_Options");
            textSoul = game.Content.Load<Texture2D>("GUI\\logo_SOUL");
            marker = game.Content.Load<Texture2D>("GUI\\menu_Marker");
            textControls = game.Content.Load<Texture2D>("GUI\\menu_Controls");
            //config = new IniFile("Content\\Config\\config.ini");
            //config.parse();

            bg = new Sprite(spriteBatch, game, Constants.MENU_BG_FILENAME);
            fade = new FadeInOut(spriteBatch, game);
            audio.playMusic(Constants.AUDIO_MENU);
            options.Add("back");
            options.Add("controls");
            options.Add("music");
            options.Add("effects");
            options.Add("fullscreen");
            options.Add("resolution");
            volumeSlider = new VolumeSlider(spriteBatch, game, audio, new Vector2((float)game.Window.ClientBounds.Width * 0.5f, 100f), "test");
            selectionTest = new Selection(spriteBatch, game, new Vector2(game.Window.ClientBounds.Width * 0.5f, 250f), "testing");
            selectionTest.AddSelection("Fullscreen", Constants.GUI_FULLSCREEN);
            selectionTest.AddSelection("Windowed", Constants.GUI_WINDOWED);
            fade.FadeIn();
        }

        public override void shutdown()
        {
            audio.stopMusic();
            bg.Dispose();
            activeOption = 0;
            changeState = false;
        }

        public override string getNextState()
        {
            return nextState;
        }

        public override bool Update(GameTime gameTime)
        {
            //volumeSlider.UpdateVolume(MediaPlayer.Volume);

            if (controls.Pause)
            {
                nextState = "MenuState";
                changeState = true;
            }
            if (controls.ShootingOnce)
            {
                ChangeOption(options.ElementAt(activeOption), true);
            }
            if (controls.MoveDownOnce)
            {
                activeOption++;
                if (activeOption > options.Count - 1)
                {
                    activeOption = 0;
                }
            }
            else if (controls.MoveUpOnce)
            {
                activeOption--;
                if (activeOption < 0)
                {
                    activeOption = options.Count - 1;
                }
            }

            if (controls.MoveLeftOnce)
            {
                ChangeOption(options.ElementAt(activeOption), false);
            }
            else if (controls.MoveRightOnce)
            {
                if (activeOption == 0)
                {
                    activeOption = 1;
                }
                else
                {
                    ChangeOption(options.ElementAt(activeOption), true);
                }
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
            bg.Draw(Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            OptionsMenu();
            volumeSlider.Draw();
            //selectionTest.Draw();
            fade.Draw();
            spriteBatch.End();
        }

        private void OptionsMenu()
        {
            if (options.ElementAt(activeOption).Equals("controls"))
            {
                spriteBatch.Draw(marker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 210, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 50 - 40, 420, 100), Color.White);
            }
            spriteBatch.Draw(textControls, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 150, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 50 - 20, 300, 60), Color.White);
                        
            if(options.ElementAt(activeOption).Equals("music"))
            {
                spriteBatch.Draw(marker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 210, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 100 - 43, 420, 100), Color.White);
                spriteBatch.Draw(slider, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + 150, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 100 - 15, 320, 48), Color.White);
                spriteBatch.Draw(sliderMarker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + 184 + (int)Math.Round(MediaPlayer.Volume * 210), Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 100 - 6, 30, 30), Color.White);
            }
            spriteBatch.Draw(textMusic, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 180, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 100 - 50, 360, 100), Color.White);
            
            if(options.ElementAt(activeOption).Equals("effects"))
            {
                spriteBatch.Draw(marker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 210, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 150 - 43, 420, 100), Color.White);
                spriteBatch.Draw(slider, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + 150, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 150 - 15, 320, 48), Color.White);
                spriteBatch.Draw(sliderMarker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + 184 + (int)Math.Round(audio.getFXVolume() * 210), Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 150 - 6, 30, 30), Color.White);
            }
            spriteBatch.Draw(textEffects, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 180, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 150 - 50, 360, 100), Color.White);
            
            if (options.ElementAt(activeOption).Equals("fullscreen"))
            {
                spriteBatch.Draw(marker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 210, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 200 - 48, 420, 100), Color.White);
                spriteBatch.Draw(leftArrow, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 120, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 200 - 7, 20, 20), Color.White);
                spriteBatch.Draw(rightArrow, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + 100, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 200 - 7, 20, 20), Color.White);
            }
            if (graphics.IsFullScreen)
            {
                spriteBatch.Draw(textFullscreen, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 180, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 200 - 50, 360, 100), Color.White);
            }
            else
            {
                spriteBatch.Draw(textWindowed, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 180, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 200 - 50, 360, 100), Color.White);
            }

            if (options.ElementAt(activeOption).Equals("resolution"))
            {
                spriteBatch.Draw(marker, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 210, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 250 - 55, 420, 100), Color.White);
                spriteBatch.Draw(leftArrow, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 100, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 250 - 15, 20, 20), Color.White);
                spriteBatch.Draw(rightArrow, new Rectangle(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 + 80, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 250 - 15, 20, 20), Color.White);
            }
            string output = graphics.PreferredBackBufferWidth.ToString() + "x" + graphics.PreferredBackBufferHeight.ToString();
            Vector2 FontPos = new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f);
            Vector2 FontOrigin = font.MeasureString(output) / 2;
            spriteBatch.DrawString(font, output, FontPos, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.Draw(textSoul, new Rectangle(50, 0, 1280, 400), Color.White);
            spriteBatch.Draw(textOptions, new Rectangle(120, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 - 50, 360, 100), Color.White);

            if (options.ElementAt(activeOption).Equals("back"))
            {
                spriteBatch.Draw(marker, new Rectangle(90, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 240 - 45, 420, 100), Color.White);
            }
            spriteBatch.Draw(textBack, new Rectangle(120, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 190, 360, 100), Color.White);
        }

        private void ChangeOption(String option, bool up)
        {
            if (option == "back")
            {
                nextState = "MenuState";
                changeState = true;
            }
            else if (option == "controls")
            {
                nextState = "ControlsState";
                changeState = true;
            }
            else if (option == "music")
            {
                float change = 0.05f;
                if (!up)
                {
                    change *= -1;
                }
                audio.setMusicVolume(change);

                double value = Math.Round(MediaPlayer.Volume * 100f);
                //config.parse();
                game.config.addModify("Audio", "MusicVolume", value.ToString());
                game.config.save();
            }
            else if (option == "effects")
            {
                float change = 0.05f;
                if (!up)
                {
                    change *= -1;
                }
                audio.setFXVolume(change);

                double value = Math.Round(audio.getFXVolume() * 100f);
                //config.parse();
                game.config.addModify("Audio", "EffectsVolume", value.ToString());
                game.config.save();
            }
            else if (option == "fullscreen")
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges(); 
                
                //config.parse();
                game.config.addModify("Video", "Fullscreen", graphics.IsFullScreen.ToString());
                game.config.save();
            }
            else if (option == "resolution")
            {
                if (up)
                {
                    currentMode++;
                }
                else
                {
                    currentMode--;
                }
                if (currentMode > displayModes.Count - 1)
                {
                    currentMode = 0;
                }
                else if (currentMode < 0)
                {
                    currentMode = displayModes.Count - 1;
                }
                Resolution.SetResolution(displayModes.ElementAt(currentMode).Width, displayModes.ElementAt(currentMode).Height, graphics.IsFullScreen);
                
                //config.parse();
                game.config.addModify("Video", "Width", displayModes.ElementAt(currentMode).Width.ToString());
                game.config.addModify("Video", "Height", displayModes.ElementAt(currentMode).Height.ToString());
                game.config.save();
            }
        }

        public override string StateData()
        {
            return "";
        }
    }
}
