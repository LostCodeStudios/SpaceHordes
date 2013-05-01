using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using System;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Helpers;

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
        Random r = new Random();

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

            Sprite s = e.GetComponent<Sprite>();

            double mes = Math.Sqrt(s.CurrentRectangle.Width * s.CurrentRectangle.Height / 4);
            Vector2 offset = new Vector2((float)((r.NextDouble() * 2 - 1) * mes), (float)((r.NextDouble() * 2 - 1) * mes));
            world.CreateEntity("GREENFAIRY", e, ConvertUnits.ToSimUnits(offset)).Refresh();
        }
    }
}