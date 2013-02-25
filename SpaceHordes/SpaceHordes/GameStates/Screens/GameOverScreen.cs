﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GameLibrary.Input;
using GameLibrary.GameStates;
using GameLibrary.Helpers;

namespace SpaceHordes.GameStates.Screens
{
    public class GameOverScreen : GameScreen
    {
        #region Fields

        string text = "";
        string text2 = "";

        PlayerIndex[] players;
        long score;

        InitialEntryScreen[] initialEntryScreens = new InitialEntryScreen[4];

        Vector2 titleLocation;
        Vector2 subtitleLocation;
        Vector2[] screenLocations = new Vector2[4];

        SpriteFont titleFont;
        SpriteFont textFont;

        List<string> initials = new List<string>();

        bool awaitCancel = false;
        bool expired = false;

        InputAction cancel;

        #endregion

        #region Properties

        public List<string> Initials
        {
            get { return initials; }
        }

        #endregion

        #region Initialization

        public GameOverScreen(PlayerIndex[] players, long score)
        {
            this.score = score;
            this.players = players;
            this.IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            cancel = new InputAction(
                new Buttons[] { Buttons.Back, Buttons.A, Buttons.B },
                new Keys[] { Keys.Space, Keys.Escape, Keys.Tab },
                true);
        }

        public override void Activate()
        {
            base.Activate();

            text = "GAME OVER";
            if (HighScoreScreen.IsHighScore(players.Length, score))
            {
                text2 = "High score! Enter your initials.";
            }

            titleFont = ScreenManager.TitleFont;
            textFont = ScreenManager.Font;

            Rectangle viewport = new Rectangle(0, 0, ScreenHelper.Viewport.Width, ScreenHelper.Viewport.Height);

            titleLocation = new Vector2(
                viewport.Center.X, viewport.Height * 0.1736111111111111f);
            subtitleLocation = new Vector2(
                viewport.Center.X, titleLocation.Y + titleFont.MeasureString(text).Y);

            screenLocations[0] = new Vector2(
                viewport.Left + viewport.Width / 4, viewport.Top + viewport.Height / 4);
            screenLocations[1] = new Vector2(
                viewport.Right - viewport.Width / 4, viewport.Top + viewport.Height / 4);
            screenLocations[2] = new Vector2(
                viewport.Left + viewport.Width / 4, viewport.Bottom - viewport.Height / 4);
            screenLocations[3] = new Vector2(
                viewport.Right - viewport.Width / 4, viewport.Bottom - viewport.Height / 4);

            if (HighScoreScreen.IsHighScore(players.Length, score))
            {
                for (int i = 0; i < players.Length; i++)
                {
                    initialEntryScreens[i] = new InitialEntryScreen(screenLocations[i], this);
                    initialEntryScreens[i].ScreenManager = ScreenManager;
                    initialEntryScreens[i].Activate();
                }
            }
        }

        #endregion

        #region Update & Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!expired)
            {
                bool allExpired = true;
                bool allNull = true;

                foreach (InitialEntryScreen screen in initialEntryScreens)
                {
                    if (screen != null)
                    {
                        screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
                        allNull = false;

                        if (!screen.Expired)
                            allExpired = false;
                    }
                }

                if (allNull)
                {
                    awaitCancel = true;
                    allExpired = false;
                }

                if (allExpired)
                {
                    string names = "";

                    for (int i = 0; i < initials.Count; i++)
                    {
                        names += initials[i];
                        if (i < initials.Count - 1)
                            names += ", ";
                    }

                    //ScreenManager.AddScreen(new BackgroundScreen("Textures/Hiscore"), ControllingPlayer);
                    ScreenManager.AddScreen(new MainMenuScreen("Space Hordes"), ControllingPlayer);
                    ScreenManager.AddScreen(new HighScoreScreen(HighScoreScreen.AddScore(players.Length, names, score)), ControllingPlayer);
                    ExitScreen();
                    expired = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Color titleColor = new Color(100, 77, 45) * TransitionAlpha;

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 titlePosition = titleLocation;
            Vector2 titleOrigin = titleFont.MeasureString(text) / 2;
            titlePosition.Y -= transitionOffset * 100;

            Vector2 subtitlePosition = subtitleLocation;
            Vector2 subtitleOrigin = textFont.MeasureString(text2) / 2;
            subtitlePosition.Y -= transitionOffset * 100;

            spriteBatch.Begin();

            spriteBatch.DrawString(titleFont, text, titlePosition, titleColor, 0,
                titleOrigin, 1f, SpriteEffects.None, 0);

            spriteBatch.DrawString(textFont, text2, subtitleLocation, Color.White,
                0, subtitleOrigin, 1f, SpriteEffects.None, 0);

            foreach (InitialEntryScreen screen in initialEntryScreens)
            {
                if (screen != null)
                    screen.Draw(gameTime);
            }

            spriteBatch.End();
        }

        #endregion

        #region Input

        public override void HandleInput(GameTime gameTime, GameLibrary.Input.InputState input)
        {
            base.HandleInput(gameTime, input);

            if (awaitCancel)
            {
                PlayerIndex index;

                if (cancel.Evaluate(input, ControllingPlayer, out index))
                {
                    ScreenManager.AddScreen(new MainMenuScreen("Space Hordes"), ControllingPlayer);
                    ExitScreen();
                }
            }

            foreach (InitialEntryScreen screen in initialEntryScreens)
            {
                if (screen != null)
                    screen.HandleInput(gameTime, input);
            }
        }

        #endregion
    }
}
