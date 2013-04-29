using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    public class Damage : Component
    {
        #region Fields

        public double DamagePerSecond;
        public float Seconds;

        #endregion Fields

        #region Constructor

        public Damage(double dps, float sec)
        {
            DamagePerSecond = dps;
            Seconds = sec;
        }

        #endregion Constructor
    }
}