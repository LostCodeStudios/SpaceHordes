using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceHordes.Entities.Templates.Events
{
    class BaseShotTemplate : IEntityGroupTemplate
    {
        public static double ShotRatio = 0.22;
#if XBOX
        public static double MaxShot = 40;
#endif

#if WINDOWS
        public static double MaxShot = 500;
#endif
        public Entity[] BuildEntityGroup(EntityWorld world, params object[] args)
        {
            Inventory inv = (args[0] as Entity).GetComponent<Inventory>();
            List<Entity> rets = new List<Entity>();

            //If the bullet type is not white
            if (inv.YELLOW > 0)
            {
                int shot = 1;
                SoundManager.Play("Shot" + shot.ToString(), .25f);
                //Shoot bullets out from base at an even division of the circle * a shot ratio

                double max = Math.PI * 2;
                double step = (Math.PI * 2 * ShotRatio) / (Math.Min(inv.YELLOW, MaxShot));
                for (double angle = 0;
                    angle < max;
                    angle += step)
                {
                    Transform fireAt = new Transform(Vector2.Zero, (float)angle);
                    
                    Entity bullet = world.CreateEntity("WhiteBullet3", fireAt);

                    Bullet bb = bullet.GetComponent<Bullet>();
                    bb.Firer = null;
                    bullet.RemoveComponent<Bullet>(bullet.GetComponent<Bullet>());
                    bullet.AddComponent<Bullet>(bb);
                    bullet.Refresh();

                    rets.Add(bullet);
                }
                inv.YELLOW = 0;
            }
            return rets.ToArray();
        }
    }
}
