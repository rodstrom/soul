using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soul.Manager;

namespace Soul
{
    public class Soul : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateManager stateManager;
        AudioManager audioManager;
        LinkedList<DisplayMode> displayModes;
        InputManager controls;
        public IniFile config;
        public IniFile constants;
        public IniFile audioList;
        public IniFile lighting;

        public Soul()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            config = new IniFile("Content\\Config\\config.ini");
            config.parse();
            constants = new IniFile("Content\\Config\\constants.ini");
            constants.parse();
            audioList = new IniFile("Content\\Config\\audiolist.ini");
            audioList.parse();
            lighting = new IniFile("Content\\Config\\lighting.ini");
            lighting.parse();
            this.IsMouseVisible = bool.Parse(config.getValue("General", "ShowMouse"));
            audioManager = new AudioManager(Content, this);
            displayModes = new LinkedList<DisplayMode>();
            foreach (DisplayMode dm in this.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                if (dm.Format == SurfaceFormat.Color && dm.AspectRatio > 1.7f && dm.AspectRatio < 2f)
                {
                    displayModes.AddLast(dm);
                }
            }
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(Constants.RESOLUTION_VIRTUAL_WIDTH, Constants.RESOLUTION_VIRTUAL_HEIGHT);
            Resolution.SetResolution(
                int.Parse(config.getValue("Video", "Width")), 
                int.Parse(config.getValue("Video", "Height")), 
                bool.Parse(config.getValue("Video", "Fullscreen"))
                );
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Intro sounds
            audioManager.addEffect(audioList.getValue("Intro", "startup"), "intro_startup");

            // Main Menu Sounds
            audioManager.addEffect(audioList.getValue("MainMenu", "move"), "menu_move");
            audioManager.addEffect(audioList.getValue("MainMenu", "select"), "menu_select");
            audioManager.addEffect(audioList.getValue("MainMenu", "start"), "menu_start");
            audioManager.addSong(audioList.getValue("MainMenu", "music"), "menu_music");

            // World Map Sounds
            audioManager.addEffect(audioList.getValue("WorldMap", "select"), "map_select");
            audioManager.addEffect(audioList.getValue("WorldMap", "move"), "map_move");
            audioManager.addEffect(audioList.getValue("WorldMap", "selectMap"), "map_select_map");
            audioManager.addEffect(audioList.getValue("WorldMap", "back"), "map_back");
            audioManager.addSong(audioList.getValue("WorldMap", "music"), "map_music");

            // In Game Sounds
            audioManager.addEffect(audioList.getValue("InGame", "player_shoot"), "player_shoot");
            audioManager.addEffect(audioList.getValue("InGame", "player_die"), "player_die");
            audioManager.addEffect(audioList.getValue("InGame", "player_powerup_pickup"), "player_powerup_pickup");
            audioManager.addEffect(audioList.getValue("InGame", "dark_thought_shoot"), "dark_thought_shoot");
            audioManager.addEffect(audioList.getValue("InGame", "dark_thought_die"), "dark_thought_die");
            audioManager.addEffect(audioList.getValue("InGame", "red_bloodvessel_die"), "red_bloodvessel_die");
            audioManager.addEffect(audioList.getValue("InGame", "blue_bloodvessel_die"), "blue_bloodvessel_die");
            audioManager.addEffect(audioList.getValue("InGame", "purple_bloodvessel_die"), "purple_bloodvessel_die");
            audioManager.addEffect(audioList.getValue("InGame", "nightmare_die"), "nightmare_die");
            audioManager.addEffect(audioList.getValue("InGame", "nightmare_hit"), "nightmare_hit");
            audioManager.addEffect(audioList.getValue("InGame", "inner_demon_die"), "inner_demon_die");
            audioManager.addEffect(audioList.getValue("InGame", "lesser_demon_spawn"), "lesser_demon_spawn");
            audioManager.addEffect(audioList.getValue("InGame", "lesser_demon_die"), "lesser_demon_die");
            audioManager.addEffect(audioList.getValue("InGame", "dark_whisper_die"), "dark_whisper_die");
            audioManager.addEffect(audioList.getValue("InGame", "dark_whisper_release_spikes"), "release_spikes");
            audioManager.addEffect(audioList.getValue("InGame", "pause_appear"), "pause_appear");
            audioManager.addSong(audioList.getValue("InGame", "music"), "main_music");
            audioManager.addSong(audioList.getValue("InGame", "music_secondary"), "secondary_music");


            stateManager = new StateManager();

            controls = new InputManager(this);
            State state = new IntroState(spriteBatch, this, audioManager, controls, "IntroState");
            stateManager.AddState(state);
            state = new MenuState(spriteBatch, this, graphics, displayModes, audioManager, controls, "MenuState");
            stateManager.AddState(state);
            state = new CreditsState(spriteBatch, this, audioManager, controls, "CreditsState");
            stateManager.AddState(state);
            state = new WorldMapState(spriteBatch, this, audioManager, controls, "WorldMapState");
            stateManager.AddState(state);
            state = new PlayState(spriteBatch, this, audioManager, controls, "PlayState");
            stateManager.AddState(state);
            state = new WarningState(spriteBatch, this, audioManager, controls, "WarningState");
            stateManager.AddState(state);
            stateManager.SetState(config.getValue("General", "StartState"));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            controls.Update(gameTime);
            if (stateManager.Update(gameTime) == true)
            {
                stateManager.SetState(stateManager.getNextState());
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            
            stateManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
