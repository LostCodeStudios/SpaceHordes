using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;
using System.Collections.Generic;
using SpaceHordes.Entities.Templates.Objects;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    public class PlayerClampSystem : GroupSystem
    {
        public PlayerClampSystem()
            : base("Players")
        {
        }

        public override void Process(Entity e)
        {
            Body b = e.GetComponent<Body>();

            Vector2 pos = ConvertUnits.ToDisplayUnits(b.Position);

            if (pos.X < -ScreenHelper.Viewport.Width/2)
                pos.X = -ScreenHelper.Viewport.Width/2;
            if (pos.Y < -ScreenHelper.Viewport.Height/2)
                pos.Y = -ScreenHelper.Viewport.Height/2;
            if (pos.X > ScreenHelper.Viewport.Width/2)
                pos.X = ScreenHelper.Viewport.Width/2;
            if (pos.Y > ScreenHelper.Viewport.Height/2)
                pos.Y = ScreenHelper.Viewport.Height/2;

            if (pos != ConvertUnits.ToDisplayUnits(b.Position))
            {
                //Reassign only when changes have been made
                b.Position = ConvertUnits.ToSimUnits(pos);
            }
        }
    }
}
