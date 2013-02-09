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
                    10,
                    1,
                    Body);
                Body.Position = ConvertUnits.ToSimUnits(new Vector2(0, 0));
                Body.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Static;

                Body.SleepingAllowed = false;
            }
            #endregion

            #region Sprite
            Sprite Sprite = e.AddComponent<Sprite>(
                new Sprite(spriteSheet,  "base",
                    Body, 1, Color.White, 0f, TimeSpan.Zero));
            #endregion



            e.AddComponent<Health>(new Health(10000));

            return e;
        }
    }
}
