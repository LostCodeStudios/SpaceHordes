using GameLibrary.Dependencies.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// The animation type enum.
    /// </summary>
    public enum AnimationType
    {
        None, //Keeps the sprite at its current sprite frame.
        Loop, //Loops the the sprite incrementally
        ReverseLoop, //Loops the sprite decrementally
        Increment, //Increments the sprites frame once and then sets its animation state to none.
        Decrement //Decrements the sprites frame once and then sets its animation state to none.
    }

    public class Animation : Component
    {
        /// <summary>
        /// Default constructor for animation component
        /// </summary>
        /// <param name="rate">The rate at which the animation updates (miliseconds).</param>
        public Animation( AnimationType type, int rate = 33)
        {
            FrameRate = rate;
            Type = type;
            _Tick = 0;
        }

        #region Properties

        /// <summary>
        /// The frame rate in miliseconds of the animation
        /// </summary>
        public int FrameRate { set; get; }

        /// <summary>
        /// The type of animation. (See AnimationType)
        /// </summary>
        public AnimationType Type { set; get; }

        #endregion

        #region Fields

        internal int _Tick;

        #endregion
    }
}
