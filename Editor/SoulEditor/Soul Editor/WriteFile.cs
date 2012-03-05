using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Soul_Editor
{
    public class WriteFile
    {
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
                if (e.type.Equals("DARK_THOUGHT") || e.type.Equals("DARK_WHISPER") || e.type.Equals("INNER_DEMON"))
                {
                    entity = entity + "300,200=300,200=;True;True;|";
                }

                lines.SetValue(entity, c++);
            }

            File.AppendAllLines(filename, lines);
            File.AppendAllText(filename, "\nEND");
        }
    }
}

/*
 * / Use "/" to add comments. Lines like this one will be ignored by the level reader.

/ Backgrounds
/ 0:0-0:0 <- this is what time the background will start scrolling at, and what time it will stop at. if it is set at 0:0-0:0, then it means it will start at the level start and never stop
/ Background_back <- this is the filename used for the background
/ 3,0 <- this is at what direction and speed it will scroll
/ 0,0 <- what layer it will be included into

[Backgrounds-back]
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

/ Pillar går från PILLAR_1 till PILLAR_13

END

/ Entity types that the level reader will recognize:
/ DARK_THOUGHT, NIGHTMARE, RED_BLOOD_VESSEL, INNER_DEMON
/ 00:00 <- time until entity will spawn |RED_BLOOD_VESSEL <- type of entity that will spawn| 0,100 <- where the entity will spawn|100,300-300,600 <- movement path (if available to entity)


[Backgrounds-front]
0:0=0:0|Scrolling|Backgrounds\\background__0002s_0000_Layer-9|2,0|0,0|
END

[Enemies]


0:2000|RED_BLOOD_VESSEL|-100,100|
0:2100|PURPLE_BLOOD_VESSEL|-100,400|
0:2300|BLUE_BLOOD_VESSEL|-100,100|
0:11000|DARK_THOUGHT|0,500|200,650=300,100=400,500=500,200=700,600=1400,0=;True;True;|
0:8000|DARK_WHISPER|-100,100|100,100=100,100=1300,0=;True;True;|
0:14200|INNER_DEMON|-100,200|300,200;True;True;|
1:5900|NIGHTMARE|-100,100|

0:2000|WEAPON_POWERUP|-100,100|
0:2000|HEALTH_POWERUP|-100,100|

END
 * */
