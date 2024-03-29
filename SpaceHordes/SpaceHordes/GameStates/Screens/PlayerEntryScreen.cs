﻿using GameLibrary.GameStates;
using GameLibrary.Helpers;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceHordes.GameStates.Screens
{
    public class PlayerEntryScreen : GameScreen
    {
        private InputAction entryAction;
        private InputAction cancelAction;
        private string font;

        private Vector2[] offsets = new Vector2[4];
        private Vector2[] corners = new Vector2[4];

        private bool[] entered = new bool[4];
        private List<PlayerIndex> indices = new List<PlayerIndex>();

        private string before = "Press A to join";

        public PlayerEntryScreen(string fontName)
        {
            font = fontName;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            entryAction = new InputAction(
                new Buttons[] { Buttons.A },
                new Keys[] { },
                true);

            cancelAction = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);

            offsets[0] = new Vector2(ScreenHelper.Viewport.Width / 3, ScreenHelper.Viewport.Height / 4);
            offsets[1] = new Vector2(2 * ScreenHelper.Viewport.Width / 3, ScreenHelper.Viewport.Height / 4);
            offsets[2] = new Vector2(ScreenHelper.Viewport.Width / 3, 3 * ScreenHelper.Viewport.Height / 4);
            offsets[3] = new Vector2(2 * ScreenHelper.Viewport.Width / 3, 3 * ScreenHelper.Viewport.Height / 4);

            corners[0] = new Vector2(-ScreenHelper.Viewport.Width / 20, -ScreenHelper.Viewport.Height / 20);
            corners[1] = new Vector2(21 * ScreenHelper.Viewport.Width / 20, -ScreenHelper.Viewport.Height / 20);
            corners[2] = new Vector2(-ScreenHelper.Viewport.Width / 20, 21 * ScreenHelper.Viewport.Height / 20);
            corners[3] = new Vector2(21 * ScreenHelper.Viewport.Width / 20, 21 * ScreenHelper.Viewport.Height / 20);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            PlayerIndex idx;
            if (cancelAction.Evaluate(input, null, out idx))
            {
                SoundManager.Play("MenuCancel");
                if (entered[(int)idx])
                {
                    indices.Remove(idx);
                    entered[(int)idx] = false;
                }
                else
                {
                    ExitScreen();
                    Manager.AddScreen(new MainMenuScreen("Space Hordes", false), null);
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Enter();
                return;
            }

            for (int x = 0; x < 4; ++x)
            {
                PlayerIndex indexx;
                idx = (PlayerIndex)x;
                if (entryAction.Evaluate(input, idx, out indexx))
                {
                    SoundManager.Play("Selection");
                    if (entered[x])
                    {
                        Enter();
                        return;
                    }
                    entered[x] = true;
                    indices.Add(idx);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch fucker = Manager.SpriteBatch;

            fucker.Begin();

            for (int i = 0; i < 4; ++i)
            {
                float trans = (float)Math.Pow(TransitionPosition, 2);//TransitionPosition;
                Vector2 pos;

                pos = offsets[i] + ((corners[i] - offsets[i]) * trans);

                string text;
                Color color;

                if (entered[i])
                {
                    SignedInGamer gamer = Gamer.SignedInGamers[(PlayerIndex)i];
                    if (gamer != null)
                        text = gamer.Gamertag;
                    else
                        text = "Player " + (i + 1).ToString();
                    color = Color.Yellow;
                }
                else
                {
                    text = before;
                    color = Color.White;
                }

                pos -= Manager.Font.MeasureString(text) / 2;

                fucker.DrawString(Manager.Font, text, pos, color);
            }

            fucker.End();
        }

        private void Enter()
        {
            LevelSelectScreen levels = new LevelSelectScreen(font, indices.ToArray());
            ExitScreen();
            Manager.AddScreen(levels, null);
        }
    }
}