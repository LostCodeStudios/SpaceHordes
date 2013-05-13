using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Systems
{
    public class HealthSystem : EntityProcessingSystem
    {
        private ComponentMapper<Health> healthMapper;
        private static Random r = new Random();

        public HealthSystem()
            : base(typeof(Health))
        {
        }

        public override void Initialize()
        {
            healthMapper = new ComponentMapper<Health>(world);
        }

        public override void Process(Entity e)
        {
            Health h = healthMapper.Get(e);

            if (h == null)
                return;

            if (e.HasComponent<Origin>())
            {
                Origin o = e.GetComponent<Origin>();
                Entity parent = o.Parent;

                if (!parent.HasComponent<Body>() || !parent.GetComponent<Health>().IsAlive)
                {
                    if (e.HasComponent<Health>())
                        e.GetComponent<Health>().SetHealth(e, 0);
                    else
                        e.Delete();
                }
            }

            if (!h.IsAlive)
            {
                h.SetHealth(e, 0);
                e.Delete();
            }
        }
    }
}