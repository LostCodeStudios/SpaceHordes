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

        public override void Added(Entity e)
        {
            e.GetComponent<AI>().TargetChangedEvent +=
                () =>
                {
                    ITransform b = e.GetComponent<ITransform>();
                    IVelocity v = e.GetComponent<IVelocity>();
                    AI a = e.GetComponent<AI>();
                    Vector2 Velocity = (a.Target.Position - b.Position);
                    if (Velocity != Vector2.Zero)
                    {
                        Velocity.Normalize();
                        Velocity *= new Vector2(5);
                        v.LinearVelocity = Velocity;
                        b.RotateTo(Velocity);
                    }
                };

            if (e.GetComponent<AI>().Target != null)
            {
                e.GetComponent<AI>().Target = e.GetComponent<AI>().Target;
            }
            
            base.Added(e);
        }

        public override void Process(Entity e)
        {
            if (e.Group == "Crystals")
            {
                Vector2 distance = e.GetComponent<AI>().Target.Position - e.GetComponent<Body>().Position;
                distance.Normalize();
                e.GetComponent<Body>().LinearVelocity = distance * new Vector2(7);
            }
        }
    }
}
