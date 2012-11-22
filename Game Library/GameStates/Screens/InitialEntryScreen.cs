using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Library.GameStates.Screens
{
    class InitialEntryScreen : GameScreen
    {
        #region Fields

        Vector2 screenOffset;

        #endregion

        #region Properties



        #endregion

        #region Initialization

        public InitialEntryScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            IsPopup = true;
        }

        #endregion

        #region Update & Draw

        #endregion
    }
}
