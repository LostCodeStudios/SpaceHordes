using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using SpaceHordes.Entities.Systems;
using System;

namespace SpaceHordes.Entities.Templates.Enemies
{
    public class CannonTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private static Random rbitch = new Random();

        private static int cannons = 0;

        public CannonTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            cannons = 0;
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            #region Body

            string spriteKey = "blaster";
            bool rotateTo = true;
            if (args.Length > 2)
            {
                spriteKey = (string)args[2];
                rotateTo = false;
            }

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.55f + (float)cannons / 1000000f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            bitch.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat3;
            bitch.OnCollision += LambdaComplex.BasicCollision();
            ++bitch.Mass;

            bitch.Position = (Vector2)args[0];

            #endregion Body

            #region Crystal

            Color crystalColor = DirectorSystem.CrystalColor();
            int amount = 5;
            e.AddComponent<Crystal>(new Crystal(crystalColor, amount));

            #endregion Crystal

            #region AI/Health

            e.AddComponent<Origin>(new Origin(args[1] as Entity));

            AI a = new AI(null,
                AI.CreateCannon(e, rotateTo), "Players");
            AI shootingAi = e.AddComponent<AI>(a);

            e.AddComponent<Health>(new Health(50)).OnDeath += LambdaComplex.BigEnemyDeath(e, _World, 50);

            #endregion AI/Health

            #region Inventory

            Inventory i = new Inventory(0, 0, 0, 0, InvType.Cannon, spriteKey);
            if (spriteKey == "killerleftgun")
            {
                i.CurrentGun.GunOffsets.Add(new Vector2(54, -19));
            }
            e.AddComponent<Inventory>(i);

            #endregion Inventory

            ++cannons;
            e.Tag = "Cannon" + cannons.ToString();
            e.Group = "Enemies";
            return e;
        }
    }
}