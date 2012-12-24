using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
    /// <summary>
    /// Class containing both the Current and previous state of both a gamepad
    /// and the keyboard. Also contains methods for accessing information from
    /// the class.
    /// </summary>
    public class InputState
    {
        #region Fields
        
        #if WINDOWS || XBOX

        public const int MaxInputs = 4;

        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;
        public MouseState CurrentMouseState;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;
        public MouseState LastMouseState;

        public readonly bool[] GamePadWasConnected;

        #endif

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            #if WINDOWS || XBOX

            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];

            #endif
        }

        #endregion

        #region Update

        /// <summary>
        /// Reads the latest state user input.
        /// </summary>
        public void Update()
        {
            #if WINDOWS || XBOX

            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];
                LastMouseState = CurrentMouseState;

                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);
                CurrentMouseState = Mouse.GetState();

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }

            #endif
        }

        #endregion

        #if WINDOWS || XBOX

        #region Keyboard

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

        #endregion

        #region Gamepad

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

        #region Mouse Location Methods

        /// <summary>
        /// Returns the Current location of the mouse.
        /// </summary>
        public Vector2 MouseLocation
        {
            get { return new Vector2(CurrentMouseState.X, CurrentMouseState.Y); }
        }

        /// <summary>
        /// Returns the Last location of the mouse.
        /// </summary>
        public Vector2 LastMouseLocation
        {
            get { return new Vector2(LastMouseState.X, LastMouseState.Y); }
        }

        /// <summary>
        /// Checks if the mouse is Currently hovering over the specified rectangle.
        /// </summary>
        public bool MouseHoverIn(Rectangle rectangle)
        {
            return
                ((MouseLocation.X > rectangle.Left) &&
                (MouseLocation.X < rectangle.Right) &&
                (MouseLocation.Y > rectangle.Top) &&
                (MouseLocation.Y < rectangle.Bottom));
        }

        /// <summary>
        /// Checks if the mouse was previously hovering over the specified rectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool DidMouseHoverIn(Rectangle rectangle)
        {
            return
                ((LastMouseLocation.X > rectangle.Left) &&
                (LastMouseLocation.X < rectangle.Right) &&
                (LastMouseLocation.Y > rectangle.Top) &&
                (LastMouseLocation.Y < rectangle.Bottom));
        }

        #endregion

        #region Left Mouse Button

        /// <summary>
        /// Checks if the left mouse button is Currently pressed.
        /// </summary>
        public bool LeftButtonDown()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Checks if the left mouse button is Currently released.
        /// </summary>
        public bool LeftButtonUp()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Checks if the left mouse button was previously pressed.
        /// </summary>
        public bool WasLeftButtonDown()
        {
            return (LastMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Checks if the left mouse button was previously released.
        /// </summary>
        /// <returns></returns>
        public bool WasLeftButtonUp()
        {
            return (LastMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Checks if the left mouse button was newly pressed.
        /// </summary>
        public bool LeftClicked()
        {
            return (LeftButtonDown() && WasLeftButtonUp());
        }

        /// <summary>
        /// Checks if the left mouse button was newly released.
        /// </summary>
        public bool LeftClickReleased()
        {
            return (LeftButtonUp() && WasLeftButtonDown());
        }

        /// <summary>
        /// Checks if the user left clicked on the specified rectangle.
        /// </summary>
        public bool LeftClickIn(Rectangle rectangle)
        {
            return (MouseHoverIn(rectangle) && LeftClicked());
        }

        /// <summary>
        /// Checks if the left mouse button is pressed while hovering over the specified rectangle.
        /// </summary>
        public bool LeftButtonDownIn(Rectangle rectangle)
        {
            return (MouseHoverIn(rectangle) && LeftButtonDown());
        }

        #endregion

        #region Right Mouse Button

        /// <summary>
        /// Checks if the right mouse button is Currently pressed.
        /// </summary>
        public bool RightButtonDown()
        {
            return (CurrentMouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Checks if the right mouse button is Currently released.
        /// </summary>
        public bool RightButtonUp()
        {
            return (CurrentMouseState.RightButton == ButtonState.Released);
        }

        /// <summary>
        /// Checks if the right mouse button was previously pressed.
        /// </summary>
        public bool WasRightButtonDown()
        {
            return (LastMouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Checks if the right mouse button was previously released.
        /// </summary>
        /// <returns></returns>
        public bool WasRightButtonUp()
        {
            return (LastMouseState.RightButton == ButtonState.Released);
        }

        /// <summary>
        /// Checks if the right mouse button was newly pressed.
        /// </summary>
        public bool RightClicked()
        {
            return (RightButtonDown() && WasRightButtonUp());
        }

        /// <summary>
        /// Checks if the right mouse button was newly released.
        /// </summary>
        public bool RightClickReleased()
        {
            return (RightButtonUp() && WasRightButtonDown());
        }

        /// <summary>
        /// Checks if the user right clicked on the specified rectangle.
        /// </summary>
        public bool RightClickIn(Rectangle rectangle)
        {
            return (MouseHoverIn(rectangle) && RightClicked());
        }

        #endregion

        #region Scroll Wheel Properties

        /// <summary>
        /// Returns the Current value of the scroll wheel.
        /// </summary>
        public int ScrollWheelValue
        {
            get { return CurrentMouseState.ScrollWheelValue; }
        }

        /// <summary>
        /// Returns the Last value of the scroll wheel.
        /// </summary>
        public int LastScrollWheelValue
        {
            get { return LastMouseState.ScrollWheelValue; }
        }

        /// <summary>
        /// Returns the scroll wheel's relative value for the Last frame.
        /// </summary>
        public int RelativeScrollValue
        {
            get { return ScrollWheelValue - LastScrollWheelValue; }
        }

        #endregion

        #endif
    }
}
