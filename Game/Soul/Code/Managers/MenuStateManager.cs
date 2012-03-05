using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Soul.Manager
{
    class MenuStateManager
    {
        private GraphicsDeviceManager graphics = null;
        private LinkedList<DisplayMode> displayModes;
        private Dictionary<string, MenuManager> menu;
        private MenuManager currentMenuManager;
        private Soul game = null;
        private SpriteBatch spriteBatch = null;
        private InputManager inputManager = null;
        private AudioManager audioManager = null;
        private int returnValue = 0;
        private bool transition = false;
        private string nextMenu = "";
        private int currentmode = -1;

        public MenuStateManager(SpriteBatch spriteBatch, Soul game, GraphicsDeviceManager graphics, LinkedList<DisplayMode> displayModes, InputManager inputManager, AudioManager audioManager)
        {
            menu = new Dictionary<string, MenuManager>();
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.inputManager = inputManager;
            this.audioManager = audioManager;
            this.graphics = graphics;
            this.displayModes = displayModes;
        }

        public void Initialize()
        {
            // Creating main menu
            MenuManager menuManager = new MenuManager(inputManager, "MainMenu");
            ImageButton button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), Constants.GUI_START, "start");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_OPTIONS, "options");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), Constants.GUI_CREDITS, "credits");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), Constants.GUI_QUIT, "quit");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            menu["MainMenu"] = menuManager;

            // Creating options menu
            menuManager = new MenuManager(inputManager, "Options");
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), Constants.GUI_GRAPHICS, "graphics");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_SOUND, "sound");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), Constants.GUI_CONTROLS, "controls");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), Constants.GUI_BACK, "options_back");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            menu["Options"] = menuManager;

            // Creating Sound Options Menu
            menuManager = new MenuManager(inputManager, "Sound");
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), Constants.GUI_EFFECTS_VOLUME, "effect_volume");
            VolumeSlider volumeSlider = new VolumeSlider(spriteBatch, game, audioManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), "effect_volume_slider");
            menuManager.AddButton(button, volumeSlider);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_MUSIC_VOLUME, "music_volume");
            volumeSlider = new VolumeSlider(spriteBatch, game, null, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), "music_volume_slider");
            menuManager.AddButton(button, volumeSlider);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), Constants.GUI_BACK, "sound_options_back");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            menu["Sound"] = menuManager;

            // Creating Graphics Options Menu
            menuManager = new MenuManager(inputManager, "Graphics");
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), Constants.GUI_SPECULAR, "specular");
            Selection selection = new Selection(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), "specular_selection");
            selection.AddSelection("High", Constants.GUI_HIGH);
            selection.AddSelection("Medium", Constants.GUI_MEDIUM);
            selection.AddSelection("Low", Constants.GUI_LOW);
            selection.AddSelection("Off", Constants.GUI_OFF);
            float specular = float.Parse(game.config.getValue("Video", "Specular"));
            selection.Selection = ParseSpecularConfig(specular);

            menuManager.AddButton(button, selection);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_DYNAMIC_LIGHTING, "dynamic_lighting");
            selection = new Selection(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), "dynamic_lighting_selection");
            selection.AddSelection("On", Constants.GUI_ON);
            selection.AddSelection("Off", Constants.GUI_OFF);
            bool dynLights = bool.Parse(game.config.getValue("Video", "DynamicLights"));
            if (dynLights == true)
            {
                selection.Selection = "On";
            }
            else
            {
                selection.Selection = "Off";
            }
             
            menuManager.AddButton(button, selection);

            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), Constants.GUI_SCREEN_MODE, "screen_mode");
            selection = new Selection(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), "screen_mode_selection");
            selection.AddSelection("Fullscreen", Constants.GUI_FULLSCREEN);
            selection.AddSelection("Windowed", Constants.GUI_WINDOWED);

            if (bool.Parse(game.config.getValue("Video", "Fullscreen")) == true)
            {
                selection.Selection = "Fullscreen";
            }
            else
            {
                selection.Selection = "Windowed";
            }

            menuManager.AddButton(button, selection);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), Constants.GUI_RESOLUTION, "resolution");
            string output = game.Window.ClientBounds.Width + "x" + game.Window.ClientBounds.Height.ToString();
            Label label = new Label(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), "resolution_label", output);
            menuManager.AddButton(button, label);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 300.0f), Constants.GUI_BACK, "graphics_options_back");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            menu["Graphics"] = menuManager;

            // Creating Controls Options Menu
            menuManager = new MenuManager(inputManager, "Controls");
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 50.0f), Constants.GUI_SHOOT, "controls_shoot");
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), Constants.GUI_UP, "controls_up");
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_DOWN, "controls_down");
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), Constants.GUI_LEFT, "controls_left");
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), Constants.GUI_RIGHT, "controls_right");
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 300.0f), Constants.GUI_BACK, "controls_back");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            menu["Controls"] = menuManager;

            currentMenuManager = menu["MainMenu"];
            currentMenuManager.FadeIn();

        }

        public void AddMenu(string id, MenuManager mm)
        {
            menu[id] = mm;
        }

        public int Update(GameTime gameTime)
        {
            returnValue = 0;
            if (inputManager.MoveUpOnce == true) currentMenuManager.decrement();
            if (inputManager.MoveDownOnce == true) currentMenuManager.increment();

            if (transition == true)
            {
                if (currentMenuManager.FadeOutDone() == true)
                {
                    currentMenuManager = menu[nextMenu];
                    currentMenuManager.FadeIn();
                    transition = false;
                }
            }

            currentMenuManager.Update(gameTime);

            if (currentMenuManager.ID == "Sound")
            {
                SoundMenu();
            }
            else if (currentMenuManager.ID == "Graphics")
            {
                GraphicsMenu();
            }
            else if (currentMenuManager.ID == "Controls")
            {
                ControlsMenu();
            }

            return returnValue;

        }

        public void Draw()
        {
            currentMenuManager.Draw();
        }

        public void ChangeMenuState(string text)
        {
            currentMenuManager.FadeOut();
            nextMenu = text;
            transition = true;
        }

        private void SoundMenu()
        {
            if (currentMenuManager.SelectionID() == "effect_volume")
            {
                if (inputManager.MoveRight == true)
                {
                    float change = 0.01f;
                    audioManager.setFXVolume(change);
                    double value = Math.Round(audioManager.getFXVolume() * 100f);
                    game.config.addModify("Audio", "EffectsVolume", value.ToString());
                    game.config.save();
                }
                else if (inputManager.MoveLeft == true)
                {
                    float change = -0.01f;
                    audioManager.setFXVolume(change);
                    double value = Math.Round(audioManager.getFXVolume() * 100f);
                    game.config.addModify("Audio", "EffectsVolume", value.ToString());
                    game.config.save();
                }
            }
            else if (currentMenuManager.SelectionID() == "music_volume")
            {
                if (inputManager.MoveRight == true)
                {
                    float change = 0.01f;
                    audioManager.setMusicVolume(change);
                    double value = Math.Round(MediaPlayer.Volume * 100f);
                    game.config.addModify("Audio", "MusicVolume", value.ToString());
                    game.config.save();
                }
                else if (inputManager.MoveLeft == true)
                {
                    float change = -0.01f;
                    audioManager.setMusicVolume(change);
                    double value = Math.Round(MediaPlayer.Volume * 100f);
                    game.config.addModify("Audio", "MusicVolume", value.ToString());
                    game.config.save();
                }
            }
        }

        private void GraphicsMenu()
        {
            if (currentMenuManager.SelectionID() == "specular")
            {
                if (inputManager.MoveLeftOnce == true)
                {
                    currentMenuManager.SetSelection(DecreaseSpecular());
                }
                else if (inputManager.MoveRightOnce == true)
                {
                    currentMenuManager.SetSelection(IncreaseSpecular());
                }
            }
            else if (currentMenuManager.SelectionID() == "screen_mode")
            {
                if (inputManager.MoveRightOnce == true)
                {
                    SwitchFullscreen();
                }
                else if (inputManager.MoveLeftOnce == true)
                {
                    SwitchFullscreen();
                }
            }
            else if (currentMenuManager.SelectionID() == "dynamic_lighting")
            {
                if (inputManager.MoveLeftOnce == true)
                {
                    SwitchDynamicLighting();
                }
                else if (inputManager.MoveRightOnce == true)
                {
                    SwitchDynamicLighting();
                }
            }
            else if (currentMenuManager.SelectionID() == "resolution")
            {
                if (inputManager.MoveLeftOnce == true)
                {
                    currentmode--;
                    if (currentmode < 0)
                    {
                        currentmode = displayModes.Count - 1;
                    }
                    SetNewResolution();
                }
                else if (inputManager.MoveRightOnce == true)
                {
                    currentmode++;
                    if (currentmode > displayModes.Count - 1)
                    {
                        currentmode = 0;
                    }
                    SetNewResolution();
                }
            }
        }

        private void ControlsMenu()
        {

        }

        private void OnButtonPress(ImageButton button)
        {
            if (currentMenuManager.ID == "MainMenu")
            {
                MainMenuControl(button);
            }
            else if (currentMenuManager.ID == "Options")
            {
                OptionsControl(button);
            }
            else if (currentMenuManager.ID == "Sound")
            {
                SoundControl(button);
            }
            else if (currentMenuManager.ID == "Graphics")
            {
                GraphicsControl(button);
            }
            else if (currentMenuManager.ID == "Controls")
            {
                ControlsControl(button);
            }
        }

        public void MainMenuControl(ImageButton button)
        {
            if (button.ID == "start")
            {
                returnValue = 1;
            }
            else if (button.ID == "options")
            {
                ChangeMenuState("Options");
            }
            else if (button.ID == "credits")
            {
                returnValue = 3;
            }
            else if (button.ID == "quit")
            {
                returnValue = -1;
            }
        }

        public void OptionsControl(ImageButton button)
        {
            if (button.ID == "options_back")
            {
                ChangeMenuState("MainMenu");
            }
            else if (button.ID == "sound")
            {
                ChangeMenuState("Sound");
            }
            else if (button.ID == "graphics")
            {
                ChangeMenuState("Graphics");
            }
            else if (button.ID == "controls")
            {
                ChangeMenuState("Controls");
            }
        }

        public void SoundControl(ImageButton button)
        {
            if (button.ID == "sound_options_back")
            {
                ChangeMenuState("Options");
            }
        }

        public void GraphicsControl(ImageButton button)
        {
            if (button.ID == "graphics_options_back")
            {
                ChangeMenuState("Options");
            }
        }

        public void ControlsControl(ImageButton button)
        {
            if (button.ID == "controls_back")
            {
                ChangeMenuState("Options");
            }
        }

        private string ParseSpecularConfig(float value)
        {
            if (value > 0.7f)
            {
                return "High";
            }
            else if (value > 0.3f && value <= 0.7f)
            {
                return "Medium";
            }
            else if (value > 0.0f && value <= 0.3f)
            {
                return "Low";
            }
            else if (value <= 0.0f)
            {
                return "Off";
            }
            return "";
        }

        private void SwitchFullscreen()
        {
            bool value = bool.Parse(game.config.getValue("Video", "Fullscreen"));
            value = !value;
            if (value == true)
            {
                currentMenuManager.SetSelection("Fullscreen");
            }
            else
            {
                currentMenuManager.SetSelection("Windowed");
            }
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();
            game.config.addModify("Video", "Fullscreen", value.ToString());
            game.config.save();
        }

        private void SwitchDynamicLighting()
        {
            bool value = bool.Parse(game.config.getValue("Video", "DynamicLights"));
            value = !value;
            if (value == true)
            {
                currentMenuManager.SetSelection("On");
            }
            else
            {
                currentMenuManager.SetSelection("Off");
            }
            game.config.addModify("Video", "DynamicLights", value.ToString());
            game.config.save();
        }

        private string IncreaseSpecular()
        {
            float value = float.Parse(game.config.getValue("Video", "Specular"));
            if (value >= 1.0f)
            {
                value = 1.0f;
                game.config.addModify("Video", "Specular", value.ToString());
                game.config.save();
                return ParseSpecularConfig(value);
            }
            else if (value > 0.3f && value <= 0.7f)
            {
                value = 1.0f;
                game.config.addModify("Video", "Specular", value.ToString());
                game.config.save();
                return ParseSpecularConfig(value);
            }
            else if (value > 0.0f && value <= 3.0f)
            {
                value = 0.7f;
                game.config.addModify("Video", "Specular", value.ToString());
                game.config.save();
                return ParseSpecularConfig(value);
            }
            else if (value <= 0.0f)
            {
                value = 0.3f;
                game.config.addModify("Video", "Specular", value.ToString());
                game.config.save();
                return ParseSpecularConfig(value);
            }
            return ParseSpecularConfig(value);
        }

        private string DecreaseSpecular()
        {
            float value = float.Parse(game.config.getValue("Video", "Specular"));
            if (value >= 1.0f)
            {
                value = 0.7f;
                game.config.addModify("Video", "Specular", value.ToString());
                game.config.save();
                return ParseSpecularConfig(value);
            }
            else if (value > 0.3f && value <= 0.7f)
            {
                value = 0.3f;
                game.config.addModify("Video", "Specular", value.ToString());
                game.config.save();
                return ParseSpecularConfig(value);
            }
            else if (value > 0.0f && value <= 3.0f)
            {
                value = 0.0f;
                game.config.addModify("Video", "Specular", value.ToString());
                game.config.save();
                return ParseSpecularConfig(value);
            }
            else if (value <= 0.0f)
            {
                value = 0.0f;
                game.config.addModify("Video", "Specular", value.ToString());
                game.config.save();
                return ParseSpecularConfig(value);
            }
            return ParseSpecularConfig(value);
        }

        private void SetNewResolution()
        {
            Resolution.SetResolution(displayModes.ElementAt(currentmode).Width, displayModes.ElementAt(currentmode).Height, graphics.IsFullScreen);
            game.config.addModify("Video", "Width", displayModes.ElementAt(currentmode).Width.ToString());
            game.config.addModify("Video", "Height", displayModes.ElementAt(currentmode).Height.ToString());
            game.config.save();
            currentMenuManager.SetSelection(game.config.getValue("Video", "Width") + " x " + game.config.getValue("Video", "Height"));
        }


        
    }
}
