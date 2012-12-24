using System;
using System.Collections.Generic;
namespace GameLibrary.Entities
{
	public sealed class ComponentMapper<T> where T : Component {
		private ComponentType type;
		private EntityManager em;

        /// <summary>
        /// Allows you to interact with components that are mapped to entities
        /// </summary>
        public ComponentMapper() { }

        /// <summary>
        /// Creates a component mapper within the given Entity World
        /// </summary>
        /// <param name="world">EntityWorld</param>
		public ComponentMapper(EntityWorld world) {
            System.Diagnostics.Debug.Assert(world != null);
			em = world.EntityManager;
			type = ComponentTypeManager.GetTypeFor<T>();
		}

        /// <summary>
        /// Sets the entity manager for this component mapper
        /// </summary>
        /// <param name="em">Entity Manager that manages the component</param>
        public EntityManager EntityManager 
        {
            set { em = value; }
        }
	
        /// <summary>
        /// Gets the component for the given entity/component type combo
        /// </summary>
        /// <param name="e">Entity in which you are interested</param>
        /// <returns>Component</returns>
		public T Get(Entity e, string name) {
            System.Diagnostics.Debug.Assert(e != null);
			return (T)em.GetComponent(e, type, name);
		}


        public Dictionary<string, T> Get(Entity e)
        {
            return em.GetComponents<T>(e);
        }
	}
}

