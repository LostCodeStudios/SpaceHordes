using GameLibrary.GameStates.Screens;
using GameLibrary.Helpers;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using System;

namespace SpaceHordes.GameStates.Screens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen(string text)
            : base(text)
        {
            //Create our menu entries.
#if WINDOWS
            MenuEntry playGameMenuEntry = new MenuEntry("Solo Defense");
#endif

#if XBOX
            MenuEntry playGameMenuEntry = new MenuEntry("Play Solo");
#endif
            MenuEntry playMultiplayerMenuEntry = new MenuEntry("Team Defense");

            MenuEntry tutorialEntry = new MenuEntry("Tutorial");

            MenuEntry highScoresMenuEntry = new MenuEntry("High Scores");
            MenuEntry bossEntry = new MenuEntry("Bosses");

            MenuEntry introEntry = new MenuEntry("Intro");

            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            //Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;

            //#if XBOX
            playMultiplayerMenuEntry.Selected += PlayMultiplayerMenuEntrySelected;
            
            //#endif

            tutorialEntry.Selected += PlayTutorialEntrySelected;

            highScoresMenuEntry.Selected += HighScoresMenuEntrySelected;
            bossEntry.Selected += BossMenuEntrySelected;
            introEntry.Selected += IntroMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            //Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
//#if (!WINDOWS && !DEMO) || DEBUG
            MenuEntries.Add(playMultiplayerMenuEntry);
//#endif
            MenuEntries.Add(highScoresMenuEntry);


#if !DEMO
            MenuEntries.Add(bossEntry);
#endif
            MenuEntries.Add(introEntry);
            MenuEntries.Add(tutorialEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            SelectionChangeSound = "SelectChanged";
            SelectionSound = "Selection";
            CancelSound = "MenuCancel";
        }

        #endregion Initialization

        #region Events

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GameplayScreen("Textures/gamefont", false));
        }

        private void PlayMultiplayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GameplayScreen("Textures/gamefont", true));
        }

        void PlayTutorialEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GameplayScreen("Textures/gamefont", false, true));
        }

        private void HighScoresMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HighScoreScreen(), e.PlayerIndex);
        }

        private static BackgroundScreen bossBackdrop;

        private void BossMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            bossBackdrop = new BackgroundScreen("Textures/GameMenu", TransitionType.Slide);
            ScreenManager.AddScreen(bossBackdrop, ControllingPlayer);
            BossScreen bosses = new BossScreen(ScreenHelper.SpriteSheet);
            bosses.OnExit += new EventHandler(BossScreenExited);
            ScreenManager.AddScreen(bosses, e.PlayerIndex);
        }

        public static void BossScreenExited(object sender, EventArgs e)
        {
            bossBackdrop.ExitScreen();
        }

        private void IntroMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            bossBackdrop = new BackgroundScreen("Textures/GameMenu", TransitionType.Slide);
            ScreenManager.AddScreen(bossBackdrop, ControllingPlayer);
            IntroScreen intro = new IntroScreen();
            intro.OnExit += new EventHandler(BossScreenExited);
            ScreenManager.AddScreen(intro, e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(false), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
        }

        #endregion Events
    }
}