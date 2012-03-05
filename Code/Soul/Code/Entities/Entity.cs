using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;
using Soul.Debug;

namespace Soul
{
    enum LevelPosition
    {
        LEVEL_POSITION_NONE = 0,
        LEVEL_POSITION_BACKGROUND,
        LEVEL_POSITION_MOVING_BETWEEN,
        LEVEL_POSITION_FRONT
    };

    enum AnimationState
    {
        RIGHT = 0,
        LEFT
    };

    abstract class Entity : IDisposable
    {
        public Vector2 position;
        protected Vector2 dimension;
        protected Sprite sprite;
        protected Vector2 velocity = Vector2.Zero;
        protected Vector2 maxVelocity = Vector2.Zero;
        protected Vector2 acceleration = Vector2.Zero;
        protected Vector2 offset = Vector2.Zero;
        protected Vector2 circleOffset = Vector2.Zero;
        protected Vector2 target = Vector2.Zero;
        protected Vector2 origin = Vector2.Zero;

        protected Color color = Color.White;
        protected Rectangle screenBoundaries;
        protected EntityType type;
        protected LevelPosition levelPosition = LevelPosition.LEVEL_POSITION_FRONT;
        protected Animation animation;
        protected SpriteBatch spriteBatch;
        protected Soul game;
        protected AudioManager audio;

        protected string alias;
        protected float scale = 1.0f;
        protected float rotation = 0.0f;
        protected float layer = 0.0f;
        protected bool killMe = false;
        protected bool ghost = false;
        protected bool disposed = false;
        protected int health = 0;
        protected int damage = 0;
        protected int animationState = 0;
        protected float hitRadius = 0;
        protected int spikeDamage = 0;
        protected int spikeSpeed = 0;
        protected int spikeRange = 0;
        protected int fireRate = 0;
        protected int minSpawn = 0;
        protected int maxSpawn = 0;
        protected int burst = 0;
        protected bool debug = false;

        public Entity(SpriteBatch spriteBatch, Soul game, string filename, Vector2 dimension, string alias, EntityType type)
        {
            sprite = new Sprite(spriteBatch, game, filename);
            screenBoundaries = new Rectangle(0, 0, Constants.RESOLUTION_VIRTUAL_WIDTH, Constants.RESOLUTION_VIRTUAL_HEIGHT);
            this.alias = alias;
            this.dimension = dimension;
            offset.X = dimension.X * 0.5f;
            offset.Y = dimension.Y * 0.5f;
            this.type = type;
            animation = new Animation(((int)(sprite.X / dimension.X)) - 1);
            this.game = game;
            this.spriteBatch = spriteBatch;

            //IniFile config = new IniFile("Content\\Config\\config.ini");
            //config.parse();
            debug = bool.Parse(game.config.getValue("Debug","Hitbox"));
            
            bool varyingEntity = false;
            List<String> varyingEntities = new List<string>();
            varyingEntities.Add("BLOOD_VESSEL");
            varyingEntities.Add("DARK_THOUGHT");
            varyingEntities.Add("DARK_WHISPER");
            varyingEntities.Add("INNER_DEMON");
            varyingEntities.Add("LESSER_DEMON");
            varyingEntities.Add("NIGHTMARE");
            varyingEntities.Add("PLAYER");
            foreach (String entityType in varyingEntities)
            {
                if (type.ToString().Equals(entityType))
                {
                    varyingEntity = true;
                }
            }

            if (varyingEntity)
            {
                //IniFile ini = new IniFile("Content\\Config\\constants.ini");
                //ini.parse();
                health = int.Parse(game.constants.getValue(type.ToString(), "HEALTH"));
                hitRadius = int.Parse(game.constants.getValue(type.ToString(), "RADIUS"));
                maxVelocity = new Vector2(float.Parse(game.constants.getValue(type.ToString(), "SPEED")), float.Parse(game.constants.getValue(type.ToString(), "SPEED")));
                if (!type.ToString().Equals("INNER_DEMON") && !type.ToString().Equals("LESSER_DEMON"))
                {
                    damage = int.Parse(game.constants.getValue(type.ToString(), "DAMAGE"));
                }
                if (type.ToString().Equals("DARK_THOUGHT") || type.ToString().Equals("PLAYER"))
                {
                    fireRate = int.Parse(game.constants.getValue(type.ToString(), "RATE"));
                } 
                if (type.ToString().Equals("DARK_THOUGHT"))
                {
                    burst = int.Parse(game.constants.getValue(type.ToString(), "BURSTPERIOD"));
                }
                if (type.ToString().Equals("INNER_DEMON"))
                {
                    minSpawn = int.Parse(game.constants.getValue(type.ToString(), "MINSPAWN"));
                    maxSpawn = int.Parse(game.constants.getValue(type.ToString(), "MAXSPAWN"));
                }
                if (type.ToString().Equals("DARK_WHISPER"))
                {
                    spikeDamage = int.Parse(game.constants.getValue(type.ToString(), "SPIKEDAMAGE"));
                    spikeSpeed = int.Parse(game.constants.getValue(type.ToString(), "SPIKESPEED"));
                    spikeRange = int.Parse(game.constants.getValue(type.ToString(), "SPIKERANGE"));
                }
            }
        }

        public Rectangle ScreenBoundaries
        {
            get { return screenBoundaries; }
            set { screenBoundaries = value; }
        }
        
        public Rectangle CollisionBox
        {
            get
            {
                int x = (int)position.X - (int)offset.X;
                int y = (int)position.Y - (int)offset.Y;
                return new Rectangle(x, y, x + (int)dimension.X, y + (int)dimension.Y);
            }
        }

        public Circle CollisionCircle
        {
            get
            {
                return new Circle(position, circleOffset, hitRadius);
            }
        }

        public Vector2 Position
        {
            get { return position - offset; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public bool KillMe
        {
            get { return killMe; }
            set { killMe = value; }
        }

        public EntityType Type
        {
            get { return type; }
        }

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        public virtual void Draw()
        {
            if (debug)
            {
                DEBUG_circleLine brush = new DEBUG_circleLine(game.GraphicsDevice);
                brush.CreateCircle(hitRadius, 100);
                brush.Position = position;
                brush.Render(spriteBatch);
            }

            Rectangle rect = new Rectangle(animation.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
            sprite.Draw(position, rect, Color.White, rotation, offset, scale, SpriteEffects.None, layer);
        }

        public bool checkCollision(Rectangle targetEntity)
        {
            int leftA = CollisionBox.X;
            int rightA = CollisionBox.Width;
            int topA = CollisionBox.Y;
            int bottomA = CollisionBox.Height;

            int leftB = targetEntity.X;
            int rightB = targetEntity.Width;
            int topB = targetEntity.Y;
            int bottomB = targetEntity.Height;

            if (leftA >= rightB) return false;
            if (rightA <= leftB) return false;
            if (topA >= bottomB) return false;
            if (bottomA <= topB) return false;

            return true;
        }

        public bool checkCollision(Circle targetEntity)
        {
            float distanceX = Math.Abs(CollisionCircle.center.X - targetEntity.center.X);
            float distanceY = Math.Abs(CollisionCircle.center.Y - targetEntity.center.Y);
            float combinedDistance = distanceX * distanceX + distanceY * distanceY;
            float maxDistance = (CollisionCircle.radius + targetEntity.radius) * (CollisionCircle.radius + targetEntity.radius);

            if(combinedDistance < maxDistance)
            {
                return true;
            }

            return false;
        }

        public bool checkCollision(Rectangle thisEntity, Rectangle targetEntity)
        {
            int leftA = thisEntity.X;
            int rightA = thisEntity.Width;
            int topA = thisEntity.Y;
            int bottomA = thisEntity.Height;

            int leftB = targetEntity.X;
            int rightB = targetEntity.Width;
            int topB = targetEntity.Y;
            int bottomB = targetEntity.Height;

            if (leftA >= rightB) return false;
            if (rightA <= leftB) return false;
            if (topA >= bottomB) return false;
            if (bottomA <= topB) return false;

            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing == true)
                {

                }
            }
            disposed = true;
        }

        public Vector2 Target
        {
            set { target = value; }
        }

        public int Health { get { return health; } }

        public LevelPosition LevelPosition
        {
            get { return levelPosition; }
            set { levelPosition = value; }
        }

        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public abstract void onCollision(Entity entity);
        public abstract int getDamage();
        public abstract void takeDamage(int value);

        public float FX { get { return FX; } }
        public bool Ghost { get { return ghost; } }


        
    }
}
