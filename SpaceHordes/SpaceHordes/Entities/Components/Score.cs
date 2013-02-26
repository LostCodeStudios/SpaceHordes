using GameLibrary.Dependencies.Entities;
using System;

namespace SpaceHordes.Entities.Components
{
    public class Score : Component
    {
        public Score()
        {
            Value = 0;
        }

        private long value;

        public long Value
        {
            get { return value; }
            set
            {
                this.value = Math.Max(0, value);
                if (OnChange != null)
                    OnChange();
            }
        }

        public event Action OnChange;
    }
}