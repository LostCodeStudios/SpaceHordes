using GameLibrary.GameStates;
using GameLibrary.Helpers;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameLibrary.GameStates.Screens;
namespace SpaceHordes.GameStates.Screens
{
    public class IntroScreen : GameScreen
    {
        string[] introText = new string[]
        {
            "SON... OUR RADAR IS PICKING UP MASSIVE SPACE HORDES.",
            "YOU MUST DEFEND THE SPACE STATION AT ALL COSTS!",
            "YOU ARE THE LAST HOPE FOR MANKIND!"
        };

        MessageDialog[] dialogs;
        int index = 0;

        Texture2D face;

        private ContentManager content;
        private InputAction cancel;

        public IntroScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            cancel = new InputAction(
                new Buttons[]
                {
                    Buttons.Back,
                    Buttons.B
                },
                new Keys[]
                {
                    Keys.Escape
                },
                true);
        }

        public override void Activate()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            ImageFont font = new ImageFont();
            font.LoadContent(content, "Textures/gamefont", 1f);
            font.SpaceWidth = 10;
            dialogs = new MessageDialog[introText.Length];

            face = content.Load<Texture2D>("Textures/Face1");

            float y = 2 * ScreenHelper.Viewport.Height / 3;
            for (int i = 0; i < introText.Length; ++i)
            {
                float x = ScreenHelper.Center.X - font.MeasureString(introText[i]).X/2;
                float height = font.MeasureString(introText[i]).Y + 10;
                dialogs[i] = new MessageDialog(font, new Vector2(x, y), introText[i], TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.5));
                y += height;
            }
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (TransitionAlpha == 1)
            {
                if (!dialogs[index].Enabled)
                    dialogs[index].Enabled = true;
                else if (dialogs[index].Complete() && index < dialogs.Length - 1)
                    dialogs[++index].Enabled = true;

                foreach (MessageDialog d in dialogs)
                {
                    d.Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            Vector2 faceDim = new Vector2(face.Width*3, face.Height*3);

            Vector2 faceOffs = Vector2.Zero;
            if (ScreenState == ScreenState.TransitionOn)
                faceOffs = new Vector2(ScreenHelper.Viewport.Width * (1 - TransitionAlpha), 0);
            else if (ScreenState == ScreenState.TransitionOff)
                faceOffs = new Vector2(-ScreenHelper.Viewport.Width * (1 - TransitionAlpha), 0);

            Rectangle location = new Rectangle((int)(ScreenHelper.Viewport.Width / 2 - faceDim.X / 2) + (int)faceOffs.X, (int)(ScreenHelper.Viewport.Height / 2.5 - faceDim.Y / 2) + (int)faceOffs.Y, (int)faceDim.X, (int)faceDim.Y);

            ScreenManager.SpriteBatch.Draw(face, location, Color.White);

            foreach (MessageDialog d in dialogs)
            {
                Vector2 transitionOffset = Vector2.Zero;
                if (ScreenState == ScreenState.TransitionOff)
                    transitionOffset = new Vector2(-ScreenHelper.Viewport.Width*(1-TransitionAlpha),0);

                d.Draw(ScreenManager.SpriteBatch, transitionOffset);
            }
            ScreenManager.SpriteBatch.End();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (ScreenState == ScreenState.TransitionOff || ScreenState == ScreenState.TransitionOn)
                return;

            PlayerIndex indx;

            if (cancel.Evaluate(input, ControllingPlayer, out indx))
            {
                ExitScreen();
                CallExit();
            }
        }
    }
}
