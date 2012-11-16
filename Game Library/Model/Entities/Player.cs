using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Game_Library.Input;

namespace Game_Library.Model.Entities
{
    public enum ShotType
    {
        Red,
        Green,
        Blue
    }

    public class Player : Ship
    {
        #region Fields

        PlayerIndex controllingPlayer;
        const int statBoxOffset = 100;

        InputAction red = new InputAction(
            new Buttons[] { Buttons.B },
            new Keys[] { Microsoft.Xna.Framework.Input.Keys.NumPad3 },
            true);

        InputAction green = new InputAction(
            new Buttons[] { Buttons.A },
            new Keys[] { Microsoft.Xna.Framework.Input.Keys.NumPad1 },
            true);

        InputAction blue = new InputAction(
            new Buttons[] { Buttons.X },
            new Keys[] { Microsoft.Xna.Framework.Input.Keys.NumPad2 },
            true);

        InputAction build = new InputAction(
            new Buttons[] { Buttons.Y },
            new Keys[] { Microsoft.Xna.Framework.Input.Keys.LeftShift },
            true);

        #if WINDOWS

        //Windows movement keys

        InputAction moveUp = new InputAction(
            new Buttons[] { },
            new Keys[] { Microsoft.Xna.Framework.Input.Keys.W },
            false);

        InputAction moveLeft = new InputAction(
            new Buttons[] { },
            new Keys[] { Microsoft.Xna.Framework.Input.Keys.A },
            false);

        InputAction moveDown = new InputAction(
            new Buttons[] { },
            new Keys[] { Microsoft.Xna.Framework.Input.Keys.S },
            false);

        InputAction moveRight = new InputAction(
            new Buttons[] { },
            new Keys[] { Microsoft.Xna.Framework.Input.Keys.D },
            false);

        #endif

        Vector2 shotAngle;
        ShotType shotType = ShotType.Green;
        bool buildMode;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        /// <summary>
        /// Makes a new player.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="sprite"></param>
        public Player(Vector2 position, float rotation, Sprite sprite)
            : base(position, rotation, sprite)
        {
        }

        #endregion

        #region Update & Draw

        public override void Update(GameTime gameTime, InputState input)
        {
            HandleInput(input);

            base.Update(gameTime);
        }

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

            #region Movement

#if WINDOWS

            float x = 0;
            float y = 0;

            if (moveUp.Evaluate(input, controllingPlayer, out index))
            {
                y = -1;
            }

            if (moveLeft.Evaluate(input, controllingPlayer, out index))
            {
                x = -1;
            }

            if (moveDown.Evaluate(input, controllingPlayer, out index))
            {
                y = 1;
            }

            if (moveRight.Evaluate(input, controllingPlayer, out index))
            {
                x = 1;
            }

            Vector2 moveVelocity = new Vector2(x, y);
            
            if (moveVelocity != Vector2.Zero)
                moveVelocity.Normalize();

            Velocity = moveVelocity;

#endif

#if XBOX

            //Gamepad movement

            Vector2 moveVelocity = input.CurrentGamePadStates[(int)controllingPlayer].ThumbSticks.Left;

            if (moveVelocity != Vector2.Zero)
                moveVelocity.Normalize();

            Velocity = moveVelocity;
            
#endif

            #endregion

            #region Shooting

            shotAngle = Vector2.Zero;

            #if WINDOWS

            //Determine shot angle based on mouse
            shotAngle = input.MouseLocation - Position;

            #endif

            #if WINDOWS || XBOX

            //Determine shot angle based on right stick
            shotAngle = input.CurrentGamePadStates[(int)controllingPlayer].ThumbSticks.Right;

            #endif

            if (shotAngle != Vector2.Zero)
                shotAngle.Normalize();


            #if WINDOWS
            if (input.LeftButtonDown() || (input.GamePadWasConnected[(int)controllingPlayer] && shotAngle != Vector2.Zero))
                Shoot();
            #endif

            #if XBOX
            if (shotAngle != Vector2.Zero)
                Shoot();
            #endif

            #endregion
        }

        #endregion

        private void Shoot()
        {
            //Shoot
        }
    }
}
