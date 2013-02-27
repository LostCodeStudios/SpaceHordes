using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Systems
{
    public class ScoreSystem : IntervalEntitySystem
    {
        private Entity Base;

        private static long pointsToGive;

        public static void GivePoints(int value)
        {
            pointsToGive += value;
        }

        public ScoreSystem()
            : base(100)
        {
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