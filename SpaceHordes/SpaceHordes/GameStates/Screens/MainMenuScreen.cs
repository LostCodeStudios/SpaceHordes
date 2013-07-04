using GameLibrary.GameStates.Screens;
using GameLibrary.Helpers;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceHordes.GameStates.Screens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        #region Initialization

        public MainMenuScreen(string text)
            : this(text, false)
        {
            menuCancel = new InputAction(new Buttons[] { }, new Keys[] { }, true);
        }

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen(string text, bool music)
            : base(text)
        {
            //Create our menu entries.
            if (music)
                MusicManager.PlaySong("Title");

            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");

            MenuEntry highScoresMenuEntry = new MenuEntry("High Scores");
            MenuEntry bossEntry = new MenuEntry("Bosses");

            MenuEntry howtospacehordes = new MenuEntry("How To Play");

            MenuEntry introEntry = new MenuEntry("Intro");

            MenuEntry creditsEntry = new MenuEntry("Credits");

            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            //Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;

            highScoresMenuEntry.Selected += HighScoresMenuEntrySelected;
            bossEntry.Selected += BossMenuEntrySelected;
            howtospacehordes.Selected += HowToSelected;
            introEntry.Selected += IntroMenuEntrySelected;
            creditsEntry.Selected += CreditEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            //Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(highScoresMenuEntry);

#if !DEMO
            MenuEntries.Add(bossEntry);
#endif
            MenuEntries.Add(howtospacehordes);
            MenuEntries.Add(introEntry);
            MenuEntries.Add(creditsEntry);

            //MenuEntries.Add(tutorialEntry);
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
            Manager.AddScreen(new PlayerEntryScreen("Textures/gamefont"), null);
        }

        private void HighScoresMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Manager.AddScreen(new HighScoreScreen(-1), e.PlayerIndex);
        }

        private static BackgroundScreen bossBackdrop;

        private void BossMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            bool storage = true;

#if XBOX
            storage = StorageHelper.CheckStorage();
#endif
            if (storage)
            {
                bossBackdrop = new BackgroundScreen("Textures/GameMenu", TransitionType.Slide);
                Manager.AddScreen(bossBackdrop, e.PlayerIndex);
            }
            BossScreen bosses = new BossScreen(ScreenHelper.SpriteSheet);
            bosses.OnExit += new EventHandler(BossScreenExited);
            Manager.AddScreen(bosses, e.PlayerIndex);
        }

        public static void BossScreenExited(object sender, EventArgs e)
        {
            bossBackdrop.ExitScreen();
        }

        private void HowToSelected(object sender, PlayerIndexEventArgs e)
        {
            bossBackdrop = new ExitableBackgroundScreen("Textures/howtoplay", TransitionType.Slide);
            Manager.AddScreen(bossBackdrop, e.PlayerIndex);
        }

        private void IntroMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            bossBackdrop = new BackgroundScreen("Textures/GameMenu", TransitionType.Slide);
            Manager.AddScreen(bossBackdrop, e.PlayerIndex);
            IntroScreen intro = new IntroScreen();
            intro.OnExit += new EventHandler(BossScreenExited);
            Manager.AddScreen(intro, e.PlayerIndex);
        }

        private void CreditEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Manager.AddScreen(new CreditsScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Manager.AddScreen(new OptionsMenuScreen(false), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
#if XBOX
            StorageHelper.DisposeContainer(); 
#endif

            Manager.Game.Exit();
        }

        #endregion Events
    }
}