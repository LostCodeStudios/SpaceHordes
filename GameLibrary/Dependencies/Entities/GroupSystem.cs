using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Dependencies.Entities
{
    public abstract class GroupSystem : EntitySystem
    {
        protected string group;
        public GroupSystem(string group) {
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(group));
            this.group = group;
        }
        
        /**
         * Process a entity this system is interested in.
         * @param e the entity to process.
         */
        public abstract void Process(Entity e);

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            Entity[] entitiesInGroup = world.GroupManager.GetEntities(group).ToArray();
            foreach (Entity e in entitiesInGroup)
            {
                Process(e);
            }
        }
    }
}
