using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Physics.Dynamics.Contacts;
using GameLibrary.Entities;
using GameLibrary.Entities.Components;

namespace GameLibrary.Entities.Systems
{
    /// <summary>
    /// A system which updates the transforms && velocities of entities to the entities' physical body
    /// </summary>
    public class PhysicsUpdateSystem : EntityProcessingSystem
    {
        ComponentMapper<Physical> physicalMapper;
        ComponentMapper<Velocity> velocityMapper;
        ComponentMapper<Transform> transformMapper;
        public PhysicsUpdateSystem() : base(typeof(Physical), typeof(Transform), typeof(Velocity))
        {
        }

        /// <summary>
        /// If an entity is added to the physics systems, then add it to collision.
        /// </summary>
        /// <param name="e"></param>
        public override void Added(Entity e)
        {
            Dictionary<string, Physical> bodies = physicalMapper.Get(e);
            //foreach(string key in bodies.Keys)
            //    bodies[key].OnCollision+= this.OnCollision;
            base.Added(e);
        }

        public override void Initialize()
        {
            physicalMapper = new ComponentMapper<Physical>(world);
            transformMapper = new ComponentMapper<Transform>(world);
            velocityMapper = new ComponentMapper<Velocity>(world);
        }
        
        /// <summary>
        /// Bridges component system w/ physicals
        /// </summary>
        /// <param name="e"></param>
        public override void Process(Entity e)
        {
            //Check to see if values are the same between the physics system and the velocity/transform systems.
            Dictionary<string, Physical> bodies = physicalMapper.Get(e);
            Dictionary<string, Transform> transforms = transformMapper.Get(e);
            Dictionary<string, Velocity> velocities = velocityMapper.Get(e);
            if(bodies != null)
                foreach (string key in bodies.Keys)
                {
                    System.Diagnostics.Debug.Assert(transforms.ContainsKey(key) && velocities.ContainsKey(key)); //Entity not created correctly
                    #region Velocities
                    if (velocities[key]._Set) //If the user changed the velocity, set physics to user defined velocity
                    {
                        bodies[key].AngularVelocity = velocities[key].AngularVelocity;
                        bodies[key].LinearVelocity = velocities[key].LinearVelocity;
                        velocities[key]._Set = false;
                    }
                    else
                    {

                        velocities[key]._AngularVelocity = bodies[key].AngularVelocity;
                        velocities[key]._LinearVelocity = bodies[key].LinearVelocity;
                    }
                    #endregion
                    #region Transforms
                    if (transforms[key]._Set) 
                    {
                        bodies[key].Rotation = transforms[key].Rotation;
                        bodies[key].Position = transforms[key].Position;
                        transforms[key]._Set = false;
                    }
                    else 
                    {
                        transforms[key]._Rotation = bodies[key].Rotation;
                        transforms[key]._Position = bodies[key].Position;
                    }
                    #endregion

                    bodies[key].RotateTo(bodies[key].LinearVelocity);
                }
        }

        public void OnBroadphaseCollision()
        {
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            return true;
        }
    }
}
