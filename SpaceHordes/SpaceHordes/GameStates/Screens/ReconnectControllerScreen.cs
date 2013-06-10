using GameLibrary.GameStates.Screens;
using GameLibrary.Input;

namespace SpaceHordes.GameStates.Screens
{
    public class ReconnectControllerScreen : MenuScreen
    {
        private MenuEntry okayEntry;

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

        private void select(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
        }
    }
}