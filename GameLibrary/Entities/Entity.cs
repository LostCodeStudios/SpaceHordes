using System;

#if !XBOX && !WINDOWS_PHONE

using System.Numerics;
using System.Collections.Generic;

#endif

#if XBOX || WINDOWS_PHONE
using BigInteger = System.Int32;
#endif

namespace GameLibrary.Entities
{
	public sealed class Entity
	{
		private int id;
		private long uniqueId;
		private BigInteger typeBits = 0;
		private BigInteger systemBits = 0;

		private EntityWorld world;
		private EntityManager entityManager;
		private bool enabled = true;
		private bool refreshingState = false;
		private bool deletingState = false;

		internal Entity(EntityWorld world, int id)
		{
			this.world = world;
			this.entityManager = world.EntityManager;
			this.id = id;
		}

		/**
		 * The internal id for this entity within the framework. No other entity will have the same ID, but
		 * ID's are however reused so another entity may acquire this ID if the previous entity was deleted.
		 *
		 * @return id of the entity.
		 */

		public int Id
		{
			get { return id; }
		}

		public long UniqueId
		{
			get { return uniqueId; }
			internal set
			{
				System.Diagnostics.Debug.Assert(uniqueId >= 0);
				uniqueId = value;
			}
		}

		internal BigInteger TypeBits
		{
			get { return typeBits; }
			set { typeBits = value; }
		}

		internal void AddTypeBit(BigInteger bit)
		{
			typeBits |= bit;
		}

		internal void RemoveTypeBit(BigInteger bit)
		{
			typeBits &= ~bit;
		}

		public bool RefreshingState
		{
			get { return refreshingState; }
			set { refreshingState = value; }
		}

		public bool DeletingState
		{
			get { return deletingState; }
			set { deletingState = value; }
		}

		internal BigInteger SystemBits
		{
			get { return systemBits; }
			set { systemBits = value; }
		}

		internal void AddSystemBit(BigInteger bit)
		{
			systemBits |= bit;
		}

		internal void RemoveSystemBit(BigInteger bit)
		{
			systemBits &= ~bit;
		}

		public void Reset()
		{
			systemBits = 0;
			typeBits = 0;
			enabled = true;
		}

		public override String ToString()
		{
			return "Entity[" + id + "]";
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		/**
		 * Add a component to this entity.
		 * @param component to add to this entity
		 */

		public Component AddComponent(string componentName, Component component)
		{
			System.Diagnostics.Debug.Assert(component != null);
			return entityManager.AddComponent(this, componentName, component);
		}

		public T AddComponent<T>(string componentName, T component) where T : Component
		{
			System.Diagnostics.Debug.Assert(component != null);
			return entityManager.AddComponent<T>(this, componentName, component);
		}
		/**
		 * Removes the component from this entity.
		 * @param component to remove from this entity.
		 */

		public void RemoveComponent<T>(string name) where T : Component
		{
			entityManager.RemoveComponent<T>(this, name);
		}

		/**
		 * Faster removal of components from a entity.
		 * @param component to remove from this entity.
		 */

		public void RemoveComponentsOfType(ComponentType type)
		{
			System.Diagnostics.Debug.Assert(type != null);
			entityManager.RemoveComponentsOfType(this, type);
		}

        /// <summary>
        /// Removes a component of this type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveComponentsOfType<T>() where T: Component
        {
            entityManager.RemoveComponentsOfType(this, ComponentTypeManager.GetTypeFor<T>());
        }

		/**
		 * Checks if the entity has been deleted from somewhere.
		 * @return if it's active.
		 */

		public bool isActive
		{
			get
			{
				return entityManager.IsActive(id);
			}
		}

		/**
		 * This is the preferred method to use when retrieving a component from a entity. It will provide good performance.
		 *
		 * @param type in order to retrieve the component fast you must provide a ComponentType instance for the expected component.
		 * @return
		 */

        ///Gets component of certain type
        public Dictionary<string,T> GetComponents<T>() where T : Component
        {
            return entityManager.GetComponents<T>(this);
        }

        public Component[] GetComponents(ComponentType type)
        {
            return entityManager.GetComponents(this, type);
        }

		/**
		 * Slower retrieval of components from this entity. Minimize usage of this, but is fine to use e.g. when creating new entities
		 * and setting data in components.
		 * @param <T> the expected return component type.
		 * @param type the expected return component type.
		 * @return component that matches, or null if none is found.
		 */

		public T GetComponent<T>(string componentName) where T : Component
		{
            return (T)entityManager.GetComponent<T>(this, componentName);
		}

		public bool HasComponent<T>(string name) where T : Component
		{
			return (T)GetComponent<T>(name) != null;
		}

		/**
		 * Get all components belonging to this entity.
		 * WARNING. Use only for debugging purposes, it is dead slow.
		 * WARNING. The returned Dictionary is only valid until this method is called again, then it is overwritten.
		 * @return all components of this entity.
		 */

        public Bag<Component> Components
		{
			get
			{
				return entityManager.GetComponentsOfEntity(this);
			}
		}

		/**
		 * Refresh all changes to components for this entity. After adding or removing components, you must call
		 * this method. It will update all relevant systems.
		 * It is typical to call this after adding components to a newly created entity.
		 */

		public void Refresh()
		{
			if (refreshingState == true)
			{
				return;
			}
			world.RefreshEntity(this);
			refreshingState = true;
		}

		/**
		 * Delete this entity from the world.
		 */

		public void Delete()
		{
			if (deletingState == true)
			{
				return;
			}
			world.DeleteEntity(this);
			deletingState = true;
		}

		/**
		 * Set the group of the entity. Same as World.setGroup().
		 * @param group of the entity.
		 */

		public String Group
		{
			get
			{
				return world.GroupManager.GetGroupOf(this);
			}
			set
			{
				world.GroupManager.Set(value, this);
			}
		}

		/**
		 * Assign a tag to this entity. Same as World.setTag().
		 * @param tag of the entity.
		 */

		public String Tag
		{
			get
			{
				return world.TagManager.GetTagOfEntity(this);
			}
			set
			{
				world.TagManager.Register(value, this);
			}
		}
	}
}