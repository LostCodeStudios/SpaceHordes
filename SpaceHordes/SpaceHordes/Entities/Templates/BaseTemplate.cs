using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Physics.Factories;
using GameLibrary;

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
            #region Physical
            Physical Body = e.AddComponent<Physical>("Body", new Physical(world, e));
            {
               FixtureFactory.AttachRectangle( //Add a basic bounding box (rectangle status)
                ConvertUnits.ToSimUnits(spriteSheet.Animations["base"][0].Width),
                ConvertUnits.ToSimUnits(spriteSheet.Animations["base"][0].Height),
                1,
                ConvertUnits.ToSimUnits(
                    new Vector2(spriteSheet.Animations["base"][0].Width/2f,
                        spriteSheet.Animations["base"][0].Height/2f)),
                Body);
                Body.Position = ConvertUnits.ToSimUnits(new Vector2(0,0));
                Body.BodyType = GameLibrary.Physics.Dynamics.BodyType.Static;

                Body.SleepingAllowed = false;
            }
            #endregion

            #region Sprite
            Sprite Sprite = e.AddComponent<Sprite>("Body",
                new Sprite(spriteSheet.Texture,  spriteSheet.Animations["base"][0],
                    Body, 1, Color.White, 0f));
            #endregion

            return e;
        }
    }
}
