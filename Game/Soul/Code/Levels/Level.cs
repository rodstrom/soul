using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soul.Manager;

namespace Soul
{
    class Level
    {
        private string id;
        private bool pause = false;
        private bool quit = false;
        private bool cleansing = false;
        private int cleansPass = 1;

        EntityManager entityManager;
        BackgroundManager backgroundManager_back;
        BackgroundManager backgroundManager_front;
        Player player;
        InputManager controls;
        LevelReader levelReader;
        MenuManager menuManager;
        SpriteBatch spriteBatch;
        Soul game;

        private RenderTarget2D colorMap_back;
        private RenderTarget2D normalMap_back;
        private RenderTarget2D shadowMap_back;
        private RenderTarget2D colorMap_front;
        private RenderTarget2D normalMap_front;
        private RenderTarget2D shadowMap_front;
        private RenderTarget2D entityLayer;

        private List<Light> lights = new List<Light>();
        private Color ambientLight = new Color(1f, 1f, 1f, 1f);
        private float specularStrenght = 0.5f;
        private float ambientStrength = 1f;
        private float ambientStrenghtScalar = 0.025f;

        private Effect lightEffect;
        private Effect lightCombinedEffect;

        private EffectTechnique lightEffectTechniquePointLight;
        private EffectParameter lightEffectParameterStrenght;
        private EffectParameter lightEffectParameterPosition;
        private EffectParameter lightEffectParameterConeDirection;
        private EffectParameter lightEffectParameterLightColor;
        private EffectParameter lightEffectParameterLightDecay;
        private EffectParameter lightEffectParameterScreenWidth;
        private EffectParameter lightEffectParameterScreenHeight;
        private EffectParameter lightEffectParameterNormalMap;

        private EffectTechnique lightCombinedEffectTechnique;
        private EffectParameter lightCombinedEffectParamAmbient;
        private EffectParameter lightCombinedEffectParamLightAmbient;
        private EffectParameter lightCombinedEffectParamAmbientColor;
        private EffectParameter lightCombinedEffectParamColorMap;
        private EffectParameter lightCombinedEffectParamShadowMap;
        private EffectParameter lightCombinedEffectParamNormalMap;

        private bool darkenScreen = false;
        private bool brightenScreen = false;
        private bool screenIsDark = false;

        private bool fulhack = true;
        private int timeStarted;

        private double ambientTimer = 0.0;

        public VertexPositionColorTexture[] Vertices;
        public VertexBuffer VertexBuffer;

        public Level(SpriteBatch spriteBatch, Soul game, EntityManager entityManager, Player player, InputManager controls, string filename, string id)
        {
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.entityManager = entityManager;
            this.id = id;
            this.controls = controls;
            levelReader = new LevelReader(filename, game);
            this.player = player;
        }

        public void initialize()
        {
            levelReader.Parse();
            entityManager.AddEntityDataList(levelReader.EntityDataList);
            entityManager.initialize();
            CreateBackgrounds();
            menuManager = new MenuManager(controls);
            ImageButton button = new ImageButton(spriteBatch, game, controls, new Vector2((float)game.Window.ClientBounds.Width * 0.5f - 150, (float)game.Window.ClientBounds.Height * 0.5f), "GUI\\button_continue", "continue");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, controls, new Vector2((float)game.Window.ClientBounds.Width * 0.5f + 150, (float)game.Window.ClientBounds.Height * 0.5f), "GUI\\button_quit_pause", "quit");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            menuManager.initialize();

            PresentationParameters pp = game.GraphicsDevice.PresentationParameters;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;
            Vertices = new VertexPositionColorTexture[4];
            Vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
            Vertices[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            Vertices[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            Vertices[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));
            VertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColorTexture), Vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(Vertices);

            colorMap_back = new RenderTarget2D(game.GraphicsDevice, width, height);
            normalMap_back = new RenderTarget2D(game.GraphicsDevice, width, height);
            shadowMap_back = new RenderTarget2D(game.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            entityLayer = new RenderTarget2D(game.GraphicsDevice, width, height);
            colorMap_front = new RenderTarget2D(game.GraphicsDevice, width, height);
            normalMap_front = new RenderTarget2D(game.GraphicsDevice, width, height);
            shadowMap_front = new RenderTarget2D(game.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            lightEffect = game.Content.Load<Effect>("Shaders\\MultiTarget");
            lightCombinedEffect = game.Content.Load<Effect>("Shaders\\DeferredCombined");

            lightEffectTechniquePointLight = lightEffect.Techniques["DeferredPointLight"];
            lightEffectParameterConeDirection = lightEffect.Parameters["coneDirection"];
            lightEffectParameterLightColor = lightEffect.Parameters["lightColor"];
            lightEffectParameterLightDecay = lightEffect.Parameters["lightDecay"];
            lightEffectParameterNormalMap = lightEffect.Parameters["NormalMap"];
            lightEffectParameterPosition = lightEffect.Parameters["lightPosition"];
            lightEffectParameterScreenHeight = lightEffect.Parameters["screenHeight"];
            lightEffectParameterScreenWidth = lightEffect.Parameters["screenWidth"];
            lightEffectParameterStrenght = lightEffect.Parameters["lightStrength"];

            lightCombinedEffectTechnique = lightCombinedEffect.Techniques["DeferredCombined2"];
            lightCombinedEffectParamAmbient = lightCombinedEffect.Parameters["ambient"];
            lightCombinedEffectParamLightAmbient = lightCombinedEffect.Parameters["lightAmbient"];
            lightCombinedEffectParamAmbientColor = lightCombinedEffect.Parameters["ambientColor"];
            lightCombinedEffectParamColorMap = lightCombinedEffect.Parameters["ColorMap"];
            lightCombinedEffectParamShadowMap = lightCombinedEffect.Parameters["ShadingMap"];
            lightCombinedEffectParamNormalMap = lightCombinedEffect.Parameters["NormalMap"];

            player.nightmareHit += new Player.NightmareHit(DarkenScreen);

        }

        public void shutdown()
        {
            fulhack = true;
        }

        public int Update(GameTime gameTime)
        {
            if (fulhack)
            {
                timeStarted = (int)gameTime.TotalGameTime.TotalMilliseconds;
                fulhack = false;
            }

            if (cleansing == true)
            {
                LevelCleansed();
            }

            if (screenIsDark == true)
            {
                HandleScreenAmbient(gameTime);
            }
            lights.Clear();
            CreateLights();

            if (pause == false)
            {
                backgroundManager_back.Update(gameTime);
                entityManager.Update(gameTime);
                backgroundManager_front.Update(gameTime);
            }
            else
            {
                if (controls.MoveLeftOnce == true)
                {
                    menuManager.decrement();
                }
                else if (controls.MoveRightOnce == true)
                {
                    menuManager.increment();
                }
                menuManager.Update(gameTime);
            }

            if (controls.Debug == true)
            {
                cleansing = true;
            }

            if (controls.Pause == true)
            {
                pause = true;
            }

            if (quit == true)
            {
                return 1;
            }
            return 0;
        }

        public void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);
            



            // Draw Color Map Back Layer
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(colorMap_back);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
            backgroundManager_back.Draw(gameTime);
            spriteBatch.End();

            // Draw Normal Map Back Layer
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(normalMap_back);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
            backgroundManager_back.DrawNormalMap(gameTime);
            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(entityLayer);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            entityManager.Draw();
            spriteBatch.End();

            // Draw Color Map Back Layer
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(colorMap_front);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
            backgroundManager_front.Draw(gameTime);
            spriteBatch.End();

            // Draw Normal Map Back Layer
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(normalMap_front);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
            backgroundManager_front.DrawNormalMap(gameTime);
            spriteBatch.End();


            game.GraphicsDevice.SetRenderTarget(null);
            GenerateShadowMap();

            game.GraphicsDevice.Clear(Color.Black);
            DrawCombinedMaps();

            if (pause)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
                menuManager.Draw();
                spriteBatch.End();
            }

            if (bool.Parse(game.config.getValue("Debug", "Timestamp")))
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
                SpriteFont font = game.Content.Load<SpriteFont>("GUI\\Extrafine");
                double time = Math.Round((gameTime.TotalGameTime.TotalMilliseconds - timeStarted + double.Parse(game.config.getValue("Debug", "StartingTime"))) / 1000) ;
                string output = time.ToString();
                spriteBatch.DrawString(font, output, new Vector2(10f), Color.Gray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            }
        }

        private void CreateBackgrounds()
        {
            List<BackgroundData> bgDataBack = levelReader.BackgroundDataListBack;
            backgroundManager_back = new BackgroundManager(spriteBatch, game);
            for (int i = 0; i < bgDataBack.Count; i++)
            {
                if (bgDataBack[i].SpawnTime != 0)
                {
                    backgroundManager_back.addToQueue(bgDataBack[i]);
                }
                else if (bgDataBack[i].Type == "Scrolling")
                {
                    ScrollingBackground bg = new ScrollingBackground(spriteBatch, game, bgDataBack[i].Filename, "bg" + i.ToString(), bgDataBack[i].SpawnTime, bgDataBack[i].DeleteTime, bgDataBack[i].Direction, bgDataBack[i].Layer);
                    backgroundManager_back.addBackground(bg);
                }
                else if (bgDataBack[i].Type == "Pillar")
                {
                    BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, backgroundManager_back.getPillarFileName(bgDataBack[i].Filename), bgDataBack[i].SpawnTime, bgDataBack[i].Direction, bgDataBack[i].Layer);
                    backgroundManager_back.addBackground(bg);
                }
                else if (bgDataBack[i].Type == "Batch")
                {
                    PillarBatch bg = new PillarBatch(spriteBatch, game, bgDataBack[i].LowestSpawnRate, bgDataBack[i].HighestSpawnRate, (int)bgDataBack[i].DeleteTime, bgDataBack[i].Direction, bgDataBack[i].RandomDirection, bgDataBack[i].RandomSpeed, bgDataBack[i].Layer);
                    backgroundManager_back.addBackground(bg);
                }
            }

            List<BackgroundData> bgDataFront = levelReader.BackgroundDataListFront;
            backgroundManager_front = new BackgroundManager(spriteBatch, game);
            for (int i = 0; i < bgDataFront.Count; i++)
            {
                if (bgDataFront[i].SpawnTime != 0)
                {
                    backgroundManager_front.addToQueue(bgDataFront[i]);
                }
                else if (bgDataFront[i].Type == "Scrolling")
                {
                    ScrollingBackground bg = new ScrollingBackground(spriteBatch, game, bgDataFront[i].Filename, "bg" + i.ToString(), bgDataFront[i].SpawnTime, bgDataFront[i].DeleteTime, bgDataFront[i].Direction, bgDataFront[i].Layer);
                    backgroundManager_front.addBackground(bg);
                }
                else if (bgDataFront[i].Type == "Pillar")
                {
                    BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, backgroundManager_front.getPillarFileName(bgDataFront[i].Filename), bgDataFront[i].SpawnTime, bgDataFront[i].Direction, bgDataFront[i].Layer);
                    backgroundManager_front.addBackground(bg);
                }
                else if (bgDataFront[i].Type == "Batch")
                {
                    PillarBatch bg = new PillarBatch(spriteBatch, game, bgDataFront[i].LowestSpawnRate, bgDataFront[i].HighestSpawnRate, (int)bgDataFront[i].DeleteTime, bgDataFront[i].Direction, bgDataFront[i].RandomDirection, bgDataFront[i].RandomSpeed, bgDataFront[i].Layer);
                    backgroundManager_front.addBackground(bg);
                }
            }
        }

        public string ID { get { return id; } }

        private void OnButtonPress(ImageButton button)
        {
            if (button.ID == "continue")
            {
                pause = false;
            }
            else if (button.ID == "quit")
            {
                quit = true;
            }
        }

        private void DrawCombinedMaps()
        {
            lightCombinedEffect.CurrentTechnique = lightCombinedEffectTechnique;
            lightCombinedEffectParamAmbient.SetValue(ambientStrength);
            lightCombinedEffectParamLightAmbient.SetValue(4);
            lightCombinedEffectParamAmbientColor.SetValue(ambientLight.ToVector4());
            lightCombinedEffectParamColorMap.SetValue(colorMap_back);
            lightCombinedEffectParamShadowMap.SetValue(shadowMap_back);
            lightCombinedEffectParamNormalMap.SetValue(normalMap_back);
            lightCombinedEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, lightCombinedEffect, Resolution.getTransformationMatrix());
            spriteBatch.Draw(colorMap_back, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(entityLayer, Vector2.Zero, Color.White);
            spriteBatch.End();

            lightCombinedEffect.CurrentTechnique = lightCombinedEffectTechnique;
            lightCombinedEffectParamAmbient.SetValue(ambientStrength);
            lightCombinedEffectParamLightAmbient.SetValue(4);
            lightCombinedEffectParamAmbientColor.SetValue(ambientLight.ToVector4());
            lightCombinedEffectParamColorMap.SetValue(colorMap_front);
            lightCombinedEffectParamShadowMap.SetValue(shadowMap_front);
            lightCombinedEffectParamNormalMap.SetValue(normalMap_front);
            lightCombinedEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, lightCombinedEffect, Resolution.getTransformationMatrix());
            spriteBatch.Draw(colorMap_front, Vector2.Zero, Color.White);
            spriteBatch.End();

        }

        private Texture2D GenerateShadowMap()
        {
            game.GraphicsDevice.SetRenderTarget(shadowMap_back);
            game.GraphicsDevice.Clear(Color.Transparent);

            foreach (var light in lights)
            {
                if (!light.IsEnabled) continue;

                game.GraphicsDevice.SetVertexBuffer(VertexBuffer);

                // Draw all light sources
                lightEffectParameterStrenght.SetValue(light.ActualPower);
                lightEffectParameterPosition.SetValue(light.Position);
                lightEffectParameterLightColor.SetValue(light.Color);
                lightEffectParameterLightDecay.SetValue(light.LightDecay);
                lightEffect.Parameters["specularStrength"].SetValue(specularStrenght);

                if (light.LightType == LightType.Point)
                {
                    lightEffect.CurrentTechnique = lightEffectTechniquePointLight;
                }

                lightEffectParameterScreenWidth.SetValue(game.GraphicsDevice.Viewport.Width);
                lightEffectParameterScreenHeight.SetValue(game.GraphicsDevice.Viewport.Height);
                lightEffect.Parameters["ambientColor"].SetValue(ambientLight.ToVector4());
                lightEffectParameterNormalMap.SetValue(normalMap_back);
                lightEffect.Parameters["ColorMap"].SetValue(colorMap_back);
                lightEffect.CurrentTechnique.Passes[0].Apply();

                game.GraphicsDevice.BlendState = BlendBlack;

                game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertices, 0, 2);
            }

            game.GraphicsDevice.SetRenderTarget(null);

            game.GraphicsDevice.SetRenderTarget(shadowMap_front);
            game.GraphicsDevice.Clear(Color.Transparent);

            foreach (var light in lights)
            {
                if (!light.IsEnabled) continue;

                game.GraphicsDevice.SetVertexBuffer(VertexBuffer);

                // Draw all light sources
                lightEffectParameterStrenght.SetValue(light.ActualPower);
                lightEffectParameterPosition.SetValue(light.Position);
                lightEffectParameterLightColor.SetValue(light.Color);
                lightEffectParameterLightDecay.SetValue(light.LightDecay);
                lightEffect.Parameters["specularStrength"].SetValue(specularStrenght);

                if (light.LightType == LightType.Point)
                {
                    lightEffect.CurrentTechnique = lightEffectTechniquePointLight;
                }

                lightEffectParameterScreenWidth.SetValue(game.GraphicsDevice.Viewport.Width);
                lightEffectParameterScreenHeight.SetValue(game.GraphicsDevice.Viewport.Height);
                lightEffect.Parameters["ambientColor"].SetValue(ambientLight.ToVector4());
                lightEffectParameterNormalMap.SetValue(normalMap_front);
                lightEffect.Parameters["ColorMap"].SetValue(colorMap_front);
                lightEffect.CurrentTechnique.Passes[0].Apply();

                game.GraphicsDevice.BlendState = BlendBlack;

                game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertices, 0, 2);
            }

            game.GraphicsDevice.SetRenderTarget(null);

            return shadowMap_back;
        }

        private void CreateLights()
        {
            lights.Add(entityManager.PlayerPointLight());

            for (int i = 0; i < entityManager.BulletListSize; i++)
            {
                PointLight pointLight = entityManager.BulletPointLight(i);
                if (pointLight == null) continue;
                lights.Add(pointLight);
            }
        }

        private void DarkenScreen()
        {
            if (darkenScreen != true)
            {
                darkenScreen = true;
                screenIsDark = true;
            }
            else
            {
                ambientTimer = 0.0;
            }


            if (brightenScreen == true)
            {
                brightenScreen = false;
            }
        }

        private void BrightenScreen()
        {
            brightenScreen = true;

            if (darkenScreen == true)
            {
                darkenScreen = false;
            }
        }

        private void HandleScreenAmbient(GameTime gameTime)
        {
            ambientTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (darkenScreen == true)
            {
                ambientLight.R -= (byte)5;
                ambientLight.G -= (byte)5;
                ambientLight.B -= (byte)5;

                if (ambientLight.R <= 0) ambientLight.R = 0;
                if (ambientLight.G <= 0) ambientLight.G = 0;
                if (ambientLight.B <= 0) ambientLight.B = 0;

                if (ambientLight.R <= 0 && ambientLight.G <= 0 && ambientLight.B <= 0)
                {
                    ambientLight.R = 0;
                    ambientLight.G = 0;
                    ambientLight.B = 0;
                    ambientTimer = 0.0;
                    darkenScreen = false;
                }
            }
            else if (brightenScreen == true)
            {
                ambientLight.R += (byte)1;
                ambientLight.G += (byte)1;
                ambientLight.B += (byte)1;

                if (ambientLight.R >= 255) ambientLight.R = 255;
                if (ambientLight.G >= 255) ambientLight.G = 255;
                if (ambientLight.B >= 255) ambientLight.B = 255;

                if (ambientLight.R >= 255 && ambientLight.G >= 255 && ambientLight.B >= 255)
                {
                    ambientLight.R = 255;
                    ambientLight.G = 255;
                    ambientLight.B = 255;
                    brightenScreen = false;
                    screenIsDark = false;
                    ambientTimer = 0.0;
                }
            }

            if (ambientTimer >= 5000.0 && brightenScreen == false)
            {
                brightenScreen = true;
            }
        }

        private void LevelCleansed()
        {
            IncreaseLightSource();
            ambientLight.B -= (byte)1;
            if (ambientLight.B <= 100)
            {
                ambientLight.B = 100;
            }
        }

        private void IncreaseLightSource()
        {
            if (cleansPass == 1)
            {
                ambientStrength += ambientStrenghtScalar;
                if (ambientStrength >= 6.0f)
                {
                    cleansPass = 2;
                }
            }
            else if (cleansPass == 2)
            {
                ambientStrength -= ambientStrenghtScalar;
                if (ambientStrength <= 4.0f)
                {
                    cleansing = false;
                }
            }
        }

        private static BlendState BlendBlack = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,

            AlphaBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.One
        };
    }
}
