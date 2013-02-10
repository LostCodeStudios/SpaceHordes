using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    public class AISystem : EntityProcessingSystem
    {
        public AISystem()
            : base(typeof(AI), typeof(ITransform), typeof(IVelocity))
        {
        }

        public override void Process(Entity e)
        {
            ITransform b = e.GetComponent<ITransform>();
            IVelocity v = e.GetComponent<IVelocity>();
            AI a = e.GetComponent<AI>();
            if (b != null)
            {
                Vector2 Velocity = (a.Target.Position - b.Position);
                if (Velocity != Vector2.Zero)
                {
                    Velocity.Normalize();
                    Velocity *= new Vector2(5);
                    v.LinearVelocity = Velocity;
                    b.RotateTo(Velocity);
                }
            }
        }
    }
}
