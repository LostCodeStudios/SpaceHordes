using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
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
            : base(typeof(IVelocity), typeof(IDamping))
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
                    Sprite s = e.GetComponent<Sprite>();
                    e.RemoveComponent(ComponentTypeManager.GetTypeFor<Sprite>());
                    s.Color = Color.White;
                    e.AddComponent<Sprite>(s);
                    e.RemoveComponent<Slow>(slow);

                    if (e.HasComponent<AI>())
                    {
                        AI a = e.GetComponent<AI>();
                        a.Target = a.Target;
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

                if (e.HasComponent<Sprite>())
                {
                    Sprite s = e.GetComponent<Sprite>();
                    if (s.Color != Color.Blue)
                    {
                        e.RemoveComponent(ComponentTypeManager.GetTypeFor<Sprite>());
                        s.Color = Color.LightBlue;
                        e.AddComponent<Sprite>(s);
                        e.Refresh();
                    }
                }
            }
        }
    }
}