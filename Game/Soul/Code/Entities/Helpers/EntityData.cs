using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// this class is used for storing entity spawn data when loading map data
namespace Soul
{
    class EntityData
    {
        // entity spawn time is in miliseconds
        private uint spawnTime = 0;
        private EntityType entityType;
        private Vector2 position = Vector2.Zero;
        private Path path = null;

        public EntityData(uint spawnTime, EntityType entityType, Vector2 position)
        {
            this.spawnTime = spawnTime;
            this.entityType = entityType;
            this.position = position;
        }

        public EntityData(uint spawnTime, EntityType entityType, Vector2 position, Path path)
        {
            this.spawnTime = spawnTime;
            this.entityType = entityType;
            this.position = position;
            if (path != null)
            {
                this.path = path;
            }
        }

        public uint SpawnTime { get { return spawnTime; } }
        public EntityType Type { get { return entityType; } }
        public Vector2 Position { get { return position; } }
        public Path PathFinding { get { return path; } }
    }
}
