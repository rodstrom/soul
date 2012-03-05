using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Soul_Editor
{
    public class SoulFile
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
            TextReader file = null;
            string line = null;
            uint lineCounter = 0;

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

                if (line[0] == '[' && line[line.Length - 1] == ']')
                {
                    string category = line.Substring(1, line.Length - 2);
                    if (category == "Enemies")
                    {
                        lineCounter++;
                        line = file.ReadLine();
                        ReadEnemies();
                    }
                }
                lineCounter++;
                line = file.ReadLine();
            }
            file.Close();
            sortData();
        }

        private static void ReadEnemies()
        {
            while (line != "END")
            {
                if (line == "")
                {
                    lineCounter++;
                    line = file.ReadLine();
                    continue;   // skip if there is nothing written on this line
                }

                if (line[0] == '/')
                {
                    lineCounter++;
                    line = file.ReadLine();
                    continue;   // skip reading the line if it is marked as a comment
                }

                if (ReadString() == false)
                {
                    throw new System.InvalidOperationException("Error: failed to read entity data at line " + lineCounter.ToString() + ".");
                }
                lineCounter++;
                line = file.ReadLine();
            }
        }

        private static bool ReadString()
        {
            int pass = 1;
            uint timeInMilliseconds = 0;
            string read = "";
            EntityType entityType = EntityType.NONE;
            Vector2 position = Vector2.Zero;
            Path path = null;

            for (int i = 0; i < line.Length; i++)
            {

                if (line[i] != '|')
                {
                    read += line[i];
                }
                else
                {
                    if (pass == 1)  // pass1, reading the timeline. When the enemy will spawn
                    {
                        timeInMilliseconds = setTime(read);
                        read = "";
                        pass++;
                    }
                    else if (pass == 2) // pass2, reading what enemy will spawn
                    {
                        if ((entityType = setEntityType(read)) == EntityType.NONE)
                        {
                            return false;
                        }
                        read = "";
                        pass++;
                    }
                    else if (pass == 3) // pass3, reading where the enemy will spawn
                    {
                        position = setPosition(read);
                        read = "";
                        pass++;
                    }
                    else if (pass == 4 && (entityType == EntityType.INNER_DEMON || entityType == EntityType.DARK_THOUGHT || entityType == EntityType.DARK_WHISPER))  // pass4, reading enemy pathfinding.
                    {
                        path = setPath(read);
                    }
                }
            }

            if (entityType == EntityType.DARK_THOUGHT && path == null)
            {
                throw new System.InvalidOperationException("Error: DARK_THOUGHT on line" + lineCounter.ToString() + " does not have a path.");
            }

            if (timeInMilliseconds > uint.Parse(config.getValue("Debug", "StartingTime")))
            {
                EntityData entityData = new EntityData(timeInMilliseconds - uint.Parse(config.getValue("Debug", "StartingTime")), entityType, position, path);
                entityDataList.Add(entityData);
            }
            return true;
        }

        private static uint setTime(string read)
        {
            uint minutes = 0;
            uint seconds = 0;
            string time = "";

            for (int i = 0; i < read.Length; i++)
            {
                if (read[i] != ':')
                {
                    time += read[i];
                }
                else
                {
                    minutes = Convert.ToUInt32(time);
                    time = "";
                }
            }

            seconds = Convert.ToUInt32(time);

            uint miliseconds = (minutes * MILLISECONDS_IN_A_MINUTE) + seconds;
            return miliseconds;
        }

        private static String setEntityType(string read)
        {
            String entityType = (String)Enum.Parse(typeof(String), read, true);
            return entityType;
        }

        private static Point setPosition(string read)
        {
            string point = "";
            Vector2 position = Vector2.Zero;
            for (int i = 0; i < read.Length; i++)
            {
                if (read[i] != ',')
                {
                    point += read[i];
                }
                else
                {
                    position.X = Convert.ToSingle(point);
                    point = "";
                }
            }

            position.Y = Convert.ToSingle(point);
            return position;
        }

        private Path setPath(string read)
        {
            Vector2 newVec = Vector2.Zero;
            List<Vector2> newList = new List<Vector2>();
            int newPos = 0;
            int oldPos = 0;
            bool repeat = false;
            bool backTrack = false;

            //while (newPos + 1 < read.Length)
            while (read[newPos + 1] != ';')
            {
                newPos = read.IndexOf(',', oldPos);
                string vec = read.Substring(oldPos, newPos - oldPos);
                oldPos = newPos + 1;
                float x = Convert.ToSingle(vec);
                newVec.X = x;
                newPos = read.IndexOf('=', oldPos);
                vec = read.Substring(oldPos, newPos - oldPos);
                float y = Convert.ToSingle(vec);
                newVec.Y = y;
                oldPos = newPos + 1;
                newList.Add(newVec);
            }

            int i = newPos + 2;
            int pass = 1;
            string status = "";
            for (; i < read.Length; i++)
            {
                if (read[i] != ';')
                {
                    status += read[i];
                }
                else if (pass == 1)
                {
                    repeat = Convert.ToBoolean(status);
                    pass++;
                    status = "";
                }
                else if (pass == 2)
                {
                    backTrack = Convert.ToBoolean(status);
                }
            }

            Path path = new Path(newList, repeat, backTrack);

            return path;
        }

        private void sortData()
        {
            for (int i = 0; i < bgData_back.Count; i++)
            {
                for (int j = 0; j < bgData_back.Count; j++)
                {
                    if (bgData_back[j].SpawnTime > bgData_back[i].SpawnTime)
                    {
                        BackgroundData tempData = null;
                        tempData = bgData_back[i];
                        bgData_back[i] = bgData_back[j];
                        bgData_back[j] = tempData;
                    }
                }
            }

            for (int i = 0; i < bgData_front.Count; i++)
            {
                for (int j = 0; j < bgData_front.Count; j++)
                {
                    if (bgData_front[j].SpawnTime > bgData_front[i].SpawnTime)
                    {
                        BackgroundData tempData = null;
                        tempData = bgData_front[i];
                        bgData_front[i] = bgData_front[j];
                        bgData_front[j] = tempData;
                    }
                }
            }

            for (int i = 0; i < entityDataList.Count; i++)
            {
                for (int j = 0; j < entityDataList.Count; j++)
                {
                    if (entityDataList[j].SpawnTime > entityDataList[i].SpawnTime)
                    {
                        EntityData tempData = null;
                        tempData = entityDataList[i];
                        entityDataList[i] = entityDataList[j];
                        entityDataList[j] = tempData;
                    }
                }
            }
        }
    }
}