using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Systems
{
    public class MovementSystem : ParallelEntityProcessingSystem
    {
        private ComponentMapper<Velocity> velocityMapper;
        private ComponentMapper<Transform> transformMapper;

        public MovementSystem()
            : base(typeof(Transform), typeof(Velocity))
        {
        }

        public override void Initialize()
        {
            velocityMapper = new ComponentMapper<Velocity>(world);
            transformMapper = new ComponentMapper<Transform>(world);
        }

        public override void Process(Entity e)
        {
            Dictionary<string, Transform> transforms = transformMapper.Get(e);
            Dictionary<string, Velocity> velocities = velocityMapper.Get(e);
            if (transforms != null)
            {
                foreach (string key in transforms.Keys)
                {
                    if(velocities.ContainsKey(key)){
                        transforms[key]._Position += velocities[key].LinearVelocity * new Vector2(World.Delta / 1000f);
                        transforms[key]._Rotation += velocities[key].AngularVelocity * (World.Delta / 1000f);
                    }
                }
            }
        }
    }
}
