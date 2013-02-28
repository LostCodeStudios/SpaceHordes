using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using GameLibrary.Dependencies.Physics.Dynamics;

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
            AI ai = e.GetComponent<AI>();

            switch (ai.Targeting)
            {
                //case Targeting.Closest:
                //    ai.Target = new Body(world.GetClosestBody(e.GetComponent<ITransform>().Position));
                //    break;

                case Targeting.Strongest:
                    ai.Target = StrongestEntity(world.GetBodiesInArea(e.GetComponent<ITransform>().Position, ai.SearchRadius)).GetComponent<Body>();
                    break;

                case Targeting.Weakest:
                    ai.Target = WeakestEntity(world.GetBodiesInArea(e.GetComponent<ITransform>().Position, ai.SearchRadius)).GetComponent<Body>();
                    break;
            }

            if (e.Group == "Crystals" && ai.Target != null)
            {
                Vector2 distance = e.GetComponent<AI>().Target.Position - e.GetComponent<Body>().Position;
                distance.Normalize();
                e.GetComponent<Body>().LinearVelocity = distance * new Vector2(7);
            }
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
    }
}