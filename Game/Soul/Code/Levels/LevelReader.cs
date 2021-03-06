﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace Soul
{
    class LevelReader
    {
        private const uint MILLISECONDS_IN_A_MINUTE = 60000;
        private const uint MILLISECONDS_IN_A_SECOND = 1000;
        private readonly string filename;
        private List<EntityData> entityDataList;
        private List<BackgroundData> bgData_back;
        private List<BackgroundData> bgData_front;
        private int bgLayer = 0;
        private TextReader file = null;
        private string line = null;
        private uint lineCounter = 0;
        private IniFile config;

        public LevelReader(string filename, Soul game)
        {
            this.filename = filename;
            file = new StreamReader(filename);
            entityDataList = new List<EntityData>();
            bgData_back = new List<BackgroundData>();
            bgData_front = new List<BackgroundData>();
            this.config = game.config;
        }

        public bool Parse()
        {
            if (System.IO.File.Exists(filename) == false)
            {
                MessageBox.Show("Error: " + filename + " could not be found.");
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
                    if (category == "Backgrounds-back")
                    {
                        lineCounter++;
                        line = file.ReadLine();
                        ReadBackgrounds();
                        bgLayer++;
                    }
                    else if (category == "Backgrounds-front")
                    {
                        lineCounter++;
                        line = file.ReadLine();
                        ReadBackgrounds();
                        bgLayer++;
                    }
                    else if (category == "Enemies")
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
            return true;
        }

        public void ReadBackgrounds()
        {
            while (line != "END")
            {
                if (line == "")
                {
                    line = file.ReadLine();
                    lineCounter++;
                    continue;   // skip if there is nothing written on this line
                }

                if (line[0] == '/')
                {
                    line = file.ReadLine();
                    lineCounter++;
                    continue;   // skip reading the line if it is marked as a comment
                }

                if (ReadStringBackgrounds() == false)
                {
                    MessageBox.Show("Error: failed to read background data at line " + lineCounter.ToString() + ".");
                    throw new System.InvalidOperationException("Error: failed to read background data at line " + lineCounter.ToString() + ".");                
                }
                lineCounter++;
                line = file.ReadLine();
            }
        }

        public void ReadEnemies()
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
                    MessageBox.Show("Error: failed to read entity data at line " + lineCounter.ToString() + ".");
                    throw new System.InvalidOperationException("Error: failed to read entity data at line " + lineCounter.ToString() + ".");
                }
                lineCounter++;
                line = file.ReadLine();
            }
        }

        public bool ReadStringBackgrounds()
        {
            string read = "";
            int pass = 1;
            uint spawnTime = 0;
            uint deleteTime = 0;
            string type = "";
            string filename = "";
            float direction = 0.0f;
            float layer = 0.0f;
            int lowestSpawn = 0;
            int highestSpawn = 0;
            bool randomDirection = false;
            bool randomSpeed = false;
            bool scrollAfterBoss = false;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != '|' && line[i] != '=')
                {
                    read += line[i];
                }
                else if (pass == 1 && line[i] == '=')
                {
                    spawnTime = setTime(read);
                    read = "";
                }
                else if (pass == 1 && line[i] != '=')
                {
                    deleteTime = setTime(read);
                    read = "";
                    pass++;
                }
                else if (pass == 2)
                {
                    type = read;
                    read = "";
                    pass++;
                }
                else if (pass == 3)
                {
                    if (type == "Batch")
                    {
                        string specific = "";
                        int internalPass = 1;
                        for (int j = 0; j < read.Length; j++)
                        {
                            if (read[j] != ';')
                            {
                                specific += read[j];
                            }
                            else if (internalPass == 1)
                            {
                                lowestSpawn = Convert.ToInt32(specific);
                                specific = "";
                                internalPass++;
                            }
                            else if (internalPass == 2)
                            {
                                highestSpawn = Convert.ToInt32(specific);
                                specific = "";
                                internalPass++;
                            }
                            else if (internalPass == 3)
                            {
                                randomDirection = Convert.ToBoolean(specific);
                                specific = "";
                                internalPass++;
                            }
                            else if (internalPass == 4)
                            {
                                randomSpeed = Convert.ToBoolean(specific);
                                specific = "";
                                internalPass++;
                            }
                        }
                        pass++;
                        read = "";
                    }
                    else
                    {
                        filename = read;
                        read = "";
                        pass++;
                    }
                }
                else if (pass == 4)
                {
                    direction = Convert.ToSingle(read);
                    read = "";
                    pass++;
                }
                else if (pass == 5)
                {
                    layer = Convert.ToSingle(read);
                    read = "";
                    pass++;
                }
                else if (pass == 6)
                {
                    scrollAfterBoss = Convert.ToBoolean(read);
                    read = "";
                    pass++;
                }
            }
            if (bgLayer == 0)
            {
                BackgroundData data = new BackgroundData(spawnTime, deleteTime, lowestSpawn, highestSpawn, type, filename, direction, randomDirection, randomSpeed, layer, scrollAfterBoss);
                bgData_back.Add(data);
            }
            else if (bgLayer == 1)
            {
                BackgroundData data = new BackgroundData(spawnTime, deleteTime, lowestSpawn, highestSpawn, type, filename, direction, randomDirection, randomSpeed, layer, scrollAfterBoss);
                bgData_front.Add(data);
            }
            return true;
        }

        public bool ReadString()
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
                    else if (pass == 3 && entityType != EntityType.CHECKPOINT) // pass3, reading where the enemy will spawn
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
                MessageBox.Show("Error: DARK_THOUGHT on line" + lineCounter.ToString() + " does not have a path.");
                throw new System.InvalidOperationException("Error: DARK_THOUGHT on line" + lineCounter.ToString() + " does not have a path.");
            }

            if (timeInMilliseconds > uint.Parse(config.getValue("Debug", "StartingTime")))
            {
                EntityData entityData = new EntityData(timeInMilliseconds - uint.Parse(config.getValue("Debug", "StartingTime")), entityType, position, path);
                entityDataList.Add(entityData);
            }
            return true;
        }

        private uint setTime(string read)
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

        private EntityType setEntityType(string read)
        {
            EntityType entityType = (EntityType)Enum.Parse(typeof(EntityType), read, true);
            return entityType;
        }

        private Vector2 setPosition(string read)
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
            while (read[newPos+1] != ';')
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


        public List<EntityData> EntityDataList { get { return entityDataList; } }
        public List<BackgroundData> BackgroundDataListBack { get { return bgData_back; } }
        public List<BackgroundData> BackgroundDataListFront { get { return bgData_front; } }


    }
}
