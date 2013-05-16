using GameLibrary.GameStates.Screens;
using GameLibrary.Helpers;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using System;
using GameLibrary.GameStates;

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

            MenuEntry tutorialEntry = new MenuEntry("Tutorial");

            MenuEntry highScoresMenuEntry = new MenuEntry("High Scores");
            MenuEntry bossEntry = new MenuEntry("Bosses");

            MenuEntry introEntry = new MenuEntry("Intro");

            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            //Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;

            tutorialEntry.Selected += PlayTutorialEntrySelected;

            highScoresMenuEntry.Selected += HighScoresMenuEntrySelected;
            bossEntry.Selected += BossMenuEntrySelected;
            introEntry.Selected += IntroMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            //Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
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
            Manager.AddScreen(new PlayerEntryScreen("Textures/gamefont", false), null);
        }

        void PlayTutorialEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Manager.AddScreen(new PlayerEntryScreen("Textures/gamefont", true), null);
        }

        private void HighScoresMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Manager.AddScreen(new HighScoreScreen(), e.PlayerIndex);
        }

        private static BackgroundScreen bossBackdrop;

        private void BossMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            bossBackdrop = new BackgroundScreen("Textures/GameMenu", TransitionType.Slide);
            Manager.AddScreen(bossBackdrop, ControllingPlayer);
            BossScreen bosses = new BossScreen(ScreenHelper.SpriteSheet);
            bosses.OnExit += new EventHandler(BossScreenExited);
            Manager.AddScreen(bosses, e.PlayerIndex);
        }

        public static void BossScreenExited(object sender, EventArgs e)
        {
            bossBackdrop.ExitScreen();
        }

        private void IntroMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            bossBackdrop = new BackgroundScreen("Textures/GameMenu", TransitionType.Slide);
            Manager.AddScreen(bossBackdrop, ControllingPlayer);
            IntroScreen intro = new IntroScreen();
            intro.OnExit += new EventHandler(BossScreenExited);
            Manager.AddScreen(intro, e.PlayerIndex);
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
            Manager.Game.Exit();
        }

        #endregion Events
    }
}