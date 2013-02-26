using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates
{
    class BaseTemplate : IEntityTemplate
    {
        private World world;
        private SpriteSheet spriteSheet;
        private Entity junkRock;
        public BaseTemplate(World world, SpriteSheet spriteSheet)
        {
            this.world = world;
            this.spriteSheet = spriteSheet;
        }
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Structures";
            e.Tag = "Base";

            #region Body
            Body Body = e.AddComponent<Body>(new Body(world, e));
            {
                FixtureFactory.AttachEllipse(//Add a basic bounding box (rectangle status)
                    ConvertUnits.ToSimUnits(spriteSheet.Animations["base"][0].Width / 2f),
                    ConvertUnits.ToSimUnits(spriteSheet.Animations["base"][0].Height / 2f),
                    20,
                    1,
                    Body);
                Body.Position = ConvertUnits.ToSimUnits(new Vector2(0, 0));
                Body.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Static;
                Body.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1;

                Body.SleepingAllowed = false;
            }
            #endregion

            #region Sprite
            Sprite Sprite = e.AddComponent<Sprite>(
                new Sprite(spriteSheet,  "base",
                    Body, 1, Color.White, 0.2f));
            #endregion

            e.AddComponent<Score>(new Score());

            e.AddComponent<Health>(new Health(10));

            return e;
        }
    }
}
