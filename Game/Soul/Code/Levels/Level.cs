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
        public bool cleansing = false;
        private int cleansPass = 1;
        private bool useDynamicLights = true;
        private bool playerNightmareHit = false;
        private bool playerHit = false;

        private double wait = -1;
        private bool doWait = false;
        private bool doneOnce = false;


        BackgroundManager backgroundManager_back;
        BackgroundManager backgroundManager_front;
        AudioManager audioManager;
        EntityManager entityManager;
        Player player;
        InputManager controls;
        LevelReader levelReader;
        MenuManager menuManager;
        SpriteBatch spriteBatch;
        Soul game;
        int timeStarted = 0;
        bool fulhack = true;
        SpriteFont font;

        private AmbientFade NightmareAmbientFade = null;
        private AmbientFade PlayerHitAmbientFade = null;
        private AmbientFade currentAmbientFade = null;

        private Sprite bg1 = null;
        private Sprite bg1_normal = null;
        private Sprite fog = null;
        private Sprite fog_normal = null;


        private RenderTarget2D colorMap_back;
        private RenderTarget2D normalMap_back;
        private RenderTarget2D shadowMap_back;
        private RenderTarget2D colorMap_front;
        private RenderTarget2D normalMap_front;
        private RenderTarget2D shadowMap_front;
        private RenderTarget2D entityLayer;

        private List<Light> lights = new List<Light>();
        private Color ambientLight;
        private Color levelAmbient;

        private float specularStrenght = 1.0f;
        private float ambientStrength = 1f;
        private float ambientStrenghtScalar = 0.025f;
        private bool tutorial = false;
        private TutorialManager tutorialManager = null;

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

        public  bool brightenScreen = false;

        public VertexPositionColorTexture[] Vertices;
        public VertexBuffer VertexBuffer;

        public Level(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, EntityManager entityManager, Player player, InputManager controls, string filename, string id)
        {
            this.spriteBatch = spriteBatch;
            this.game = game;
            this.entityManager = entityManager;
            this.id = id;
            this.controls = controls;
            levelReader = new LevelReader(filename, game);
            this.player = player;
            font = game.Content.Load<SpriteFont>("GUI\\Extrafine");
            this.audioManager = audioManager;
            levelAmbient = new Color(byte.Parse(game.lighting.getValue("AmbientLight", "ColorR")), byte.Parse(game.lighting.getValue("AmbientLight", "ColorG")), byte.Parse(game.lighting.getValue("AmbientLight", "ColorB")), byte.Parse(game.lighting.getValue("AmbientLight", "ColorA")));
            ambientLight = new Color(byte.Parse(game.lighting.getValue("AmbientLight", "ColorR")), byte.Parse(game.lighting.getValue("AmbientLight", "ColorG")), byte.Parse(game.lighting.getValue("AmbientLight", "ColorB")), byte.Parse(game.lighting.getValue("AmbientLight", "ColorA")));
        }

        public void initialize()
        {
            levelReader.Parse();
            entityManager.AddEntityDataList(levelReader.EntityDataList);
            entityManager.AddLightList(lights);
            entityManager.initialize();
            CreateBackgrounds();
            menuManager = new MenuManager(controls);
            ImageButton button = new ImageButton(spriteBatch, game, controls, new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f - 150, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f), Constants.GUI_CONTINUE, "continue");
            button.onClick += new ImageButton.ButtonEventHandler(OnButtonPress);
            menuManager.AddButton(button);
            button = new ImageButton(spriteBatch, game, controls, new Vector2((float)Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 150, (float)Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f), Constants.GUI_QUIT, "quit");
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

            bg1 = new Sprite(spriteBatch, game, "Backgrounds\\background__0002s_0008_Layer-1_combined");
            bg1_normal = new Sprite(spriteBatch, game, "Backgrounds\\background__0002s_0008_Layer-1_combined_depth");
            fog = new Sprite(spriteBatch, game, Constants.BACKGROUND_FOG);
            fog_normal = new Sprite(spriteBatch, game, Constants.BACKGROUND_FOG_NORMAL);
            


            player.nightmareHit += new Player.NightmareHit(DarkenScreen);
            player.hitFlash += new Player.HitFlash(PlayerHitFlash);

            lights.Add(player.PointLight);
            lights.Add(player.HealthLight);

            specularStrenght = float.Parse(game.config.getValue("Video", "Specular"));
            useDynamicLights = bool.Parse(game.config.getValue("Video", "DynamicLights"));

            NightmareAmbientFade = new AmbientFade("NightmareAmbientFade", new Color(0, 0, 0, 255), 10, 2, 5000.0);
            NightmareAmbientFade.SetCurrentColor(ambientLight);
            PlayerHitAmbientFade = new AmbientFade("PlayerAmbientHit", new Color(255, 150, 150, 255), 15, 15);
            this.tutorial = bool.Parse(game.config.getValue("General", "Tutorial"));

            if (tutorial == true)
            {
                tutorialManager = new TutorialManager(spriteBatch, game, font, controls);
                tutorialManager.Initialize("movement");
            }
        }

        public void shutdown()
        {
            fulhack = true;
        }

        public int Update(GameTime gameTime)
        {
            if (tutorial == true)
            {
                tutorialManager.Update(gameTime, player.position);
                if (tutorialManager.AllTutorialsDone() == true)
                {
                    entityManager.tutorial = false;
                    tutorial = false;
                }
            }

            if (currentAmbientFade != null)
            {
                currentAmbientFade.Update(gameTime);

                if (currentAmbientFade.Done != true)
                {
                    ambientLight = currentAmbientFade.CurrentColor;
                    if (playerHit == true)
                    {
                        playerHit = false;
                    }
                    else if (playerNightmareHit == true)
                    {
                        playerNightmareHit = false;
                    }
                }

                if (currentAmbientFade.ID == "PlayerAmbientHit")
                {
                    if (currentAmbientFade.Done == true && NightmareAmbientFade.Done != true)
                    {
                        currentAmbientFade = NightmareAmbientFade;
                    }
                }
            }

            checkForDeadLights();
            if (fulhack)
            {
                timeStarted = (int)gameTime.TotalGameTime.TotalMilliseconds;
                fulhack = false;
            }

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
                    audioManager.playSound("menu_move");
                }
                else if (controls.MoveRightOnce == true)
                {
                    menuManager.increment();
                    audioManager.playSound("menu_move");
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
                menuManager.FadeIn();
                audioManager.playSound("pause_appear");
            }

            if (quit == true)
            {
                return 1;
            }

            if (cleansing)
            {
                LevelCleansed(gameTime);
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
            bg1.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            backgroundManager_back.Draw();
            fog.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

            // Draw Normal Map Back Layer
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(normalMap_back);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Resolution.getTransformationMatrix());
            bg1_normal.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            backgroundManager_back.DrawNormalMap();
            fog_normal.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(entityLayer);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Resolution.getTransformationMatrix());
            entityManager.Draw();
            spriteBatch.End();
            
            // Draw Color Map Front Layer
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(colorMap_front);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Resolution.getTransformationMatrix());
            player.Draw();
            if (tutorial == true)
            {
                tutorialManager.Draw();
            }
            backgroundManager_front.Draw();

            if (pause)
            {
                menuManager.Draw();
            }

            if (bool.Parse(game.config.getValue("Debug", "InfoCorner")))
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Resolution.getTransformationMatrix());
                double time = Math.Round((gameTime.TotalGameTime.TotalMilliseconds - timeStarted + double.Parse(game.config.getValue("Debug", "StartingTime"))) / 1000);
                string output = time.ToString();
                string ambientInfo = "Ambient Light: " + ambientLight.ToString();
                string ambientAmplifyInfo = "Ambient Amplifier: " + ambientStrength.ToString();
                string specular = "Specular: " + specularStrenght.ToString();
                string numberOfLights = "Lights: " + lights.Count.ToString();
                string waitTimer = "WaitTimer: " + wait.ToString();
                spriteBatch.DrawString(font, output, new Vector2(10f), Color.Gray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font, ambientInfo, new Vector2(10f, 40f), Color.Gray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font, ambientAmplifyInfo, new Vector2(10f, 80f), Color.Gray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font, specular, new Vector2(10f, 120f), Color.Gray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font, numberOfLights, new Vector2(10f, 160f), Color.Gray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(font, waitTimer, new Vector2(10f, 200f), Color.Gray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            }

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);
            GenerateShadowMap();

            game.GraphicsDevice.Clear(Color.Black);
            DrawCombinedMaps();

            


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
                    ScrollingBackground bg = new ScrollingBackground(spriteBatch, game, bgDataBack[i].Filename, "bg" + i.ToString(), bgDataBack[i].SpawnTime, bgDataBack[i].DeleteTime, bgDataBack[i].Direction, bgDataBack[i].Layer, bgDataBack[i].PersistScroll);
                    backgroundManager_back.addBackground(bg);
                }
                else if (bgDataBack[i].Type == "Pillar")
                {
                    BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, backgroundManager_back.getPillarFileName(bgDataBack[i].Filename), bgDataBack[i].SpawnTime, bgDataBack[i].Direction, bgDataBack[i].Layer, bgDataBack[i].PersistScroll);
                    backgroundManager_back.addBackground(bg);
                }
                else if (bgDataBack[i].Type == "Batch")
                {
                    PillarBatch bg = new PillarBatch(backgroundManager_back, spriteBatch, game, bgDataBack[i].LowestSpawnRate, bgDataBack[i].HighestSpawnRate, (int)bgDataBack[i].DeleteTime, bgDataBack[i].Direction, bgDataBack[i].RandomDirection, bgDataBack[i].RandomSpeed, bgDataBack[i].Layer, bgDataBack[i].PersistScroll);
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
                    ScrollingBackground bg = new ScrollingBackground(spriteBatch, game, bgDataFront[i].Filename, "bg" + i.ToString(), bgDataFront[i].SpawnTime, bgDataFront[i].DeleteTime, bgDataFront[i].Direction, bgDataFront[i].Layer, bgDataFront[i].PersistScroll);
                    backgroundManager_front.addBackground(bg);
                }
                else if (bgDataFront[i].Type == "Pillar")
                {
                    BackgroundPillar bg = new BackgroundPillar(spriteBatch, game, backgroundManager_front.getPillarFileName(bgDataFront[i].Filename), bgDataFront[i].SpawnTime, bgDataFront[i].Direction, bgDataFront[i].Layer, bgDataFront[i].PersistScroll);
                    backgroundManager_front.addBackground(bg);
                }
                else if (bgDataFront[i].Type == "Batch")
                {
                    PillarBatch bg = new PillarBatch(backgroundManager_front, spriteBatch, game, bgDataFront[i].LowestSpawnRate, bgDataFront[i].HighestSpawnRate, (int)bgDataFront[i].DeleteTime, bgDataFront[i].Direction, bgDataFront[i].RandomDirection, bgDataFront[i].RandomSpeed, bgDataFront[i].Layer, bgDataFront[i].PersistScroll);
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
                menuManager.FadeOut();
                audioManager.playSound("menu_select");
            }
            else if (button.ID == "quit")
            {
                quit = true;
                menuManager.FadeOut();
                audioManager.playSound("menu_select");
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
            spriteBatch.Draw(colorMap_back, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(entityLayer, new Rectangle(0, 0, Constants.RESOLUTION_VIRTUAL_WIDTH, Constants.RESOLUTION_VIRTUAL_HEIGHT), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(colorMap_front, new Rectangle(0, 0, Constants.RESOLUTION_VIRTUAL_WIDTH, Constants.RESOLUTION_VIRTUAL_HEIGHT), Color.White);
            spriteBatch.End();

        }

        private Texture2D GenerateShadowMap()
        {
            game.GraphicsDevice.SetRenderTarget(shadowMap_back);
            game.GraphicsDevice.Clear(Color.Transparent);

            if (useDynamicLights == true)
            {
                foreach (var light in lights)
                {
                    if (!light.IsEnabled) continue;

                    game.GraphicsDevice.SetVertexBuffer(VertexBuffer);

                    // Draw all light sources
                    lightEffectParameterStrenght.SetValue(light.ActualPower);
                    lightEffectParameterPosition.SetValue(light.Position);
                    lightEffectParameterLightColor.SetValue(light.Color);
                    lightEffectParameterLightDecay.SetValue(light.LightDecay);
                    if (light.renderSpecular)
                    {
                        lightEffect.Parameters["specularStrength"].SetValue(specularStrenght);
                    }

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
            }
            game.GraphicsDevice.SetRenderTarget(null);

            return shadowMap_back;
        }

        private void PlayerHitFlash()
        {
            if (playerHit == false)
            {
                currentAmbientFade = PlayerHitAmbientFade;
                currentAmbientFade.Reset();
                currentAmbientFade.SetCurrentColor(ambientLight);
                currentAmbientFade.SetTargetColor(NightmareAmbientFade.CurrentColor);
            }

            playerHit = true;

        }

        private void DarkenScreen()
        {

            if (playerNightmareHit == false && playerHit == false)
            {
                currentAmbientFade = NightmareAmbientFade;
                currentAmbientFade.Reset();
                currentAmbientFade.SetCurrentColor(ambientLight);
                currentAmbientFade.SetTargetColor(new Color(255, 255, 255, 255));
            }
            playerNightmareHit = true;
        }

        public void stopBgScroll()
        {
            backgroundManager_back.stopScroll();
            backgroundManager_front.stopScroll();
        }

        private void resetLights()
        {
            brightenScreen = false;
            levelAmbient = Color.White;
            ambientLight = Color.White;
        }

        private void LevelCleansed(GameTime gameTime)
        {
            if (!doneOnce)
            {
                resetLights();
                doneOnce = true;
            }

            IncreaseLightSource();
            if (ambientLight.B > 100)
            {
                decreaseAmbientB(1);

                foreach (Light l in lights)
                {
                    l.Power = l.Power - 0.0002f;
                }
            }

            if (ambientLight.B <= 100)
            {
                doWait = true;
            }

            if (doWait)
            {
                wait += gameTime.ElapsedGameTime.Milliseconds;
                if (wait > 3000)
                {
                    quit = true;
                }
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
            }
        }

        private void checkForDeadLights()
        {
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].dead == true)
                {
                    lights.RemoveAt(i);
                    i--;
                }
            }
        }

        #region AmbientControl
        private void increaseAmbientR(int value)
        {
            int r = (int)ambientLight.R;
            r += value;
            if (r >= (int)levelAmbient.R)
            {
                r = (int)levelAmbient.R;
                ambientLight.R = (byte)r;
            }
            else
            {
                ambientLight.R = (byte)r;
            }
        }

        private void increaseAmbientG(int value)
        {
            int g = (int)ambientLight.G;
            g += value;
            if (g >= (int)levelAmbient.G)
            {
                g = (int)levelAmbient.G;
                ambientLight.G = (byte)g;
            }
            else
            {
                ambientLight.G = (byte)g;
            }
        }

        private void increaseAmbientB(int value)
        {
            int b = (int)ambientLight.B;
            b += value;
            if (b >= (int)levelAmbient.B)
            {
                b = (int)levelAmbient.B;
                ambientLight.B = (byte)b;
            }
            else
            {
                ambientLight.B = (byte)b;
            }
        }

        private void decreaseAmbientR(int value)
        {
            int r = (int)ambientLight.R;
            r -= value;
            if (r <= 0)
            {
                r = 0;
            }
            ambientLight.R = (byte)r;
        }

        private void decreaseAmbientG(int value)
        {
            int g = (int)ambientLight.G;
            g -= value;
            if (g <= 0)
            {
                g = 0;
            }
            ambientLight.G = (byte)g;
        }

        private void decreaseAmbientB(int value)
        {
            int b = (int)ambientLight.B;
            b -= value;
            if (b <= 0)
            {
                b = 0;
            }
            ambientLight.B = (byte)b;
        }
        #endregion AmbientControl

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
