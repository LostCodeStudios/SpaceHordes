using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    public class Damage : Component
    {
        #region Fields

        public int Amount;
        public int Interval;
        public int Elapsed;
        public int Uses;

        #endregion Fields

        #region Constructor

        public Damage(int amount, int uses, int interval)
        {
            Amount = amount;
            Uses = uses;
            Interval = interval;
            Elapsed = 0;
        }

        #endregion Constructor
    }
}