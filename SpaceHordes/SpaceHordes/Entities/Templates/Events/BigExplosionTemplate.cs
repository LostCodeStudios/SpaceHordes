using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Templates
{
    public class BigExplosionTemplate : IEntityGroupTemplate
    {
        List<Entity> explosions = new List<Entity>();
        SpriteSheet spriteSheet;

        public BigExplosionTemplate(SpriteSheet sheet)
        {
            spriteSheet = sheet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="world"></param>
        /// <param name="args">args[0] = center location, args[1] = explosion intensity, args[2] = Entity ent</param>
        /// <returns></returns>
        public Entity[] BuildEntityGroup(EntityWorld world, params object[] args)
        {
            Vector2 center = (Vector2)args[0];
            int intensity = (int)args[1];
            Entity ent = (Entity)args[2];

            int[] size = new int[4];

            for (int i = 4; i >= 0; i--)
            {
                size[i] = intensity / i;
                int radius = spriteSheet["splosion" + i.ToString()][0].Width/2;

                Vector2 offset = new Vector2(0, radius);

                world.CreateEntity("Explosion", 0.5f, center + offset, ent, i);

                intensity %= i;
            }

            return explosions.ToArray();
        }
    }
}
