using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components.Render;

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
                        if (!e.HasComponent<SpriteEffect>())
                        {
                            Sprite s = e.GetComponent<Sprite>();
                            s.Color = Color.Red;

                            e.AddComponent<SpriteEffect>(new SpriteEffect(s, 10));
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
            if (!h.IsAlive)
                e.Delete();
        }
    }
}
