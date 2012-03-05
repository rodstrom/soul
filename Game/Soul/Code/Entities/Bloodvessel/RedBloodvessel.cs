using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class RedBloodvessel : BloodVessel
    {
        public RedBloodvessel(SpriteBatch spriteBatch, Soul game, EntityManager entityManager, string alias)
            : base(spriteBatch, game, entityManager, new Vector2(Constants.RED_BLOOD_VESSEL_WIDTH), EntityType.RED_BLOOD_VESSEL, alias, Constants.RED_BLOOD_VESSEL_FILENAME)
        {
            maxVelocity = new Vector2(Constants.RED_BLOOD_VESSEL_MAX_SPEED);
            acceleration = new Vector2(Constants.RED_BLOOD_VESSEL_ACCELERATION);
            moveRight = true;
            this.health = Constants.RED_BLOOD_VESSEL_MAX_HEALTH;
            this.damage = Constants.RED_BLOOD_VESSEL_DAMAGE;
            this.hitRadius = Constants.RED_BLOOD_VESSEL_RADIUS;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
