using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Systems
{
    public class DirectorSystem : IntervalEntitySystem
    {
        Random r = new Random();

        Entity Base;

        int difficulty = 0;

        //Start off with a minute worth of time so spawns don't delay by a minute due to casting
        float elapsedSeconds = 60f;
        float elapsedMinutes = 1f;

        int playerDeaths = 0;
        int baseDamage = 0;

        public DirectorSystem()
            : base(500)
        {
        }

        public void LoadContent(Entity Base)
        {
            this.Base = Base;
        }

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);

            int milliseconds = 500;

            elapsedSeconds += milliseconds/1000;
            elapsedMinutes += milliseconds/60000;

            difficulty = (int)(elapsedMinutes) - ((int)(playerDeaths / 2) + baseDamage);

            //Spawn mooks per second equal to Difficulty
            if (elapsedSeconds % 1 == 0)
            {
                for (int i = 0; i < difficulty; i++)
                {
                    int type = r.Next(9);
                    World.CreateEntity("Mook", type, Base.GetComponent<Body>()).Refresh();
                }
            }
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
