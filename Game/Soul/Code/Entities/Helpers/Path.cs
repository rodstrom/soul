using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Soul
{
    class Path
    {
        List<Vector2> pathList = new List<Vector2>();
        private bool repeat = false;
        private bool backTrack = false;
        private bool backtracking = false;
        private Vector2 currentPath;
        private int path = 0;

        public Path() { }

        public Path(Vector2 path)
        {
            pathList.Add(path);
            currentPath = pathList[0];
        }

        public Path(List<Vector2> pathList, bool repeat, bool backTrack)
        {
            this.repeat = repeat;
            this.BackTrack = backTrack;
            this.pathList = pathList;
            currentPath = pathList[0];
        }

        public void initialize()
        {
            currentPath = pathList[0];
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            if (checkWaypoint(position) == true)
            {
                NextPath();
            }
        }

        public void AddPath(Vector2 path)
        {
            pathList.Add(path);
        }

        public bool setPath(int value)
        {
            if (value < 0 || value > pathList.Count - 1)
            {
                return false;
            }

            currentPath = pathList[value];
            return true;
        }

        public void NextPath()
        {
            if (backtracking == false)
            {
                this.path++;
            }
            else
            {
                this.path--;
            }

            if (backtracking == true && path == 0)
            {
                backtracking = false;
            }


            if (this.path >= pathList.Count && repeat == true && backTrack == false)
            {
                path = 0;
            }
            else if (this.path >= pathList.Count && repeat == true && backTrack == true)
            {
                path = pathList.Count - 1;
                backtracking = true;
            }
            else if (this.path >= pathList.Count && repeat == false)
            {
                return;
            }
            currentPath = pathList[path];
        }

        public Vector2 CurrentPath
        {
            get { return currentPath; }
        }

        public bool Repeat
        {
            set { repeat = value; }
        }

        public bool BackTrack { set { backTrack = value; } }

        private bool checkWaypoint(Vector2 position)
        {
            float difference = Vector2.Distance(position, currentPath);

            if (difference <= 20.0f)
            {
                return true;
            }
            return false;
        }
    }
}
