#region Using Statements

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Game_Library.Input
{
    /// <summary>
    /// Class containing both the current and previous state of both a gamepad
    /// and the keyboard. Also contains methods for accessing information from
    /// the class.
    /// </summary>
    public class InputState
    {
        #region Fields

        public const int MaxInputs = 4;

        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the latest state user input.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }
        }

        /// <summary>
        /// Helper for checking if a key was down during this update.
        /// </summary>
        /// <param name="key">The key to be checked.</param>
        /// <param name="controllingPlayer">Specifies which player to read input for. If null, input will be accepted from any player.</param>
        /// <param name="playerIndex">Reports which player pressed the key.</param>
        /// <returns>True if key is down, false otherwise.</returns>
        public bool IsKeyDown(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentKeyboardStates[i].IsKeyDown(key);
            }
            else
            {
                // Accept input from any player.
                return (IsKeyDown(key, PlayerIndex.One, out playerIndex) ||
                    IsKeyDown(key, PlayerIndex.Two, out playerIndex) ||
                    IsKeyDown(key, PlayerIndex.Three, out playerIndex) ||
                    IsKeyDown(key, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Helper for checking if a button was pressed during this update.
        /// </summary>
        /// <param name="button">The button to be checked.</param>
        /// <param name="controllingPlayer">Specifies which player to read input for. If null, input will be accepted from any player.</param>
        /// <param name="playerIndex">Reports which player pressed the button.</param>
        /// <returns>True if button is down, false otherwise.</returns>
        public bool IsButtonDown(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentGamePadStates[i].IsButtonDown(button);
            }
            else
            {
                //Accept input from any player.
                return (IsButtonDown(button, PlayerIndex.One, out playerIndex) ||
                    IsButtonDown(button, PlayerIndex.Two, out playerIndex) ||
                    IsButtonDown(button, PlayerIndex.Three, out playerIndex) ||
                    IsButtonDown(button, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update.
        /// </summary>
        /// <param name="key">The key to be checked.</param>
        /// <param name="controllingPlayer">Specefies which player to read input for. If null, input will be accepted from any player.</param>
        /// <param name="playerIndex">Reports which player pressed the key.</param>
        /// <returns>True if key was newly pressed, false otherwise.</returns>
        public bool IsKeyPressed(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                    LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsKeyPressed(key, PlayerIndex.One, out playerIndex) ||
                    IsKeyPressed(key, PlayerIndex.Two, out playerIndex) ||
                    IsKeyPressed(key, PlayerIndex.Three, out playerIndex) ||
                    IsKeyPressed(key, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during the update.
        /// </summary>
        /// <param name="button">The button to be checked.</param>
        /// <param name="controllingPlayer">Specefies which player to read input for. If null, input will be accepted from any player.</param>
        /// <param name="playerIndex">Reports which player pressed the key.</param>
        /// <returns>True if the button was newly pressed, false otherwise.</returns>
        public bool IsButtonPressed(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                    LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsButtonPressed(button, PlayerIndex.One, out playerIndex) ||
                    IsButtonPressed(button, PlayerIndex.Two, out playerIndex) ||
                    IsButtonPressed(button, PlayerIndex.Three, out playerIndex) ||
                    IsButtonPressed(button, PlayerIndex.Four, out playerIndex));
            }
        }

        #endregion
    }
}
