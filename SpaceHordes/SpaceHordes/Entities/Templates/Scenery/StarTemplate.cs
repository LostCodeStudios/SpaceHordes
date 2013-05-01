﻿using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using System;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Templates
{
    public class StarTemplate : IEntityTemplate
    {
        private SpriteSheet spriteSheet;
        private static Random rbitch = new Random();
        private int stars = 0;
        private int nebulas = 0;
        private static Vector2[] nebulaLocs = new Vector2[12];

        public StarTemplate(SpriteSheet spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args">args[0] = bool bigStar</param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Stars";
            e.Tag = "Star" + stars.ToString();
            ++stars;

            #region Sprite

            Vector2 loc = new Vector2(rbitch.Next(-ScreenHelper.Viewport.Width  , ScreenHelper.Viewport.Width  ), rbitch.Next(-ScreenHelper.Viewport.Height  , ScreenHelper.Viewport.Height  ));
            if ((bool)args[0] == true)
            {
                Sprite s = new Sprite(spriteSheet, "redstar", 0f);

                //Sprite s = e.AddComponent<Sprite>(new Sprite(spriteSheet, "redstar", loc, 1f, Color.White, 0));
                s.FrameIndex = rbitch.Next(0, 3);
                e.AddComponent<Sprite>(s);
                Animation a = e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 5));
                Particle p = e.AddComponent<Particle>(new Particle(e, ConvertUnits.ToSimUnits(loc), (float)Math.Atan2(loc.Y, loc.X), Vector2.Zero, (float)rbitch.Next(-3, 3) * 0.01f));
            }
            else
            {
                #region Nebula Positioning

                if (nebulas > 0)
                {
                    bool topLeft = false;
                    bool topRight = false;
                    bool bottomLeft = false;
                    bool bottomRight = false;
                    for (int i = 1; i <= nebulas; ++i)
                    {
                        Vector2 avoid = nebulaLocs[nebulas - i];

                        if (!topLeft) //only set bools true once
                            topLeft = (avoid.X < ScreenHelper.Center.X  && avoid.Y < ScreenHelper.Center.Y );

                        if (!topRight)
                            topRight = (avoid.X > ScreenHelper.Center.X  && avoid.Y < ScreenHelper.Center.Y );

                        if (!bottomLeft)
                            bottomLeft = (avoid.X < ScreenHelper.Center.X  && avoid.Y > ScreenHelper.Center.Y );

                        if (!bottomRight)
                            bottomRight = (avoid.X > ScreenHelper.Center.X  && avoid.Y > ScreenHelper.Center.Y );
                    }

                    bool loop = true;
                    while (loop)
                    {
                        if (topLeft)
                        {
                            if (loc.X < ScreenHelper.Center.X  && loc.Y < ScreenHelper.Center.Y )
                            {
                                loc = new Vector2(rbitch.Next(-ScreenHelper.Viewport.Width  , ScreenHelper.Viewport.Width  ), rbitch.Next(-ScreenHelper.Viewport.Height  , ScreenHelper.Viewport.Height  ));
                                continue;
                            }
                        }
                        if (topRight)
                        {
                            if (loc.X > ScreenHelper.Center.X  && loc.Y < ScreenHelper.Center.Y )
                            {
                                loc = new Vector2(rbitch.Next(-ScreenHelper.Viewport.Width  , ScreenHelper.Viewport.Width  ), rbitch.Next(-ScreenHelper.Viewport.Height  , ScreenHelper.Viewport.Height  ));
                                continue;
                            }
                        }
                        if (bottomLeft)
                        {
                            if (loc.X < ScreenHelper.Center.X  && loc.Y > ScreenHelper.Center.Y )
                            {
                                loc = new Vector2(rbitch.Next(-ScreenHelper.Viewport.Width  , ScreenHelper.Viewport.Width  ), rbitch.Next(-ScreenHelper.Viewport.Height  , ScreenHelper.Viewport.Height  ));
                                continue;
                            }
                        }
                        if (bottomRight)
                        {
                            if (loc.X > ScreenHelper.Center.X  && loc.Y > ScreenHelper.Center.Y )
                            {
                                loc = new Vector2(rbitch.Next(-ScreenHelper.Viewport.Width  , ScreenHelper.Viewport.Width  ), rbitch.Next(-ScreenHelper.Viewport.Height  , ScreenHelper.Viewport.Height  ));
                                continue;
                            }
                        }
                        loop = false;
                    }
                }
                nebulaLocs[nebulas] = loc;
                ++nebulas;

                #endregion Nebula Positioning

                Sprite s = e.AddComponent<Sprite>(new Sprite(spriteSheet, "rednebula", 0f));
                Particle p = e.AddComponent<Particle>(new Particle(e, ConvertUnits.ToSimUnits(loc), (float)Math.Atan2(loc.Y, loc.X), Vector2.Zero, (float)rbitch.Next(-3, 3) * 0.001f));
            }

            #endregion Sprite

            return e;
        }
    }
}