using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Soul_Editor
{
    public partial class Form1 : Form
    {
        public List<Entity> _items = new List<Entity>();
        String tempEntityName = "";
        public int maxEntityCount = 0;
        private int numberOfBackgrounds = -1;
        public int startTime = 0;

        public Form1()
        {
            InitializeComponent();
            this.backgrounds = new PictureBox[1000];
            this.addBackground();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox6.Text = Directory.GetCurrentDirectory() + "\\Level01.map";
        }

        private void addEntity(String entity, Point pos)
        {
            Entity e = new Entity(this, entity, pos.X, pos.Y , 0);

            _items.Add(e);
            e.id = listBox2.Items.Add(e.name);
            //listBox2.DataSource = _items;
            listBox2.SelectedIndex = e.id;
            //listBox2.Update();
        }

        private void addBackground()
        {
            numberOfBackgrounds++;
            this.backgrounds[numberOfBackgrounds] = new PictureBox();
            this.backgrounds[numberOfBackgrounds].BackgroundImage = global::Soul_Editor.Properties.Resources.background__0002s_0008_Layer_1;
            this.backgrounds[numberOfBackgrounds].BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backgrounds[numberOfBackgrounds].Cursor = System.Windows.Forms.Cursors.Default;
            this.backgrounds[numberOfBackgrounds].Image = global::Soul_Editor.Properties.Resources.background__0002s_0000_Layer_9;
            this.backgrounds[numberOfBackgrounds].Location = new System.Drawing.Point(1280 * numberOfBackgrounds, 0);
            this.backgrounds[numberOfBackgrounds].Margin = new System.Windows.Forms.Padding(0);
            this.backgrounds[numberOfBackgrounds].Name = "background";
            this.backgrounds[numberOfBackgrounds].Size = new System.Drawing.Size(1280, 360);
            this.backgrounds[numberOfBackgrounds].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backgrounds[numberOfBackgrounds].TabIndex = 1;
            this.backgrounds[numberOfBackgrounds].TabStop = false;
            this.panel2.Width += 1280;
            this.panel2.Controls.Add(this.backgrounds[numberOfBackgrounds]);

            foreach (Entity e in _items)
            {
                e.Move(1280, 0, true);
            }

            updateLevelTimeLabel();
        }

        public void updatePosition(int i)
        {
            if (i >= 0)
            {
                listBox2.SelectedIndex = i;
                name.Text = _items.ElementAt(i).name.ToString();
                minutes.Text = _items.ElementAt(i).levelTime.X.ToString();
                milliseconds.Text = _items.ElementAt(i).levelTime.Y.ToString();
                posY.Text = (_items.ElementAt(i).pos.Y * 2).ToString();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                name.Text = _items.ElementAt(listBox2.SelectedIndex).name.ToString();
                minutes.Text = _items.ElementAt(listBox2.SelectedIndex).levelTime.X.ToString();
                milliseconds.Text = _items.ElementAt(listBox2.SelectedIndex).levelTime.Y.ToString();
                posY.Text = (_items.ElementAt(listBox2.SelectedIndex).pos.Y * 2).ToString();
            }
            else
            {
                if (listBox2.Items.Count > 0)
                {
                    listBox2.SelectedIndex = 0;
                }
            }
        }

        private void position_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (minutes.Text == null)
                {
                    minutes.Text = "0";
                }
                int ms = int.Parse(minutes.Text.ToString()) * 60000 + int.Parse(milliseconds.Text.ToString());
                int x = panel2.Width - ((ms - startTime) / 2);
                _items.ElementAt(listBox2.SelectedIndex).Move(x, int.Parse(posY.Text.ToString()) / 2);
            }
        }

        private void background_DragDrop(object sender, DragEventArgs e)
        {
            Point pos = this.panel2.PointToClient(new Point(e.X, e.Y));
            addEntity(tempEntityName, pos);
        }

        private void background_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            tempEntityName = e.Item.ToString().Substring(10);
            this.treeView1.DoDragDrop(e.Item.ToString(), DragDropEffects.Move);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _items.ElementAt(listBox2.SelectedIndex).picture.Dispose();
            _items.RemoveAt(listBox2.SelectedIndex);
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult d = folderBrowserDialog1.ShowDialog();
            textBox6.Text = folderBrowserDialog1.SelectedPath + "\\Level01.map";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WriteFile.Write(this, textBox6.Text.ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            addBackground();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != null)
            {
                startTime = int.Parse(textBox2.Text);
            }
            else
            {
                startTime = 0;
            }

            foreach (Entity i in _items)
            {
                i.calculateTime();
            }
            updatePosition(listBox2.SelectedIndex);
            updateLevelTimeLabel();
        }

        private void updateLevelTimeLabel()
        {
            this.label5.Text = startTime + " - " + (this.panel2.Width / 500 + startTime).ToString() + " seconds";
        }
    }
}
