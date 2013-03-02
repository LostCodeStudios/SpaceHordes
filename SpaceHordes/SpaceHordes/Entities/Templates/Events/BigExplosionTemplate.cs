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
        static Random r = new Random();
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

            for (int i = 0; i < 4; i++)
            {
                size[i] = intensity / (i + 1);
                float radius = 0;
                for (int k = 4 - i; k > i; k--)
                {
                    radius += spriteSheet["splosion" + k.ToString()][0].Width/3;
                }
                radius = ConvertUnits.ToSimUnits(radius);
                Vector2 offset;

                for (int j = 0; j < size[i]; j++)
                {
                    double x = 2 * r.NextDouble() - 1;
                    double y = 2 * r.NextDouble() - 1;

                    offset = new Vector2((float)x, (float)y);
                    offset.Normalize();
                    offset *= radius;

                    if (j == 3 && i == 3)
                        world.CreateEntity("Explosion", 0.5f, center, ent, (i+1)).Refresh();
                    else
                        world.CreateEntity("Explosion", 0.5f, center + offset, ent, (i+1)).Refresh();
                }
            }

            return explosions.ToArray();
        }
    }
}
