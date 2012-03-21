using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Soul_Editor
{
    public class SoulFile
    {
        private static string line = null;
        private static uint lineCounter = 0;
        private static StreamReader file;

        public static void Write(Form1 form, String filename)
        {
            if(!form.checkBox1.Checked)
            {
                String backgrounds = @"[Backgrounds-back]
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0008_Layer-1|1,0|1,0|
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0007_Layer-2|0,5|0,0|
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0006_Layer-3|1,0|0,1|
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0005_Layer-4|1,0|0,2|
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0005_Layer-4|1,0|0,0|

0:0=0:0|Scrolling|Backgrounds\\background__0002s_0004_Layer-5|1,0|0,0|
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0003_Layer-6|2,0|0,0|
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0002_Layer-7|2,0|0,0|
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0001_Layer-8|2,0|0,0|

0:0=0:0|Batch|1000;3000;True;True;|5,0|0,4|
0:0=0:0|Pillar|PILLAR_1|-4,0|1,0|
END

[Backgrounds-front]
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0000_Layer-9|2,0|0,0|
END

[Enemies]
";
                File.WriteAllText(filename, backgrounds);
            }

            int c = 0;
            string[] lines = new string[form._items.Count];
            foreach (Entity e in form._items)
            {
                String entity = e.levelTime.X.ToString() +
                    ":" + e.levelTime.Y.ToString() +
                    "|" + e.type.ToString() +
                    "|-100," + (e.pos.Y * 2).ToString() + "|";
                if (e.type.Equals("DARK_THOUGHT") || (e.type.Equals("DARK_WHISPER") && e.isPath) || e.type.Equals("INNER_DEMON"))
                {
                    entity = entity + e.pathText + ";" + e.pathLoop.ToString() + ";" + e.pathLoopType.ToString() + ";|";
                }

                lines.SetValue(entity, c++);
            }

            File.AppendAllLines(filename, lines);
            File.AppendAllText(filename, "\nEND");
        }

        public static void Read(List<Entity> e, String filename)
        {
            e.Clear();
            file = new StreamReader(filename);

            if (System.IO.File.Exists(filename) == false)
            {
                throw new System.InvalidOperationException("Error: " + filename + " could not be found.");
            }
            line = file.ReadLine();
            while (line != null)
            {
                if (line == "")
                {
                    line = file.ReadLine();
                    lineCounter++;
                    continue;   // skip if the line is empty
                }

                if (line[0] == '/')
                {
                    line = file.ReadLine();
                    lineCounter++;
                    continue;   // skip reading the line if it is marked as a comment
                }

                if (line == "[Enemies]")
                {
                    lineCounter++;
                    line = file.ReadLine();
                    ReadEnemies(e);
                }
                //lineCounter++;
                //line = file.ReadLine();
            }
            file.Close();
        }

        private static void ReadEnemies(List<Entity> e)
        {
            
        }
    }
}