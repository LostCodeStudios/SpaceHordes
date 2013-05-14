using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    public class Timer : Component
    {
        TimeSpan time;

        public Timer(double time)
        {
            this.time = TimeSpan.FromSeconds(time);
        }

        public bool Update(int ticks)
        {
            time -= TimeSpan.FromMilliseconds(ticks);

            if (time <= TimeSpan.Zero)
                return true;

            return false;
        }
    }
}
