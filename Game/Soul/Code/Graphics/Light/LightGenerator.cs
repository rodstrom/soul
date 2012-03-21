using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class LightGenerator
    {
        private List<Light> lights = null;
        private List<TempLight> tempLights = null;
        private double timer = 0.0;
        private double spawnTime = 0.0;
        private int lowestSpawnTime = 0;
        private int highestSpawnTime = 0;
        private Random random = null;
        private Soul game = null;
        private float zpos = 0f;
        private int maxX, maxY, minY, minX = 0;
        private float powerSclar = 0f;
        private Vector4 colorValue = Vector4.Zero;
        private float power = 0f;
        private int lightDecay = 0;
        private bool specular = false;
        private float maxPower, minPower = 0f;

        public LightGenerator(Soul game, List<Light> lights)
        {
            this.lights = lights;
            this.tempLights = new List<TempLight>();
            this.random = new Random();
            this.game = game;
            this.lowestSpawnTime = int.Parse(game.lighting.getValue("LightGenerator", "MinSpawnTime"));
            this.highestSpawnTime = int.Parse(game.lighting.getValue("LightGenerator", "MaxSpawnTime"));
            this.powerSclar = float.Parse(game.lighting.getValue("LightGenerator", "PowerScalar"));
            this.colorValue = new Vector4(float.Parse(game.lighting.getValue("LightGenerator", "ColorR")), float.Parse(game.lighting.getValue("LightGenerator", "ColorG")), float.Parse(game.lighting.getValue("LightGenerator", "ColorB")), float.Parse(game.lighting.getValue("LightGenerator", "ColorA")));
            this.power = float.Parse(game.lighting.getValue("LightGenerator", "Power"));
            this.lightDecay = int.Parse(game.lighting.getValue("LightGenerator", "LightDecay"));
            this.specular = bool.Parse(game.lighting.getValue("LightGenerator", "Specular"));
            this.maxX = int.Parse(game.lighting.getValue("LightGenerator", "MaxXPosition"));
            this.minX = int.Parse(game.lighting.getValue("LightGenerator", "MinXPosition"));
            this.maxY = int.Parse(game.lighting.getValue("LightGenerator", "MaxYPosition"));
            this.minY = int.Parse(game.lighting.getValue("LightGenerator", "MinYPosition"));
            this.minPower = float.Parse(game.lighting.getValue("LightGenerator", "MinPower"));
            this.maxPower = float.Parse(game.lighting.getValue("LightGenerator", "MaxPower"));
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < tempLights.Count; i++)
            {
                tempLights[i].Update(gameTime);
                if (tempLights[i].pointLight.dead == true)
                {
                    tempLights.RemoveAt(i);
                    i--;
                }
            }

            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= spawnTime)
            {
                tempLights.Add(SpawnLight());
                timer = 0.0;
                spawnTime = (double)random.Next(lowestSpawnTime, highestSpawnTime);
            }
        }

        private TempLight SpawnLight()
        {
            TempLight tmpLight = new TempLight(game, GetRandomPosition(), powerSclar, colorValue, lightDecay, power, specular, minPower, maxPower);
            lights.Add(tmpLight.pointLight);
            return tmpLight;
        }

        private Vector3 GetRandomPosition()
        {
            float x = (float)random.Next(minX, maxX);
            float y = (float)random.Next(minY, maxY);
            return new Vector3(x, y, float.Parse(game.lighting.getValue("LightGenerator", "ZPosition")));
        }
    }
}
