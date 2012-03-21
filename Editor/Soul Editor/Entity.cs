using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Soul_Editor
{
    public class Entity
    {
        public String name;
        public String type;
        public Point pos;
        public Point levelTime;
        Form1 form;
        //public int scroll;
        //private PictureBox image;
        //private Bitmap imagePath;
        public TransparentControl picture;
        public int id = 0;
        private bool isDragging = false;
        public LinkedList<Point> path = new LinkedList<Point>();
        public int lastPoint = 99;
        public String pathText;
        public bool isPath = false;
        public bool pathLoop = true;
        public bool pathLoopType = true;

        public Entity(Form1 form, String type, int x, int y, int id)
        {
            this.type = type;
            this.form = form;
            this.id = id;
            name = form.maxEntityCount++ + "_" + type;
            picture = new TransparentControl(name);
            this.type = this.type.ToUpper();
            setUniqueData();
            //picture.Image = global::Soul_Editor.Properties.Resources.RedBloodvessel;
            picture.Size = new Size((int)picture.Image.PhysicalDimension.Width / 2, (int)picture.Image.PhysicalDimension.Height / 2);
            Move(x, y);
            //picture.Location = new Point(pos.X, pos.Y);
            picture.Bounds = new Rectangle(picture.Location, picture.Size);
            form.panel2.Controls.Add(picture);
            picture.BringToFront();
            picture.MouseDown += new MouseEventHandler(entity_MouseDown);
            picture.MouseUp += new MouseEventHandler(entity_MouseUp);
            if (isPath)
            {
                path.AddFirst(new Point(1280, pos.Y * 2));
                path.AddFirst(new Point(0, pos.Y * 2));
                updatePathText();
            }
            //picture.MouseMove += new MouseEventHandler(entity_MouseMove);
        }

        public Entity(Form1 form, String type, int ms, int y)
        {
            this.type = type;
            this.form = form;
            this.id = 0;
            name = form.maxEntityCount++ + "_" + type;
            picture = new TransparentControl(name);
            this.type = this.type.ToUpper();
            setUniqueData();
            //picture.Image = global::Soul_Editor.Properties.Resources.RedBloodvessel;
            picture.Size = new Size((int)picture.Image.PhysicalDimension.Width / 2, (int)picture.Image.PhysicalDimension.Height / 2);

            calculatePosition(ms);
            pos.Y = y / 2;
            Move(pos.X, pos.Y);

            //picture.Location = new Point(pos.X, pos.Y);
            picture.Bounds = new Rectangle(picture.Location, picture.Size);
            form.panel2.Controls.Add(picture);
            picture.BringToFront();
            picture.MouseDown += new MouseEventHandler(entity_MouseDown);
            picture.MouseUp += new MouseEventHandler(entity_MouseUp);

            if (isPath)
            {
                path.AddFirst(new Point(1280, pos.Y * 2));
                path.AddFirst(new Point(0, pos.Y * 2));
                updatePathText();
            }
            //picture.MouseMove += new MouseEventHandler(entity_MouseMove);
        }

        private void setUniqueData()
        {
            if (type.Equals("BLUE_BLOOD_VESSEL"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.BlueBloodvessel;
            }
            else if (type.Equals("RED_BLOOD_VESSEL"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.RedBloodvessel;
            }
            else if (type.Equals("PURPLE_BLOOD_VESSEL"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.PurpleBloodvessel;
            }
            else if (type.Equals("DARK_THOUGHT"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.DarkThought;
                isPath = true;
            }
            else if (type.Equals("DARK_WHISPER"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.DarkWhisper;
                isPath = true;
            }
            else if (type.Equals("NIGHTMARE"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.Nightmare;
            }
            else if (type.Equals("INNER_DEMON"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.InnerDemon;
                isPath = true;
            }
            else if (type.Equals("HEALTH_POWERUP"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.Health;
            }
            else if (type.Equals("WEAPON_POWERUP_SPREAD"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.Spread;
            }
            else if (type.Equals("WEAPON_POWERUP_RAPID"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.Rapid;
            }
            else if (type.Equals("BOSS"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.Boss;
            }
        }

        public void Move(int x, int y)
        {
            pos.X = x;
            pos.Y = y;
            picture.Location = new Point(x - picture.Width / 2, y - picture.Height / 2);

            calculateTime();
        }

        public void Move(int x, int y, bool lol)
        {
            pos.X += x - picture.Width / 2;
            pos.Y += y - picture.Height / 2;
            picture.Location = new Point(pos.X - picture.Width / 2, pos.Y - picture.Height / 2);

            calculateTime();
        }

        public void calculateTime()
        {
            int milliSeconds = (form.panel2.Width - pos.X) * 4 + form.startTime * 1000;
            int minutes = milliSeconds / 60000;
            milliSeconds = milliSeconds % 60000;
            levelTime = new Point(minutes, milliSeconds);
        }

        public void calculatePosition(int ms)
        {
            ms += form.startTime * 1000;
            int minutes = ms / 60000;
            int milliseconds = ms % 60000;
            levelTime = new Point(minutes, milliseconds);

            pos.X = form.panel2.Width - (ms / 4);
        }

        private void entity_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isDragging)
            {
                isDragging = true; 
                form.updatePosition(id);
            }
        }

        private void entity_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Move(e.X, e.Y, true);
            }
        }

        private void entity_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Move(e.X, e.Y, true);
                isDragging = false;
                form.listBox2.SelectedIndex = id;
            }
        }

        public void addPath(int x, int y)
        {
            path.AddLast(new Point(x, y));
            updatePathText();
        }

        public void changePath(String p)
        {
            pathText = p;
            path.Clear();
            String[] ps = p.Split('=');
            foreach (String s in ps)
            {
                if(!s.Equals(""))
                {
                    String[] ss = s.Split(',');
                    path.AddLast(new Point(int.Parse(ss[0]), int.Parse(ss[1])));
                }
            }
        }

        private void updatePathText()
        {
            pathText = "";
            foreach (Point p in path)
            {
                pathText += p.X.ToString() + "," + p.Y.ToString() + "=";
            }
        }
    }
}
