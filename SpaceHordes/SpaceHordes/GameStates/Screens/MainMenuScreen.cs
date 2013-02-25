using Microsoft.Xna.Framework;

using GameLibrary.Input;
using GameLibrary.GameStates.Screens;
using GameLibrary.Helpers;

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
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
#endif

#if XBOX
            MenuEntry playGameMenuEntry = new MenuEntry("Play Solo");
            MenuEntry playMultiplayerMenuEntry = new MenuEntry("Multiplayer");
#endif

            MenuEntry highScoresMenuEntry = new MenuEntry("High Scores");
            MenuEntry bossEntry = new MenuEntry("Bosses");

            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            //Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
#if XBOX
            playMultiplayerMenuEntry.Selected += PlayMultiplayerMenuEntrySelected;
#endif

            highScoresMenuEntry.Selected += HighScoresMenuEntrySelected;
            bossEntry.Selected += BossMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            //Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
#if XBOX
            MenuEntries.Add(playMultiplayerMenuEntry);
#endif
            MenuEntries.Add(highScoresMenuEntry);
            MenuEntries.Add(bossEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GameplayScreen("Textures/gamefont", false));
        }

        void PlayMultiplayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GameplayScreen("Textures/gamefont", true));
        }

        void HighScoresMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HighScoreScreen(), e.PlayerIndex);
        }

        void BossMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new BossScreen("Textures/GameMenu", ScreenHelper.SpriteSheet), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
        }

        #endregion
    }
}
