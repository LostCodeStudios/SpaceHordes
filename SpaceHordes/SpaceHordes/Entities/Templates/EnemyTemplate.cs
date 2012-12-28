using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using GameLibrary.Dependencies.Physics.Common;
using GameLibrary.Dependencies.Physics.Collision.Shapes;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Dynamics.Joints;
using GameLibrary.Helpers;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary;
using SpaceHordes.Entities.Components;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;

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
            e.Tag = "Enemy" + e.Id;
            

            #region Body
            //Set up initial body
            Body Body = e.AddComponent<Body>(new Body(world,e));
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
            Body.BodyType = BodyType.Dynamic;
            Body.SleepingAllowed = false;
            #endregion

            #region Sprite

            Sprite Sprite = e.AddComponent<Sprite>(
                new Sprite(spriteSheet.Texture, spriteSheet.Animations["purpleship"][0],
                    Body, 1, Color.White, 0f));

            #endregion

            #region Bullet
            Gun Gun = e.AddComponent<Gun>(new Gun(100000, "TestBullet"));
            #endregion

            return e;
        }
    }
}
