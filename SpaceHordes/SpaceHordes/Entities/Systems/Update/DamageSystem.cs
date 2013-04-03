using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using System;

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

            if (d.Uses == 0)
            {
                e.RemoveComponent<Damage>(d);
                e.Refresh();
                return;
            }

            d.Elapsed += 33;
            if (d.Elapsed > d.Interval)
            {
                d.Elapsed = 0;
                d.Uses--;

                h.SetHealth(e, h.CurrentHealth - d.Amount);
            }
        }
    }
}