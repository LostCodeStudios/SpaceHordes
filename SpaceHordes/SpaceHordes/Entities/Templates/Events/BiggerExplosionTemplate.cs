using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Templates
{
    public class BiggerExplosionTemplate : IEntityGroupTemplate
    {
        private static Random r = new Random();
        private List<Entity> explosions = new List<Entity>();

        private SpriteSheet spriteSheet;

        public BiggerExplosionTemplate(SpriteSheet sheet)
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
            Vector2 velocity = (Vector2)args[3];
            world.CreateEntityGroup("BigExplosion", "Explosions", center, 15, ent);

            double max = Math.PI * 2;
            double step = (Math.PI * 2) / intensity;
            for (double angle = 0; angle < max; angle += step)
            {
                float radius = 75;
                radius = ConvertUnits.ToSimUnits(radius);
                Vector2 offset = new Vector2(radius * (float)Math.Cos(angle), radius * (float)Math.Sin(angle));
                offset.Normalize();
                offset *= radius;

                world.CreateEntityGroup("BigExplosion", "Explosions", center + offset, 15, ent);
            }

            return explosions.ToArray();
        }
    }
}