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
        public Entity[] BuildEntityGroup(EntityWorld world, params object[] args)
        {
            Inventory inv = (args[0] as Entity).GetComponent<Inventory>();
            List<Entity> rets = new List<Entity>();

            //If the bullet type is not white
            if (inv.CurrentGun != inv.WHITE && inv.CurrentGun.Ammunition > 0)
            {
                int shot = 1;
                SoundManager.Play("Shot" + shot.ToString(), .25f);
                //Shoot bullets out from base at an even division of the circle * a shot ratio
                for (double angle = 0;
                    angle < Math.PI * 2;
                    angle += (Math.PI * 2 * ShotRatio) / ((double)inv.CurrentGun.Ammunition))
                {
                    Transform fireAt = new Transform(Vector2.Zero, (float)angle);
                    Entity bullet = world.CreateEntity(inv.CurrentGun.BulletTemplateTag, fireAt);
                    Bullet bb = bullet.GetComponent<Bullet>();
                    bullet.RemoveComponent<Bullet>(bullet.GetComponent<Bullet>());
                    bullet.AddComponent<Bullet>(bb);
                    bullet.Refresh();



                    rets.Add( bullet);
                }
                inv.CurrentGun.Ammunition = 0;
            }
            return rets.ToArray();
        }
    }
}
