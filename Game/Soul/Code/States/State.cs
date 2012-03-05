using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    abstract class State
    {
        protected string id;
        protected bool changeState = false;
        protected string nextState = "";
        protected InputManager controls;
        protected SpriteBatch spriteBatch;
        protected Soul game;
        protected AudioManager audio;

        public State(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, string id) 
        {
            this.id = id;
            this.spriteBatch = spriteBatch;
            this.audio = audioManager;
            this.game = game;
            this.controls = controls;
        }

        public abstract bool Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void initialize(string data);
        public abstract void shutdown();
        public abstract string StateData();
        public virtual string getNextState()
        {
            return "";
        }

        public string ID
        {
            get { return id; }
        }
    }
}
