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
                    AI a = e.GetComponent<AI>();
                    if (a.Target == null)
                        return;
                    ITransform b = e.GetComponent<ITransform>();
                    IVelocity v = e.GetComponent<IVelocity>();
                    Vector2 Velocity = (a.Target.Position - b.Position);
                    if (Velocity != Vector2.Zero)
                    {
                        float speed = 5f;

                        if (e.Tag == "Boss")
                            speed = 1f;


                        Velocity.Normalize();
                        Velocity *= speed;
                        v.LinearVelocity = Velocity;
                        if (e.Tag != "Boss")
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
                case Targeting.Closest:
                    ai.Target = ClosestTarget(e);
                    break;

                case Targeting.Strongest:
                    ai.Target = StrongestEntity(world.GetBodiesInArea(e.GetComponent<ITransform>().Position, ai.SearchRadius)).GetComponent<Body>();
                    break;

                case Targeting.Weakest:
                    ai.Target = WeakestEntity(world.GetBodiesInArea(e.GetComponent<ITransform>().Position, ai.SearchRadius)).GetComponent<Body>();
                    break;
            }

            if (e.Group == "Crystals")
            {
                if ((ai.Target.UserData as Entity).DeletingState != true)
                {
                    Vector2 distance = e.GetComponent<AI>().Target.Position - e.GetComponent<Body>().Position;
                    distance.Normalize();
                    e.GetComponent<Body>().LinearVelocity = distance * new Vector2(7);
                }
                else
                {
                    e.Delete();
                    //ai.Target = ClosestTarget(e);
                    
                }
            }
            e.RemoveComponent<AI>(e.GetComponent<AI>());
            e.AddComponent<AI>(ai);
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
            PhysicsBody pb = world.GetClosestBody(e.GetComponent<ITransform>().Position, a.HostileGroup);
            Body b = new Body(world, pb.UserData as Entity);
            return b;
        }
    }
}