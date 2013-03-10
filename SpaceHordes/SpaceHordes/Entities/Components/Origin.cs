using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    public class Origin : Component
    {
        public Entity Parent;

        public Origin(Entity p)
        {
            Parent = p;
        }
    }
}