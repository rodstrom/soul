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

        public Entity(Form1 form, String type, int x, int y, int id)
        {
            this.type = type;
            this.form = form;
            this.id = id;
            name = form.maxEntityCount++ + "_" + type;
            picture = new TransparentControl(name);
            setUniqueData();
            this.type = this.type.ToUpper();
            //picture.Image = global::Soul_Editor.Properties.Resources.RedBloodvessel;
            picture.Size = new Size((int)picture.Image.PhysicalDimension.Width / 2, (int)picture.Image.PhysicalDimension.Height / 2);
            Move(x, y);
            //picture.Location = new Point(pos.X, pos.Y);
            picture.Bounds = new Rectangle(picture.Location, picture.Size);
            form.panel2.Controls.Add(picture);
            picture.BringToFront();
            picture.MouseDown += new MouseEventHandler(entity_MouseDown);
            picture.MouseUp += new MouseEventHandler(entity_MouseUp);
            //picture.MouseMove += new MouseEventHandler(entity_MouseMove);
        }

        private void setUniqueData()
        {
            if (type.Equals("Blue_blood_vessel"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.BlueBloodvessel;
            }
            else if (type.Equals("Red_blood_vessel"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.RedBloodvessel;
            }
            else if (type.Equals("Purple_blood_vessel"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.PurpleBloodvessel;
            }
            else if (type.Equals("Dark_Thought"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.DarkThought;
            }
            else if (type.Equals("Dark_Whisper"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.DarkWhisper;
            }
            else if (type.Equals("Nightmare"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.Nightmare;
            }
            else if (type.Equals("Inner_Demon"))
            {
                picture.Image = global::Soul_Editor.Properties.Resources.InnerDemon;
            }
            else
            {
                picture.Image = global::Soul_Editor.Properties.Resources.RedBloodvessel;
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
            int milliSeconds = (form.panel2.Width - pos.X) * 2 + form.startTime * 1000;
            int minutes = milliSeconds / 60000;
            milliSeconds = milliSeconds % 60000;
            levelTime = new Point(minutes, milliSeconds);
        }

        public void entity_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isDragging)
            {
                isDragging = true; 
                form.updatePosition(id);
            }
        }

        public void entity_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Move(e.X, e.Y, true);
            }
        }

        public void entity_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Move(e.X, e.Y, true);
                isDragging = false;
                form.listBox2.SelectedIndex = id;
            }
        }

    }
}
