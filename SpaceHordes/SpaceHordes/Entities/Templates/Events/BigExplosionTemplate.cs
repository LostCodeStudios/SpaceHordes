using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Templates
{
    public class BigExplosionTemplate : IEntityGroupTemplate
    {
        private static Random r = new Random();
        private List<Entity> explosions = new List<Entity>();

        private SpriteSheet spriteSheet;

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
            int intensity = 15;
            Entity ent = (Entity)args[2];
            Vector2 velocity = (Vector2)args[3];

            int[] size = new int[4];

            for (int i = 1; i < 4; ++i)
            {
                size[i] = intensity / (i + 1);
                float radius = 0;
                for (int k = 5 - i; k > i; k--)
                {
                    radius += (float)spriteSheet["splosion" + k.ToString()][0].Width / (k == i + 1 && i == 2 ? 2f : 3f);
                }
                radius = ConvertUnits.ToSimUnits(radius);
                Vector2 offset;

                double max = Math.PI * 2;
                double step = (Math.PI * 2) / (double)size[i];
                for (double angle = 0; angle < max; angle += step)
                {
                    offset = new Vector2(radius * (float)Math.Cos(angle), radius * (float)Math.Sin(angle));

                    world.CreateEntity("Explosion", 0.05f * (i + 1), center + offset, ent, (i + 1), velocity).Refresh();
                }
            }

            return explosions.ToArray();
        }
    }
}