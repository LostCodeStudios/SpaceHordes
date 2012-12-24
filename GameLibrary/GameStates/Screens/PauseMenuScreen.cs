using Microsoft.Xna.Framework;

using GameLibrary;
using GameLibrary.Input;

namespace GameLibrary.GameStates.Screens
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        public PauseMenuScreen()
            : base("Paused")
        {
            //Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            //Hook up menu eventhandlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            //Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BackgroundScreen background = new BackgroundScreen("Textures/hiscore");
            MainMenuScreen mainMenu = new MainMenuScreen("Space Hordes");

            ScreenManager.AddScreen(background, ControllingPlayer);
            ScreenManager.AddScreen(mainMenu, ControllingPlayer);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null,
                new BackgroundScreen("Textures/Hiscore"), new MainMenuScreen("Space Hordes"));
        }

        #endregion
    }
}
