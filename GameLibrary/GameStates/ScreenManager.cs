using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameLibrary.GameStates
{
    /// <summary>
    /// A component which manages one or more GameScreen instances. It
    /// maintains a stack of screens, calls their Update and Draw methods
    /// at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        private const string StateFilename = "ScreenManagerState.xml";

#if XBOX
        StorageDevice storageDevice;
#endif

        private List<GameScreen> screens = new List<GameScreen>();
        private List<GameScreen> tempScreensList = new List<GameScreen>();

        private InputState input = new InputState();

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private SpriteFont titleFont;
        private SpriteFont initialEntryFont;

        private Texture2D blankTexture;

        private bool isInitialized;

        private bool traceEnabled;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Default SpriteBatch shared by all screens.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        /// <summary>
        /// Default font shared by all screens.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// Font used for larger titles.
        /// </summary>
        public SpriteFont TitleFont
        {
            get { return titleFont; }
        }

        public SpriteFont InitialEntryFont
        {
            get { return initialEntryFont; }
        }

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. Useful for making sure everything
        /// is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        /// <summary>
        /// Blank texture that can be used by all screens.
        /// </summary>
        public Texture2D BlankTexture
        {
            get { return blankTexture; }
        }

        /// <summary>
        /// Public reference of the main input state.
        /// </summary>
        public InputState Input
        {
            get { return input; }
        }

#if XBOX

        /// <summary>
        /// The game's storage device.
        /// </summary>
        public StorageDevice StorageDevice
        {
            get { return storageDevice; }
            set { storageDevice = value; }
        }
#endif

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        /// <param name="game"></param>
        public ScreenManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            //Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("Fonts/menufont");
            titleFont = content.Load<SpriteFont>("Fonts/titlefont");
            initialEntryFont = content.Load<SpriteFont>("Fonts/InitialEntryFont");

            blankTexture = content.Load<Texture2D>("Textures/blank");

            //Tell each of the screens to load their content.
            foreach (GameScreen screen in screens)
            {
                screen.Activate();
            }
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Tell each of the screens to unload their content.
            foreach (GameScreen screen in screens)
            {
                screen.Unload();
            }
        }

        #endregion Initialization

        #region Update and Draw

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //Read the keyboard and gamepad
            input.Update();

            //Make a copy of the master screen list, to avoid confusion
            //if the process of updating one screen adds or removes others
            tempScreensList.Clear();

            foreach (GameScreen screen in screens)
                tempScreensList.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            //Loop as long as there are screens waiting to be updated.
            while (tempScreensList.Count > 0)
            {
                //Pop the topmost screen off the waiting list.
                GameScreen screen = tempScreensList[tempScreensList.Count - 1];

                tempScreensList.RemoveAt(tempScreensList.Count - 1);

                //Update the screen
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    //If this is the first active screen we came across,
                    //give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(gameTime, input);

                        otherScreenHasFocus = true;
                    }

                    //If this is an active non-popup, inform any subsequent
                    //screens that they are covered by it..
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }

                //Print debug trace?
                if (traceEnabled)
                    TraceScreens();
            }
        }

        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        private void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        #endregion Update and Draw

        #region Public Methods

        /// <summary>
        /// Adds a new screen to the screen manager
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="controllingPlayer"></param>
        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            //If we have a graphics device, tell the screen to load content.
            if (isInitialized)
            {
                screen.Activate();
            }

            screens.Add(screen);
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen() instead of calling this directly,
        /// so the screen can gradually transition off rather than being
        /// instantly removed.
        /// </summary>
        /// <param name="screen"></param>
        public void RemoveScreen(GameScreen screen)
        {
            //If we have a graphics device, tell the screen to unload content.
            if (isInitialized)
            {
                screen.Unload();
            }

            screens.Remove(screen);
            tempScreensList.Remove(screen);
        }

        /// <summary>
        /// Expose an array holding all the screens. We return a copy
        /// rather than the real master list, because the screens
        /// should only ever be added or removed using the AddScreen
        /// and RemoveScreen methods.
        /// </summary>
        /// <returns></returns>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for
        /// fading screens in and out, and for darkening the background
        /// behind popups.
        /// </summary>
        /// <param name="alpha"></param>
        public void FadeBackBufferToBlack(float alpha)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            spriteBatch.End();
        }

        #endregion Public Methods
    }
}