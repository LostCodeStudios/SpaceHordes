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
        private static Random r = new Random();
        private static int mooks = 0;

        public static float dist = (float)ScreenHelper.Viewport.Width / 1.75f;

        public MookTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            mooks = 0;
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
                    spriteKey = "bluecrystalship";
                    break;

                case 3:
                    spriteKey = "brownthingwithbluelight";
                    break;

                case 4:
                    spriteKey = "grayshipwithtwoprongs";
                    break;

                case 5:
                    spriteKey = "greyshipbrownbulb";
                    break;

                case 6:
                    spriteKey = "blueshipgraybulb";
                    break;
            }

            if (args.Length > 2)
                spriteKey = (string)args[2];

            #endregion Sprite

            #region Body

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.53f + (float)mooks / 1000000f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            bitch.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat3 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat6;
            bitch.OnCollision += LambdaComplex.BasicCollision();
            ++bitch.Mass;

            Vector2 pos = new Vector2((float)(r.NextDouble() * 2) - 1, (float)(r.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= dist;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;

            #endregion Body

            #region Animation

            if (s.Source.Count() > 1)
                e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 10));

            #endregion Animation

            #region Crystal

            int amount = 3;
            Color crystalColor = DirectorSystem.CrystalColor();
            if (crystalColor == Color.Gray)
                amount = 2;
            e.AddComponent<Crystal>(new Crystal(crystalColor, amount));

            #endregion Crystal

            #region AI/Health

            AI a = new AI(args[1] as Body, AI.CreateFollow(e, 5, true), "Base");
            a.Recalculate = false;
            e.AddComponent<AI>(a);

            e.AddComponent<Health>(new Health(1)).OnDeath += LambdaComplex.SmallEnemyDeath(e, _World as SpaceWorld, 5);

            #endregion AI/Health

            e.Tag = "Mook" + mooks.ToString();
            e.Group = "Enemies";
            ++mooks;
            return e;
        }
    }
}