using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class TutorialBase
    {
        private List<TutorialWidget> widgetList = null;
        private string id = "";
        public bool done = false;

        public TutorialBase(string id)
        {
            widgetList = new List<TutorialWidget>();
            this.id = id;
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            foreach (TutorialWidget widget in widgetList)
            {
                widget.Update(gameTime, playerPosition);
            }
        }

        public void Draw()
        {
            foreach (TutorialWidget widget in widgetList)
            {
                widget.Draw();
            }
        }

        public void AddTutorialWidget(TutorialWidget widget)
        {
            widgetList.Add(widget);
        }

        public void FadeOut(string id)
        {
            for (int i = 0; i < widgetList.Count; i++)
            {
                if (widgetList[i].ID == id)
                {
                    widgetList[i].FadeOut();
                }
            }
        }

        public void FadeIn(string id)
        {
            for (int i = 0; i < widgetList.Count; i++)
            {
                if (widgetList[i].ID == id)
                {
                    widgetList[i].FadeIn();
                }
            }
        }

        public void FadeIn()
        {
            for (int i = 0; i < widgetList.Count; i++)
            {
                widgetList[i].FadeIn();
            }
        }

        

        public bool Done()
        {
            foreach (TutorialWidget i in widgetList)
            {
                if (i.done == false)
                {
                    return false;
                }
            }
            return true;
        }

        public string ID { get { return id; } }
    }
}
