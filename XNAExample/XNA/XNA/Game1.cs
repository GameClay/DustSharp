using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using GameClay.Dust;
using GameClay.Dust.Emitter;
using GameClay.Dust.Simulation;

namespace XNAExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StandardSimulation simulation;
        BoxEmitter emitter;
        Texture2D texture;
        SphereEmitter emitter2;
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        SpriteBatch fpsBatch;
        SpriteFont spriteFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            simulation = new StandardSimulation(20000);
            simulation.Is2D = true;

            Parameters emitterConfig = new Parameters();
            emitterConfig.SetParameter("ParticlesPerSecond", 20);
            emitterConfig.SetParameter("InitialSpeed", 30.0f);
            emitterConfig.SetParameter("InitialLifespan", 4.0f);
            emitterConfig.SetParameter("EmitOnSurfaceOnly", true);
            emitterConfig.SetParameter("Width", 200.0f);
            emitterConfig.SetParameter("Height", 200.0f);
            emitterConfig.SetParameter("Persistent", true);

            Parameters emitterConfig2 = new Parameters();
            emitterConfig2.SetParameter("ParticlesPerSecond", 20);
            emitterConfig2.SetParameter("InitialSpeed", 30.0f);
            emitterConfig2.SetParameter("InitialLifespan", 4.0f);
            emitterConfig2.SetParameter("Radius", 1.0f);
            emitterConfig2.SetParameter("EmitOnSurfaceOnly", true);
            emitterConfig2.SetParameter("EmitRingOnly", true);
            emitterConfig2.SetParameter("Persistent", true);

            emitter = new BoxEmitter(emitterConfig);
            emitter.Simulation = simulation;
            emitter.Active = true;

            emitter2 = new SphereEmitter(emitterConfig2);
            emitter2.Simulation = simulation;
            emitter2.Active = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            texture = Content.Load<Texture2D>("white"); ;

            // Add framerate counter
            fpsBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            simulation.AdvanceTime(dt);
            emitter.AdvanceTime(dt, dt);
            emitter2.AdvanceTime(dt, dt);
            // FPS
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque);

            ISystemData systemData = simulation.SystemData;
            float width = Window.ClientBounds.Width / 2;
            float height = Window.ClientBounds.Height / 2;
            for (int i = 0; i < systemData.NumParticles; i++)
            {
                Vector2 pos;
                pos.X = systemData.PositionX[i] + width;
                pos.Y = systemData.PositionY[i] + height;

                Color col = Color.White;
                col.R = (byte)(255.0f * (systemData.TimeRemaining[i] / systemData.Lifespan[i]));
                spriteBatch.Draw(texture, pos, col);
            }

            spriteBatch.End();

            // FPS

            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);
            string particles = string.Format("particles: {0}", simulation.SystemData.NumParticles);

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);

            spriteBatch.DrawString(spriteFont, particles, new Vector2(33, 1), Color.Black);
            spriteBatch.DrawString(spriteFont, particles, new Vector2(32, 0), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
