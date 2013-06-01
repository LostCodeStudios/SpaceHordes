using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System.Collections.Generic;
using System;

namespace SpaceHordes.Entities.Templates
{
    public class PlayerTemplate : IEntityTemplate
    {
        private World world;
        private SpriteSheet spriteSheet;
        static Random r = new Random();

        public PlayerTemplate(World world, SpriteSheet spriteSheet)
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
            PlayerIndex index = (PlayerIndex)args[0];
            e.Group = "Players";
            string tag = "Player" + ((int)index + 1);
            e.Tag = "P" + ((int)index + 1);

            try
            {
                int playerIndex = int.Parse(e.Tag.Replace("P", "")) - 1;
            }
            catch
            {
                throw new FormatException("Yeah fuck you" + e.Tag);
            }
            #region Body

            //Set up initial body
            Body Body = e.AddComponent<Body>(new Body(world, e));
            FixtureFactory.AttachEllipse( //Add a basic bounding box (rectangle status)
                ConvertUnits.ToSimUnits(spriteSheet.Animations[tag][0].Width / 2f),
                ConvertUnits.ToSimUnits(spriteSheet.Animations[tag][0].Height / 2f),
                6,
                1,

                Body);

            if (locations.Values.Count == 0)
                SetStartingLocations();

            //Set the position
            Body.Position = locations[index];
            Body.BodyType = BodyType.Dynamic;
            Body.SleepingAllowed = false;
            Body.FixedRotation = true;
            Body.RotateTo(Body.Position);
            Body.Mass = 2;

            Body.CollisionCategories = Category.Cat1 | Category.Cat12;

            #endregion Body

            #region Sprite

            Sprite Sprite = e.AddComponent<Sprite>(
                new Sprite(spriteSheet, tag,
                    Body, 1, Color.White, 0.5f));

            #endregion Sprite

            #region Animation

            if (Sprite.Source.Length > 1)
                e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 10));

            #endregion Animation

            #region Health

            Health h = new Health(1);

            h.OnDeath += LambdaComplex.SmallEnemyDeath(e, world as SpaceWorld, 0);
            h.OnDeath +=
                ent =>
                {
                    SoundManager.SetVibration(index, 0.4f, 0.5f);
                };
            e.AddComponent<Health>(h);

            #endregion Health

            #region Inventory

            uint yellow = 0;
#if DEBUG
            yellow = 1000;
#endif
            Inventory inventory = e.AddComponent<Inventory>(new Inventory(50, 50, 50, yellow, InvType.Player, ""));

            #endregion Inventory

            return e;
        }

        #region Helpers

        private static Dictionary<PlayerIndex, Vector2> locations = new Dictionary<PlayerIndex, Vector2>();

        private static void SetStartingLocations()
        {
            Vector2 loc = new Vector2(640, 360);
            float distFromBase = 150f;

            loc.Normalize();
            loc *= distFromBase;

            locations.Add(PlayerIndex.One,
                ConvertUnits.ToSimUnits(new Vector2(-loc.X, -loc.Y)));
            locations.Add(PlayerIndex.Two,
                ConvertUnits.ToSimUnits(new Vector2(loc.X, -loc.Y)));
            locations.Add(PlayerIndex.Three,
                ConvertUnits.ToSimUnits(new Vector2(-loc.X, loc.Y)));
            locations.Add(PlayerIndex.Four,
                ConvertUnits.ToSimUnits(new Vector2(loc.X, loc.Y)));
        }

        #endregion Helpers
    }
}