using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using SpaceHordes.Entities.Systems;
using SpaceHordes.GameStates.Screens;
using System;
using System.Linq;

namespace SpaceHordes.Entities.Templates.Enemies
{
    internal struct BossInfo
    {
        public string SpriteKey;
        public string BossName;

        public BossInfo(string key, string name)
        {
            SpriteKey = key;
            BossName = name;
        }
    }

    public class BossTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private SpaceWorld _World;
        private static Random rbitch = new Random();

        private static int spawned = 0;

        private static BossInfo[] bosses = new BossInfo[]
        {
            new BossInfo("smasher", "The Smasher"),
            new BossInfo("greenbossship", "Big Green"),
            new BossInfo("clawbossthing", "Clawdia"),
            new BossInfo("eye", "The Oculus"),
            new BossInfo("brain", "Father Brain"),
            new BossInfo("bigredblobboss", "Big Red"),
            new BossInfo("blimp", "Lead Zeppelin"),
            new BossInfo("giantgraybossship", "Big Blue"),
            new BossInfo("birdbody", "The Harbinger"),
            new BossInfo("redgunship", "The Gunner"),
            new BossInfo("flamer", "The Flamer"),
            new BossInfo("massivebluemissile", "The Jabber-W0K"),
            new BossInfo("killerhead", "The Destroyer")
        };

        public BossTemplate(SpriteSheet spriteSheet, SpaceWorld world)
        {
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            int tier = (int)args[0];

            #region Sprite

            string spriteKey = "";
            int type = 0;

            switch (tier)
            {
                case 1:
                    type = rbitch.Next(0, 4);
                    break;
                case 2:
                    type = rbitch.Next(5, 8);
                    break;
                case 3:
                    type = rbitch.Next(8, 13);
                    break;
            }
            spriteKey = bosses[type].SpriteKey;

            #endregion Sprite

            #region Body

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.5f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            bitch.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat3;
            bitch.OnCollision +=
                (f1, f2, c) =>
                {
                    if (f2.Body.UserData != null && f2.Body.UserData is Entity && (f1.Body.UserData as Entity).HasComponent<Health>())
                        if ((f2.Body.UserData as Entity).Group != "Crystals")
                        {
                            (f2.Body.UserData as Entity).GetComponent<Health>().SetHealth(f1.Body.UserData as Entity,
                                (f2.Body.UserData as Entity).GetComponent<Health>().CurrentHealth
                                - (f1.Body.UserData as Entity).GetComponent<Health>().CurrentHealth);
                            (f1.Body.UserData as Entity).GetComponent<Health>().SetHealth(f2.Body.UserData as Entity, 0f);
                        }
                    return true;
                };
            bitch.Mass++;

            Vector2 pos = new Vector2(0, -1);
            pos.Normalize();
            pos *= ScreenHelper.Viewport.Height;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;

            if (spriteKey == "massivebluemissile")
            {
                bitch.Rotation = MathHelper.ToRadians(90);
            }

            #endregion Body

            #region Animation

            if (s.Source.Count() > 1)
                e.AddComponent<Animation>(new Animation(AnimationType.None, 10));

            #endregion Animation

            #region Crystal

            Color crystalColor = Color.Red;
            int colorchance = rbitch.Next(100);
            int amount = 25;
            if (colorchance > 50)
            {
                crystalColor = Color.Blue;
                amount = 15;
            }
            if (colorchance > 70)
            {
                crystalColor = Color.Green;
                amount = 10;
            }
            if (colorchance > 80)
            {
                crystalColor = Color.Yellow;
                amount = 35;
            }
            if (colorchance > 90)
            {
                crystalColor = Color.Gray;
                amount = 3;
            }
            e.AddComponent<Crystal>(new Crystal(crystalColor, amount));

            #endregion Crystal

            #region AI/Health

            e.AddComponent<AI>(new AI((args[1] as Body),
               AI.CreateFollow(e,1, false)));

            int points = 0;
            int health = 0;
            switch (tier)
            {
                case 1:
                    points = 100;
                    health = 100;
                    break;

                case 2:
                    points = 500;
                    health = 500;
                    break;

                case 3:
                    points = 1000;
                    health = 1000;
                    break;
            }

            e.AddComponent<Health>(new Health(health)).OnDeath +=
               ent =>
               {
                   Vector2 poss = e.GetComponent<ITransform>().Position;
                   _World.CreateEntityGroup("BigExplosion", "Explosions", poss, 50, ent);

                   int splodeSound = rbitch.Next(1, 5);
                   SoundManager.Play("Explosion" + splodeSound.ToString());

                   if (ent is Entity && (ent as Entity).Group != null && ((ent as Entity).Group == "Players" || (ent as Entity).Group == "Structures"))
                   {
                       if ((ent as Entity).Group == "Structures" && ((ent as Entity).HasComponent<Origin>()))
                       {
                           Entity e2 = (ent as Entity).GetComponent<Origin>().Parent;
                           _World.CreateEntity("Crystal", e.GetComponent<ITransform>().Position, e.GetComponent<Crystal>().Color, e.GetComponent<Crystal>().Amount, e2);
                       }
                       else
                       {
                           _World.CreateEntity("Crystal", e.GetComponent<ITransform>().Position, e.GetComponent<Crystal>().Color, e.GetComponent<Crystal>().Amount, ent);
                       }
                   }

                   if (ent.Tag != "Base")
                   {
                       ScoreSystem.GivePoints(points);
                       BossScreen.BossKilled(bosses[type].BossName);
                   }

                   _World.enemySpawnSystem.ResetTags();

                   #region Special Cases

                   #endregion
               };

            #endregion AI/Health

            e.Tag = "Boss" + spawned.ToString();
            e.Group = "Enemies";

            spawned++;

            #region Special Cases

            if (spriteKey == "smasher")
            {
                _World.CreateEntity("SmasherBall", e).Refresh();
            }

            if (spriteKey == "eye")
            {
                _World.enemySpawnSystem.MookSprite = "eyeshot";
                _World.enemySpawnSystem.ThugSpawnRate = -1;
                _World.enemySpawnSystem.GunnerSpawnRate = -1;
                _World.enemySpawnSystem.HunterSpawnRate = -1;
                _World.enemySpawnSystem.DestroyerSpawnRate = -1;
            }

            if (spriteKey == "clawbossthing")
            {
                _World.enemySpawnSystem.MookSprite = "8prongbrownthingwithfangs";
                _World.enemySpawnSystem.ThugSprite = "minibrownclawboss";
                _World.enemySpawnSystem.GunnerSpawnRate = -1;
                _World.enemySpawnSystem.HunterSpawnRate = -1;
                _World.enemySpawnSystem.DestroyerSpawnRate = -1;
            }

            #endregion Special Cases

            return e;
        }
    }
}