using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Systems.Update
{
    public class FadingTextProcessingSystem : EntityProcessingSystem
    {
        public FadingTextProcessingSystem()
            : base(typeof(FadingText))
        {
        }

        public override void Process(Entity e)
        {
            if (!e.HasComponent<FadingText>())
                return;

            FadingText f = e.GetComponent<FadingText>();

            f.Update(world.Delta);

            if (f.Fraction() <= 0)
            {
                e.Delete();
            }
        }
    }
}