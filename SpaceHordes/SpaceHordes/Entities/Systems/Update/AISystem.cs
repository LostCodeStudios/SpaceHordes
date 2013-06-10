using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Collision;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System.Collections.Generic;

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
            if (!e.HasComponent<AI>())
                return;

            AI ai = e.GetComponent<AI>();

            bool behavior;

            if (ai.Target != null)
            {
                if (World.EntityManager.GetEntity((ai.Target.UserData as Entity).Id) == null)
                    ai.Target = null;
                else if (!(!ai.Recalculate && ai.Calculated) && ai.Target != null && (ai.Target.UserData as Entity) != null && !(behavior = ai.Behavior(ai.Target))) //Run ai behavior, if behavior returns true look for new target.
                {
                    ai.Calculated = true;

                    if (ai.Target == null && e.Group != "Structures" && !e.Tag.Contains("Cannon"))
                    {
                        if (e.HasComponent<Health>())
                            e.GetComponent<Health>().SetHealth(null, 0);
                        else
                            e.Delete();
                    }

                    return;
                }
            }

            ai.Calculated = true;
            ai.Target = _FindNewTarget(ai, e.GetComponent<Body>());

            if (ai.Target == null && e.Group != "Structures" && e.Tag.Contains("Boss"))
            {
                if (e.HasComponent<Health>())
                    e.GetComponent<Health>().SetHealth(null, 0);
                else
                    e.Delete();
            }

            e.Refresh();
        }

        /// <summary>
        /// Finds a new target for AI component
        /// </summary>
        /// <param name="ai"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private Body _FindNewTarget(AI ai, Body location)
        {
            //find all fixtures in world around the location.
            AABB aabb = new AABB(location.Position, ai.SearchRadius, ai.SearchRadius);
#if WINDOWS
            HashSet<PhysicsBody> bodies = new HashSet<PhysicsBody>();
#else
            List<PhysicsBody> bodies = new List<PhysicsBody>();
#endif

            world.QueryAABB(x =>
            {
                if (x.Body.BodyId != location.BodyId)
                {
                    if (string.IsNullOrEmpty(ai.TargetGroup) || ai.TargetGroup.Equals((x.Body.UserData as Entity).Group))
                    {
                        bodies.Add(x.Body);
                    }
                }
                return true;
            }, ref aabb);

            if (bodies.Count > 0) //IF BODIES IN SEARCH RADIUS.
            {
                PhysicsBody[] list = new PhysicsBody[bodies.Count];
                bodies.CopyTo(list);
                Entity ent = (location.UserData as Entity);

                //SORT BY TARGETING TYPE.
                switch (ai.Targeting)
                {
                    case Targeting.Closest:
                        return ClosestEntity(location.Position, list).GetComponent<Body>();

                    case Targeting.Strongest:
                        return StrongestEntity(list).GetComponent<Body>();

                    case Targeting.Weakest:
                        return WeakestEntity(list).GetComponent<Body>();

                    case Targeting.None:
                        return null;

                    default:
                        return null;
                }
            }
            else
                return null;
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

        public Entity ClosestEntity(Vector2 location, PhysicsBody[] list)
        {
            PhysicsBody closest = list[0];
            foreach (PhysicsBody pb in list)
                if (Vector2.Distance(location, pb.Position) < Vector2.Distance(location, closest.Position))
                    closest = pb;

            return closest.UserData as Entity;
        }
    }
}