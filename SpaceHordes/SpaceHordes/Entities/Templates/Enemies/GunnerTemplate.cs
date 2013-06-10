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
    public class GunnerTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private static Random rbitch = new Random();

        private static int gunners = 0;

        public GunnerTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            gunners = 0;
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            int type = (int)args[0];

            #region Body

            string spriteKey = "";

            switch (type)
            {
                case 0:
                    spriteKey = "purpleship";
                    break;

                case 1:
                    spriteKey = "brownarmship";
                    break;
            }

            if (args.Length > 1)
                spriteKey = (string)args[1];

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.51f + (float)gunners / 1000000f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            bitch.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat3;
            bitch.OnCollision += LambdaComplex.BasicCollision();
            ++bitch.Mass;

            Vector2 pos = new Vector2((float)(rbitch.NextDouble() * 2) - 1, (float)(rbitch.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= MookTemplate.dist;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;

            #endregion Body

            #region Animation

            if (s.Source.Count() > 1)
                e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 10));

            #endregion Animation

            #region Crystal

            Color crystalColor = DirectorSystem.CrystalColor();
            int amount = 5;
            if (crystalColor == Color.Gray)
                amount = 2;
            e.AddComponent<Crystal>(new Crystal(crystalColor, amount));

            #endregion Crystal

            #region AI/Health

            AI a = new AI(null,
                AI.CreateShoot(e, ConvertUnits.ToSimUnits(4f), ConvertUnits.ToSimUnits(400), true), "Structures");
            AI shootingAi = e.AddComponent<AI>(a);

            e.AddComponent<Health>(new Health(1)).OnDeath += LambdaComplex.SmallEnemyDeath(e, _World as SpaceWorld, 30);

            #endregion AI/Health

            #region Inventory

            Inventory i = new Inventory(0, 0, 0, 0, InvType.Gunner, spriteKey);
            e.AddComponent<Inventory>(i);

            #endregion Inventory

            ++gunners;
            e.Group = "Enemies";
            return e;
        }
    }
}