﻿using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Systems
{
    public class DirectorSystem : IntervalEntitySystem
    {
        private Random r = new Random();

        private Entity Base;
        private Entity Boss;

        private int difficulty = 0;

        //Start off with a minute worth of time so spawns don't delay by a minute due to casting
        private float elapsedSeconds;
        private float elapsedMinutes;

        private int mooksToSpawn = 0;
        private int thugsToSpawn = 0;
        private int gunnersToSpawn = 0;
        private int huntersToSpawn = 0;
        private int destroyersToSpawn = 0;

        public static int MookSpawnRate = 0;
        public static int ThugSpawnRate = 0;
        public static int GunnerSpawnRate = 0;
        public static int HunterSpawnRate = 0;
        public static int DestroyerSpawnRate = 0;

        public static int SpawnRate
        {
            set
            {
                MookSpawnRate = value;
                ThugSpawnRate = value;
                GunnerSpawnRate = value;
                HunterSpawnRate = value;
                DestroyerSpawnRate = value;
            }
        }

        public static string MookTemplate = "Mook";
        public static string MookSprite = "";

        public static string ThugTemplate = "Thug";
        public static string ThugSprite = "";

        public static string GunnerTemplate = "Gunner";
        public static string HunterTemplate = "Hunter";
        public static string DestroyerTemplate = "Destroyer";
        public static string BossTemplate = "Boss";

        public static void ResetTags()
        {
            MookSprite = "";
            ThugSprite = "";

            GunnerTemplate = "Gunner";
            HunterTemplate = "Hunter";
            DestroyerTemplate = "Destroyer";
            BossTemplate = "Boss";
        }

        private int timesCalled = 0;
        private float intervalSeconds = 0f;

        private int lastBoss = 2;

        bool surge = false;
        int elapsedWarning = 0;
        int elapsedSurge = 0;
        int warningTime = 3000;
        int surgeTime = 30000;
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
            elapsedSeconds += .333f;
            intervalSeconds += .333f;
            elapsedMinutes += .333f / 60f;

            SpaceWorld w = world as SpaceWorld;
            difficulty = (int)(elapsedMinutes) * w.Players;

            if (surge)
            {
                elapsedWarning += 333;
                elapsedSurge += 333;

                if (elapsedWarning >= warningTime)
                {
                    HUDRenderSystem.SurgeWarning = false;
                    elapsedWarning = 0; 
                }

                if (elapsedSurge >= surgeTime)
                {
                    surge = false;
                    elapsedSurge = 0;
                    SpawnRate = 0;
                }
            }

            #region Scoring

            Score s = Base.GetComponent<Score>();

            if (intervalSeconds >= 1)
            {
                ScoreSystem.GivePoints(10 * difficulty);
                intervalSeconds = 0;
            }

            #endregion Scoring

            #region Spawning

            //Every 5/3 seconds spawn

            if (timesCalled % 5 == 0)
            {
                int type;
                mooksToSpawn = (MookSpawnRate != -1) ? ((MookSpawnRate == 0) ? difficulty : difficulty * MookSpawnRate) : 0;
                if (ThugSpawnRate != -1)
                {
                    thugsToSpawn = (ThugSpawnRate == 0) ? 
                        ((r.Next(1, 100) * difficulty > 90) ? 1 : 0) : 
                        ((r.Next(1, 100) * ThugSpawnRate) > 90 ? 
                            difficulty * ThugSpawnRate : 0);
                }
                    
                gunnersToSpawn = (GunnerSpawnRate != -1) ? ((GunnerSpawnRate == 0) ? (int)(difficulty / 9) : difficulty * (int)(GunnerSpawnRate/9)) : 0;
                huntersToSpawn = (HunterSpawnRate != -1) ? ((HunterSpawnRate == 0) ? (int)(difficulty / 15) : difficulty * (int)(HunterSpawnRate/15)) : 0;
                destroyersToSpawn = (DestroyerSpawnRate != -1) ? ((DestroyerSpawnRate != 0) ? (int)(difficulty / 30) : difficulty * (int)(DestroyerSpawnRate/30)) : 0;

                type = r.Next(8);
                for (int i = 0; i < mooksToSpawn; i++)
                {
                    if (string.IsNullOrEmpty(MookSprite))
                        World.CreateEntity(MookTemplate, type, Base.GetComponent<Body>()).Refresh();
                    else
                        World.CreateEntity(MookTemplate, type, Base.GetComponent<Body>(), MookSprite).Refresh();
                }

                type = r.Next(4);
                for (int i = 0; i < thugsToSpawn; i++)
                {
                    if (string.IsNullOrEmpty(ThugSprite))
                        World.CreateEntity(ThugTemplate, type, Base.GetComponent<Body>()).Refresh();
                    else
                        World.CreateEntity(ThugTemplate, type, Base.GetComponent<Body>(), ThugSprite).Refresh();
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

            if ((int)(elapsedMinutes) >= lastBoss || timesCalled == 1)
            {
                
                int chance = r.Next(1, 100);

                if (chance > 66)
                {
                    //SURGE
                    surge = true;
                    HUDRenderSystem.SurgeWarning = true;
                    SpawnRate = 2;
                }
                else
                {
                    //Boss.
                    int tier = Math.Min(difficulty, 3);
                    Boss = World.CreateEntity(BossTemplate, tier, Base.GetComponent<Body>());
                    Boss.Refresh();
                    lastBoss++;
                }
            }

            #endregion Spawning
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