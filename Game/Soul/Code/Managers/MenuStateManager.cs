using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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
        private int active = 5;
        private string alert = "";
        private bool wait = false;
        private long waitTime = 0;
        private long keyWaitTime = 5000;
        private Stopwatch timer = Stopwatch.StartNew();
        private Stopwatch keyWaitTimer = Stopwatch.StartNew();
        private bool changeKey = false;
        private Vector2 keyChangeWarning = Vector2.Zero;
        private SpriteFont spriteFont = null;

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
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 50.0f), Constants.GUI_GAME_PLAY, "gameplay");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
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

            // Creating Game Play Options Menu
            menuManager = new MenuManager(inputManager, "GamePlay");
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_TUTORIAL, "tutorial");
            Selection selection = new Selection(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), "tutorial_selection");
            selection.AddSelection("On", Constants.GUI_ON);
            selection.AddSelection("Off", Constants.GUI_OFF);
            menuManager.AddButton(button, selection);
            bool tut = bool.Parse(game.config.getValue("General", "Tutorial"));
            if (tut == true)
            {
                selection.Selection = "On";
            }
            else
            {
                selection.Selection = "Off";
            }

            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), Constants.GUI_BACK, "game_play_options_back");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            menu["GamePlay"] = menuManager;

            // Creating Graphics Options Menu
            menuManager = new MenuManager(inputManager, "Graphics");
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), Constants.GUI_SPECULAR, "specular");
            selection = new Selection(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), "specular_selection");
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
            label = new Label(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 50.0f), "shoot_string", inputManager.shoot.ToString(), false);
            menuManager.AddButton(button, label);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), Constants.GUI_UP, "controls_up");
            label = new Label(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 100.0f), "up_string", inputManager.up.ToString(), false);
            menuManager.AddButton(button, label);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_DOWN, "controls_down");
            label = new Label(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), "down_string", inputManager.down.ToString(), false);
            menuManager.AddButton(button, label);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), Constants.GUI_LEFT, "controls_left");
            label = new Label(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200.0f), "left_string", inputManager.left.ToString(), false);
            menuManager.AddButton(button, label);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), Constants.GUI_RIGHT, "controls_right");
            label = new Label(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), "right_string", inputManager.right.ToString(), false);
            menuManager.AddButton(button, label);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 300.0f), Constants.GUI_BACK, "controls_back");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            menu["Controls"] = menuManager;

            // Creating Credits Options Menu
            menuManager = new MenuManager(inputManager, "Credits");
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), Constants.GUI_BACK, "credits_back");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            MenuImage menuImage = new MenuImage(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_DEVELOPERS_CREDITS, "dev_text");
            menuManager.AddButton(button, menuImage);
            menuManager.initialize();
            menu["Credits"] = menuManager;

            // Creating Quit Menu
            menuManager = new MenuManager(inputManager, "Quit");
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f - 200, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_YES, "quit_yes");
            label = new Label(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f), "quit_text", "Are you sure you want to quit?", false);
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button, label);
            button = new ImageButton(spriteBatch, game, inputManager, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 200, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 150.0f), Constants.GUI_NO, "quit_no");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            menu["Quit"] = menuManager;

            currentMenuManager = menu["MainMenu"];
            currentMenuManager.FadeIn();

            keyChangeWarning = new Vector2(game.Window.ClientBounds.Width * 0.5f, game.Window.ClientBounds.Height * 0.5f);
            spriteFont = game.Content.Load<SpriteFont>(Constants.GUI_FONT);
        }

        public void AddMenu(string id, MenuManager mm)
        {
            menu[id] = mm;
        }

        public int Update(GameTime gameTime)
        {
            returnValue = 0;

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

            if (wait == false && changeKey == false && currentMenuManager.ID != "Quit")
            {
                if (inputManager.MoveUpOnce == true)
                {
                    audioManager.playSound("menu_move");
                    currentMenuManager.decrement();
                }
                else if (inputManager.MoveDownOnce == true)
                {
                    audioManager.playSound("menu_move");
                    currentMenuManager.increment();
                }

                if (inputManager.ShootingOnce == true && currentMenuManager.SelectionID() != "start")
                {
                    audioManager.playSound("menu_select");
                }
                else if (inputManager.ShootingOnce == true && currentMenuManager.SelectionID() == "start")
                {
                    audioManager.playSound("menu_start");
                }
            }
            else if (currentMenuManager.ID == "Quit")
            {
                if (inputManager.MoveLeftOnce == true)
                {
                    audioManager.playSound("menu_move");
                    currentMenuManager.increment();
                }
                else if (inputManager.MoveRightOnce == true)
                {
                    audioManager.playSound("menu_move");
                    currentMenuManager.decrement();
                }

                if (inputManager.ShootingOnce == true)
                {
                    audioManager.playSound("menu_select");
                }
            }

            if (changeKey == true)
            {
                if (keyWaitTimer.ElapsedMilliseconds > keyWaitTime)
                {
                    changeKey = false;
                    if (wait == true) wait = false;
                }
            }
            else if (wait == true)
            {
                if (timer.ElapsedMilliseconds > waitTime)
                {
                    wait = false;
                }
            }


            if (currentMenuManager.ID == "Sound")
            {
                SoundMenu();
            }
            else if (currentMenuManager.ID == "GamePlay")
            {
                GamePlayMenu();
            }
            else if (currentMenuManager.ID == "Graphics")
            {
                GraphicsMenu();
            }
            else if (currentMenuManager.ID == "Controls")
            {
                ControlsMenu();
            }
            else if (currentMenuManager.ID == "MainMenu" && inputManager.Pause == true)
            {
                ChangeMenuState("Quit");
            }

            return returnValue;

        }

        public void Draw()
        {
            currentMenuManager.Draw();
            if (changeKey == true)
            {
                spriteBatch.DrawString(spriteFont, alert, keyChangeWarning, Color.Red);
            }
        }

        public void ChangeMenuState(string text)
        {
            currentMenuManager.FadeOut();
            nextMenu = text;
            transition = true;
        }

        private void GamePlayMenu()
        {
            if (currentMenuManager.SelectionID() == "tutorial")
            {
                if (inputManager.MoveRightOnce == true)
                {
                    SwitchTutorial();
                }
                else if (inputManager.MoveLeftOnce == true)
                {
                    SwitchTutorial();
                }
            }
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
            if (currentMenuManager.SelectionID() != "controls_back" && inputManager.ShootingOnce == true && changeKey == false && wait == false)
            {
                changeKey = true;
                keyWaitTimer.Reset();
                keyWaitTimer.Start();
            }

            if (changeKey == true && wait == false)
            {
                alert = "Please select new key";

                if (inputManager.keyState.GetPressedKeys().Length > 0)
                {
                    wait = true;
                    changeKey = false;
                    alert = "";
                    waitTime = 250;
                    timer.Reset();
                    timer.Start();
                    Keys newKey = inputManager.keyState.GetPressedKeys().First();
                    inputManager.setKey(active, newKey);
                    currentMenuManager.SetCurrentSelection(newKey.ToString());
                    alert = "";
                }
            }

            if (currentMenuManager.SelectionID() == "controls_shoot")
            {
                active = 0;
            }
            else if (currentMenuManager.SelectionID() == "controls_up")
            {
                active = 1;
            }
            else if (currentMenuManager.SelectionID() == "controls_down")
            {
                active = 2;
            }
            else if (currentMenuManager.SelectionID() == "controls_left")
            {
                active = 3;
            }
            else if (currentMenuManager.SelectionID() == "controls_right")
            {
                active = 4;
            }
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
            else if (currentMenuManager.ID == "GamePlay")
            {
                GamePlayControl(button);
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
            else if (currentMenuManager.ID == "Credits")
            {
                CreditsControl(button);
            }
            else if (currentMenuManager.ID == "Quit")
            {
                QuitControl(button);
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
                ChangeMenuState("Credits");
            }
            else if (button.ID == "quit")
            {
                ChangeMenuState("Quit");
            }
        }

        public void OptionsControl(ImageButton button)
        {
            if (button.ID == "options_back")
            {
                ChangeMenuState("MainMenu");
            }
            else if (button.ID == "gameplay")
            {
                ChangeMenuState("GamePlay");
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

        public void GamePlayControl(ImageButton button)
        {
            if (button.ID == "game_play_options_back")
            {
                ChangeMenuState("Options");
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

        public void CreditsControl(ImageButton button)
        {
            if (button.ID == "credits_back")
            {
                ChangeMenuState("MainMenu");
            }
        }

        public void QuitControl(ImageButton button)
        {
            if (button.ID == "quit_yes")
            {
                returnValue = -1;
            }
            else if (button.ID == "quit_no")
            {
                ChangeMenuState("MainMenu");
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
            currentMenuManager.SetSelection(game.config.getValue("Video", "Width") + "x" + game.config.getValue("Video", "Height"));
            returnValue = -2;
        }

        private void SwitchTutorial()
        {
            bool value = bool.Parse(game.config.getValue("General", "Tutorial"));
            value = !value;
            if (value == true)
            {
                currentMenuManager.SetSelection("On");
            }
            else
            {
                currentMenuManager.SetSelection("Off");
            }
            game.config.addModify("General", "Tutorial", value.ToString());
            game.config.save();
        }

        public void FadeInMenu()
        {
            currentMenuManager.FadeIn();
        }

        public void FadeOutMenu()
        {
            currentMenuManager.FadeOut();
        }

        public bool IsMenuFadingDone()
        {
            if (currentMenuManager.FadeOutDone() == true)
            {
                return true;
            }
            return false;
        }

        public bool IsMenuFadingInDone()
        {
            if (currentMenuManager.FadeInDone() == true)
            {
                return true;
            }
            return false;
        }


        
    }
}
