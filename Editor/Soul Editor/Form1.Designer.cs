using System.Windows.Forms;
namespace Soul_Editor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Dark_Thought");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Dark_Whisper");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Inner_Demon");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Nightmare");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Red_blood_vessel");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Blue_blood_vessel");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Purple_blood_vessel");
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Bloodvessels", new System.Windows.Forms.TreeNode[] {
            treeNode28,
            treeNode29,
            treeNode30});
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Enemies", new System.Windows.Forms.TreeNode[] {
            treeNode24,
            treeNode25,
            treeNode26,
            treeNode27,
            treeNode31});
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Health_Powerup");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Weapon_Powerup");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Powerups", new System.Windows.Forms.TreeNode[] {
            treeNode33,
            treeNode34});
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Tinted color 1");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Tinted color 2");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Cliff formation 1");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Cloud formation 1");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Cloud formation 2");
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Large leftleaning 1");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Large leftleaning 2");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("Small rightleaning 1");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Large rightleaning 1");
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("Pillars", new System.Windows.Forms.TreeNode[] {
            treeNode41,
            treeNode42,
            treeNode43,
            treeNode44});
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("Backgrounds", new System.Windows.Forms.TreeNode[] {
            treeNode36,
            treeNode37,
            treeNode38,
            treeNode39,
            treeNode40,
            treeNode45});
            this.minutes = new System.Windows.Forms.TextBox();
            this.pos = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.milliseconds = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.posY = new System.Windows.Forms.TextBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button7 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // minutes
            // 
            this.minutes.Location = new System.Drawing.Point(9, 52);
            this.minutes.Name = "minutes";
            this.minutes.Size = new System.Drawing.Size(27, 20);
            this.minutes.TabIndex = 3;
            this.minutes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.position_KeyPress);
            // 
            // pos
            // 
            this.pos.AutoSize = true;
            this.pos.Location = new System.Drawing.Point(107, 36);
            this.pos.Name = "pos";
            this.pos.Size = new System.Drawing.Size(34, 13);
            this.pos.TabIndex = 4;
            this.pos.Text = "Y-pos";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(138, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.path_KeyPress);
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Location = new System.Drawing.Point(6, 19);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(68, 13);
            this.name.TabIndex = 7;
            this.name.Text = "Enemy name";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(6, 19);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(188, 108);
            this.listBox2.TabIndex = 18;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(9, 38);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(156, 20);
            this.textBox6.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Filename";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel1.Size = new System.Drawing.Size(960, 380);
            this.panel1.TabIndex = 21;
            // 
            // panel2
            // 
            this.panel2.AllowDrop = true;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 360);
            this.panel2.TabIndex = 0;
            this.panel2.DragDrop += new System.Windows.Forms.DragEventHandler(this.background_DragDrop);
            this.panel2.DragEnter += new System.Windows.Forms.DragEventHandler(this.background_DragEnter);
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Location = new System.Drawing.Point(6, 19);
            this.treeView1.Name = "treeView1";
            treeNode24.Name = "Node5";
            treeNode24.Text = "Dark_Thought";
            treeNode25.Name = "Node6";
            treeNode25.Text = "Dark_Whisper";
            treeNode26.Name = "Node7";
            treeNode26.Text = "Inner_Demon";
            treeNode27.Name = "Node8";
            treeNode27.Text = "Nightmare";
            treeNode28.Name = "Node10";
            treeNode28.Text = "Red_blood_vessel";
            treeNode29.Name = "Node11";
            treeNode29.Text = "Blue_blood_vessel";
            treeNode30.Name = "Node12";
            treeNode30.Text = "Purple_blood_vessel";
            treeNode31.Name = "Node9";
            treeNode31.Text = "Bloodvessels";
            treeNode32.Name = "Node1";
            treeNode32.Text = "Enemies";
            treeNode33.Name = "Node3";
            treeNode33.Text = "Health_Powerup";
            treeNode34.Name = "Node4";
            treeNode34.Text = "Weapon_Powerup";
            treeNode35.Name = "Node2";
            treeNode35.Text = "Powerups";
            treeNode36.Name = "Node13";
            treeNode36.Text = "Tinted color 1";
            treeNode37.Name = "Node14";
            treeNode37.Text = "Tinted color 2";
            treeNode38.Name = "Node15";
            treeNode38.Text = "Cliff formation 1";
            treeNode39.Name = "Node16";
            treeNode39.Text = "Cloud formation 1";
            treeNode40.Name = "Node17";
            treeNode40.Text = "Cloud formation 2";
            treeNode41.Name = "Node19";
            treeNode41.Text = "Large leftleaning 1";
            treeNode42.Name = "Node20";
            treeNode42.Text = "Large leftleaning 2";
            treeNode43.Name = "Node21";
            treeNode43.Text = "Small rightleaning 1";
            treeNode44.Name = "Node22";
            treeNode44.Text = "Large rightleaning 1";
            treeNode45.Name = "Node18";
            treeNode45.Text = "Pillars";
            treeNode46.Name = "Node0";
            treeNode46.Text = "Backgrounds";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode32,
            treeNode35,
            treeNode46});
            this.treeView1.Size = new System.Drawing.Size(188, 108);
            this.treeView1.TabIndex = 22;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 401);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 140);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add entity";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBox2);
            this.groupBox2.Location = new System.Drawing.Point(218, 401);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 140);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Existing entities";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.milliseconds);
            this.groupBox3.Controls.Add(this.checkBox4);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.posY);
            this.groupBox3.Controls.Add(this.name);
            this.groupBox3.Controls.Add(this.minutes);
            this.groupBox3.Controls.Add(this.pos);
            this.groupBox3.Location = new System.Drawing.Point(424, 401);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(167, 140);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Edit entity";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox3);
            this.groupBox5.Controls.Add(this.checkBox2);
            this.groupBox5.Controls.Add(this.button6);
            this.groupBox5.Controls.Add(this.textBox1);
            this.groupBox5.Enabled = false;
            this.groupBox5.Location = new System.Drawing.Point(17, 78);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(150, 62);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Path settings";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(6, 39);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(50, 17);
            this.checkBox3.TabIndex = 26;
            this.checkBox3.Text = "Loop";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(62, 39);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(50, 17);
            this.checkBox2.TabIndex = 25;
            this.checkBox2.Text = "Type";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(121, 33);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(23, 23);
            this.button6.TabIndex = 24;
            this.button6.Text = "+";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Mins";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Milliseconds";
            // 
            // milliseconds
            // 
            this.milliseconds.Location = new System.Drawing.Point(42, 52);
            this.milliseconds.Name = "milliseconds";
            this.milliseconds.Size = new System.Drawing.Size(62, 20);
            this.milliseconds.TabIndex = 15;
            this.milliseconds.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.position_KeyPress);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(114, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // posY
            // 
            this.posY.Location = new System.Drawing.Point(110, 52);
            this.posY.Name = "posY";
            this.posY.Size = new System.Drawing.Size(47, 20);
            this.posY.TabIndex = 12;
            this.posY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.position_KeyPress);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Enabled = false;
            this.checkBox4.Location = new System.Drawing.Point(2, 99);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(15, 14);
            this.checkBox4.TabIndex = 27;
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.textBox2);
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.textBox6);
            this.groupBox4.Location = new System.Drawing.Point(597, 401);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 140);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Level data";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(114, 88);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(79, 17);
            this.checkBox1.TabIndex = 26;
            this.checkBox1.Text = "Append file";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Starts at (s)";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(67, 86);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(41, 20);
            this.textBox2.TabIndex = 24;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(171, 36);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(23, 23);
            this.button5.TabIndex = 23;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(91, 60);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 22;
            this.button4.Text = "Save";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 60);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "Load";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(815, 395);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(157, 23);
            this.button7.TabIndex = 24;
            this.button7.Text = "Add background to the left";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(815, 421);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Time in level:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(890, 421);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "label5";
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::Soul_Editor.Properties.Resources.background__0002s_0008_Layer_1;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.button2);
            this.panel3.Enabled = false;
            this.panel3.Location = new System.Drawing.Point(182, 22);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(640, 360);
            this.panel3.TabIndex = 1;
            this.panel3.Visible = false;
            this.panel3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(517, 334);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Done setting path";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 552);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Soul Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Panel panel2;
        public System.Windows.Forms.ListBox listBox2;
        public CheckBox checkBox1;

        private System.Windows.Forms.PictureBox[] backgrounds;
        private System.Windows.Forms.TextBox minutes;
        private System.Windows.Forms.Label pos;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox posY;
        private System.Windows.Forms.Panel panel1;
        private Button button1;
        private Label label3;
        private Label label2;
        private TextBox milliseconds;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button5;
        private Button button4;
        private Button button3;
        private Button button6;
        private Button button7;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox textBox2;
        private GroupBox groupBox5;
        private CheckBox checkBox3;
        private CheckBox checkBox2;
        private Panel panel3;
        private CheckBox checkBox4;
        private Button button2;
    }
}

