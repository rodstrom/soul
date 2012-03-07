using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Debug;

namespace Soul.Manager
{
    class EntityManager
    {
        private List<Entity> entityList = new List<Entity>();
        private List<Entity> killList = new List<Entity>();
        private List<Bullet> bulletList = new List<Bullet>();
        private List<EntityData> queueList = new List<EntityData>();
        private List<Light> lights;

        private CollisionManager collisionManager = null;
        private Random random;
        private SpriteBatch spriteBatch;
        private Soul game;
        private Entity player = null;
        private SpawnEnemies spawnEnemies = new SpawnEnemies();

        private int powerupCount = 0;
        private uint timer = 0;
        private uint enemySpawnCounter = 0;

        private bool powerupsDisabled = true;

        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public Soul Game { get { return game; } }

        public EntityManager(SpriteBatch spriteBatch, Soul game)
        {
            this.random = new Random();
            this.spriteBatch = spriteBatch;
            this.game = game;
        }

        public void AddEntityDataList(List<EntityData> queueList)
        {
            this.queueList = queueList;
        }

        public void AddLightList(List<Light> lights)
        {
            this.lights = lights;
        }

        public void AddPointLight(PointLight light)
        {
            lights.Add(light);
        }

        public void initialize()
        {
            player = findPlayer();
            collisionManager = new CollisionManager(entityList, bulletList);
            collisionManager.initialize();

            //IniFile config = new IniFile("Content\\Config\\config.ini");
            //config.parse();
            powerupsDisabled = !bool.Parse(game.config.getValue("Debug", "DisablePowerups"));
        }



        public int size
        {
            get { return entityList.Count; }
        }

        public bool addEntity(Entity entity)
        {
            if (doesEntityExist(entity) == true)
            {
                return false;
            }
            entityList.Add(entity);
            return true;
        }

        public bool doesEntityExist(Entity entity)
        {
            foreach (Entity i in entityList)
            {
                if (i.Alias == entity.Alias)
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw()
        {
            foreach (Bullet bullet in bulletList)
            {
                bullet.Draw();
            }

             foreach (Entity i in entityList)
             {
                 i.Draw();
             }
        }

        public void Update(GameTime gameTime)
        {
            timer += (uint)gameTime.ElapsedGameTime.Milliseconds;
            
            CheckSpawnQueue(gameTime);
            collisionManager.checkCollision();
            Vector2 tmpPlayerPosition = Vector2.Zero;
            for (int i = 0; i < entityList.Count; i++)
            {
                if (entityList[i].Type == EntityType.NIGHTMARE || entityList[i].Type == EntityType.INNER_DEMON || entityList[i].Type == EntityType.LESSER_DEMON || entityList[i].Type == EntityType.DARK_WHISPER)
                {
                    if (player != null)
                    {
                        entityList[i].Target = player.position; ;
                    }
                }
                entityList[i].Update(gameTime);
                if (entityList[i].KillMe == true)
                {
                    killList.Add(entityList[i]);
                }
            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                bulletList[i].Update(gameTime);
            }

            if (killList.Count > 0)
            {
                RemoveEntity();
            }

            

            #region spawn_enemies_dev
            if(bool.Parse(game.config.getValue("Debug", "SpawnKeys")))
            {
                spawnEnemies.Update(gameTime);
                if (spawnEnemies.One == true)
                {
                    int count = EntityCount(EntityType.NIGHTMARE);
                    Nightmare nightmare = new Nightmare(spriteBatch, game, this, "nightmare_spawned" + count.ToString());
                    nightmare.onDie += new Nightmare.PowerupReleaseHandle(ReleasePowerup);
                    nightmare.position.X = 0.0f;
                    float newY = random.Next(50, game.Window.ClientBounds.Bottom - 50);
                    nightmare.position.X = 0.0f;
                    nightmare.position.Y = newY;
                    while (addEntity(nightmare) == false)
                    {
                        count++;
                        nightmare.Alias = "nightmare" + count.ToString();
                    }
                }

                if (spawnEnemies.Two == true)
                {
                    int count = EntityCount(EntityType.PURPLE_BLOOD_VESSEL);
                    PurpleBloodvessel bloodvessel = new PurpleBloodvessel(spriteBatch, game, this, "bloodvessel_spawned" + count.ToString());
                    bloodvessel.onDie += new PurpleBloodvessel.PowerupReleaseHandle(ReleasePowerup);
                    bloodvessel.position.X = 0.0f;
                    float newY = random.Next(50, game.Window.ClientBounds.Bottom - 50);
                    bloodvessel.position.X = 0.0f;
                    bloodvessel.position.Y = newY;
                    while (addEntity(bloodvessel) == false)
                    {
                        count++;
                        bloodvessel.Alias = "bloodvessel" + count.ToString();
                    }

                }

                if (spawnEnemies.Three == true)
                {
                    int count = EntityCount(EntityType.DARK_THOUGHT);
                    Vector2 waypoint = new Vector2(100.0f, game.Window.ClientBounds.Center.Y);
                    Path waypoints = new Path(waypoint);
                    waypoint = new Vector2(200.0f, 200.0f);
                    waypoints.AddPath(waypoint);
                    waypoint = new Vector2(1000.0f, 600.0f);
                    waypoints.AddPath(waypoint);
                    DarkThought darkThought = new DarkThought(spriteBatch, game, gameTime, "darkthought_spawned" + count.ToString(), this, waypoints);
                    darkThought.onDie += new DarkThought.PowerupReleaseHandle(ReleasePowerup);
                    darkThought.position.X = 100.0f;
                    darkThought.position.Y = 100.0f;
                    while (addEntity(darkThought) == false)
                    {
                        count++;
                        darkThought.Alias = "darkthought" + count.ToString();
                    }
                }

                if (spawnEnemies.Four == true)
                {
                    int count = EntityCount(EntityType.DARK_WHISPER);
                    DarkWhisper darkWhisper = new DarkWhisper(spriteBatch, game, "dark_whisper_spawned" + count.ToString(), this, null);
                    darkWhisper.onDie += new DarkWhisper.PowerupReleaseHandle(ReleasePowerup);
                    darkWhisper.position.X = 200.0f;
                    darkWhisper.position.Y = 200.0f;
                    while (addEntity(darkWhisper) == false)
                    {
                        count++;
                        darkWhisper.Alias = "dark_whisper_spawned" + count.ToString();
                    }
                }

                if (spawnEnemies.Five == true)
                {
                    int count = EntityCount(EntityType.INNER_DEMON);
                    Vector2 waypoint = new Vector2(100.0f, game.Window.ClientBounds.Center.Y);
                    Path waypoints = new Path(waypoint);
                    waypoint = new Vector2(200.0f, 200.0f);
                    waypoints.AddPath(waypoint);
                    waypoint = new Vector2(1000.0f, 600.0f);
                    waypoints.AddPath(waypoint);
                    InnerDemon innerDemon = new InnerDemon(spriteBatch, game, "inner_demon_spawned" + count.ToString(), this, waypoints);
                    innerDemon.onDie += new InnerDemon.PowerupReleaseHandle(ReleasePowerup);
                    innerDemon.position.X = 200.0f;
                    innerDemon.position.Y = 200.0f;
                    while (addEntity(innerDemon) == false)
                    {
                        count++;
                        innerDemon.Alias = "inner_demon_spawned" + count.ToString();
                    }
                }
                #endregion spawn_enemies_dev
            }
        }


        public Entity GetPlayer
        {
            get
            {
                foreach (Entity i in entityList)
                {
                    if (i.Type == EntityType.PLAYER)
                    {
                        return i;
                    }
                }
                return null;
            }
        }

        public int EntityCount(EntityType type)
        {
            int count = 0;
            foreach (Entity i in entityList)
            {
                if (i.Type == type)
                {
                    count++;
                }
            }
            return count;
        }

        private void RemoveEntity()
        {
            for (int i = 0; i < killList.Count; i++)
            {
                for (int j = 0; j < entityList.Count; j++)
                {
                    if (killList[i].Alias == entityList[j].Alias)
                    {
                        entityList.RemoveAt(j);
                    }
                }
            }
            killList.Clear();
        }

        public void addBullet(Bullet bullet)
        {
            bulletList.Add(bullet);
            if (bullet.Type == EntityType.PLAYER_BULLET)
            {
                lights.Add(bullet.PointLight);
            }
        }

        public Entity findPlayer()
        {
            foreach (Entity player in entityList)
            {
                if (player.Type == EntityType.PLAYER)
                {
                    return player;
                }
            }
            return null;
        }

        public bool IsPlayerDead()
        {
            if (player.KillMe == true)
            {
                return true;
            }
            return false;
        }

        private void ReleasePowerup(Entity entity)
        {
            if (player != null && powerupsDisabled)
            {
                if (player.Health >= Constants.PLAYER_MAX_HEALTH)
                {
                    int value = random.Next(0, 100);
                    if (value >= 50)
                    {
                        WeaponPowerup wpnPowerup = new WeaponPowerup(spriteBatch, game, "wpnpowerUp" + powerupCount.ToString(), entity.position);
                        addEntity(wpnPowerup);
                        powerupCount++;
                    }
                }
                else
                {
                    int powerupType = random.Next(0, 10);
                    if (powerupType < 8)
                    {
                        int value = random.Next(0, 100);
                        if (value >= 50)
                        {
                            HealthPowerup healthPowerup = new HealthPowerup(spriteBatch, game, "healthpowerUp" + powerupCount.ToString(), entity.position);
                            addEntity(healthPowerup);
                            powerupCount++;
                        }
                    }
                    else if (powerupType >= 8)
                    {
                        int value = random.Next(0, 100);
                        if (value >= 50)
                        {
                            WeaponPowerup wpnPowerup = new WeaponPowerup(spriteBatch, game, "wpnpowerUp" + powerupCount.ToString(), entity.position);
                            addEntity(wpnPowerup);
                            powerupCount++;
                        }
                    }
                }
            } 
        }

        private void CheckSpawnQueue(GameTime gameTime)
        {
           for (int i = 0; i < queueList.Count; i++)
           {
               if (queueList[i].SpawnTime <= timer)
               {
                   SpawnEnemy(queueList[i], gameTime);
                   queueList.RemoveAt(i);
                   i--;
               }
               if (i >= 10)
               {
                   break;
               }
            }
        }

        private void SpawnEnemy(EntityData entityData, GameTime gameTime)
        {
            if (entityData.Type == EntityType.BOSS)
            {
                Boss boss = new Boss(spriteBatch, game, gameTime, "boss" + enemySpawnCounter.ToString(), this);
                boss.position = entityData.Position;
                addEntity(boss);
            }
            else if (entityData.Type == EntityType.NIGHTMARE)
            {
                Nightmare nightmare = new Nightmare(spriteBatch, game, this, "nightmare" + enemySpawnCounter.ToString());
                nightmare.onDie += new Nightmare.PowerupReleaseHandle(ReleasePowerup);
                nightmare.position = entityData.Position;
                addEntity(nightmare);
            }
            else if (entityData.Type == EntityType.BLUE_BLOOD_VESSEL)
            {
                BlueBloodvessel bloodvessel = new BlueBloodvessel(spriteBatch, game, this, "blue_bloodvessel" + enemySpawnCounter.ToString());
                bloodvessel.onDie += new BlueBloodvessel.PowerupReleaseHandle(ReleasePowerup);
                bloodvessel.position = entityData.Position;
                addEntity(bloodvessel);
            }
            else if (entityData.Type == EntityType.RED_BLOOD_VESSEL)
            {
                RedBloodvessel bloodvessel = new RedBloodvessel(spriteBatch, game, this, "red_bloodvessel" + enemySpawnCounter.ToString());
                bloodvessel.onDie += new RedBloodvessel.PowerupReleaseHandle(ReleasePowerup);
                bloodvessel.position = entityData.Position;
                addEntity(bloodvessel);
            }
            else if (entityData.Type == EntityType.PURPLE_BLOOD_VESSEL)
            {
                PurpleBloodvessel bloodvessel = new PurpleBloodvessel(spriteBatch, game, this, "purple_bloodvessel" + enemySpawnCounter.ToString());
                bloodvessel.onDie += new PurpleBloodvessel.PowerupReleaseHandle(ReleasePowerup);
                bloodvessel.position = entityData.Position;
                addEntity(bloodvessel);
            }
            else if (entityData.Type == EntityType.DARK_THOUGHT)
            {
                DarkThought darkThought = new DarkThought(spriteBatch, game, gameTime, "darkthought" + enemySpawnCounter.ToString(), this, entityData.PathFinding);
                darkThought.onDie += new DarkThought.PowerupReleaseHandle(ReleasePowerup);
                darkThought.position = entityData.Position;
                addEntity(darkThought);
            }
            else if (entityData.Type == EntityType.INNER_DEMON)
            {
                InnerDemon innerDemon = new InnerDemon(spriteBatch, game, "inner_demon" + enemySpawnCounter.ToString(), this, entityData.PathFinding);
                innerDemon.onDie += new InnerDemon.PowerupReleaseHandle(ReleasePowerup);
                innerDemon.position = entityData.Position;
                addEntity(innerDemon);
            }
            else if (entityData.Type == EntityType.DARK_WHISPER)
            {
                DarkWhisper darkWhisper = new DarkWhisper(spriteBatch, game, "dark_whisper" + enemySpawnCounter.ToString(), this, entityData.PathFinding);
                darkWhisper.onDie += new DarkWhisper.PowerupReleaseHandle(ReleasePowerup);
                darkWhisper.position = entityData.Position;
                addEntity(darkWhisper);
            }
            else if (entityData.Type == EntityType.WEAPON_POWERUP)
            {
                WeaponPowerup wpnPowerup = new WeaponPowerup(spriteBatch, game, "wpnPowerup" + enemySpawnCounter.ToString(), entityData.Position);
                addEntity(wpnPowerup);
            }
            else if (entityData.Type == EntityType.HEALTH_POWERUP)
            {
                HealthPowerup healthPowerup = new HealthPowerup(spriteBatch, game, "healthPowerup" + enemySpawnCounter.ToString(), entityData.Position);
                addEntity(healthPowerup);
            }

            enemySpawnCounter++;
        }

        public Vector2 PlayerPosition { get { return player.Position; } }

        public void clearEntities()
        {
            entityList.Clear();
            killList.Clear();
            bulletList.Clear();
            queueList.Clear();
        }

        public int BulletListSize { get { return bulletList.Count; } }
        public int EntityListSize { get { return entityList.Count; } }

        public PointLight getGlowLight(int i)
        {
            if (i < 0 || i > entityList.Count)
            {
                return null;
            }

            if (entityList[i].Type != EntityType.DIE_PARTICLE)
            {
                return null;
            }

            return entityList[i].PointLight;
        }

        public Bullet BulletPosition(int i)
        {
            if (i < 0 || i > bulletList.Count)
            {
                return null;
            }

            if (bulletList[i].Type != EntityType.PLAYER_BULLET)
            {
                return null;
            }

            return bulletList[i];
        }

        public PointLight PlayerPointLight()
        {
            return player.PointLight;
        }

        public PointLight BulletPointLight(int i)
        {
            if (i < 0 || i > bulletList.Count)
            {
                return null;
            }

            return bulletList[i].PointLight;
        }
    }
}
