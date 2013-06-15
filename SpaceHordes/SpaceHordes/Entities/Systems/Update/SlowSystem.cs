using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;

namespace SpaceHordes.Entities.Systems
{
    public class SlowSystem : EntityProcessingSystem
    {
        private ComponentMapper<IVelocity> velocityMapper;
        private ComponentMapper<IDamping> dampingMapper;
        private ComponentMapper<Slow> slowMapper;
        private static Random r = new Random();

        public SlowSystem()
            : base(typeof(IVelocity), typeof(Slow))
        {
        }

        public override void Initialize()
        {
            velocityMapper = new ComponentMapper<IVelocity>(world);
            dampingMapper = new ComponentMapper<IDamping>(world);
            slowMapper = new ComponentMapper<Slow>(world);
        }

        public override void Process(Entity e)
        {
            Slow slow = slowMapper.Get(e);
            if (slow != null && slow != Slow.None) //If particle is slowing
            {
                slow.Elapsed--;
                if (slow.Elapsed <= 0)
                {
                    e.RemoveComponent<Slow>(slow);
                    IDamping d = dampingMapper.Get(e);
                    d.LinearDamping = 0;
                    d.AngularDamping = 0;
                    if (e.HasComponent<AI>())
                    {
                        AI a = e.GetComponent<AI>();
                        e.RemoveComponent<AI>(e.GetComponent<AI>());
                        a.Calculated = false;
                        e.AddComponent<AI>(a);
                    }

                    e.Refresh();
                    return;
                }
                IVelocity velocity = velocityMapper.Get(e);
                IDamping damping = dampingMapper.Get(e);

                //Slow particle angular speed
                if (velocity.AngularVelocity > slow.AngularTargetVelocity || damping.AngularDamping != slow.AngularSlowRate)
                    damping.AngularDamping = slow.AngularSlowRate;
                else
                    damping.AngularDamping = 0;

                //Slow particle linear speed
                if (velocity.LinearVelocity.Length() - slow.LinearTargetVelocity.Length() > 1 || damping.LinearDamping != slow.LinearSlowRate)
                    damping.LinearDamping = slow.LinearSlowRate;
                else
                    damping.LinearDamping = 0;

                SpawnFrostEffect(e);
            }
        }

        public void SpawnFrostEffect(Entity e)
        {
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
            world.CreateEntity("FrostParticle", e, ConvertUnits.ToSimUnits(offset)).Refresh();
        }
    }
}