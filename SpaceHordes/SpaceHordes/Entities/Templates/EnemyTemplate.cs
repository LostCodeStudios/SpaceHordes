using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using GameLibrary.Physics.Common;
using GameLibrary.Physics.Collision.Shapes;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Physics.Dynamics.Joints;
using GameLibrary.Helpers;
using GameLibrary.Physics.Factories;
using GameLibrary;

namespace SpaceHordes.Entities.Templates
{
    public class EnemyTemplate : IEntityTemplate
    {
        private World world;
        private SpriteSheet spriteSheet;
        public EnemyTemplate(World world, SpriteSheet spriteSheet)
        {
            this.world = world;
            this.spriteSheet = spriteSheet;
        }

        /// <summary>
        /// Builds a player based on the specific index
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Enemies";
            string tag = "Enemy";
            

            #region Physical
            //Set up initial body
            Physical Body = e.AddComponent<Physical>("Body", new Physical(world,e));
            FixtureFactory.AttachRectangle( //Add a basic bounding box (rectangle status)
                ConvertUnits.ToSimUnits(spriteSheet.Animations["purpleship"][0].Width),
                ConvertUnits.ToSimUnits(spriteSheet.Animations["purpleship"][0].Height),
                1,
                ConvertUnits.ToSimUnits(
                    new Vector2(spriteSheet.Animations["purpleship"][0].Width / 2f,
                        spriteSheet.Animations["purpleship"][0].Height / 2f)),
                Body);


            //Set the position
            Body.Position = new Vector2(2);
            Body.Friction = 1f;
            Body.BodyType = BodyType.Dynamic;
            Body.SleepingAllowed = false;
            #endregion

            #region Sprite

            Sprite Sprite = e.AddComponent<Sprite>("Body",
                new Sprite(spriteSheet.Texture, spriteSheet.Animations["purpleship"][0],
                    Body, 1, Color.White, 0f));

            #endregion

            return e;
        }
    }
}
