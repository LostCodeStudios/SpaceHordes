using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    class SlowSystem : EntityProcessingSystem
    {
        ComponentMapper<IVelocity> velocityMapper;
        ComponentMapper<IDamping> dampingMapper;
        ComponentMapper<Slow> slowMapper;

        public SlowSystem() : base(typeof(IVelocity), typeof(IDamping))
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
                IVelocity velocity = velocityMapper.Get(e);
                IDamping damping = dampingMapper.Get(e);
                //Slow particle angular speed
                if (velocity.AngularVelocity > slow.AngularTargetVelocity || damping.AngularDamping != slow.AngularSlowRate)
                    damping.AngularDamping = slow.AngularSlowRate;
                else
                    damping.AngularDamping = 0;

                //Slow particle linear speed
                if (Vector2.Distance(velocity.LinearVelocity, slow.LinearTargetVelocity) > 1 || damping.LinearDamping != slow.LinearSlowRate)
                    damping.LinearDamping = slow.LinearSlowRate;
                else
                    damping.LinearDamping = 0;

                if (e.HasComponent<Sprite>())
                {
                    Sprite s = e.GetComponent<Sprite>();
                    if (s.Color != Color.Blue)
                    {
                        e.RemoveComponent(ComponentTypeManager.GetTypeFor<Sprite>());
                        s.Color = Color.Blue;
                        e.AddComponent<Sprite>(s);
                    }
                }


            }

        }
    }
}
