using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    public class ScoreSystem : IntervalEntitySystem
    {
        Entity Base;

        static long pointsToGive;
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

            Score s = Base.GetComponent<Score>();

            s.Value += pointsToGive;
            pointsToGive = 0;
        }
    }
}