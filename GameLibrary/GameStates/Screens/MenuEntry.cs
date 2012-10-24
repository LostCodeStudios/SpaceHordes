using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Game_Library.Input;

namespace Game_Library.GameStates.Screens
{
    /// <summary>
    /// A single entry in a Menu Screen.
    /// </summary>
    public class MenuEntry
    {
        #region Fields

        string text;
        float selectionFade;
        Vector2 position;

        #endregion

        #region Properties

        /// <summary>
        /// The text of this menu entry.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// The position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Returns the scale at which the menuEntry's text is drawn.
        /// </summary>
        public float Scale
        {
            get { return 1 + selectionFade; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when this menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;
    
        /// <summary>
        /// Method for raising the selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new menu entry with specified text.
        /// </summary>
        public MenuEntry(string text)
        {
            Text = text;
        }

        #endregion

        #region Update & Draw

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 3;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 0.5f);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        /// <summary>
        /// Draws the menu entry. Can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            //Draw the selected entry in yellow, and others in white.
            Color color = isSelected ? Color.Yellow : Color.White;

            //Pulsate the size of the selected menu entry.
            //double time = gameTime.TotalGameTime.TotalSeconds;

            //float pulsate = (float)Math.Sin(time * 6) + 1;

            //float scale = 1 + pulsate * 0.05f * selectionFade;

            ////Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            //Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0,
                origin, Scale, SpriteEffects.None, 0);

        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }

        #endregion
    }
}
