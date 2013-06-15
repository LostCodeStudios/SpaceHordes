using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;
using System.Collections.Generic;
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

        public static int spawned = 0;

        #region Boss Info

        private static BossInfo[] bosses = new BossInfo[]
        {
            new BossInfo("smasher", "The Smasher"), //0 //Fully implemented
            new BossInfo("greenbossship", "Big Green"),  //1
            new BossInfo("clawbossthing", "The Killer"), //2 //Fully implemented
            new BossInfo("eye", "The Oculus"), //3 //Fully implemented
            new BossInfo("brain", "Father Brain"), //4 //Fully implemented
            new BossInfo("bigredblobboss", "The War Machine"), //5 //Fully implemented
            new BossInfo("giantgraybossship", "The Mother Ship"), //6 //Fully implemented
            new BossInfo("flamer", "The Flamer"), //7 //Fully implemented
            new BossInfo("massivebluemissile", "The Jabber-W0K"), //8 //Fully implemented
            new BossInfo("killerhead", "The Destroyer") //9 //Fully implemented
        };

        #endregion Boss Info

        public BossTemplate(SpriteSheet spriteSheet, SpaceWorld world)
        {
            spawned = 0;
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            int tier = 1;
            if (spawned > 2)
                tier = 2;
            if (spawned > 5)
                tier = 3;

            #region Sprite

            string spriteKey = "";
            int type = spawned;

            spriteKey = bosses[type].SpriteKey;

            #endregion Sprite

            #region Body

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 20, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, spriteKey != "bigredblobboss" ? 0.5f + (float)type / 10000f : 0.55f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            bitch.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat3 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat6;
            bitch.OnCollision += LambdaComplex.BossCollision();
            ++bitch.Mass;

            Vector2 pos = new Vector2(0, -1);
            pos.Normalize();
            pos *= ScreenHelper.Viewport.Height / 1.5f;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;
            bitch.SleepingAllowed = false;

            #endregion Body

            #region Animation

            Animation a = null;
            if (s.Source.Count() > 1)
            {
                a = new Animation(AnimationType.None, 10);
                e.AddComponent<Animation>(a);
            }

            #endregion Animation

            #region AI/Health

            e.AddComponent<HealthRender>(new HealthRender());

            if (spriteKey == "flamer")
                e.AddComponent<AI>(new AI((args[0] as Body),
                    AI.CreateFlamer(e, 0.5f, bitch, s, _World), "Base"));

            else if (spriteKey == "bigredblobboss")
                e.AddComponent<AI>(new AI(null,
                    AI.CreateWarMachine(e, 0.5f, bitch, 10f, 0.7f, s, _World)));

            else if (spriteKey == "killerhead")
                e.AddComponent<AI>(new AI(null,
                    AI.CreateKiller(e, 0.5f, 20f, _World)));

            else if (spriteKey == "greenbossship")
                e.AddComponent<AI>(new AI(null,
                    AI.CreateBigGreen(e, 0.5f, 10f, 2f, 0.05f, 3.5f, s, _World)));

            else
                e.AddComponent<AI>(new AI((args[0] as Body),
                    AI.CreateFollow(e, 1, false), "Base", false));

            int points = 0;
            int health = 0;

            switch (tier)
            {
                case 1:
                    points = 300;
                    health = 150;
                    break;

                case 2:
                    points = 500;
                    health = 175;
                    break;

                case 3:
                    points = 1000;
                    health = 200;
                    break;
            }

            Health h = new Health(health);
            h.OnDeath += LambdaComplex.BossDeath(type, _World, e, s, tier, points, bosses[type].BossName);

            if (type == 1)
            {
                h.OnDeath += 
                    ex =>
                        {
                               Console.WriteLine("DEad");
                        };
            }

            if (a != null)
            {
                h.OnDamage +=
                    ent =>
                    {
                        if (h.IsAlive && a.Type == AnimationType.None)
                        {
                            e.RemoveComponent<Sprite>(s);

                            double healthFraction = (h.CurrentHealth / h.MaxHealth);

                            int frame = 0;
                            int frames = s.Source.Length;

                            frame = (int)(frames - (frames * healthFraction));

                            if (frame != s.FrameIndex)
                            {
                                int splodeSound = rbitch.Next(1, 5);
                                SoundManager.Play("Explosion" + splodeSound.ToString());
                                Vector2 poss = e.GetComponent<ITransform>().Position;
                                _World.CreateEntityGroup("BigExplosion", "Explosions", poss, 15, e, e.GetComponent<IVelocity>().LinearVelocity);
                            }
                            s.FrameIndex = frame;

                            e.AddComponent<Sprite>(s);
                        }
                    };
            }

            if (spriteKey.Equals("flamer"))
            {
                h.OnDamage +=
                    ent =>
                    {
                        //Fire flame from random spot

                        int range = s.CurrentRectangle.Width / 2;
                        float posx = rbitch.Next(-range, range);
                        Vector2 pos1 = bitch.Position + ConvertUnits.ToSimUnits(new Vector2(posx, 0));

                        float x = posx / range;

                        float y = 1;

                        Vector2 velocity = new Vector2(x, y);
                        velocity.Normalize();
                        velocity *= 7;

                        _World.CreateEntity("Fire", pos1, velocity).Refresh();
                    };
            }

            e.AddComponent<Health>(h);

            #endregion AI/Health

            e.Tag = "Boss" + spawned.ToString();
            e.Group = "Enemies";

            ++spawned;

            #region Special Cases

            if (spriteKey == "smasher")
            {
                _World.CreateEntity("SmasherBall", e).Refresh();
            }

            if (spriteKey == "brain")
            {
                Vector2 offset = new Vector2(2, 0);
                Vector2 position = bitch.Position + offset;
                _World.CreateEntity("Cannon", position, e).Refresh();
                position = bitch.Position - offset;
                _World.CreateEntity("Cannon", position, e).Refresh();
            }

            if (spriteKey == "massivebluemissile")
            {
                _World.enemySpawnSystem.ThugSprite = "bluemissile";
                _World.enemySpawnSystem.SpawnRate = 0;
                _World.enemySpawnSystem.ThugSpawnRate = 3;
            }

            if (spriteKey == "killerhead")
            {
                List<Entity> children = new List<Entity>();
                Vector2 offset = new Vector2(2.85f, -0.6f);
                Vector2 position = bitch.Position + offset;
                Entity x;
                x = _World.CreateEntity("KillerGun", position, e, "killerrightgun", offset, e);
                x.Refresh();
                children.Add(x);
                offset.X = -3.15f;
                position = bitch.Position + offset;
                x = _World.CreateEntity("KillerGun", position, e, "killerleftgun", offset);
                x.Refresh();
                children.Add(x);
                e.AddComponent<Children>(new Children(children));
            }

            if (spriteKey == "eye")
            {
                _World.enemySpawnSystem.MookSprite = "eyeshot";
                _World.enemySpawnSystem.SpawnRate = 0;
                _World.enemySpawnSystem.MookSpawnRate = 2;
            }

            if (spriteKey == "clawbossthing")
            {
                _World.enemySpawnSystem.MookSprite = "8prongbrownthingwithfangs";
                _World.enemySpawnSystem.ThugSprite = "minibrownclawboss";
                _World.enemySpawnSystem.SpawnRate = 0;
                _World.enemySpawnSystem.MookSpawnRate = 2;
                _World.enemySpawnSystem.ThugSpawnRate = 2;
            }

            if (spriteKey == "flamer")
            {
                _World.enemySpawnSystem.SpawnRate = 0;
            }

            #endregion Special Cases

            return e;
        }
    }
}