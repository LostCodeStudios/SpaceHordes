using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Helpers
{
    /// <summary>
    /// Screen helper class
    /// </summary>
    public static class ScreenHelper
    {
        #region Initialization

        /// <summary>
        /// Initializes the Screen class with the game's viewport.
        /// </summary>
        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        #endregion

        #region Properties

        public static GraphicsDevice GraphicsDevice
        {
            set;
            get;
        }

        public static KeyboardState input;
        /// <summary>
        /// The game's viewport.
        /// </summary>
        public static Viewport Viewport
        {
            get
            {
                return GraphicsDevice.Viewport;
            }
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

        #region Methods

        #region Helpers

        public static Color[,] TextureTo2DArray(Texture2D texture)
        {
             Color[] colors1D = new Color[texture.Width * texture.Height];
             texture.GetData(colors1D);

             Color[,] colors2D = new Color[texture.Width, texture.Height];
             for (int x = 0; x < texture.Width; x++)
                 for (int y = 0; y < texture.Height; y++)
                     colors2D[x, y] = colors1D[x + y * texture.Width];

             return colors2D;
        }

        /// <summary>
        /// Returns the proper coordinates for a string to be centered on the screen.
        /// </summary>
        public static Vector2 CenterString(string text, float scale, SpriteFont font)
        {
            return new Vector2(
                Center.X - ((font.MeasureString(text).X / 2) * scale),
                Center.Y - ((font.MeasureString(text).Y / 2)) * scale);
        }

        #endregion

        #endregion
    }
}
