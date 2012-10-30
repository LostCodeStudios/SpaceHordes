using System;
using System.Threading;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Game_Library.Input;
using Game_Library.Model.Managers;
using Game_Library.Model;

namespace Game_Library.GameStates.Screens
{
    /// <summary>
    /// This screen implements the actual game logic.
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        EntityManager entities;
        SpriteFont gameFont;
        string fontName;

        float pauseAlpha;
        Texture2D spriteSheet;
        InputAction pauseAction;

        #endregion

        #region Properties

        /// <summary>
        /// The gameplay screen's content manager.
        /// </summary>
        public ContentManager Content
        {
            get { return content; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(string fontName)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.fontName = fontName;

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }
        
        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>(fontName);

            spriteSheet = Content.Load<Texture2D>("Textures/spritesheet");

            entities = new EntityManager();
            Entity test = new Entity(new Vector2(400, 400), 0f, 0);
            test.Sprites.Add("body",new Sprite(new Vector2(400,400),spriteSheet,new Rectangle(50,50,50,50),Vector2.Zero));
            entities.Add("tester",test);

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }

        #endregion

        #region Update & Draw

        /// <summary>
        /// Updates the state of the game. This method 
        /// checks the GameScreen.IsActive property, so the
        /// game will stop updating when the pause menu is 
        /// active, or if you tab away to a different
        /// application.
        /// </summary>
        public override void Update(
            GameTime gameTime,
            bool otherScreenHasFocus, 
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //Gradually fade in or out depending on if we are covered by another screen
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                
                //TODO: Game
                entities.Update(gameTime);
            }

        }

        /// <summary>
        /// Lets the game respond to user input. Only called when this screen
        /// is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int playerIndex = (int)ControllingPlayer.Value;

            #if WINDOWS
                KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            #endif
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            
            //TODO: All drawing for gameplay.
            entities.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion
    }
}
