using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Render;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    internal class SlowSystem : EntityProcessingSystem
    {
        private ComponentMapper<IVelocity> velocityMapper;
        private ComponentMapper<IDamping> dampingMapper;
        private ComponentMapper<Slow> slowMapper;

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

        public override void Added(Entity e)
        {
            Slow slow = e.GetComponent<Slow>();
            if (e.HasComponent<Sprite>())
            {
                Sprite s = e.GetComponent<Sprite>();
                s.Color = Color.LightBlue;

                if (!e.HasComponent<SpriteEffect>())
                    e.AddComponent<SpriteEffect>(new SpriteEffect(s, slow.Elapsed));
                else
                {
                    e.GetComponent<SpriteEffect>().AddEffect(s, slow.Elapsed);
                }
                e.Refresh();
            } 
            base.Added(e);
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

                    if (e.HasComponent<AI>())
                    {
                        AI a = e.GetComponent<AI>();
                        e.RemoveComponent<AI>(e.GetComponent<AI>());
                        e.AddComponent<AI>(a);
                    }

                    e.Refresh();
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
            }
        }
    }
}