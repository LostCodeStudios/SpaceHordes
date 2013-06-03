using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameStates.Screens;
using Microsoft.Xna.Framework;
using GameLibrary.Input;

namespace SpaceHordes.GameStates.Screens
{
    public class ReconnectControllerScreen : MenuScreen
    {
        MenuEntry okayEntry;

        public ReconnectControllerScreen()
            : base("")
        {
            okayEntry = new MenuEntry("Done");

            okayEntry.Selected += select;

            MenuEntries.Add(okayEntry);
        }

        public override void Activate()
        {
            base.Activate();

            okayEntry.Text = "Reconnect Player " + ControllingPlayer.ToString();
        }

        void select(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }
    }
}
