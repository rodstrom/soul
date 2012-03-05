using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soul
{
    class BackgroundData
    {
        // time is in milliseconds
        private uint spawnTime = 0;
        private uint deleteTime = 0;
        private string filename = "";
        private float direction = 0.0f;
        private float layer = 0.0f;
        private string type = "";
        private int lowestSpawn = 0;
        private int highestSpawn = 0;
        private bool randomDirection = false;
        private bool randomSpeed = false;

        public BackgroundData(uint spawnTime, uint deleteTime, string type, string filename, float direction, float layer)
        {
            this.spawnTime = spawnTime;
            this.deleteTime = deleteTime;
            this.type = type;
            this.filename = filename;
            this.direction = direction;
            this.layer = layer;
        }

        public BackgroundData(uint spawnTime, uint deleteTime, int lowestSpawn, int highestSpawn, string type, string filename, float direction, bool randomDirection, bool randomSpeed, float layer)
        {
            this.filename = filename;
            this.spawnTime = spawnTime;
            this.deleteTime = deleteTime;
            this.type = type;
            this.direction = direction;
            this.layer = layer;
            this.lowestSpawn = lowestSpawn;
            this.highestSpawn = highestSpawn;
            this.randomDirection = randomDirection;
            this.randomSpeed = randomSpeed;
        }

        public uint SpawnTime { get { return spawnTime; } }
        public uint DeleteTime { get { return deleteTime; } }
        public string Type { get { return type; } }
        public string Filename { get { return filename; } }
        public float Direction { get { return direction; } }
        public float Layer { get { return layer; } }
        public int LowestSpawnRate { get { return lowestSpawn; } }
        public int HighestSpawnRate { get { return highestSpawn; } }
        public bool RandomDirection { get { return randomDirection; } }
        public bool RandomSpeed { get { return randomSpeed; } }

    }
}
