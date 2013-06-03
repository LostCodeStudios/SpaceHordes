using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameStates.Screens;
using GameLibrary.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace SpaceHordes.GameStates.Screens
{
    public class StartScreen : MenuScreen
    {
        public StartScreen()
            : this(false)
        {
        }

        public StartScreen(bool theyMessedUp) :
            base(theyMessedUp ? "Sign In" : "")
        {
            menuCancel = new InputAction(new Buttons[] { }, new Keys[] { }, true);

            MenuEntry enter = new MenuEntry("Press A");

            enter.Selected += entry;

            MenuEntries.Add(enter);
        }

        void entry(object sender, PlayerIndexEventArgs e)
        {
#if XBOX
            SpaceHordes.needStorageDevice = true;
            SpaceHordes.controllingIndex = e.PlayerIndex;
#endif
            ExitScreen();
#if WINDOWS
            Manager.AddScreen(new MainMenuScreen("Space Hordes"), null);
#endif
        }
    }
}
