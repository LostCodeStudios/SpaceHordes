using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Systems;

namespace SpaceHordes.Entities.Templates.Enemies
{
    public class SmasherBallTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private static Random rbitch = new Random();

        public SmasherBallTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args">args[0] = Entity smasher, args[1] = Smasher loc</param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {

            string spriteKey = "smasherball";

            #region Body

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 20, 1f, bitch);
            Sprite s = new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.5f);
            e.AddComponent<Sprite>(s);

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

            Entity smasher = (args[0] as Entity);

            float dist = ConvertUnits.ToSimUnits(20f);
            Vector2 pos = smasher.GetComponent<Body>().Position + new Vector2(0, dist);
            bitch.Position = pos;

            #endregion Body

            #region Animation

            if (s.Source.Count() > 1)
                e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 10));

            #endregion Animation

            #region Health

            e.AddComponent<Health>(new Health(1000000)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntityGroup("BigExplosion", "Explosions", poss, 10, e);

                    int splodeSound = rbitch.Next(1, 5);
                    SoundManager.Play("Explosion" + splodeSound.ToString());
                };

            #endregion AI/Health

            e.AddComponent<Origin>(new Origin(smasher));
            e.Tag = "SmasherBall";
            return e;
        }
    }
}
