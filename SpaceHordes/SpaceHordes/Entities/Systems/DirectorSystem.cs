using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    public class DirectorSystem : IntervalEntitySystem
    {
        Random r = new Random();
        int difficulty = 0;
        TimeSpan elapsedTime;

        int playerDeaths;
        int baseDamage;

        public DirectorSystem()
            : base(500)
        {
        }

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);

            int ticks = 500;

            elapsedTime.Ticks += ticks;

            difficulty = 1500 - (d/2 + b)
        }

        public static float ClampInverse(float value, float min, float max)
        {
            if (value > min && value < max)
            {
                if (MathHelper.Distance(min, value) < MathHelper.Distance(max, value))
                    return min;
                else
                    return max;
            }
            else
                return value;
           
        }
    }
}
