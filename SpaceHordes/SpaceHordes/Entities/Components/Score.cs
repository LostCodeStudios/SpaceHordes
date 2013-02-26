using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    public class Score : Component
    {
        public Score()
        {
            Value = 0;
        }

        long value;
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
