using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using GameLibrary.Physics.Common;
using GameLibrary.Physics.Collision.Shapes;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Physics.Dynamics.Joints;
using GameLibrary.Helpers;
using GameLibrary.Physics.Factories;

namespace SpaceHordes.Entities.Templates
{
    public class PlayerTemplate : IEntityTemplate
    {
        private EntityWorld world;
        private SpriteSheet spriteSheet;
        public PlayerTemplate(EntityWorld world, SpriteSheet spriteSheet)
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
            e.Group = "Players";
            string tag = "Player" + ((int)((PlayerIndex)args[0])+1);
            e.Tag = tag;
            

            #region Physical
            //Set up initial body
            Physical Body = e.AddComponent<Physical>("Body", new Physical(world,e));
            FixtureFactory.AttachRectangle( //Add a basic bounding box (rectangle status)
                ConvertUnits.ToSimUnits(spriteSheet.Animations[tag][0].Width),
                ConvertUnits.ToSimUnits(spriteSheet.Animations[tag][0].Height),
                1,
                ConvertUnits.ToSimUnits(
                    new Vector2(spriteSheet.Animations[tag][0].Width/2f,
                        spriteSheet.Animations[tag][0].Height/2f)),
                Body);

            if (locations.Values.Count == 0)
                SetStartingLocations();
            //Set the position
            Body.Position = locations[(PlayerIndex)args[0]];
            #endregion

            #region Sprite

            Sprite Sprite = e.AddComponent<Sprite>("Body",
                new Sprite(spriteSheet.Texture, spriteSheet.Animations[tag][0],
                    Body, 1, Color.White, 0f));

            #endregion

            return e;
        }

        #region Helpers
        private static Dictionary<PlayerIndex, Vector2> locations = new Dictionary<PlayerIndex, Vector2>();
        private static void SetStartingLocations()
        {
            locations.Add(PlayerIndex.One,
                ConvertUnits.ToSimUnits(new Vector2(ScreenHelper.Center.X, ScreenHelper.Center.Y )));
            locations.Add(PlayerIndex.Two,
                ConvertUnits.ToSimUnits(new Vector2(ScreenHelper.Center.X, ScreenHelper.Center.Y )));
            locations.Add(PlayerIndex.Three,
                ConvertUnits.ToSimUnits(new Vector2(ScreenHelper.Center.X , ScreenHelper.Center.Y )));
            locations.Add(PlayerIndex.Four,
                ConvertUnits.ToSimUnits(new Vector2(ScreenHelper.Center.X , ScreenHelper.Center.Y )));
        }
        #endregion

    }
}
