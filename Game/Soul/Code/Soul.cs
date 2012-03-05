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
        LevelReader levelReader;
        AudioManager audioManager;
        LinkedList<DisplayMode> displayModes;
        InputManager controls;
        public IniFile config;
        public IniFile constants;

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
            this.IsMouseVisible = bool.Parse(config.getValue("General", "ShowMouse"));
            audioManager = new AudioManager(Content, this);
            displayModes = new LinkedList<DisplayMode>();
            foreach (DisplayMode dm in this.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                if (dm.Format == SurfaceFormat.Color)
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
            stateManager = new StateManager();

            levelReader = new LevelReader("Content\\Levels\\level01.map", this);
            levelReader.Parse();

            controls = new InputManager(this);
            State state = new IntroState(spriteBatch, this, audioManager, controls, "IntroState");
            stateManager.AddState(state);
            state = new MenuState(spriteBatch, this, audioManager, controls, "MenuState");
            stateManager.AddState(state);
            state = new OptionsState(spriteBatch, this, audioManager, graphics, displayModes, controls, "OptionsState");
            stateManager.AddState(state);
            state = new ControlsState(spriteBatch, this, audioManager, controls, "ControlsState");
            stateManager.AddState(state);
            state = new CreditsState(spriteBatch, this, audioManager, controls, "CreditsState");
            stateManager.AddState(state);
            state = new WorldMapState(spriteBatch, this, audioManager, controls, "WorldMapState");
            stateManager.AddState(state);
            state = new PlayState(spriteBatch, this, audioManager, controls, "PlayState");
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
