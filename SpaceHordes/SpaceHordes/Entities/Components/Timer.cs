using GameLibrary.Dependencies.Entities;
using System;

namespace SpaceHordes.Entities.Components
{
    public class Timer : Component
    {
        private TimeSpan time;

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