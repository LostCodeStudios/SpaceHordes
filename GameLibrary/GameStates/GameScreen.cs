using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using GameLibrary.Input;

namespace GameLibrary.GameStates
{
    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    /// <summary>
    /// A single layer that has update and draw logic, and which can
    /// be combined with other layers to build a complex menu system.
    /// </summary>
    public abstract class GameScreen
    {
        #region Fields

        bool isPopup = false;
        TimeSpan transitionOnTime = TimeSpan.Zero;
        TimeSpan transitionOffTime = TimeSpan.Zero;
        float transitionPosition = 1;
        ScreenState screenState = ScreenState.TransitionOn;
        bool isExiting = false;
        bool otherScreenHasFocus;
        ScreenManager screenManager;
        PlayerIndex? controllingPlayer;
        bool isSerializable = true;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether lower screens need to be hidden.
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        /// <summary>
        /// How long the screen takes to transition on when activated.
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        /// <summary>
        /// How long the screen takes to transition off when deactivated.
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        /// <summary>
        /// The current position of the screen transition, ranging 
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        /// <summary>
        /// Gets the current alpha of the screen position, ranging from
        /// 1 (fully active, no transition) to 0 (transitioned fully
        /// off to nothing).
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        /// <summary>
        /// Gets the current transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        /// <summary>
        /// Indicates whether the screen is going away temporarily or
        /// for good. If true, the screen will automatically remove
        /// itself as soon as the transition finishes.
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        /// <summary>
        /// Checks whether the screen is active and can respond to user
        /// input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus && //If no other screens have focus
                    (screenState == ScreenState.TransitionOn ||
                    screenState == ScreenState.Active); //And if it is either active or becoming active
            }
        }

        /// <summary>
        /// Gets the manager that the screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        /// <summary>
        /// The index of the player currently controlling this screen.
        /// If null, input will be accepted from any index.
        /// </summary>
        public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        /// <summary>
        /// Whether or not this screen is serializable. If false,
        /// will be ignored during serialization. Assumed to be true.
        /// </summary>
        public bool IsSerializable
        {
            get { return isSerializable; }
            protected set { isSerializable = value; }
        }

        #endregion

        #region Methods

        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            //How much to move by?
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            // Update the transition position
            transitionPosition += transitionDelta * direction;

            //Did we reach the end of the transitipon?
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            //Otherwise we are still transitioning
            return true;
        }

        /// <summary>
        /// Tells the screen to go away, giving it a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                //If the screen's transition time is zero, remove immediately
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                //Otherwise flag that it should transition off then exit.
                isExiting = true;
            }
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Activates the screen. Called when the screen is added to
        /// the screen manager or if the game resumes from being paused.
        /// </summary>
        public virtual void Activate() { }

        /// <summary>
        /// Deactivates the screen. Called when the game is being
        /// deactivated due to pausing.
        /// </summary>
        public virtual void Deactivate() { }

        /// <summary>
        /// Unload content for the screen. Called when the screen is
        /// removed from the screen manager.
        /// </summary>
        public virtual void Unload() { }

        /// <summary>
        /// Allows the screen to handle user input. Only called when
        /// this screen is active.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="input"></param>
        public virtual void HandleInput(GameTime gameTime, InputState input) { }

        /// <summary>
        /// Allows the screen to run logic. Called regardless of the
        /// screen's state.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    //When the transition finishes, remove the screen.
                    ScreenManager.RemoveScreen(this);
                }
            }

            else if (coveredByOtherScreen)
            {
                //If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    //Still busy transitioning.
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    //Transition finished.
                    screenState = ScreenState.Hidden;
                }
            }

            else
            {
                //Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    //Still busy transitioning
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    //Transition finished!
                    screenState = ScreenState.Active;
                }
            }
        }

        /// <summary>
        /// Called when the screen should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime) { }

        #endregion
    }
}
