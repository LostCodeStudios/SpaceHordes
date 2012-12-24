using System;
using System.Collections.Generic;

namespace GameLibrary.Entities
{
	public delegate void RemovedComponentHandler(Entity e, Component c);

	public delegate void RemovedEntityHandler(Entity e);

	public delegate void AddedComponentHandler(Entity e, Component c);

	public delegate void AddedEntityHandler(Entity e);

	public sealed class EntityManager
	{
		private EntityWorld world;
        private Bag<Entity> activeEntities = new Bag<Entity>();
        private Bag<Entity> removedAndAvailable = new Bag<Entity>();
		private int nextAvailableId;
		private int count;
		private long totalCreated;
		private long totalRemoved;

		public event RemovedComponentHandler RemovedComponentEvent;

		public event RemovedEntityHandler RemovedEntityEvent;

		public event AddedComponentHandler AddedComponentEvent;

		public event AddedEntityHandler AddedEntityEvent;

		public EntityManager(EntityWorld world)
		{
			System.Diagnostics.Debug.Assert(world != null);
			this.world = world;
		}

		/// <summary>
		/// Create a new, "blank" entity
		/// </summary>
		/// <returns>New entity</returns>
		public Entity Create()
		{
			Entity e = removedAndAvailable.RemoveLast();
			if (e == null)
			{
				e = new Entity(world, nextAvailableId++);
			}
			else
			{
				e.Reset();
			}
			e.UniqueId = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
			activeEntities.Set(e.Id, e);
			count++;
			totalCreated++;
			if (AddedEntityEvent != null)
			{
				AddedEntityEvent(e);
			}

            EntityComponents[e.Id] = new Bag<Dictionary<string,Component>>();
			return e;
		}

		/// <summary>
		/// Check if this entity is active, or has been deleted, within the framework.
		/// </summary>
		/// <param name="entityId">entityId</param>
		/// <returns>active or not.</returns>
		public bool IsActive(int entityId)
		{
			return activeEntities.Get(entityId) != null;
		}

        /// <summary>
		/// Ensure the any changes to components are synced up with the entity - ensure systems "see" all components
		/// </summary>
		/// <param name="e">The entity whose components you want to refresh</param>
		internal void Refresh(Entity e)
		{
			SystemManager systemManager = world.SystemManager;
            Bag<EntitySystem> systems = systemManager.Systems;
			for (int i = 0, s = systems.Size; s > i; i++)
			{
				systems.Get(i).OnChange(e);
			}
		}

		/// <summary>
		/// Get the entity for the given entityId
		/// </summary>
		/// <param name="entityId">Desired EntityId</param>
		/// <returns>Entity</returns>
		public Entity GetEntity(int entityId)
		{
			System.Diagnostics.Debug.Assert(entityId >= 0);

			return activeEntities.Get(entityId);
		}

		/// <summary>
		/// Get how many entities are currently active
		/// </summary>
		/// <returns>How many entities are currently active</returns>
		public int ActiveEntitiesCount
		{
			get { return count; }
		}

        /// <summary>
        /// Get all active Entities
        /// </summary>
        /// <returns>Bag of active entities</returns>
        public Bag<Entity> ActiveEntities
        {
            get { return activeEntities; }
        }

		/// <summary>
		/// Get how many entities have been created since start.
		/// </summary>
		/// <returns>The total number of entities created</returns>
		public long TotalCreated
		{
			get { return totalCreated; }
		}

		/// <summary>
		/// Gets how many entities have been removed since start.
		/// </summary>
		/// <returns>The total number of removed entities</returns>
		public long TotalRemoved
		{
			get { return totalRemoved; }
		}

        #region Components
        public Bag<Bag<Dictionary<string, Component>>> EntityComponents = new Bag<Bag<Dictionary<string, Component>>>(); //1st = Bag of entities; 2nd = Types of components; 3rd = Dictionary of components in that entity of that type with string ids.

        #region Add
        #region AddComponent
        /// <summary>
        /// Add the given component to the given entity
        /// </summary>
        /// <param name="e">Entty for which you want to add the component</param>
        /// <param name="component">Component you want to add</param>
        internal Component AddComponent(Entity e, string componentName, Component component)
        {
            return this.AddComponent<Component>(e, componentName, component);
        }

        /// <summary>
        /// Add a component to the given entity
        /// If the component's type does not already exist, add it to the Bag of availalbe component types
        /// </summary>
        /// <typeparam name="T">Component type you want to add</typeparam>
        /// <param name="e">The entity to which you want to add the component</param>
        /// <param name="component">The component instance you want to add</param>
        internal T AddComponent<T>(Entity e, string componentName, T component) where T : Component
        {
            if (e.Id < EntityComponents.Size) //If entity exists
            {
                if (!(ComponentTypeManager.GetTypeFor(component.GetType()).Id
                    < EntityComponents.Get(e.Id).Size)) //If type in entity does not exist
                {
                    EntityComponents[e.Id][ComponentTypeManager.GetTypeFor(component.GetType()).Id] = new Dictionary<string, Component>();
                    e.AddTypeBit(ComponentTypeManager.GetTypeFor(component.GetType()).Bit);
                }
                //Update components
                {
                    //Add the components
                    Dictionary<string, Component> d = EntityComponents.Get(e.Id).Get(ComponentTypeManager.GetTypeFor(component.GetType()).Id);
                    d.Add(componentName, component);

                    return component;
                }
                
            }
            return default(T);
        }
        #endregion

        #region AddComponents
        /// <summary>
        /// Adds components of type component to an entity
        /// </summary>
        /// <param name="e">The entity to which the components will be added.</param>
        /// <param name="component">The first (required component)</param>
        /// <param name="otherComponents">The other components to add.</param>
        internal void AddComponents(Entity e, KeyValuePair<string, Component> component, params KeyValuePair<string, Component>[] otherComponents)
        {
            this.AddComponents<Component>(e, component, otherComponents);
        }



        /// <summary>
        /// Adds components of a user defined type to an entity
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <param name="e">The entity to which the components will be added.</param>
        /// <param name="component">The first (required component)</param>
        /// <param name="otherComponents">The other components to add.</param>
        internal void AddComponents<T>(Entity e, KeyValuePair<string, T> component, params KeyValuePair<string, T>[] otherComponents) where T : Component
        {
            //Null assertion
            System.Diagnostics.Debug.Assert(e != null);

            this.AddComponent<Component>(e, component.Key, component.Value);
            foreach (KeyValuePair<string, T> c in otherComponents)
            {
                this.AddComponent<Component>(e, component.Key, component.Value);
            }
        }

        #endregion
        #endregion

        #region Remove


        /// <summary>
        /// Remove an entity from the world
        /// </summary>
        /// <param name="e">Entity you want to remove</param>
        public void Remove(Entity e)
        {
            System.Diagnostics.Debug.Assert(e != null);
            activeEntities.Set(e.Id, null);

            e.TypeBits = 0;

            Refresh(e);

            RemoveComponentsOfEntity(e);

            count--;
            totalRemoved++;

            removedAndAvailable.Add(e);
            if (RemovedEntityEvent != null)
            {
                RemovedEntityEvent(e);
            }
        }

        /// <summary>
        /// Strips all components from the given entity
        /// </summary>
        /// <param name="e">Entity for which you want to remove all components</param>
        internal void RemoveComponentsOfEntity(Entity e)
        {
            System.Diagnostics.Debug.Assert(e != null);

            if (e.Id < EntityComponents.Size)
            {
                EntityComponents.Remove(e.Id);
                //TODO: ADD EVENT FOR RMEOVED
            }


            //int entityId = e.Id;
            //for (int a = 0, b = _ComponentsByType.Size; b > a; a++)
            //{
            //    Bag<Bag<Component>> componentsOfTypeA = _ComponentsByType.Get(a);
            //    if (componentsOfTypeA != null && entityId < componentsOfTypeA.Size)
            //    {
            //        if (RemovedComponentEvent != null)
            //        {
            //            RemovedComponentEvent(e, componentsOfTypeA.Get(entityId));
            //        }
            //        componentsOfTypeA.Set(entityId, null);
            //    }
            //}
        }

        /// <summary>
        /// Removes the given component from the given entity
        /// </summary>
        /// <typeparam name="T">The type of the component you want to remove</typeparam>
        /// <param name="e">The entity for which you are removing the component</param>
        /// <param name="component">The specific component instance you want removed</param>
        internal void RemoveComponent<T>(Entity e, string componentName) where T : Component
        {
            System.Diagnostics.Debug.Assert(e != null);
            ComponentType type = ComponentTypeManager.GetTypeFor<T>();
            RemoveComponent(e, type, componentName);
        }

        /// <summary>
        /// Reemoves the given component type from the given entity
        /// </summary>
        /// <param name="e">The entity for which you want to remove the component</param>
        /// <param name="type">The component type you want to remove</param>
        internal void RemoveComponent(Entity e, ComponentType type, string componentName)
        {
            System.Diagnostics.Debug.Assert(e != null);
            System.Diagnostics.Debug.Assert(type != null);

            if (EntityComponents.Size > e.Id &&
                EntityComponents.Get(e.Id).Size > type.Id) //If component type exists under the name
            {
                Dictionary<string, Component> d = EntityComponents.Get(e.Id).Get(type.Id);
                Component c = d[componentName];
                d.Remove(componentName); //Remove it.
                if (d.Keys.Count == 0) //Remove type from entity
                    e.RemoveTypeBit(type.Bit);
                //Event
                if (RemovedComponentEvent != null && c != null)
                {
                    RemovedComponentEvent(e, c);
                }
                //Set the original colleciton.
                EntityComponents.Set(e.Id, EntityComponents.Get(e.Id).Set(type.Id, d));
            }

        }

        /// <summary>
        /// Removes a component based on a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        internal void RemoveComponents<T>(Entity e) where T : Component
        {
            this.RemoveComponentsOfType(e, ComponentTypeManager.GetTypeFor<T>());
        }

        internal void RemoveComponentsOfType(Entity e, ComponentType type)
        {
            if (e.Id < EntityComponents.Size && EntityComponents.Get(e.Id).Size > type.Id)
                EntityComponents.Get(e.Id).Remove(type.Id);
        }

        #endregion

        #region Get
        /// <summary>
        /// Get all components assigned to an entity
        /// </summary>
        /// <param name="e">Entity for which you want the components</param>
        /// <returns>Bag of components</returns>
        public Bag<Component> GetComponentsOfEntity(Entity e)
        {
            System.Diagnostics.Debug.Assert(e != null);
            if (e.Id < EntityComponents.Size) //If components in bag
            {
                Bag<Component> components = new Bag<Component>();
                for (int type = 0; type < EntityComponents.Get(e.Id).Size; type++)
                {
                    Dictionary<string, Component> current = EntityComponents.Get(e.Id).Get(type);
                    foreach (string key in current.Keys)
                        components.Add(current[key]);
                }
                return components;
            }
            return null;
        }

        /// <summary>
        /// Gets the component with a specified name and type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        internal Component GetComponent<T>(Entity e, string componentName) where T : Component
        {
            return this.GetComponent(e, ComponentTypeManager.GetTypeFor<T>(), componentName);
        }

        /// <summary>
        /// Gets the component with a specified name and type.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="type"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        internal Component GetComponent(Entity e, ComponentType type, string componentName)
        {
            if (e.Id < EntityComponents.Size && EntityComponents.Get(e.Id).Size > type.Id)
                return EntityComponents.Get(e.Id).Get(type.Id)[componentName];
            else
                return null;
        }

        internal Dictionary<string, T> GetComponents<T>(Entity e) where T : Component
        {
            ComponentType type = ComponentTypeManager.GetTypeFor<T>();

            if (e.Id < EntityComponents.Size && EntityComponents.Get(e.Id).Size > type.Id)
            {
                Dictionary<string, T> comps = new Dictionary<string, T>();
                foreach (string key in EntityComponents.Get(e.Id).Get(type.Id).Keys)
                {
                    comps[key] = (T)EntityComponents.Get(e.Id).Get(type.Id)[key];
                }
                return comps;
            }
            else
                return null; 

        }

        internal Component[] GetComponents(Entity e, ComponentType type)
        {
            if (e.Id < EntityComponents.Size && EntityComponents.Get(e.Id).Size > type.Id)
            {
                Component[] comps = new Component[EntityComponents.Get(e.Id).Get(type.Id).Count];
                int i = 0;
                foreach (string key in EntityComponents.Get(e.Id).Get(type.Id).Keys)
                {
                    comps[i] = EntityComponents.Get(e.Id).Get(type.Id)[key];
                    i++;
                }
                return comps;
            }
            else
                return null;
        }
        #endregion

        #endregion

	}
}