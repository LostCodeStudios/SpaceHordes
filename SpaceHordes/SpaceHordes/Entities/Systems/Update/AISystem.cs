using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;

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
            //e.GetComponent<AI>().TargetChangedEvent +=
            //    () =>
            //    {
            //        AI a = e.GetComponent<AI>();

            //        //if (a.Target == null)
            //        //    return;
            //        ITransform b = e.GetComponent<ITransform>();
            //        IVelocity v = e.GetComponent<IVelocity>();
            //        Vector2 Velocity = (a.Target.Position - b.Position);
            //        if (Velocity != Vector2.Zero)
            //        {
            //            float speed = 5f;

            //            if (e.Tag == "Boss")
            //                speed = 1f;

            //            Velocity.Normalize();
            //            Velocity *= speed;
            //            v.LinearVelocity = Velocity;
            //            if (e.Tag != "Boss")
            //                b.RotateTo(Velocity);
            //        }
            //    };

            base.Added(e);
        }

        public override void Process(Entity e)
        {
            AI ai = e.GetComponent<AI>();
            if(ai.Target != null)
                if (ai.Behavior(e, ai.Target)) //Run ai behavior, if behavior returns true look for new target.
                {
                    ai.Target = _FindNewTarget(ai, e.GetComponent<Body>().Position);
                    e.Refresh();
                }
        }


        private Body _FindNewTarget(AI ai, Vector2 location)
        {
            throw new NotImplementedException();
        }

        public Entity StrongestEntity(PhysicsBody[] list)
        {
            Entity strongest = default(Entity);
            double mostHealth = 0.0;
            foreach (PhysicsBody b in list)
            {
                Entity e = b.UserData as Entity;
                if (e.HasComponent<Health>())
                {
                    double health = e.GetComponent<Health>().CurrentHealth;
                    if (health > mostHealth)
                    {
                        mostHealth = health;
                        strongest = e;
                    }
                }
            }
            return strongest;
        }

        public Entity WeakestEntity(PhysicsBody[] list)
        {
            Entity weakest = default(Entity);
            double leastHealth = 0.0;
            foreach (PhysicsBody b in list)
            {
                Entity e = b.UserData as Entity;
                if (e.HasComponent<Health>())
                {
                    double health = e.GetComponent<Health>().CurrentHealth;
                    if (health < leastHealth)
                    {
                        leastHealth = health;
                        weakest = e;
                    }
                }
            }
            return weakest;
        }

        public Body ClosestTarget(Entity e)
        {
            AI a = e.GetComponent<AI>();
            PhysicsBody pb = world.GetClosestBody(e.GetComponent<ITransform>().Position, a.TargetGroup);
            Body b = new Body(world, pb.UserData as Entity);
            return b;
        }
    }
}