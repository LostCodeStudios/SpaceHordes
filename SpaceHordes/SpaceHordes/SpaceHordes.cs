using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Game_Library;
using Game_Library.Input;
using Game_Library.GameStates;
using Game_Library.GameStates.Screens;
using Game_Library.Gameplay;

/***Some documentation notes:
 * From this point, herein, standard regions for classes must be use and stuff. lol.

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialization

        #endregion

        #region Update & Draw

        #endregion

 * * 
 */


namespace SpaceHordes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceHordes : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;

        #endregion

        #region Initalization

        public SpaceHordes()
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
            #if WINDOWS_PHONE 
                graphics.PreferredBackBufferHeight = 480; 
                graphics.PreferredBackBufferWidth = 800; 
            #else
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 720;
            #endif

            graphics.ApplyChanges();

            #if WINDOWS
            this.IsMouseVisible = true;
            #endif

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

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

            base.LoadContent();

            // TODO: use this.Content to load your game content here
            screenManager.AddScreen(new BackgroundScreen("Textures/Hiscore"), null);
            screenManager.AddScreen(new MainMenuScreen("Space Hordes"), null);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Update & Draw

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

            

            //UPDATE


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        #endregion
    }
}
