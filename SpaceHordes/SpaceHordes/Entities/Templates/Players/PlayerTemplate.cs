using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Templates
{
    public class PlayerTemplate : IEntityTemplate
    {
        private World world;
        private SpriteSheet spriteSheet;
        private static Random r = new Random();

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
            inventory.CurrentGun = inventory.WHITE;

            #endregion Inventory

            #region AI
            //Create AI if it's an artificial player.
            if (args.Length >= 2 && (bool)args[1])
            {
                AI ai = e.AddComponent<AI>(new AI(null,
                    (target) =>
                    {

                        bool returnCode = false;
                        AI eai = e.GetComponent<AI>();
                        if (target == null)
                        {
                            #region Movement
                            //When there is no target focus on building structures around the base on random sides and possibly preparing mines.
                            //If AI cannot afford to build, then commence swarm code (move around base randomly awaiting a target.
                            Body.LinearVelocity = Vector2.Zero;

                            Vector2 Omega = Vector2.Zero - Body.Position;

                            Body.ApplyLinearImpulse(new Vector2(Omega.Y, -Omega.X) * 2);

                            Body.RotateTo(Vector2.Zero - Body.Position);
                            Body.Rotation += (float)Math.PI;

                            if (Vector2.Distance(Vector2.Zero, Body.Position) > ConvertUnits.ToSimUnits(200))
                                Body.ApplyLinearImpulse((Vector2.Zero - Body.Position)*0.5f);
                            else
                                if (inventory.YELLOW >= 25 && r.Next(100) > 95)
                                {
                                    world.CreateEntity("Turret", ConvertUnits.ToDisplayUnits(Body.Position), e).Refresh();
                                    inventory.YELLOW -= 25;
                                    SoundManager.Play("Construction");
                                }



                            #endregion

                            returnCode = true;
                        }
                        else
                        {
                            #region Movement

                            Body.LinearVelocity = Vector2.Zero;


                            if (Vector2.Distance(Body.Position, target.Position) > 3)
                                Body.ApplyLinearImpulse(new Vector2((float)Math.Cos(Body.Rotation), (float)Math.Sin(Body.Rotation)) * 10);
                            else
                            {
                                //V = OMEGA x R
                                Vector2 R = Body.Position - target.Position;
                                Vector2 Omega = target.Position - Body.Position;

                                Body.ApplyLinearImpulse(new Vector2(Omega.Y, -Omega.X) * 3);
                            }
                            


                            #endregion

                            #region Shooting
                            //TRUE AI CODE OCCURS HERE
                            //Begin to seek an attack
                            Gun g = inventory.CurrentGun;
                            g.BulletsToFire = true;

                            /* Aiming *\
                                * X = v*t + x_o therefore
                                *  let X_aim = v_tar*t + x_tar
                                *            = v_bul*t + x_bul
                                *  therefore
                                *      0 = v_tar*t + x_tar - (v_bul*t + x_bul) =>
                                *  therefore
                                *      x_bul - x_tar = t(v_tar - v_bul)
                                *  so that
                                *      t = (x_bul - x_tar)/(v_tar - v_bul)
                                *  therefore X = (v_tar)*(x_bul - x_tar)/(v_tar - v_bul) + x_tar
                                */

                            //Vector2 zero = target.LinearVelocity*time - inv.CurrentGun.BulletVelocity*time + target.Position - Body.Position;
                            Vector2 time = (Body.Position - target.Position) /
                                (target.LinearVelocity - inventory.CurrentGun.BulletVelocity);

                            Vector2 XFinal = (target.LinearVelocity) * time + target.Position;
                            Body.RotateTo(XFinal - Body.Position);
                            #endregion
                        }

                        return returnCode;
                    }, "Enemies", ConvertUnits.ToSimUnits(450f)));
            }
            #endregion

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