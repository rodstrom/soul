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
        //public event ButtonEventHandler onClick = null;
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
        private bool changeState = false;

        public BrainMapManager(SpriteBatch spriteBatch, Soul game, InputManager controls, Vector2 position)
        {
            bg = new Sprite(spriteBatch, game, Constants.BRAIN_MAP_BG);
            mapList = new List<List<BrainMapMarker>>();

            List<BrainMapMarker> tmpList = new List<BrainMapMarker>();
            mapList.Add(tmpList);
            tmpList = new List<BrainMapMarker>();
            mapList.Add(tmpList);
            tmpList = new List<BrainMapMarker>();
            mapList.Add(tmpList);

            menuManager = new MenuManager(controls);
            ImageButton button = new ImageButton(spriteBatch, game, controls, new Vector2((float)game.Window.ClientBounds.Width * 0.5f - 150, (float)game.Window.ClientBounds.Height * 0.5f + 200), "GUI\\button_continue", "cleanse");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, controls, new Vector2((float)game.Window.ClientBounds.Width * 0.5f + 150, (float)game.Window.ClientBounds.Height * 0.5f + 200), "GUI\\button_quit_pause", "back");
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

        public bool Update(GameTime gameTime)
        {
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
            return changeState;
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
        }

        private void OnButtonPress(ImageButton button)
        {
            if (button.ID == "cleanse")
            {
                changeState = true;
            }
            else if (button.ID == "back")
            {
                showMenu = false;
                fadeinOut.FadeIn();
                mapList[_x][_y].Deselect();
                menuManager.FadeOut();
                menuManager.Reset();
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
            }
            else if (controls.MoveRightOnce == true)
            {
                menuManager.increment();
            }
        }

        public string CurrentLevel { get { return currentLevel; } }

    }
}
