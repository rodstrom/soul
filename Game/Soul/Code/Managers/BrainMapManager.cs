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
        private List<BrainMapMarker> mapList;
        private int currentPosition = 0;
        private string currentLevel = "";
        private bool showMenu = false;
        private MenuManager menuManager = null;
        private FadeInOut fadeinOut = null;
        private InputManager controls = null;
        private AudioManager audioManager = null;
        private bool changeState = false;
        private int returnValue = 0;
        private BrainMapMarker currentMapMarker = null;

        public BrainMapManager(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, Vector2 position)
        {
            this.audioManager = audioManager;
            bg = new Sprite(spriteBatch, game, Constants.BRAIN_MAP_BG);
            mapList = new List<BrainMapMarker>();

            List<BrainMapMarker> tmpList = new List<BrainMapMarker>();
            /*mapList.Add(tmpList);
            tmpList = new List<BrainMapMarker>();
            mapList.Add(tmpList);
            tmpList = new List<BrainMapMarker>();
            mapList.Add(tmpList);*/

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
            mapList[0].Focus = true;
            mapList[0].alpha = 255;
            changeState = false;
        }

        public int Update(GameTime gameTime)
        {
            returnValue = 0;
            if (showMenu == true && controls.Pause == true)
            {
                showMenu = false;
                fadeinOut.FadeIn();
                mapList[currentPosition].Deselect();
                menuManager.FadeOut();
                menuManager.Reset();
                audioManager.playSound("map_back");
            }
            else if (showMenu == false && controls.Pause == true)
            {
                returnValue = -1;
            }

            fadeinOut.Update(gameTime);

            if (mapList[currentPosition].Selected == false)
            {
                Input();
                for (int i = 0; i < mapList.Count; i++)
                {
                    mapList[i].Update(gameTime);
                }
            }
            else
            {
                mapList[currentPosition].Update(gameTime);
            }

            if (showMenu == true && mapList[currentPosition].Scaling == false && changeState == false)
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
                mapList[i].Draw();
            }

            if (showMenu == true && mapList[currentPosition].Scaling == false)
            {
                menuManager.Draw();
            }
        }

        public void addBrainMap(BrainMapMarker brainMapMarker)
        {
            brainMapMarker.onClick += new BrainMapMarker.ButtonEventHandler(OnBrainMarkerPress);
            mapList.Add(brainMapMarker);
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
                mapList[currentPosition].Deselect();
                menuManager.FadeOut();
                menuManager.Reset();
                audioManager.playSound("map_back");
            }
        }

#region MenuMovement
        /*public void increment()
        {
            mapList[currentPosition].Focus = false;
            mapList[currentPosition].Disappear();
            if (currentPosition + 1 > mapList.Count)
            {
                currentPosition = 0;
            }
            else
            {
                currentPosition++;
            }

            mapList[currentPosition].Focus = true;
            mapList[currentPosition].Appear();
            audioManager.playSound("map_move");

        }

        public void decrement()
        {
            mapList[currentPosition].Focus = false;
            mapList[currentPosition].Disappear();
            if (currentPosition - 1 > 0)
            {
                currentPosition = mapList.Count;
            }
            else
            {
                currentPosition--;
            }

            mapList[currentPosition].Focus = true;
            mapList[currentPosition].Appear();
            audioManager.playSound("map_move");
        }*/

        public void moveLeft()
        {
            mapList[currentPosition].Focus = false;
            mapList[currentPosition].Disappear();
            if (currentPosition + 1 >= mapList.Count)
            {
                currentPosition = 0;
            }
            else
            {
                currentPosition++;
            }

            mapList[currentPosition].Focus = true;
            mapList[currentPosition].Appear();
            audioManager.playSound("map_move");
        }

        public void moveRight()
        {
            mapList[currentPosition].Focus = false;
            mapList[currentPosition].Disappear();
            if (currentPosition - 1 < 0)
            {
                currentPosition = mapList.Count - 1;
            }
            else
            {
                currentPosition--;
            }

            mapList[currentPosition].Focus = true;
            mapList[currentPosition].Appear();
            audioManager.playSound("map_move");
        }
#endregion MenuMovement

        private void Input()
        {
            /*if (controls.MoveDownOnce == true)
            {
                decrement();
            }
            else if (controls.MoveUpOnce == true)
            {
                increment();
            }*/

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
