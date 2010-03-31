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
        RingEmitter emitter2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            simulation = new StandardSimulation(20000);
            emitter = new BoxEmitter();
            emitter2 = new RingEmitter();
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
            simulation.Is2D = true;

            emitter.Simulation = simulation;
            emitter.Active = true;
            emitter.Configuration.ParticlesPerSecond = 200;
            emitter.Configuration.InitialSpeed = 30;
            emitter.Configuration.InitialLifespan = 9;
            emitter.Configuration.EmitOnSurfaceOnly = true;

            emitter.BoxConfiguration.Width = 200;
            emitter.BoxConfiguration.Height = 200;
            
            emitter2.Simulation = simulation;
            emitter2.Active = true;
            emitter2.Configuration.ParticlesPerSecond = 200;
            emitter2.Configuration.InitialSpeed = 30;
            emitter2.Configuration.InitialLifespan = 9;
            emitter2.Configuration.EmitOnSurfaceOnly = true;

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

            base.Draw(gameTime);
        }
    }
}
