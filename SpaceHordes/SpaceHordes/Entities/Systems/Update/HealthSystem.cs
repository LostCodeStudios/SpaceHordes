using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Render;
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

        public override void Added(Entity e)
        {
            if (e.HasComponent<Sprite>()) //Change color to red when hit
                e.GetComponent<Health>().OnDamage +=
                    (setter) =>
                    {
                        if (!e.HasComponent<SpriteEffect>())
                        {
                            Sprite s = e.GetComponent<Sprite>();
                            s.Color = Color.Red;

                            if (!e.HasComponent<SpriteEffect>())
                                e.AddComponent<SpriteEffect>(new SpriteEffect(s, 10));
                            else
                                e.GetComponent<SpriteEffect>().AddEffect(s, 10);
                            e.Refresh();
                        }
                    };
            base.Added(e);
        }

        public override void Initialize()
        {
            healthMapper = new ComponentMapper<Health>(world);
        }

        public override void Process(Entity e)
        {
            Health h = healthMapper.Get(e);

            if (e.HasComponent<Origin>())
            {
                Origin o = e.GetComponent<Origin>();
                Entity parent = o.Parent;

                if (!parent.HasComponent<Body>() || !parent.GetComponent<Health>().IsAlive)
                {
                    e.GetComponent<Health>().SetHealth(e, 0);
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