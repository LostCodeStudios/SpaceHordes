using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary;
using SpaceHordes.Entities.Components;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    class GunSystem : EntityProcessingSystem
    {
        ComponentMapper<Transform> transformMapper;
        ComponentMapper<Gun> gunMapper;

        public GunSystem() : base(typeof(Gun),typeof(Transform))
        {
        }

        public override void Initialize()
        {
            gunMapper = new ComponentMapper<Gun>(world);
            transformMapper = new ComponentMapper<Transform>(world);
        }

        public override void Process(Entity e)
        {
            //Process guns
            Dictionary<string, Gun> guns = gunMapper.Get(e);
            Dictionary<string, Transform> transforms = transformMapper.Get(e);
            if (transforms != null && transforms.Count > 0)
            {
                foreach (string key in guns.Keys)
                {
                    for (; guns[key].Ammunition > 0 && guns[key].BulletsToFire > 0; guns[key].Ammunition--)
                    {
                        guns[key].BulletsToFire--;
                        
                        Entity bullet = world.CreateEntity(guns[key].BulletTemplateTag, transforms.Values.First());
                        bullet.Refresh();
                    }
                }
            }

        }
    }
}
