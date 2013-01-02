using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceHordes.Entities.Systems
{
    public class EnemyMovementSystem : GroupSystem
    {
        public EnemyMovementSystem()
            : base("Enemies")
        {
        }

        public void LoadContent(ITransform target)
        {
            this.target = target;
        }
        ITransform target;
        
        public override void Process(Entity e)
        {
            Body b = e.GetComponent<Body>();

            if (b != null)
            {
                Vector2 Velocity = (target.Position - b.Position);
                if (Velocity != Vector2.Zero)
                {
                    Velocity.Normalize();

                    Velocity *= new Vector2(World.Delta / 1000f);
                    b.ApplyForce(Velocity);
                }
            }
        }
    }
}
