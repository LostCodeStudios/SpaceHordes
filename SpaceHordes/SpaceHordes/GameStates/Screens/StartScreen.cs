using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameStates.Screens;
using GameLibrary.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework;

namespace SpaceHordes.GameStates.Screens
{
    public class StartScreen : MenuScreen
    {
        public StartScreen() :
            base("")
        {
            menuCancel = new InputAction(new Buttons[] { }, new Keys[] { }, true);

            MenuEntry enter = new MenuEntry("Press A");

            enter.Selected += entry;

            MenuEntries.Add(enter);
        }

        bool entered = false;
        PlayerIndex entryIndex;

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (entered)
            {
#if XBOX
                SignedInGamer gamer = Gamer.SignedInGamers[entryIndex];

                if ((gamer == null || gamer.IsGuest) && !Guide.IsVisible)
                {
                    try
                    {
                        Guide.ShowSignIn(4, false);
                    }
                    catch
                    {
                    }
                }

                else
                {
                    SpaceHordes.needStorageDevice = true;
                    SpaceHordes.controllingIndex = entryIndex;
                    ExitScreen();
                }
                return;
#endif
#if WINDOWS
                ExitScreen();
                Manager.AddScreen(new MainMenuScreen("Space Hordes"), null);
#endif
            }
        }

        void entry(object sender, PlayerIndexEventArgs e)
        {
            entered = true;
            entryIndex = e.PlayerIndex;
        }
    }
}
