using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using SpaceHordes.Entities.Systems;
using System;
using System.Linq;

namespace SpaceHordes.Entities.Templates.Enemies
{
    public class ThugTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private static Random rbitch = new Random();

        private static int thugs = 0;
        public ThugTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            thugs = 0;
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            int type = (int)args[0];

            #region Sprite

            string spriteKey = "";
            switch (type)
            {
                case 0:
                    spriteKey = "bluemissile";
                    break;

                case 1:
                    spriteKey = "graymissile";
                    break;

                case 2:
                    spriteKey = "swastika";
                    break;

                case 3:
                    spriteKey = "swastika2";
                    break;

                case 4:
                    spriteKey = "graye";
                    break;

                case 5:
                    spriteKey = "satellite";
                    break;
            }

            if (args.Length > 2)
                spriteKey = (string)args[2];

            #endregion Sprite

            #region Body

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.5f + (float)thugs/1000000f);
            if (spriteKey.Contains("swastika"))
                s.Origin = new Vector2(s.CurrentRectangle.Width / 2, s.CurrentRectangle.Height / 2);
            e.AddComponent<Sprite>(s);
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            bitch.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat3;
            bitch.OnCollision +=
                (f1, f2, c) =>
                {
                    if (f2.Body.UserData != null && f2.Body.UserData is Entity && (f1.Body.UserData as Entity).HasComponent<Health>())
                        if ((f2.Body.UserData as Entity).Group != "Crystals")
                        {
                            try
                            {
                                (f2.Body.UserData as Entity).GetComponent<Health>().SetHealth(f1.Body.UserData as Entity,
                                    (f2.Body.UserData as Entity).GetComponent<Health>().CurrentHealth
                                    - (f1.Body.UserData as Entity).GetComponent<Health>().CurrentHealth);
                                (f1.Body.UserData as Entity).GetComponent<Health>().SetHealth(f2.Body.UserData as Entity, 0f);
                            }
                            catch
                            {
                                }
                        }
                    return false;
                };
            ++bitch.Mass;

            Vector2 pos = new Vector2((float)(rbitch.NextDouble() * 2) - 1, (float)(rbitch.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= ScreenHelper.Viewport.Width;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;
            bool rotateTo = true;
            if (spriteKey.Contains("swastika"))
            {
                e.GetComponent<Body>().AngularVelocity = (float)Math.PI * 4;
                rotateTo = false;
            }

            #endregion Body

            #region Animation

            if (s.Source.Count() > 1 && spriteKey != "satellite")
                e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 10));

            #endregion Animation

            #region Crystal

            Color crystalColor = Color.Red;
            int colorchance = rbitch.Next(100);
            int amount = 9;
            if (colorchance > 50)
            {
                crystalColor = Color.Yellow;
                amount = 15;
            }
            if (colorchance > 70)
            {
                crystalColor = Color.Blue;
                amount = 6;
                
            }
            if (colorchance > 80)
            {
                crystalColor = Color.Green;
                amount = 3;
            }
            if (colorchance > 90)
            {
                crystalColor = Color.Gray;
                amount = 2;
            }
            e.AddComponent<Crystal>(new Crystal(crystalColor, amount));

            #endregion Crystal

            #region AI/Health

            AI a = new AI(args[1] as Body, AI.CreateFollow(e, 3, rotateTo), "Base");
            e.AddComponent<AI>(a);

            e.AddComponent<Health>(new Health(5)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntityGroup("BigExplosion", "Explosions", poss, 10, e);

                    int splodeSound = rbitch.Next(1, 5);
                    SoundManager.Play("Explosion" + splodeSound.ToString());

                    if (ent is Entity && (ent as Entity).Group != null && ((ent as Entity).Group == "Players") || (ent as Entity).Group == "Structures")
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
                        ScoreSystem.GivePoints(10);
                    }
                };

            #endregion AI/Health

            e.Group = "Enemies";
            return e;
        }
    }
}