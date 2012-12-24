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
    public class PhysicsSystem : EntityProcessingSystem
    {
        ComponentMapper<Physical> physicalMapper;
        public PhysicsSystem() : base(typeof(Physical)){
        }
        public override void Added(Entity e)
        {
            Dictionary<string, Physical> bodies = physicalMapper.Get(e);
            foreach(string key in bodies.Keys)
                bodies[key].OnCollision+= this.OnCollision;
            base.Added(e);
        }

        public override void Initialize()
        {
            physicalMapper = new ComponentMapper<Physical>(world);
        }
        
        public override void Process(Entity e)
        {
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
