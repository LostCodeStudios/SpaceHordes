using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    public class HealthSystem : EntityProcessingSystem
    {
        ComponentMapper<Health> healthMapper;
        static Random r = new Random();

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
                        Sprite s = e.GetComponent<Sprite>();
                        if (s.Color != Color.Red)
                        {
                            e.RemoveComponent(ComponentTypeManager.GetTypeFor<Sprite>());
                            s.Color = Color.Red;
                            e.AddComponent<Sprite>(s);
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
            h.Tick--;
            if (h.Tick <= 0)
            {
                Sprite s = e.GetComponent<Sprite>();
                    e.RemoveComponent(ComponentTypeManager.GetTypeFor<Sprite>());
                    s.Color = Color.White;
                    e.AddComponent<Sprite>(s);
            }

        }
    }
}
