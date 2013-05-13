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
    public class KillerGunTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private static Random rbitch = new Random();

        public KillerGunTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {

            #region Body
            string spriteKey = (string)args[2];
            Vector2 offset = (Vector2)args[3];

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.55f));
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
                    return true;
                };
            ++bitch.Mass;

            bitch.Position = (Vector2)args[0];

            #endregion Body

            #region AI/Health

            Entity o = args[1] as Entity;
            e.AddComponent<Origin>(new Origin(o));
            Function f;
            if (spriteKey == "killerleftgun")
                f = new Function(AI.CreateKillerGun(e, o, offset, 0.05f, 1f, 0.5f, s, _World, false));
            else
                f = new Function(AI.CreateKillerGun(e, o, offset, 0.05f, 1f, 0.5f, s, _World, true));
            e.AddComponent<Function>(f);

            e.AddComponent<Health>(new Health(50)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntityGroup("BigExplosion", "Explosions", poss, 10, ent, e.GetComponent<IVelocity>().LinearVelocity);

                    int splodeSound = rbitch.Next(1, 5);
                    SoundManager.Play("Explosion" + splodeSound.ToString());

                    if (ent is Entity && (ent as Entity).Group != null && ((ent as Entity).Group == "Players" || (ent as Entity).Group == "Structures"))
                    {
                        ScoreSystem.GivePoints(10);
                    }
                };

            #endregion AI/Health

            e.Group = "Enemies";
            return e;
        }
    }
}