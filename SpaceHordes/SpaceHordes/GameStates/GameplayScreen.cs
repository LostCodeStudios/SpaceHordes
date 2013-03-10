using GameLibrary.GameStates;
using GameLibrary.Helpers;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceHordes.Entities.Components;
using System;
using System.Collections.Generic;

namespace SpaceHordes.GameStates.Screens
{
    /// <summary>
    /// This screen implements the actual game logic.
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        #region Fields

        private ContentManager content;
        private ImageFont gameFont;
        private SpriteBatch spriteBatch;
        private string fontName;

        private float pauseAlpha;
        private InputAction pauseAction;
        private bool multiplayer;

        private Vector2 mouseLoc;

        private long score = 0;
        private Vector2 scoreLocation;
        private float scoreScale;

        private SpaceWorld World;

        private bool over;
        private TimeSpan elapsed = TimeSpan.Zero;
        private TimeSpan beforeGameOver = TimeSpan.FromSeconds(1);

        #endregion Fields

        #region Properties

        /// <summary>
        /// The gameplay screen's content manager.
        /// </summary>
        public ContentManager Content
        {
            get { return content; }
        }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(string fontName, bool multiplayer)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.multiplayer = multiplayer;
            this.fontName = fontName;
            gameFont = new ImageFont();

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
            #region Screen

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            gameFont.LoadContent(Content, fontName);
            gameFont.SpaceWidth = 8;
            gameFont.CharSpaceWidth = 1;

            scoreLocation = new Vector2(ScreenHelper.Center.X, 0);
            scoreScale = 1f;

            spriteBatch = ScreenManager.SpriteBatch;
            List<PlayerIndex> players = new List<PlayerIndex>();

            if (multiplayer)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (ScreenManager.Input.GamePadWasConnected[x])
                    {
                        players.Add((PlayerIndex)x);
                    }
                }
            }

            #endregion Screen

            //World
            World = new SpaceWorld(ScreenManager.Game, ScreenHelper.SpriteSheet);
            World.Initialize();
            World.LoadContent(content, players.ToArray());

            ScreenManager.Game.ResetElapsedTime();
            World.Base.GetComponent<Health>().OnDeath +=
                (x) =>
                {
                    EndGame();
                };
            World.Base.GetComponent<Score>().OnChange +=
                () =>
                {
                    score = World.Base.GetComponent<Score>().Value;
                };

            MusicManager.PlaySong("Bootleg");
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

        #endregion Initialization

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
                World.Update(gameTime); //Update the world.
                if (over)
                {
                    elapsed += gameTime.ElapsedGameTime;
                    if (elapsed > beforeGameOver)
                        GameOver();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.Black, 0, 0);

            World.Draw(gameTime); //Draw the world.

            spriteBatch.Begin();
            Vector2 scoreSize = gameFont.MeasureString(score.ToString()) * scoreScale;
            gameFont.DrawString(
                spriteBatch,
                new Vector2(
                    scoreLocation.X - scoreSize.X / 2,
                    scoreLocation.Y),
                score.ToString(),
                scoreScale);
            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion Update & Draw

        #region Input

        /// <summary>
        /// Lets the game respond to user input. Only called when this screen
        /// is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int playerIndex = (int)ControllingPlayer.Value;

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                input.GamePadWasConnected[playerIndex];

#if WINDOWS || XBOX

            PlayerIndex playerI;
            if (input.CurrentKeyboardStates[0].IsKeyDown(Keys.Delete))
                GameOver();

            if (pauseAction.Evaluate(input, ControllingPlayer, out playerI) || gamePadDisconnected)
            {
                MusicManager.Pause();
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

#endif

            mouseLoc = input.MouseLocation;
        }

        #endregion Input

        #region Methods

        private void GameOver()
        {
            ScreenManager.AddScreen(new GameOverScreen(new PlayerIndex[] { (PlayerIndex)ControllingPlayer }, score), ControllingPlayer);
            ExitScreen();
            MusicManager.Stop();
        }

        private void EndGame()
        {
            over = true;
        }

        #endregion Methods
    }
}