using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Soul.Manager;

namespace Soul
{
    class MenuState : State
    {

        private List<Light> lights = new List<Light>();
        private Color ambientLight = new Color(.1f, .1f, .1f, 1f);

        private RenderTarget2D colorMap = null;
        private RenderTarget2D normalMap = null;
        private RenderTarget2D shadowMap = null;

        private float ambientStrength = 1f;
        private float ambientStrenghtScalar = 0.025f;

        private float specularStrenght = 1.0f;
        private List<GlowParticle> glowList = null;

        #region LightEffects
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
        #endregion LightEffects

        public VertexPositionColorTexture[] Vertices;
        public VertexBuffer VertexBuffer;

        private MenuManager askToQuit = null;
        private MenuStateManager menuStateManager = null; 
        private Sprite bg;
        private Sprite bg_normal;
        private Sprite bg_front;
        private Sprite bg_front_normal;
        private Sprite bg_spikes;
        private Sprite fog;
        private Sprite logo;
        private SpriteFont spriteFont = null;
        private FadeInOut fade;
        private GlowFX glowFX;
        private GraphicsDeviceManager graphics  =  null;
        private LinkedList<DisplayMode> displayModes;
        private bool quit = false;
        private bool confirm = false;
        private Random random = null;
        private int lightSpawnMin = 1000;
        private int lightSpawnMax = 2000;
        private double lightSpawnTime = 0.0;
        private double timer = 0.0;

        public MenuState(SpriteBatch spriteBatch, Soul game, GraphicsDeviceManager graphics, LinkedList<DisplayMode> displayModes, AudioManager audioManager, InputManager controls, string id) : base(spriteBatch, game, audioManager, controls, id)
        {
            this.graphics = graphics;
            this.displayModes = displayModes;
        }

        public override void initialize(string data)
        {
            glowList = new List<GlowParticle>();
            nextState = "";
            bg = new Sprite(spriteBatch, game, Constants.MENU_COMBINED_BG_COLORMAP);
            bg_normal = new Sprite(spriteBatch, game, Constants.MENU_COMBINED_BG_NORMALMAP);
            bg_front = new Sprite(spriteBatch, game, Constants.MENU_FRONT_BG_COLORMAP);
            bg_front_normal = new Sprite(spriteBatch, game, Constants.MENU_FRONT_BG_NORMALMAP);
            bg_spikes = new Sprite(spriteBatch, game, Constants.MENU_SPIKES_BG_COLORMAP);
            fog = new Sprite(spriteBatch, game, "Backgrounds\\background__0002s_0001_Layer-8");
            logo = new Sprite(spriteBatch, game, "GUI\\logo_SOUL");
            fade = new FadeInOut(spriteBatch, game);
            audio.playMusic("menu_music");
            glowFX = new GlowFX(game);
            menuStateManager = new MenuStateManager(spriteBatch, game, graphics, displayModes, controls, audio);
            menuStateManager.Initialize();
            spriteFont = game.Content.Load<SpriteFont>(Constants.GUI_FONT);
            askToQuit = new MenuManager(controls, "askToQuit");
            ImageButton button = new ImageButton(spriteBatch, game, controls, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f - 300, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), Constants.GUI_YES, "quit_yes");
            Label label = new Label(spriteBatch, game, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f), "quit_text", "Are you sure you want to quit?", false);
            button.onClick += new ImageButton.ButtonEventHandler(QuitConfirm);
            askToQuit.AddButton(button, label);
            button = new ImageButton(spriteBatch, game, controls, new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH * 0.5f + 300, Constants.RESOLUTION_VIRTUAL_HEIGHT * 0.5f + 250.0f), Constants.GUI_NO, "quit_no");
            button.onClick += new ImageButton.ButtonEventHandler(QuitConfirm);
            askToQuit.AddButton(button);
            askToQuit.initialize();

            #region InitializeLighting
            PresentationParameters pp = game.GraphicsDevice.PresentationParameters;
            int width = game.Window.ClientBounds.Width;
            int height = game.Window.ClientBounds.Height;
            SurfaceFormat format = pp.BackBufferFormat;
            Vertices = new VertexPositionColorTexture[4];
            Vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
            Vertices[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            Vertices[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            Vertices[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));
            VertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColorTexture), Vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(Vertices);

            colorMap = new RenderTarget2D(game.GraphicsDevice, width, height);
            normalMap = new RenderTarget2D(game.GraphicsDevice, width, height);
            shadowMap = new RenderTarget2D(game.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

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
            #endregion InitializeLighting

            /*PointLight light = new PointLight()
            {
                Color = new Vector4(1f, 1f, 1f, 1f),
                Power = 8f,
                LightDecay = 300,
                Position = new Vector3(200f, 400f, 50f),
                IsEnabled = true
            };

            lights.Add(light);*/
            random = new Random();
            fade.FadeIn();
        }

        public override void shutdown()
        {
            audio.stopMusic();
            bg.Dispose();
            changeState = false;
            lights.Clear();
            glowList.Clear();
        }

        public override string getNextState()
        {
            return nextState;
        }

        public override bool Update(GameTime gameTime)
        {
            checkForDeadLights();
            SpawnRandomLight(gameTime);

            //lights[0].Position = new Vector3(Mouse.GetState().X, Mouse.GetState().Y, lights[0].Position.Z);

            foreach (GlowParticle particle in glowList)
            {
                particle.Update(gameTime);
            }

            int value = 0;
            if (confirm == true)
            {
                askToQuit.Update(gameTime);

                if (controls.MoveLeftOnce == true)
                {
                    askToQuit.increment();
                }
                else if (controls.MoveRightOnce == true)
                {
                    askToQuit.decrement();
                }

                if (askToQuit.FadeOutDone() == true)
                {
                    confirm = false;
                }

                if (menuStateManager.IsMenuFadingDone() != true)
                {
                    //menuStateManager.FadeOutMenu();
                }
            }
            else
            {
                /*if (menuStateManager.IsMenuFadingInDone() == false)
                {
                    menuStateManager.FadeInMenu();
                }*/
            }

            if (confirm == false)
            {
                value = menuStateManager.Update(gameTime);
            }
            
            
            if (value == -1)
            {
                fade.FadeOut();
                quit = true;
            }
            else if (value == 1)
            {
                nextState = "WorldMapState";
                glowFX.glowMax = .9f;
                glowFX.glowFx = .9f;
                glowFX.glowScalar = 0.005f;
                fade.FadeOut();
            }
            else if (value == -2)
            {
                RecreateWindow();
            }

            if (fade.FadeOutDone == true)
            {
                if (quit == true)
                {
                    game.Exit();
                }
                else
                {
                    changeState = true;
                }
            }
            else if (fade.IsFading == true)
            {
                fade.Update(gameTime);
                
            }
            glowFX.Update();
            return changeState;
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);

            // draw color map
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(colorMap);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            bg.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            bg_front.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

            // draw normal map
            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.SetRenderTarget(normalMap);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            bg_normal.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            bg_front_normal.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();


            /*spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            bg.Draw(new Vector2(0f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();*/

            game.GraphicsDevice.SetRenderTarget(null);
            GenerateShadowMap();

            game.GraphicsDevice.Clear(Color.Black);
            DrawCombinedMaps();

            spriteBatch.Begin(0, null, null, null, null, glowFX.Effect, Resolution.getTransformationMatrix());
            foreach (GlowParticle particle in glowList)
            {
                particle.Draw();
            }
            fog.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            bg_spikes.Draw(Vector2.Zero, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            logo.Draw(new Vector2(50f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            menuStateManager.Draw();

            if (confirm == true) askToQuit.Draw();

            fade.Draw();
            spriteBatch.End();
        }


        private void QuitConfirm(ImageButton button)
        {
            if (button.ID == "quit_yes")
            {
                fade.FadeOut();
                quit = true;
            }
            else if (button.ID == "quit_no")
            {
                askToQuit.FadeOut();
                //menuStateManager.FadeInMenu();
            }
        }

        public override string StateData()
        {
            return "";
        }

        private void DrawCombinedMaps()
        {
            lightCombinedEffect.CurrentTechnique = lightCombinedEffectTechnique;
            lightCombinedEffectParamAmbient.SetValue(ambientStrength);
            lightCombinedEffectParamLightAmbient.SetValue(4);
            lightCombinedEffectParamAmbientColor.SetValue(ambientLight.ToVector4());
            lightCombinedEffectParamColorMap.SetValue(colorMap);
            lightCombinedEffectParamShadowMap.SetValue(shadowMap);
            lightCombinedEffectParamNormalMap.SetValue(normalMap);
            lightCombinedEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, lightCombinedEffect, Resolution.getTransformationMatrix());
            spriteBatch.Draw(colorMap, new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height), Color.White);
            spriteBatch.End();

        }

        private Texture2D GenerateShadowMap()
        {
            game.GraphicsDevice.SetRenderTarget(shadowMap);
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
                    lightEffect.Parameters["specularStrength"].SetValue(float.Parse(game.config.getValue("Video","Specular")));

                    if (light.LightType == LightType.Point)
                    {
                        lightEffect.CurrentTechnique = lightEffectTechniquePointLight;
                    }

                    lightEffectParameterScreenWidth.SetValue(game.GraphicsDevice.Viewport.Width);
                    lightEffectParameterScreenHeight.SetValue(game.GraphicsDevice.Viewport.Height);
                    lightEffect.Parameters["ambientColor"].SetValue(ambientLight.ToVector4());
                    lightEffectParameterNormalMap.SetValue(normalMap);
                    lightEffect.Parameters["ColorMap"].SetValue(colorMap);
                    lightEffect.CurrentTechnique.Passes[0].Apply();

                    game.GraphicsDevice.BlendState = BlendBlack;

                    game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertices, 0, 2);
                }
            
            game.GraphicsDevice.SetRenderTarget(null);

            return shadowMap;
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

        private void SpawnRandomLight(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= lightSpawnTime)
            {
                GlowParticle glow = new GlowParticle(spriteBatch, game, "glow", new Vector2(-100f, (float)random.Next(100, 600)), new Vector2((float)random.Next(3, 5), 0f));
                lights.Add(glow.PointLight);
                glowList.Add(glow);
                lightSpawnTime = (double)random.Next(lightSpawnMin, lightSpawnMax);
                timer = 0.0;
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

            for (int i = 0; i < glowList.Count; i++)
            {
                if (glowList[i].position.X > Constants.RESOLUTION_VIRTUAL_WIDTH + 100)
                {
                    glowList[i].PointLight.dead = true;
                    glowList.RemoveAt(i);
                    i--;
                }


            }
        }

        private void RecreateWindow()
        {
            PresentationParameters pp = game.GraphicsDevice.PresentationParameters;
            int width = game.Window.ClientBounds.Width;
            int height = game.Window.ClientBounds.Height;
            SurfaceFormat format = pp.BackBufferFormat;
            Vertices = new VertexPositionColorTexture[4];
            Vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
            Vertices[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            Vertices[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            Vertices[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));
            VertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColorTexture), Vertices.Length, BufferUsage.None);
            VertexBuffer.SetData(Vertices);

            colorMap = new RenderTarget2D(game.GraphicsDevice, width, height);
            normalMap = new RenderTarget2D(game.GraphicsDevice, width, height);
            shadowMap = new RenderTarget2D(game.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            graphics.ApplyChanges();
        }
    }
}
