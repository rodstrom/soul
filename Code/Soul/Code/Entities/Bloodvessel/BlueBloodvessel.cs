using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class BlueBloodvessel : BloodVessel
    {
        public BlueBloodvessel(SpriteBatch spriteBatch, Soul game, EntityManager entityManager, string alias) 
            : base(spriteBatch, game, entityManager, new Vector2(Constants.BLUE_BLOOD_VESSEL_WIDTH), EntityType.BLUE_BLOOD_VESSEL, alias, Constants.BLUE_BLOOD_VESSEL_FILENAME)
        {
            maxVelocity = new Vector2(Constants.BLUE_BLOOD_VESSEL_MAX_SPEED, Constants.BLUE_BLOOD_VESSEL_MAX_SPEED);
            acceleration = new Vector2(Constants.BLUE_BLOOD_VESSEL_ACCELERATION, Constants.BLUE_BLOOD_VESSEL_ACCELERATION);
            moveRight = true;
            this.health = Constants.BLUE_BLOOD_VESSEL_MAX_HEALTH;
            this.damage = Constants.BLUE_BLOOD_VESSEL_DAMAGE;
            this.hitRadius = Constants.BLUE_BLOOD_VESSEL_RADIUS;
        }
    }
}
