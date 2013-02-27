using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Components
{
    public class Slow : Component
    {
        #region SlowRate

        /// <summary>
        /// The meter/second quantity by which the linear velocity will slow.
        /// </summary>
        public float LinearSlowRate { set; get; }

        /// <summary>
        /// The radian/second quantity by which the angular velocity (omega) will slow.
        /// </summary>
        public float AngularSlowRate { set; get; }

        #endregion SlowRate

        #region Target

        /// <summary>
        /// The target linear velocity of a slow
        /// </summary>
        public Vector2 LinearTargetVelocity { set; get; }

        /// <summary>
        /// The target angular velocity of a slow
        /// </summary>
        public float AngularTargetVelocity { set; get; }

        #endregion Target

        public Slow(int slowTime, float angRate, float linearRate, Vector2 linearTarget, float angularTarget)
        {
            AngularSlowRate = angRate;
            LinearSlowRate = linearRate;
            LinearTargetVelocity = linearTarget;
            AngularTargetVelocity = angularTarget;
            this.Elapsed = slowTime;
        }

        public Slow()
            : this(0, 0f, 0f, Vector2.Zero, 0f)
        {
        }

        public int Elapsed = 0;

        public static readonly Slow None = new Slow();
    }
}