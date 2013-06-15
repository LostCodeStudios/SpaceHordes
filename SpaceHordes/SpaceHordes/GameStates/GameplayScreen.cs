using GameLibrary.GameStates;
using GameLibrary.Helpers;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceHordes.Entities.Components;
using SpaceHordes.Entities.Systems;
using SpaceHordes.Entities.Templates.Enemies;
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

        private Vector2 mouseLoc;

        private long score = 0;
        private Vector2 scoreLocation;
        private float scoreScale;

        private SpaceWorld World;

        private int bossStart;
        private bool over;
        private bool victory;
        private TimeSpan elapsed = TimeSpan.Zero;
        private TimeSpan beforeGameOver = TimeSpan.FromSeconds(1);

        private List<PlayerIndex> playerIndices = new List<PlayerIndex>();
        private SpawnState[] states;
        private int level;

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

        public bool tutorial;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(string fontName, int bossStart, PlayerIndex[] players, int level, SpawnState[] states)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.fontName = fontName;
            this.tutorial = false;
            gameFont = new ImageFont();
            this.bossStart = bossStart;
            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);

            foreach (PlayerIndex idx in players)
            {
                playerIndices.Add(idx);
            }

            this.level = level;
            this.states = states;
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate()
        {
            #region Screen

            if (content == null)
                content = new ContentManager(Manager.Game.Services, "Content");
            gameFont.LoadContent(Content, fontName, 0f);
            gameFont.SpaceWidth = 8;
            gameFont.CharSpaceWidth = 1;

#if XBOX
            scoreLocation = new Vector2(ScreenHelper.Center.X, ScreenHelper.TitleSafeArea.Y);
#endif

#if WINDOWS
            scoreLocation = new Vector2(ScreenHelper.Center.X, 30);
#endif
            scoreScale = 1f;

            spriteBatch = Manager.SpriteBatch;

            #endregion Screen

            //World
            World = new SpaceWorld(Manager.Game, ScreenHelper.SpriteSheet, this, tutorial);
            World.Initialize();
            World.LoadContent(content, playerIndices.ToArray(), level, states);

            Manager.Game.ResetElapsedTime();
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
            World.enemySpawnSystem.OnVictory += new Action(WinGame);
            BossTemplate.spawned = bossStart;
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
                if (World.enemySpawnSystem.CurrentDialog != null)
                    World.enemySpawnSystem.CurrentDialog.Update(gameTime);
                if (!World.Base.HasComponent<Health>())
                {
                    EndGame();
                }

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
            Manager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.Black, 0, 0);

            World.Draw(gameTime); //Draw the world.

            spriteBatch.Begin();
            if (tutorial && World.enemySpawnSystem.CurrentDialog != null)
                World.enemySpawnSystem.CurrentDialog.Draw(spriteBatch);
            if (!tutorial)
            {
                Vector2 scoreSize = gameFont.MeasureString(score.ToString()) * scoreScale;
                gameFont.DrawString(
                    spriteBatch,
                    new Vector2(
                        scoreLocation.X - scoreSize.X / 2,
                        scoreLocation.Y),
                    score.ToString(),
                    scoreScale);
            }
            spriteBatch.End();
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                Manager.FadeBackBufferToBlack(alpha);
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

            bool gamePadDisconnected = false;
            PlayerIndex disc;
            foreach (PlayerIndex index in playerIndices)
            {
                if (!GamePad.GetState(index).IsConnected)
                {
                    gamePadDisconnected = true;
                    disc = index;
                    removebackgroundscreens();
                    Manager.AddScreen(new ReconnectControllerScreen(), disc);
                }
            }

            PlayerIndex playerI;

            if (pauseAction.Evaluate(input, null, out playerI))
            {
                MusicManager.Pause();
                removebackgroundscreens();

                PlayerIndex? idx = playerI;
                if (gamePadDisconnected)
                    idx = null;

                Manager.AddScreen(new PauseMenuScreen(), idx);
            }

            mouseLoc = input.MouseLocation;
        }

        private void removebackgroundscreens()
        {
            List<GameScreen> remove = new List<GameScreen>();
            foreach (GameScreen screen in Manager.GetScreens())
            {
                if (screen is BackgroundScreen)
                    remove.Add(screen);
            }

            foreach (GameScreen screen in remove)
            {
                Manager.RemoveScreen(screen);
            }
        }

        #endregion Input

        #region Methods

        private void GameOver()
        {
            List<GameScreen> remove = new List<GameScreen>();
            foreach (GameScreen screen in Manager.GetScreens())
            {
                remove.Add(screen);
            }

            foreach (GameScreen screen in remove)
            {
                Manager.RemoveScreen(screen);
            }
            Manager.AddScreen(new GameOverScreen(SpaceWorld.Indices.ToArray(), score, victory), null);
            ExitScreen();
            MusicManager.Stop();
        }

        private void WinGame()
        {
            over = true;
            victory = true;
        }

        private void EndGame()
        {
            over = true;
        }

        #endregion Methods
    }
}