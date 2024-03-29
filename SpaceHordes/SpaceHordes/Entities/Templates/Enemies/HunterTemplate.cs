﻿using GameLibrary.Dependencies.Entities;
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
    public class HunterTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private static Random r = new Random();
        private static int hunters = 0;

        public HunterTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            hunters = 0;
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
                    spriteKey = "graytriangleship";
                    break;

                case 1:
                    spriteKey = "grayshipwithtwowings";
                    break;

                case 2:
                    spriteKey = "brownplane";
                    break;
            }

            if (args.Length > 2)
                spriteKey = (string)args[2];

            #endregion Sprite

            #region Body

            Body b = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, b);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, b, 1f, Color.White, 0.52f + (float)hunters / 1000000f));
            b.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            b.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            b.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat3;
            b.OnCollision += LambdaComplex.BasicCollision();
            ++b.Mass;

            Vector2 pos = new Vector2((float)(r.NextDouble() * 2) - 1, (float)(r.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= MookTemplate.dist;
            pos = ConvertUnits.ToSimUnits(pos);
            b.Position = pos;

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

            e.AddComponent<AI>(new AI((args[1] as Body),
                    AI.CreateFollow(e, 7, true), "Players"));

            e.AddComponent<Health>(new Health(1)).OnDeath += LambdaComplex.SmallEnemyDeath(e, _World as SpaceWorld, 25);

            #endregion AI/Health

            e.Tag = "Hunter" + hunters.ToString();
            e.Group = "Enemies";

            ++hunters;
            return e;
        }
    }
}