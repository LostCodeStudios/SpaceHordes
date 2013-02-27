using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;

namespace GameLibrary.Entities.Systems
{
    public class ParticleMovementSystem : EntityProcessingSystem
    {
        private ComponentMapper<Particle> particleMapper;

        public ParticleMovementSystem()
            : base(typeof(Particle))
        {
        }

        public override void Initialize()
        {
            particleMapper = new ComponentMapper<Particle>(world);
        }

        public override void Process(Entity e)
        {
            Particle particle = particleMapper.Get(e);

            float dt = World.Delta / 1000f;

            //Damping (dx/dt) = (-c*dx/dt) * DT
            particle.AngularVelocity += -particle.AngularDamping * dt * particle.AngularVelocity;
            particle.LinearVelocity += new Vector2(-particle.LinearDamping * dt) * particle.LinearVelocity;

            //Add the velocity of a particle to its transform
            particle.Position += particle.LinearVelocity * new Vector2(dt); //x = int(dx/dt)[delta t]
            particle.Rotation += particle.AngularVelocity * (dt); //theta = int(dtheta/dt)[delta t]
        }
    }
}