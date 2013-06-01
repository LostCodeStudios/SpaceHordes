using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using SpaceHordes.Entities.Systems;
using SpaceHordes.GameStates.Screens;

namespace SpaceHordes.Entities.Templates
{
    internal static class LambdaComplex
    {
        static Random rbitch = new Random();

        #region Collision

        public static OnCollisionEventHandler BasicCollision()
        {
            return (f1, f2, c) =>
                {
                    if (f1.Body.UserData != null && f2.Body.UserData != null && f1.Body.UserData is Entity && f2.Body.UserData is Entity)
                    {
                        Entity e1 = f1.Body.UserData as Entity;
                        Entity e2 = f2.Body.UserData as Entity;

                        if (e1.HasComponent<Health>() && e2.HasComponent<Health>())
                        {
                            Health h1 = e1.GetComponent<Health>();
                            Health h2 = e2.GetComponent<Health>();

                            h1.Damage(e2, h2.MaxHealth);
                            h2.Damage(e1, h1.MaxHealth);
                        }
                    }
                    return false;
                };
        }

        public static OnCollisionEventHandler CrystalCollision()
        {
            return (f1, f2, c) =>
            {
                if (f1.Body.UserData != null && f2.Body.UserData != null && f1.Body.UserData is Entity && f2.Body.UserData is Entity)
                {
                    Entity e1 = f1.Body.UserData as Entity;
                    Entity e2 = f2.Body.UserData as Entity;

                    if (e2.Group == "Players")
                    {
                        e2.GetComponent<Inventory>().GiveCrystals(e1.GetComponent<Crystal>());
                        e1.Delete();
                        SoundManager.Play("Pickup1");
                    }
                }
                return false;
            };
        }

        public static OnCollisionEventHandler BossCollision()
        {
            return (f1, f2, c) =>
            {
                if (f1.Body.UserData != null && f2.Body.UserData != null && f1.Body.UserData is Entity && f2.Body.UserData is Entity)
                {
                    Entity e1 = f1.Body.UserData as Entity;
                    Entity e2 = f2.Body.UserData as Entity;

                    if (e1.HasComponent<Health>() && e2.HasComponent<Health>())
                    {
                        if (e2.Group == "Base")
                        {
                            e1.GetComponent<Health>().SetHealth(e2, 0);
                        }
#if !DEBUG 
                        e2.GetComponent<Health>().SetHealth(e1, 0);
#endif
                    }
                }

                return false;
            };
        }

        #endregion

        #region Damage

        #endregion

        #region Death

        public static Action<Entity> SmallEnemyDeath(Entity e, SpaceWorld _World, int points)
        {
            return ent =>
            {
                Vector2 poss = e.GetComponent<ITransform>().Position;
                _World.CreateEntity("Explosion", 0.5f, poss, ent, 3, e.GetComponent<IVelocity>().LinearVelocity).Refresh();

                int splodeSound = rbitch.Next(1, 5);
                SoundManager.Play("Explosion" + splodeSound.ToString());

                if (ent is Entity && (ent as Entity).Group != null && ((ent as Entity).Group == "Players" || (ent as Entity).Group == "Structures") && e.HasComponent<Crystal>())
                {
                    _World.CreateEntity("Crystal", e.GetComponent<ITransform>().Position, e.GetComponent<Crystal>().Color, e.GetComponent<Crystal>().Amount);
                    if (points != 0)
                    {
                        ScoreSystem.GivePoints(points);
                        _World.CreateEntity("Score", points.ToString(), poss).Refresh();
                    }
                }
            };
        }

        public static Action<Entity> BigEnemyDeath(Entity e, EntityWorld _World, int points)
        {
            return ent =>
            {
                Vector2 poss = e.GetComponent<ITransform>().Position;
                _World.CreateEntityGroup("BigExplosion", "Explosions", poss, 10, ent, e.GetComponent<IVelocity>().LinearVelocity);

                int splodeSound = rbitch.Next(1, 5);
                SoundManager.Play("Explosion" + splodeSound.ToString());

                if (ent is Entity && (ent as Entity).Group != null && ((ent as Entity).Group == "Players" || (ent as Entity).Group == "Structures") && e.HasComponent<Crystal>())
                {
                    _World.CreateEntity("Crystal", e.GetComponent<ITransform>().Position, e.GetComponent<Crystal>().Color, e.GetComponent<Crystal>().Amount);
                    ScoreSystem.GivePoints(points);
                    _World.CreateEntity("Score", points.ToString(), poss).Refresh();
                }
            };
        }

        public static Action<Entity> BossDeath(int type, SpaceWorld _World, Entity e, Sprite s, int tier, int points, string bossName)
        {
            return blarg =>
            {
                Vector2 poss = e.GetComponent<ITransform>().Position;

                if (type < 6)
                    _World.CreateEntityGroup("BigExplosion", "Explosions", poss, 15, blarg, e.GetComponent<IVelocity>().LinearVelocity);
                else
                {
                    _World.CreateEntityGroup("BiggerExplosion", "Explosions", poss, 7, blarg, e.GetComponent<IVelocity>().LinearVelocity);
                }

                int splodeSound = rbitch.Next(1, 5);
                SoundManager.Play("Explosion" + splodeSound.ToString());

                if (blarg is Entity && (blarg as Entity).Group != null && ((blarg as Entity).Group == "Players" || (blarg as Entity).Group == "Structures"))
                {
                    for (int m = 0; m < (_World as SpaceWorld).Players; ++m)
                    {
                        Entity ent = (_World as SpaceWorld).Player.ToArray()[m];
                        for (int cry = 0; cry < 5; ++cry)
                        {
                            Color crystalColor = DirectorSystem.CrystalColor();
                            int amount = 25 * tier;
                            Vector2 p = e.GetComponent<ITransform>().Position;
                            float range = (float)Math.Sqrt(s.CurrentRectangle.Width * s.CurrentRectangle.Height);
                            float x = 2 * (float)rbitch.NextDouble() - 1;
                            float y = 2 * (float)rbitch.NextDouble() - 1;
                            Vector2 offs = ConvertUnits.ToSimUnits(new Vector2(x, y) * range);
                            p += offs;

                            (_World as SpaceWorld).enemySpawnSystem.SpawnCrystal(p, crystalColor, amount, m);
                        }
                    }
                }

                if (blarg != null && blarg.Tag != "Base")
                {
                    ScoreSystem.GivePoints(points);
                    _World.CreateEntity("Score", points.ToString(), poss).Refresh();
                    BossScreen.BossKilled(bossName);
                }

                _World.enemySpawnSystem.ResetTags();
                _World.enemySpawnSystem.SpawnRate = 1;
            };
        }

        #endregion
    }
}
