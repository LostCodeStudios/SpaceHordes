using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    public class DamageSystem : IntervalEntityProcessingSystem
    {
        public DamageSystem()
            : base(250, typeof(Damage), typeof(Health))
        {
        }

        public override void Process(Entity e)
        {
            Damage d = e.GetComponent<Damage>();
            Health h = e.GetComponent<Health>();

            if (d.Uses == 0)
            {
                e.RemoveComponent<Damage>(d);
            }

            d.Elapsed += 250;
            if (d.Elapsed > d.Interval)
            {
                d.Elapsed = 0;
                d.Uses--;

                h.SetHealth(e, h.CurrentHealth - d.Amount);
                Console.WriteLine(e.Id.ToString() + " takes " + d.Amount + " continuous damage");
                //TODO: Add green sprite effect
            }
        }
    }
}
