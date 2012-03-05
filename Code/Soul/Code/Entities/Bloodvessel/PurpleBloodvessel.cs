using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class PurpleBloodvessel : BloodVessel
    {
        public PurpleBloodvessel(SpriteBatch spriteBatch, Soul game, EntityManager entityManager, string alias)
            : base(spriteBatch, game, entityManager, new Vector2(Constants.PURPLE_BLOOD_VESSEL_WIDTH), EntityType.PURPLE_BLOOD_VESSEL, alias, Constants.PURPLE_BLOOD_VESSEL_FILENAME)
        {
            maxVelocity = new Vector2(Constants.PURPLE_BLOOD_VESSEL_MAX_SPEED);
            acceleration = new Vector2(Constants.PURPLE_BLOOD_VESSEL_ACCELERATION);
            moveRight = true;
            this.health = Constants.PURPLE_BLOOD_VESSEL_MAX_HEALTH;
            this.damage = Constants.PURPLE_BLOOD_VESSEL_DAMAGE;
            this.hitRadius = Constants.PURPLE_BLOOD_VESSEL_RADIUS;
        }
    }
}
