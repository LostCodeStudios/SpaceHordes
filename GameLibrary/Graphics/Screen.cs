#region Using Statements

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Game_Library.Graphics
{
    public static class Screen
    {
        #region Initialization

        /// <summary>
        /// Initializes the Screen class with the game's viewport.
        /// </summary>
        public static void Initialize(Viewport viewport)
        {
            Viewport = viewport;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The game's viewport.
        /// </summary>
        public static Viewport Viewport
        {
            get;
            set;
        }

        /// <summary>
        /// The center of the screen.
        /// </summary>
        public static Vector2 Center
        {
            get
            {
                return new Vector2(
                    Viewport.Width / 2,
                    Viewport.Height / 2);
            }
        }

        /// <summary>
        /// Returns the width and height of a third of the screen.
        /// </summary>
        public static Vector2 Third
        {
            get
            {
                return new Vector2(
                    Viewport.Width / 3,
                    Viewport.Height / 3);
            }
        }

        /// <summary>
        /// Returns the screen's title safe area.
        /// </summary>
        public static Rectangle TitleSafeArea
        {
            get
            {
                return Viewport.TitleSafeArea;
            }
        }

        #endregion
    }
}