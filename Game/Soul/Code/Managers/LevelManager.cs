using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul.Manager
{
    class LevelManager
    {
        List<Level> levelList;
        Level currentLevel = null;

        public LevelManager()
        {
            levelList = new List<Level>();
        }

        public int Update(GameTime gameTime)
        {
            int value = 0;
            if (currentLevel != null)
            {
                value = currentLevel.Update(gameTime);
            }
            return value;
        }

        public void Draw(GameTime gameTime)
        {
            if (currentLevel != null)
            {
                currentLevel.Draw(gameTime);
            }
        }

        public void setLevel(string id)
        {
            Level tmpLevel = null;
            foreach (Level level in levelList)
            {
                if (level.ID == id)
                {
                    tmpLevel = level;
                    break;
                }
            }

            if (tmpLevel == null)
            {
                // TEMP FOR DEBUG
                currentLevel = levelList[0];
                //currentLevel = tmpLevel;
                currentLevel.initialize();
                //throw new System.InvalidOperationException("Error: level " + id + " could not be found.");
            }
            else
            {
                if (currentLevel != null)
                {
                    currentLevel.shutdown();
                }
                currentLevel = tmpLevel;
                currentLevel.initialize();
            }

            
        }

        public void AddLevel(Level level)
        {
            levelList.Add(level);
        }

        public string CurrentLevelID
        {
            get
            {
                if (currentLevel != null)
                {
                    return currentLevel.ID;
                }
                return "";
            }
        }

        public void stopBgScroll()
        {
            currentLevel.stopBgScroll();
        }

        public void cleansedLevel()
        {
            currentLevel.cleansing = true;
        }
    }
}
