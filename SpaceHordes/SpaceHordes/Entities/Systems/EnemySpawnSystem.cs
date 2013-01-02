using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    public class EnemySpawnSystem : IntervalEntitySystem
    {
        Random r = new Random();
        public EnemySpawnSystem()
            : base(500)
        {
        }

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            Vector2 spawn = new Vector2(ClampInverse(r.Next(-5, 5), -4, 4), ClampInverse(r.Next(-5, 5), -4, 4));

            world.CreateEntity("Enemy", spawn).Refresh(); //TODO SPAWN LOCATIONS
            base.ProcessEntities(entities);
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
