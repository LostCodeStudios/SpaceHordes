#region Using Statements

using System;

using Microsoft.Xna.Framework;

#endregion

namespace GameLibrary.Input
{
    /// <summary>
    /// Custom event argument which includes the index of the player who triggered
    /// the event.
    /// </summary>
    public class PlayerIndexEventArgs : EventArgs
    {
        PlayerIndex playerIndex;

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }
    }
}
