using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using System;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Systems
{
    public class DamageSystem : EntityProcessingSystem
    {
        public DamageSystem()
            : base(typeof(Damage))
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Process()
        {
            base.Process();
        }

        public override void Process(Entity e)
        {
            Damage d = e.GetComponent<Damage>();
            Health h = e.GetComponent<Health>();

            if (d.Seconds <= 0)
            {
                e.RemoveComponent<Damage>(d);
                e.Refresh();

                return;
            }

            d.Seconds -= (float)world.Delta / 1000;

            h.SetHealth(e, h.CurrentHealth - d.DamagePerSecond * (world.Delta / 1000));
            world.CreateEntity("GREEENFAIRY", e).Refresh();
        }
    }
}