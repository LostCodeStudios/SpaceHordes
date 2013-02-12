using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    class HealthSystem : EntityProcessingSystem
    {
        ComponentMapper<Health> healthMapper;

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
            Health health = healthMapper.Get(e);

            if (!health.IsAlive)
            {
                Vector2 pos = e.GetComponent<ITransform>().Position;
                World.CreateEntity("Explosion", 5, pos).Refresh();
                e.Delete();
            }
        }
    }
}
