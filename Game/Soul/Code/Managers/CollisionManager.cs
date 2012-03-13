using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul.Manager
{
    class CollisionManager
    {
        private List<Entity> entityList;
        private List<Bullet> bulletList;
        private Entity player = null;
        private Rectangle screenSize;

        public CollisionManager(List<Entity> entityList, List<Bullet>bulletList)
        {
            this.entityList = entityList;
            this.bulletList = bulletList;
        }

        public void initialize()
        {
            player = findPlayer();
            screenSize = player.ScreenBoundaries;
        }

        public void checkCollision()
        {
            checkPlayerAndEnemy();
            checkEntityToBullet();
        }

        public void checkPlayerAndEnemy()
        {
            if (player != null)
            {
                foreach (Entity entity in entityList)
                {
                    if (entity.Type == EntityType.PLAYER || entity.Ghost == true) continue;
                    if (player.checkCollision(entity.CollisionCircle) == true)
                    {
                        player.onCollision(entity);
                        entity.onCollision(player);
                    }
                }
            }
        }

        public void checkEntityToBullet()
        {
            for (int j = 0; j < entityList.Count; j++ )
            {
                for (int i = 0; i < bulletList.Count; i++)
                {
                    if (bulletList[i].Type == EntityType.PLAYER_BULLET && entityList[j].Type != EntityType.PLAYER && entityList[j].Type != EntityType.WEAPON_POWERUP && entityList[j].Type != EntityType.HEALTH_POWERUP && entityList[j].Ghost == false)
                    {
                        if (entityList[j].checkCollision(bulletList[i].CollisionBox) == true)
                        {
                            entityList[j].onCollision(bulletList[i]);
                            bulletList[i].RemoveLight();
                            bulletList.RemoveAt(i);
                            i--;
                        }
                    }
                    else if ( (bulletList[i].Type == EntityType.BOSS_BULLET || bulletList[i].Type == EntityType.DARK_THOUGHT_BULLET) && entityList[j].Type == EntityType.PLAYER && entityList[j].Type != EntityType.WEAPON_POWERUP && entityList[j].Type != EntityType.HEALTH_POWERUP)
                    {
                        if (entityList[j].checkCollision(bulletList[i].CollisionBox) == true)
                        {
                            entityList[j].onCollision(bulletList[i]);
                            bulletList[i].RemoveLight();
                            bulletList.RemoveAt(i);
                            i--;
                        }
                    }
                    else if (bulletList[i].Type == EntityType.DARK_WHISPER_SPIKE && entityList[j].Type != EntityType.WEAPON_POWERUP && entityList[j].Type != EntityType.HEALTH_POWERUP)
                    {
                        if (entityList[j].checkCollision(bulletList[i].CollisionBox) == true)
                        {
                            entityList[j].onCollision(bulletList[i]);
                            if (entityList[j].Type != EntityType.DARK_WHISPER)
                            {
                                bulletList.RemoveAt(i);
                                i--;
                            }
                        }
                    }

                    if (i >= 0)
                    {
                        if (bulletList[i].CollisionBox.X > Constants.RESOLUTION_VIRTUAL_WIDTH || bulletList[i].CollisionBox.Width < 0 || bulletList[i].CollisionBox.Y > Constants.RESOLUTION_VIRTUAL_HEIGHT || bulletList[i].CollisionBox.Height < 0)
                        {
                            bulletList[i].RemoveLight();
                            bulletList.RemoveAt(i);
                            i--;
                        }
                    }
                }
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
    }
}
