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
    public class DestroyerTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private static Random rbitch = new Random();

        private static int destroyers = 0;
        public DestroyerTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            destroyers = 0;
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {

            #region Body
            string spriteKey = "redgrayblobship";

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.55f + (float)destroyers / 1000000f));
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
                                (f1.Body.UserData as Entity).GetComponent<Health>().SetHealth(f2.Body.UserData as Entity,
                                    (f1.Body.UserData as Entity).GetComponent<Health>().CurrentHealth
                                    - (f2.Body.UserData as Entity).GetComponent<Health>().CurrentHealth);
                            }
                            catch
                            {
                            }
                        }
                    return true;
                };
            ++bitch.Mass;

            Vector2 pos = new Vector2((float)(rbitch.NextDouble() * 2) - 1, (float)(rbitch.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= MookTemplate.dist;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;

            #endregion Body

            #region Crystal

            Color crystalColor = Color.Red;
            int colorchance = rbitch.Next(100);
            int amount = 5;
            if (colorchance > 50)
            {
                crystalColor = Color.Yellow;
                amount = 10;
            }
            if (colorchance > 70)
            {
                crystalColor = Color.Blue;
                amount = 5;
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

            AI a = new AI(null,
                AI.CreateShoot(e, 2, ConvertUnits.ToSimUnits(400), true), "Players");
            AI shootingAi = e.AddComponent<AI>(a);

            e.AddComponent<Health>(new Health(10)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntityGroup("BigExplosion", "Explosions", poss, 10, ent, e.GetComponent<IVelocity>().LinearVelocity);

                    int splodeSound = rbitch.Next(1, 5);
                    SoundManager.Play("Explosion" + splodeSound.ToString());

                    if (ent is Entity && (ent as Entity).Group != null && ((ent as Entity).Group == "Players" || (ent as Entity).Group == "Structures"))
                    {
                        _World.CreateEntity("Crystal", e.GetComponent<ITransform>().Position, e.GetComponent<Crystal>().Color, e.GetComponent<Crystal>().Amount);

                        ScoreSystem.GivePoints(10);
                    }
                };

            #endregion AI/Health

            #region Inventory

            Inventory i = new Inventory(0, 0, 0, 0, InvType.Cannon, spriteKey);
            e.AddComponent<Inventory>(i);

            #endregion

            ++destroyers;
            e.Group = "Enemies";
            return e;
        }
    }
}