using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using System;

namespace SpaceHordes.Entities.Systems
{
    public class FireRemovalSystem : GroupSystem
    {
        public FireRemovalSystem()
            : base("Fire")
        {
        }

        public override void Process(Entity e)
        {
            Body b = e.GetComponent<Body>();

            if (b.Position.Y > ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Height) / 2 || Math.Abs(b.Position.X) > ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width) / 2)
            {
                e.Delete();
            }
        }
    }
}