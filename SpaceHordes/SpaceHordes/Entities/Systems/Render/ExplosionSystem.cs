using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    public class ExplosionSystem : GroupSystem
    {
        public ExplosionSystem()
            : base("Explosions")
        {
        }

        public override void Process(Entity e)
        {
            Animation a = e.GetComponent<Animation>();

            if (a.Type == AnimationType.None)
            {
                e.Delete();
            }
        }
    }
}