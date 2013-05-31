using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpaceHordes.GameStates.Screens
{
    public class ExitableBackgroundScreen : BackgroundScreen
    {
        InputAction exit;

        public ExitableBackgroundScreen(string filename, TransitionType type)
            : base(filename, type)
        {
            exit = new InputAction(
                new Buttons[]
                {
                    Buttons.A,
                    Buttons.B,
                    Buttons.Back
                },
                new Keys[]
                {
                    Keys.Space,
                    Keys.Escape
                },
                true);
        }

        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            PlayerIndex indx;
            if (exit.Evaluate(input, ControllingPlayer, out indx))
            {
                ExitScreen();
            }
        }
    }
}
