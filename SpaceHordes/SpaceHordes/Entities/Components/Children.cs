using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Components
{
    public class Children : Component
    {
        private List<Entity> children = new List<Entity>();

        public Children(List<Entity> c)
        {
            children = c;
        }

        public void CallChildren(Body b)
        {
            foreach (Entity e in children)
            {
                if (e != null && e.HasComponent<Function>())
                {
                    Function f = e.GetComponent<Function>();

                    f.Behavior.Invoke(b);
                }
            }
        }
    }
}