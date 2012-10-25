using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Game_Library.Input;

namespace Game_Library.GameStates.Screens
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class MessageBoxScreen : GameScreen
    {
        #region Fields

        string message;
        Texture2D gradientTexture;
        string filename;

        MenuEntry accept;
        MenuEntry cancel;

        bool acceptSelected;
        bool cancelSelected;

        #if XBOX

        InputAction menuSelect;
        InputAction menuCancel;

        #endif

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor automatically inclued the standard A=ok, B=cancel usage text prompt.
        /// </summary>
        /// <param name="message"></param>
        public MessageBoxScreen(string message, string filename)
            : this(message, filename, true)
        {
        }

        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// A=OK, B=Cancel usage text prompt.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="includeUsageText"></param>
        public MessageBoxScreen(string message, string filename, bool includeUsageText)
        {
            const string usageText = "\nA button, Space, Enter: OK" +
                                     "\nB button, Esc: Cancel";

            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            this.filename = filename;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            accept = new MenuEntry("Accept");
            cancel = new MenuEntry("Cancel");
            #if XBOX

            menuSelect = new InputAction(
                new Buttons[] { Buttons.A, Buttons.Start },
                new Keys[] { Keys.Space, Keys.Enter },
                true);
            menuCancel = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape, Keys.Back },
                true);

            #endif
        }

        public override void Activate()
        {
            ContentManager content = ScreenManager.Game.Content;
            gradientTexture = content.Load<Texture2D>(filename);
        }
        
        #endregion

        #region Handle Input

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            #if WINDOWS

            if (input.LeftClickIn(accept.ClickRectangle))
            {
                
                if (Accepted != null)
                    Accepted(this, null);

                ExitScreen();
            }

            if (input.LeftClickIn(cancel.ClickRectangle))
            {
                if (Cancelled != null)
                    Cancelled(this, null);

                ExitScreen();
            }

            #endif

            #if XBOX

            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (menuSelect.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                //Raise the accepted event, then exit the message box.
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));

                ExitScreen();
            }
            else if (menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                //Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));

                ExitScreen();
            }

            #endif
        }

        #endregion

        #region Draw
        
        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            //Darken down any other screens that were drawn beneath popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            //Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            //The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle(
                (int)textPosition.X - hPad,
                (int)textPosition.Y - vPad,
                (int)textSize.X + hPad * 2,
                (int)textSize.Y + vPad * 2);

            //Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            //Draw the background rectangle
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);
            accept.Draw(this, acceptSelected, gameTime);
            cancel.Draw(this, cancelSelected, gameTime);

            //Draw the message box text.
            spriteBatch.DrawString(font, message, textPosition, color);

            spriteBatch.End();
        }

        #endregion
    }
}
