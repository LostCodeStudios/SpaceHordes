using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Game_Library.Input;

namespace Game_Library.Gameplay
{
    public enum ShotType
    {
        Red,
        Green,
        Blue
    }

    class Player
    {
        #region Fields

        PlayerIndex controllingPlayer;
        const int statBoxOffset = 100;

        InputAction red = new InputAction(
            new Buttons[] { Buttons.B },
            new Keys[] { Keys.NumPad3 },
            true);

        InputAction green = new InputAction(
            new Buttons[] { Buttons.A },
            new Keys[] { Keys.NumPad1 },
            true);

        InputAction blue = new InputAction(
            new Buttons[] { Buttons.X },
            new Keys[] { Keys.NumPad2 },
            true);

        InputAction build = new InputAction(
            new Buttons[] { Buttons.Y },
            new Keys[] { Keys.LeftShift },
            true);

        ShotType shotType = ShotType.Green;
        bool buildMode;

        #endregion

        #region Update & Draw

        #endregion

        #region Input

        private void HandleInput(InputState input)
        {
            PlayerIndex index;

            #region Actions

            //Toggle build mode
            if (build.Evaluate(input, controllingPlayer, out index))
                buildMode = !buildMode;

            //Handle input within build mode
            if (buildMode)
            {
                if (green.Evaluate(input, controllingPlayer, out index))
                {
                    //Build a turret
                }

                if (blue.Evaluate(input, controllingPlayer, out index))
                {
                    //Build a barrier
                }

                if (red.Evaluate(input, controllingPlayer, out index))
                {
                    //Build a mine
                }
            }

            //Handle input for non build mode.
            else
            {
                if (green.Evaluate(input, controllingPlayer, out index))
                {
                    shotType = ShotType.Green;
                }

                if (blue.Evaluate(input, controllingPlayer, out index))
                {
                    shotType = ShotType.Blue;
                }

                if (red.Evaluate(input, controllingPlayer, out index))
                {
                    shotType = ShotType.Red;
                }
            }

            #endregion

            
        }

        #endregion
    }
}
