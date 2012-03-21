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
                String backgrounds = @"/ Use / to add comments. Lines like this one will be ignored by the level reader.

/ Backgrounds
/ 0:0-0:0 <- this is what time the background will start scrolling at, and what time it will stop at. if it is set at 0:0-0:0, then it means it will start at the level start and never stop
/ Background_back <- this is the filename used for the background
/ 3,0 <- this is at what direction and speed it will scroll
/ 0,0 <- what layer it will be included into
/ 0:2000|DARK_THOUGHT|0,500|200,250=400,200=600,600=;True;True;| <- NEW syntax for adding paths
/ the first True decides if the enemy will repeat its path, the second True complements the first by deciding if it will go back the same path or restart from the first point


[Backgrounds-back]
1:10000=0:0|Scrolling|Backgrounds\\background__0002s_0006_Layer-3|1,3|0,9|True|
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0006_Layer-3|1,3|0,9|True|
1:40000=0:0|Scrolling|Backgrounds\\background__0002s_0005_Layer-4|2,0|0,7|True|
/0:0=0:20000|Scrolling|Backgrounds\\background__0002s_0005_Layer-4|2,0|0,7|True|

/0:0=0:0|Batch|1000;3000;False;True;|2,2|0,7|False|
0:0=0:0|Pillar|PILLAR_1|2,5|0,6|False|
0:1000=0:0|Pillar|PILLAR_2|2,3|0,6|False|
0:5000=0:0|Pillar|PILLAR_3|2,7|0,7|False|

0:8000=0:0|Pillar|PILLAR_8|2,5|0,8|False|

0:10000=0:0|Pillar|PILLAR_6|2,5|0,8|False|

0:14000=0:0|Pillar|PILLAR_5|2,5|0,7|False|
0:18000=0:0|Pillar|PILLAR_5|2,5|0,7|False|

0:17000=0:0|Pillar|PILLAR_7|2,5|0,7|False|
0:25000=0:0|Pillar|PILLAR_7|2,5|0,7|False|


0:15000=0:0|Pillar|PILLAR_9|2,5|0,8|False|

0:28000=0:0|Pillar|PILLAR_10|2,3|0,8|False|

0:30000=0:0|Pillar|PILLAR_11|2,3|0,9|False|

0:25000=0:0|Pillar|PILLAR_12|2,5|0,9|False|
0:35000=0:0|Pillar|PILLAR_13|1,9|0,9|False|

0:34000=0:0|Pillar|PILLAR_4|2,7|0,8|False|

0:40000=0:0|Pillar|PILLAR_7|2,5|0,7|False|


0:45000=0:0|Pillar|PILLAR_1|2,5|0,6|False|
0:46000=0:0|Pillar|PILLAR_2|2,3|0,6|False|
0:50000=0:0|Pillar|PILLAR_3|2,7|0,7|False|

0:53000=0:0|Pillar|PILLAR_8|2,5|0,8|False|

0:55000=0:0|Pillar|PILLAR_6|2,5|0,8|False|

0:59000=0:0|Pillar|PILLAR_5|2,5|0,7|False|
1:3000=0:0|Pillar|PILLAR_5|2,5|0,7|False|

1:2000=0:0|Pillar|PILLAR_7|2,5|0,7|False|
1:10000=0:0|Pillar|PILLAR_7|2,5|0,7|False|


1:0=0:0|Pillar|PILLAR_9|2,5|0,8|False|

1:13000=0:0|Pillar|PILLAR_10|2,3|0,8|False|

1:15000=0:0|Pillar|PILLAR_11|2,3|0,9|False|

1:10000=0:0|Pillar|PILLAR_12|2,5|0,9|False|
1:20000=0:0|Pillar|PILLAR_13|1,9|0,9|False|

1:19000=0:0|Pillar|PILLAR_4|2,7|0,8|False|

1:25000=0:0|Pillar|PILLAR_7|2,5|0,7|False|


1:0=0:0|Pillar|PILLAR_1|2,5|0,6|False|
1:1000=0:0|Pillar|PILLAR_2|2,3|0,6|False|
1:5000=0:0|Pillar|PILLAR_3|2,7|0,7|False|

1:8000=0:0|Pillar|PILLAR_8|2,5|0,8|False|

1:10000=0:0|Pillar|PILLAR_6|2,5|0,8|False|

1:14000=0:0|Pillar|PILLAR_5|2,5|0,7|False|
1:18000=0:0|Pillar|PILLAR_5|2,5|0,7|False|

1:17000=0:0|Pillar|PILLAR_7|2,5|0,7|False|
1:25000=0:0|Pillar|PILLAR_7|2,5|0,7|False|


1:15000=0:0|Pillar|PILLAR_9|2,5|0,8|False|

1:28000=0:0|Pillar|PILLAR_10|2,3|0,8|False|

1:30000=0:0|Pillar|PILLAR_11|2,3|0,9|False|

1:25000=0:0|Pillar|PILLAR_12|2,5|0,9|False|
1:35000=0:0|Pillar|PILLAR_13|1,9|0,9|False|
1:34000=0:0|Pillar|PILLAR_4|2,7|0,8|False|
1:40000=0:0|Pillar|PILLAR_7|2,5|0,7|False|


2:0=0:0|Pillar|PILLAR_1|2,5|0,6|False|
2:1000=0:0|Pillar|PILLAR_2|2,3|0,6|False|
2:5000=0:0|Pillar|PILLAR_3|2,7|0,7|False|

2:8000=0:0|Pillar|PILLAR_8|2,5|0,8|False|

2:10000=0:0|Pillar|PILLAR_6|2,5|0,8|False|

2:14000=0:0|Pillar|PILLAR_5|2,5|0,7|False|
2:18000=0:0|Pillar|PILLAR_5|2,5|0,7|False|

2:17000=0:0|Pillar|PILLAR_7|2,5|0,7|False|
2:25000=0:0|Pillar|PILLAR_7|2,5|0,7|False|


2:15000=0:0|Pillar|PILLAR_9|2,5|0,8|False|

2:28000=0:0|Pillar|PILLAR_10|2,3|0,8|False|

2:30000=0:0|Pillar|PILLAR_11|2,3|0,9|False|

2:25000=0:0|Pillar|PILLAR_12|2,5|0,9|False|
2:35000=0:0|Pillar|PILLAR_13|1,9|0,9|False|
2:34000=0:0|Pillar|PILLAR_4|2,7|0,8|False|
2:40000=0:0|Pillar|PILLAR_7|2,5|0,7|False|

2:45000=0:0|Pillar|PILLAR_1|2,5|0,6|False|
2:46000=0:0|Pillar|PILLAR_2|2,3|0,6|False|
2:50000=0:0|Pillar|PILLAR_3|2,7|0,7|False|

2:53000=0:0|Pillar|PILLAR_8|2,5|0,8|False|

2:55000=0:0|Pillar|PILLAR_6|2,5|0,8|False|

2:59000=0:0|Pillar|PILLAR_5|2,5|0,7|False|
3:3000=0:0|Pillar|PILLAR_5|2,5|0,7|False|

3:2000=0:0|Pillar|PILLAR_7|2,5|0,7|False|
3:10000=0:0|Pillar|PILLAR_7|2,5|0,7|False|


3:0=0:0|Pillar|PILLAR_9|2,5|0,8|False|

3:13000=0:0|Pillar|PILLAR_10|2,3|0,8|False|

3:15000=0:0|Pillar|PILLAR_11|2,3|0,9|False|

3:10000=0:0|Pillar|PILLAR_12|2,5|0,9|False|
3:20000=0:0|Pillar|PILLAR_13|1,9|0,9|False|

3:19000=0:0|Pillar|PILLAR_4|2,7|0,8|False|

3:25000=0:0|Pillar|PILLAR_7|2,5|0,7|False|

END

[Backgrounds-front]
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0000_Layer-9|3,0|0,1|False|

END

/ Entity types that the level reader will recognize:
/ DARK_THOUGHT, NIGHTMARE, BLOOD_VESSEL, INNER_DEMON, DARK_WHISPER,
/ 00:00 <- time until entity will spawn |BLOOD_VESSEL <- type of entity that will spawn| 0,100 <- where the entity will spawn|100,300-300,600 <- movement path (if available to entity)

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

        public static void Read(Form1 form, String filename)
        {
            lineCounter = 0;
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
                    ReadEnemies(form);
                    return;
                }
                lineCounter++;
                line = file.ReadLine();
            }
            file.Close();
        }

        private static void ReadEnemies(Form1 form)
        {
            while (line != null)
            {
                lineCounter++;
                line = file.ReadLine();

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

                string[] keys = line.Split(new char[] { '|' });
                string[] time = keys[0].Split(new char[] { ':' });
                string[] posY = keys[2].Split(new char[] { ',' });
                int ms =  (((int.Parse(time[0]) * 60000) + int.Parse(time[1])));
                string name = keys[1];
                //Point pos = new Point(-x, int.Parse(posY[1]));

                form.addEntity(name, ms, int.Parse(posY[1]));

                if (line == "[END]")
                {
                    file.Close();
                    return;
                }
                if (lineCounter > 200)
                {
                    file.Close();
                    return;
                }
            }

            file.Close();
            return;
        }
    }
}