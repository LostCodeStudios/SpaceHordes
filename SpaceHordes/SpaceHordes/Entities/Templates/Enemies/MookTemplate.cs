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
    public class MookTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private static Random rbitch = new Random();
        private static int mooks = 0;

        public MookTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
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
                    spriteKey = "squidship";
                    break;

                case 1:
                    spriteKey = "brownfangship";
                    break;

                case 2:
                    spriteKey = "grayshipwithtwoprongs";
                    break;

                case 3:
                    spriteKey = "grayshipwithtwowings";
                    break;

                case 4:
                    spriteKey = "brownplane";
                    break;

                case 5:
                    spriteKey = "bluecrystalship";
                    break;

                case 6:
                    spriteKey = "brownthingwithbluelight";
                    break;

                case 7:
                    spriteKey = "graytriangleship";
                    break;

                case 8:
                    spriteKey = "eyeshot";
                    break;
            }

            if (args.Length > 2)
                spriteKey = (string)args[2];

            #endregion Sprite

            #region Body

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.5f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            bitch.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1;
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

            Vector2 pos = new Vector2((float)(rbitch.NextDouble() * 2) - 1, (float)(rbitch.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= ScreenHelper.Viewport.Width;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;

            #endregion Body

            #region Animation

            if (s.Source.Count() > 1)
                e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 10));

            #endregion Animation

            #region Crystal

            Color crystalColor = Color.Red;
            int colorchance = rbitch.Next(100);
            int amount = 3;
            if (colorchance > 50)
            {
                crystalColor = Color.Blue;
                amount = 2;
            }
            if (colorchance > 70)
            {
                crystalColor = Color.Green;
                amount = 1;
            }
            if (colorchance > 80)
            {
                crystalColor = Color.Yellow;
                amount = 5;
            }
            if (colorchance > 90)
            {
                crystalColor = Color.Gray;
                amount = 2;
            }
            e.AddComponent<Crystal>(new Crystal(crystalColor, amount));

            #endregion Crystal

            #region AI/Health

            //AI ai = new AI();
            //ai.Targeting = Targeting.Closest;
            //ai.HostileGroup = "Players";
            //e.AddComponent<AI>(ai);
            e.AddComponent<AI>(new AI(args[1] as Body));

            e.AddComponent<Health>(new Health(1)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntity("Explosion", 0.5f, poss, ent, 3).Refresh();

                    int splodeSound = rbitch.Next(1, 5);
                    SoundManager.Play("Explosion" + splodeSound.ToString());

                    if (ent is Entity && (ent as Entity).Group != null && (ent as Entity).Group == "Players")
                    {
                        _World.CreateEntity("Crystal", e.GetComponent<ITransform>().Position, e.GetComponent<Crystal>().Color, e.GetComponent<Crystal>().Amount, ent);
                        ScoreSystem.GivePoints(1);
                    }
                };

            #endregion AI/Health

            e.Tag = "Mook" + mooks.ToString();
            e.Group = "Enemies";
            mooks++;
            return e;
        }
    }
}