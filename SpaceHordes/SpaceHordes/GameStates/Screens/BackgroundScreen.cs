using GameLibrary.GameStates;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceHordes.GameStates.Screens
{
    public enum TransitionType
    {
        Slide,
        Fade
    }

    public class BackgroundScreen : GameScreen
    {
        #region Fields

        private ContentManager content;
        private Texture2D backgroundTexture;
        private string filename;
        private TransitionType transitionType;

        #endregion Fields

        #region Initialization

        /// <summary>
        /// Constructs a new background screen.
        /// </summary>
        public BackgroundScreen(string filename, TransitionType trans)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.filename = filename;
            transitionType = trans;
        }

        /// <summary>
        /// Loads graphics content for this screen.
        /// </summary>
        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(Manager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>(filename);
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }

        #endregion Initialization

        #region Update & Draw

        /// <summary>
        /// Updates the background screen.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            //pass in false so the background won't transition off even when covered
        }

        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Manager.SpriteBatch;
            Viewport viewport = Manager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (transitionType == TransitionType.Fade)
            {
                spriteBatch.Draw(backgroundTexture, fullscreen,
                    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }

            else if (transitionType == TransitionType.Slide)
            {
                float transitionOffset = TransitionPosition;

                if (ScreenState == ScreenState.TransitionOff)
                {
                    fullscreen.X -= (int)(transitionOffset * ScreenHelper.Viewport.Width);
                }
                else if (ScreenState == ScreenState.TransitionOn)
                {
                    fullscreen.X += (int)(transitionOffset * ScreenHelper.Viewport.Width);
                }
                spriteBatch.Draw(backgroundTexture, fullscreen,
                    Color.White);
            }

            spriteBatch.End();
        }

        #endregion Update & Draw
    }
}