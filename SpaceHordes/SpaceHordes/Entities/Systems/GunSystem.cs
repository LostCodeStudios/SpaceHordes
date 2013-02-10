using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary;
using SpaceHordes.Entities.Components;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Systems
{
    class GunSystem : IntervalEntityProcessingSystem
    {
        ComponentMapper<ITransform> transformMapper;
        ComponentMapper<Inventory> invMapper;

        int elapsedMilli = 0;

        public GunSystem() : base(16, typeof(Inventory),typeof(ITransform))
        {
        }

        public override void Initialize()
        {
            invMapper = new ComponentMapper<Inventory>(world);
            transformMapper = new ComponentMapper<ITransform>(world);
        }

        public override void Process()
        {
            elapsedMilli += 16;
            base.Process();
        }

        public override void Process(Entity e)
        {
            //Process guns
            Inventory inv = invMapper.Get(e);
            Gun gun = inv.CurrentGun;
            ITransform transform = transformMapper.Get(e);

            gun.Elapsed += elapsedMilli;
            //Fire bullets bro
            if (gun.Elapsed > gun.Interval && gun.BulletsToFire)
            {
                    gun.BulletsToFire = false;

                    Entity bullet = world.CreateEntity(gun.BulletTemplateTag, transform);
                    bullet.Refresh();

                    gun.Elapsed = 0;
            }
        }
    }
}
