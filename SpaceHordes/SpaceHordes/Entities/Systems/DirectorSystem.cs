using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components.Physics;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    public class DirectorSystem : IntervalEntitySystem
    {
        Random r = new Random();

        Entity Base;

        int difficulty = 0;

        //Start off with a minute worth of time so spawns don't delay by a minute due to casting
        float elapsedSeconds;
        float elapsedMinutes;

        int playerDeaths = 0;
        int baseDamage = 0;

        int mooksToSpawn = 0;
        int thugsToSpawn = 0;
        int gunnersToSpawn = 0;
        int huntersToSpawn = 0;
        int destroyersToSpawn = 0;

        public static int MookSpawnRate = 0;
        public static int ThugSpawnRate = 0;
        public static int GunnerSpawnRate = 0;
        public static int HunterSpawnRate = 0;
        public static int DestroyerSpawnRate = 0;

        public static string MookTemplate = "Mook";
        public static string ThugTemplate = "Thug";
        public static string GunnerTemplate = "Gunner";
        public static string HunterTemplate = "Hunter";
        public static string DestroyerTemplate = "Destroyer";
        public static string BossTemplate = "Boss";

        int timesCalled = 0;

        public DirectorSystem()
            : base(333)
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

            timesCalled++;
            elapsedSeconds += .25f;
            elapsedMinutes += .25f/60f;

            difficulty = (int)(elapsedMinutes - (playerDeaths / 2 + baseDamage));

            #region Scoring

            Score s = Base.GetComponent<Score>();

            if (elapsedSeconds % 1 == 0)
                ScoreSystem.GivePoints(10 * difficulty);

            #endregion

            #region Spawning

            //Every 5 seconds spawn

            if (timesCalled == 1)
            {
                World.CreateEntity(BossTemplate, 1, Base.GetComponent<Body>()).Refresh();
            }

            if (timesCalled % 5 == 0)
            {
                int type;
                mooksToSpawn = (MookSpawnRate == 0) ? difficulty : MookSpawnRate;
                thugsToSpawn = (ThugSpawnRate == 0) ? ((r.Next(1, 100) * difficulty > 90) ? 1 : 0) : ThugSpawnRate;
                gunnersToSpawn = (GunnerSpawnRate == 0) ? (int)(difficulty / 9) : GunnerSpawnRate;
                huntersToSpawn = (HunterSpawnRate == 0) ? (int)(difficulty / 15) : HunterSpawnRate;
                destroyersToSpawn = (DestroyerSpawnRate != 0) ? (int)(difficulty / 30) : DestroyerSpawnRate;

                type = r.Next(8);
                for (int i = 0; i < mooksToSpawn; i++)
                {
                    World.CreateEntity(MookTemplate, type, Base.GetComponent<Body>()).Refresh();
                }

                type = r.Next(4);
                for (int i = 0; i < thugsToSpawn; i++)
                {
                    World.CreateEntity(ThugTemplate, type, Base.GetComponent<Body>()).Refresh();
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

            if (elapsedMinutes % 3 == 0)
            {
                //Boss.
                int tier = difficulty / 3;
                World.CreateEntity(BossTemplate, tier).Refresh();
            }

            #endregion
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
