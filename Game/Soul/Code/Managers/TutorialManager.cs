using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class TutorialManager
    {
        private List<TutorialBase> tutorialList = null;
        private SpriteBatch spriteBatch = null;
        private Soul game = null;
        private TutorialBase currentTutorial = null;
        private SpriteFont spriteFont = null;
        private InputManager inputManager = null;

        public TutorialManager(SpriteBatch spriteBatch, Soul game, SpriteFont spriteFont, InputManager inputManager)
        {
            this.tutorialList = new List<TutorialBase>();
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.spriteFont = spriteFont;
            this.inputManager = inputManager;
        }

        public void Initialize(string id)
        {
            CreateTutorial();
            foreach (TutorialBase i in tutorialList)
            {
                if (i.ID == id)
                {
                    currentTutorial = i;
                    break;
                }
            }
            currentTutorial.FadeIn();
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (currentTutorial != null)
            {
                if (currentTutorial.ID == "movement")
                {

                    if (inputManager.MoveLeft == true)
                    {
                        currentTutorial.FadeOut("arrow_left");
                    }

                    if (inputManager.MoveRight == true)
                    {
                        currentTutorial.FadeOut("arrow_right");
                    }

                    if (inputManager.MoveUp == true)
                    {
                        currentTutorial.FadeOut("arrow_up");
                    }

                    if (inputManager.MoveDown == true)
                    {
                        currentTutorial.FadeOut("arrow_down");
                    }
                }
                else if (currentTutorial.ID == "shoot")
                {
                    if (inputManager.Shooting == true)
                    {
                        currentTutorial.FadeOut("shoot");
                    }
                }

                

                currentTutorial.Update(gameTime, playerPosition);
                if (currentTutorial.ID == "movement")
                {
                    if (currentTutorial.Done() == true)
                    {
                        currentTutorial = setNewTutorialBase("shoot");
                        currentTutorial.FadeIn();
                    }
                }
            }
        }

        public void Draw()
        {
            if (currentTutorial != null)
            {
                currentTutorial.Draw();
            }
        }

        private void CreateTutorial()
        {
            TutorialSprite tSprite = null;
            TutorialString tString = null;

            // Creating movement tutorial base
            TutorialBase tBase = new TutorialBase("movement");
            string key = game.config.getValue("Controls", "Up");
            if (key == "Up")
            {
                tSprite = new TutorialSprite(spriteBatch, game, "arrow_up", Constants.TUTORIAL_ARROW, CreateFrame(key), new Vector2(0f, 100f));
                tBase.AddTutorialWidget(tSprite);
            }
            else
            {
                tString = new TutorialString(spriteBatch, game, "arrow_up", spriteFont, key, CreateFrame(key), new Vector2(0f, 100f));
                tBase.AddTutorialWidget(tString);
            }

            key = game.config.getValue("Controls", "Down");
            if (key == "Down")
            {
                tSprite = new TutorialSprite(spriteBatch, game, "arrow_down", Constants.TUTORIAL_ARROW, CreateFrame(key), new Vector2(0f, -100f), (float)Math.PI);
                tBase.AddTutorialWidget(tSprite);
            }
            else
            {
                tString = new TutorialString(spriteBatch, game, "arrow_down", spriteFont, key, CreateFrame(key), new Vector2(0f, -100f));
                tBase.AddTutorialWidget(tString);
            }

            key = game.config.getValue("Controls", "Left");
            if (key == "Left")
            {
                tSprite = new TutorialSprite(spriteBatch, game, "arrow_left", Constants.TUTORIAL_ARROW, CreateFrame(key), new Vector2(100f, 0f), -(float)Math.PI * 0.5f);
                tBase.AddTutorialWidget(tSprite);
            }
            else
            {
                tString = new TutorialString(spriteBatch, game, "arrow_left", spriteFont, key, CreateFrame(key), new Vector2(100f, 0f));
                tBase.AddTutorialWidget(tString);
            }

            key = game.config.getValue("Controls", "Right");
            if (key == "Right")
            {
                tSprite = new TutorialSprite(spriteBatch, game, "arrow_right", Constants.TUTORIAL_ARROW, CreateFrame(key), new Vector2(-100f, 0f), (float)Math.PI * 0.5f);
                tBase.AddTutorialWidget(tSprite);
            }
            else
            {
                tString = new TutorialString(spriteBatch, game, "arrow_right", spriteFont, key, CreateFrame(key), new Vector2(-100f, 0f));
                tBase.AddTutorialWidget(tString);
            }

            tutorialList.Add(tBase);

            tBase = new TutorialBase("shoot");
            key = game.config.getValue("Controls", "Shoot");
            if (key == "Enter")
            {
                tSprite = new TutorialSprite(spriteBatch, game, "shoot", Constants.TUTORIAL_BUTTON_ENTER, CreateFrame(key), new Vector2(120f, 0f));
                tBase.AddTutorialWidget(tSprite);
            }
            else if (key == "LeftShift" || key == "RightShift")
            {
                tSprite = new TutorialSprite(spriteBatch, game, "shoot", Constants.TUTORIAL_BUTTON_SHIFT, CreateFrame(key), new Vector2(120f, 0f));
                tBase.AddTutorialWidget(tSprite);
            }
            else
            {
                tString = new TutorialString(spriteBatch, game, "shoot", spriteFont, key, CreateFrame(key), new Vector2(120f, 0f));
                tBase.AddTutorialWidget(tString);
            }

            tutorialList.Add(tBase);

        }

        private string CreateFrame(string text)
        {
            if (text == "Space")
            {
                return Constants.TUTORIAL_BUTTON_FRAME_XLLARGE;
            }
            else if (text == "Enter")
            {
                return Constants.TUTORIAL_BUTTON_FRAME_ENTER;
            }
            else if (text == "LeftShift" || text == "RightShift" || text == "LeftAlt" || text == "RightAlt")
            {
                return Constants.TUTORIAL_BUTTON_FRAME_LARGE;
            }

            return Constants.TUTORIAL_BUTTON_FRAME;
        }

        private TutorialBase setNewTutorialBase(string id)
        {
            foreach (TutorialBase i in tutorialList)
            {
                if (i.ID == id)
                {
                    return i;
                }
            }
            return null;
        }
    }
}
