using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Templates
{
    public class StarFieldTemplate : IEntityGroupTemplate
    {
        List<Entity> stars = new List<Entity>();
        static int starNum = 100;
        static int nebNum = 3;

        public Entity[] BuildEntityGroup(EntityWorld world, params object[] args)
        {
            for (int i = 0; i < starNum; i++)
            {
                Entity e = world.CreateEntity("Star", true);
                e.Refresh();
                stars.Add(e);
            }

            for (int i = 0; i < nebNum; i++)
            {
                Entity e = world.CreateEntity("Star", false);
                e.Refresh();
                stars.Add(e);
            }

            return stars.ToArray();
        }
    }
}
