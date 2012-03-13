using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul.Manager
{
    class BrainMapManager
    {
        public delegate void ButtonEventHandler(bool value);
        private Sprite bg = null;
        private Vector2 offset = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        private List<List<BrainMapMarker>> mapList;
        private int _x = 0;
        private int _y = 0;
        private string currentLevel = "";
        private bool showMenu = false;
        private MenuManager menuManager = null;
        private FadeInOut fadeinOut = null;
        private InputManager controls = null;
        private AudioManager audioManager = null;
        private bool changeState = false;
        private int returnValue = 0;

        public BrainMapManager(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, Vector2 position)
        {
            this.audioManager = audioManager;
            bg = new Sprite(spriteBatch, game, Constants.BRAIN_MAP_BG);
            mapList = new List<List<BrainMapMarker>>();

            List<BrainMapMarker> tmpList = new List<BrainMapMarker>();
            mapList.Add(tmpList);
            tmpList = new List<BrainMapMarker>();
            mapList.Add(tmpList);
            tmpList = new List<BrainMapMarker>();
            mapList.Add(tmpList);

            menuManager = new MenuManager(controls);
            ImageButton button = new ImageButton(spriteBatch, game, controls, new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f - 150, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200), Constants.GUI_CLEANSE, "cleanse");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, controls, new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 150, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 200), Constants.GUI_BACK, "back");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();
            fadeinOut = new FadeInOut(spriteBatch, game, 100);
            this.position = position;
            offset = new Vector2((float)bg.X * 0.5f, (float)bg.Y *0.5f);
            this.controls = controls;
        }

        public void initialize()
        {
            mapList[0][0].Focus = true;
            mapList[0][0].alpha = 255;
            changeState = false;
        }

        public int Update(GameTime gameTime)
        {
            returnValue = 0;
            if (showMenu == true && controls.Pause == true)
            {
                showMenu = false;
                fadeinOut.FadeIn();
                mapList[_x][_y].Deselect();
                menuManager.FadeOut();
                menuManager.Reset();
                audioManager.playSound("map_back");
            }
            else if (showMenu == false && controls.Pause == true)
            {
                returnValue = -1;
            }

            fadeinOut.Update(gameTime);

            if (mapList[_x][_y].Selected == false)
            {
                Input();
                for (int i = 0; i < mapList.Count; i++)
                {
                    for (int j = 0; j < mapList[i].Count; j++)
                    {
                        mapList[i][j].Update(gameTime);
                    }
                }
            }
            else
            {
                mapList[_x][_y].Update(gameTime);
            }

            if (showMenu == true && mapList[_x][_y].Scaling == false && changeState == false)
            {
                MenuInput();
                menuManager.Update(gameTime);
            }
            return returnValue;
        }

        public void Draw()
        {
            bg.Draw(position, Color.White, 0f, offset, 1f, SpriteEffects.None, 0f);
            fadeinOut.Draw();
            for (int i = 0; i < mapList.Count; i++)
            {
                for (int j = 0; j < mapList[i].Count; j++)
                {
                    mapList[i][j].Draw();
                }
            }

            if (showMenu == true && mapList[_x][_y].Scaling == false)
            {
                menuManager.Draw();
            }
        }

        public void addBrainMap(BrainMapMarker brainMapMarker, int value)
        {
            if (value >= 3)
            {
                return;
            }
            brainMapMarker.onClick += new BrainMapMarker.ButtonEventHandler(OnBrainMarkerPress);
            mapList[value].Add(brainMapMarker);
        }

        private void OnBrainMarkerPress(BrainMapMarker brainMapMarker)
        {
            currentLevel = brainMapMarker.ID;
            fadeinOut.Reset();
            fadeinOut.FadeOut();
            menuManager.FadeIn();
            showMenu = true;
            audioManager.playSound("map_select");
        }

        private void OnButtonPress(ImageButton button)
        {
            if (button.ID == "cleanse")
            {
                changeState = true;
                audioManager.playSound("map_select_map");
                returnValue = 1;
            }
            else if (button.ID == "back")
            {
                showMenu = false;
                fadeinOut.FadeIn();
                mapList[_x][_y].Deselect();
                menuManager.FadeOut();
                menuManager.Reset();
                audioManager.playSound("map_back");
            }
        }

#region MenuMovement
        public void increment()
        {
            if (_y + 1 >= 2)
            {
                return;
            }
            mapList[_x][_y].Focus = false;
            mapList[_x][_y].Disappear();
            _y++;
            mapList[_x][_y].Focus = true;
            mapList[_x][_y].Appear();
            audioManager.playSound("map_move");

        }

        public void decrement()
        {
            if (_y - 1 < 0)
            {
                return;
            }
            mapList[_x][_y].Focus = false;
            mapList[_x][_y].Disappear();
            _y--;
            mapList[_x][_y].Focus = true;
            mapList[_x][_y].Appear();
            audioManager.playSound("map_move");
        }

        public void moveLeft()
        {
            if (_x + 1 >= 3)
            {
                return;
            }
            mapList[_x][_y].Focus = false;
            mapList[_x][_y].Disappear();
            _x++;
            mapList[_x][_y].Focus = true;
            mapList[_x][_y].Appear();
            audioManager.playSound("map_move");
        }

        public void moveRight()
        {
            if (_x - 1 < 0)
            {
                return;
            }
            mapList[_x][_y].Focus = false;
            mapList[_x][_y].Disappear();
            _x--;
            mapList[_x][_y].Focus = true;
            mapList[_x][_y].Appear();
            audioManager.playSound("map_move");
        }
#endregion MenuMovement

        private void Input()
        {
            if (controls.MoveDownOnce == true)
            {
                decrement();
            }
            else if (controls.MoveUpOnce == true)
            {
                increment();
            }

            if (controls.MoveLeftOnce)
            {
                moveLeft();
            }
            else if (controls.MoveRightOnce == true)
            {
                moveRight();
            }
        }

        private void MenuInput()
        {
            if (controls.MoveLeftOnce == true)
            {
                menuManager.decrement();
                audioManager.playSound("menu_move");
            }
            else if (controls.MoveRightOnce == true)
            {
                menuManager.increment();
                audioManager.playSound("menu_move");
            }
        }

        public string CurrentLevel { get { return currentLevel; } }

    }
}
