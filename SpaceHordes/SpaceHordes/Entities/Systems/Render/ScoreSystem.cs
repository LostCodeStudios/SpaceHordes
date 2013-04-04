using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Systems
{
    public class ScoreSystem : IntervalEntitySystem
    {
        private Entity Base;
        private static SpaceWorld _world;
        private static long pointsToGive;

        public static void GivePoints(int value)
        {
            if (!_world.Tutorial)
                pointsToGive += value;
        }

        public ScoreSystem(SpaceWorld world)
            : base(100)
        {
            _world = world;
            pointsToGive = 0;
        }

        public void LoadContent(Entity Base)
        {
            this.Base = Base;
        }

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);

            if (Base.HasComponent<Score>())
            {
                Score s = Base.GetComponent<Score>();

                s.Value += pointsToGive;
                pointsToGive = 0;
            }
        }
    }
}