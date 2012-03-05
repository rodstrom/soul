using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    abstract class Weapon : IDisposable
    {
        protected SpriteBatch spriteBatch;
        protected Soul game;
        protected Vector2 velocity = Vector2.Zero;
        protected int spriteHeight;
        protected int damage = 0;

        public Weapon(SpriteBatch spriteBatch, Soul game, int spriteHeight)
        {
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.spriteHeight = spriteHeight;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public abstract Vector2 getVelocity();
        public abstract Vector2 getPosition(Vector2 position);
        public abstract Bullet Shoot(Vector2 position);
    }
}
