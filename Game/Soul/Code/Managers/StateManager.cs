using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Soul.Manager
{
    class StateManager
    {
        private List<State> states;
        State currentState = null;

        public StateManager() { states = new List<State>(); }

        public bool AddState(State state)
        {
            if (DoesStateExist(state) == true)
            {
                return false;
            }
            states.Add(state);
            return true;
        }

        public bool DoesStateExist(State state)
        {
            foreach (State i in states)
            {
                if (state.ID == i.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public State GetState(string id)
        {
            foreach (State state in states)
            {
                if (state.ID == id)
                {
                    return state;
                }
            }
            return null;
        }

        public void SetState(string id)
        {
            string stateData = "";
            foreach (State state in states)
            {
                if (state.ID == id)
                {
                    if (currentState != null)
                    {
                        stateData = currentState.StateData();
                        currentState.shutdown();
                    }
                    currentState = state;
                    currentState.initialize(stateData);
                }
            }
        }

        public string getNextState()
        {
            return currentState.getNextState();
        }

        public bool Update(GameTime gameTime)
        {
            if (currentState.Update(gameTime) == true)
            {
                return true;
            }
            return false;
        }

        public void Draw(GameTime gameTime)
        {
            currentState.Draw(gameTime);
        }
    }
}
