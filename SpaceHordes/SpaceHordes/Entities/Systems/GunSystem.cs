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
    class GunSystem : EntityProcessingSystem
    {
        ComponentMapper<ITransform> transformMapper;
        ComponentMapper<Gun> gunMapper;

        public GunSystem() : base(typeof(Gun),typeof(ITransform))
        {
        }

        public override void Initialize()
        {
            gunMapper = new ComponentMapper<Gun>(world);
            transformMapper = new ComponentMapper<ITransform>(world);
        }

        public override void Process(Entity e)
        {
            //Process guns
            Gun gun = gunMapper.Get(e);
            ITransform transform = transformMapper.Get(e);

            //Fire bullets bro
            for (; gun.Ammunition > 0 && gun.BulletsToFire > 0; gun.Ammunition--)
            {
                gun.BulletsToFire--;
                        
                Entity bullet = world.CreateEntity(gun.BulletTemplateTag, transform);
                bullet.Refresh();
            }


        }
    }
}
