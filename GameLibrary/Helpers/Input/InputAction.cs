using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
    /// <summary>
    /// Defines an action that is designated by some set of buttons and/or keys.
    ///
    /// The way actions work is you define a set of buttons and keys that trigger
    /// the action. You can then evaluate the action against and InputState which
    /// will test to see if any of the buttons or keys are pressed by the player.
    /// You can also set a flag that indicates if the action only occurs once when
    /// the buttons/keys are first pressed or whether the action should occur each
    /// frame.
    ///
    /// Using this InputAction class means that you can configure new actions based
    /// on keys without having to directly modify the InputState type. This means
    /// more customization without having to change the core classes of the Game
    /// Library.
    /// </summary>
    public class InputAction
    {
        #region Fields

        private readonly Buttons[] buttons;
        private readonly Keys[] keys;
        private readonly bool newPressOnly;

        // These delegate types map to the methods on InputState. We use these to
        // simplify the evaluate method by allowing us to map the appropriate
        // delegates and invoke them, rather than having two separate code paths.
        private delegate bool ButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex player);

        private delegate bool KeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex player);

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new InputAction.
        /// </summary>
        /// <param name="buttons">An array of buttons that can trigger the action.</param>
        /// <param name="keys">An array of keys that can trigger the action.</param>
        /// <param name="newPressOnly">Whether the action only occurs on the first
        /// press of one of the buttons/keys, or false if it occurs each frame one
        /// of the buttons/keys is down.</param>
        public InputAction(Buttons[] buttons, Keys[] keys, bool newPressOnly)
        {
            // Store the buttons and keys. If the arrays are null, we create
            // a 0 length array so we don't have to do null checks in the Evaluate
            // method.
            this.buttons = buttons != null ? buttons.Clone() as Buttons[] : new Buttons[0];
            this.keys = keys != null ? keys.Clone() as Keys[] : new Keys[0];

            this.newPressOnly = newPressOnly;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Evaluates the action against a given InputState.
        /// </summary>
        /// <param name="state">The InputState to test for the action.</param>
        /// <param name="controllingPlayer">The player to test, or null to allow any player.</param>
        /// <param name="player">If controllingPlayer is null, this is the player that performed the action.</param>
        /// <returns>True if the action occured, false otherwise.</returns>
        public bool Evaluate(InputState state, PlayerIndex? controllingPlayer, out PlayerIndex player)
        {
            // Figure out which delgate methods to map from the state which takes
            // care of our "newPressOnly" logic
            ButtonPress buttonTest;
            KeyPress keyTest;
            if (newPressOnly)
            {
                buttonTest = state.IsButtonPressed;
                keyTest = state.IsKeyPressed;
            }
            else
            {
                buttonTest = state.IsButtonDown;
                keyTest = state.IsKeyDown;
            }

            // Now we simply need to invoke the appropriate methods for each button
            // and key in our collections
            foreach (Buttons button in buttons)
            {
                if (buttonTest(button, controllingPlayer, out player))
                    return true;
            }
            foreach (Keys key in keys)
            {
                if (keyTest(key, controllingPlayer, out player))
                    return true;
            }

            // If we got here, the action is not matched
            player = PlayerIndex.One;
            return false;
        }

        #endregion Methods
    }
}