using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
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

        private Random r = new Random();

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

            Vector2 offset;
            if (!e.Tag.Contains("Boss"))
            {
                double mes = Math.Sqrt(s.CurrentRectangle.Width * s.CurrentRectangle.Height / 4);
                offset = new Vector2((float)((r.NextDouble() * 2 - 1) * mes), (float)((r.NextDouble() * 2 - 1) * mes));
            }
            else
            {
                offset = new Vector2((float)((r.NextDouble() * 2 - 1) * s.CurrentRectangle.Width / 2), (float)((r.NextDouble() * 2 - 1) * s.CurrentRectangle.Height / 2));
            }
            world.CreateEntity("GREENFAIRY", e, ConvertUnits.ToSimUnits(offset)).Refresh();
        }
    }
}