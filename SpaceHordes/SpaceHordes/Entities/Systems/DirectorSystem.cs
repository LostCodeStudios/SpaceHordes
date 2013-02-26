using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Systems
{
    public class DirectorSystem : IntervalEntitySystem
    {
        private Random r = new Random();

        private Entity Base;

        private int difficulty = 0;

        //Start off with a minute worth of time so spawns don't delay by a minute due to casting
        private float elapsedSeconds;

        private float elapsedMinutes;

        private int playerDeaths = 0;
        private int baseDamage = 0;

        private int mooksToSpawn = 0;
        private int gunnersToSpawn = 0;
        private int huntersToSpawn = 0;
        private int destroyersToSpawn = 0;

        public DirectorSystem()
            : base(500)
        {
            elapsedSeconds = 60f;
            elapsedMinutes = 1f;
        }

        public void LoadContent(Entity Base)
        {
            this.Base = Base;
        }

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);

            elapsedSeconds += .5f;
            elapsedMinutes += .5f / 60f;

            difficulty = (int)(elapsedMinutes - (playerDeaths / 2 + baseDamage));

            //Spawn mooks per second equal to Difficulty
            if (elapsedSeconds % 5 == 0)
            {
                mooksToSpawn = 3 * difficulty;
                gunnersToSpawn = (int)(difficulty / 3);
                huntersToSpawn = (int)(difficulty / 5);
                destroyersToSpawn = (int)(difficulty / 10);

                int type = r.Next(9);
                for (int i = 0; i < mooksToSpawn; i++)
                {
                    World.CreateEntity("Mook", type, Base.GetComponent<Body>()).Refresh();
                }

                for (int i = 0; i < gunnersToSpawn; i++)
                {
                }

                for (int i = 0; i < huntersToSpawn; i++)
                {
                }

                for (int i = 0; i < destroyersToSpawn; i++)
                {
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