using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    internal class BulletCollisionSystem : EntityProcessingSystem
    {
        private ComponentMapper<Bullet> bulletMapper;
        private ComponentMapper<Particle> particleMapper;

        public BulletCollisionSystem()
            : base(typeof(Bullet), typeof(Particle))
        {
        }

        public override void Initialize()
        {
            bulletMapper = new ComponentMapper<Bullet>(world);
            particleMapper = new ComponentMapper<Particle>(world);
        }

        public override void Process(Entity e)
        {
            Particle particle = particleMapper.Get(e);
            Bullet bullet = bulletMapper.Get(e);


                world.RayCast(
                    delegate(Fixture fix, Vector2 point, Vector2 normal, float fraction) //On hit
                    {
                        ++bullet.collisionChecked;
                        if (fix.Body.UserData is Entity)
                        {
                            if ((fix.Body.UserData as Entity).HasComponent<Health>()
                                && (fix.Body.UserData as Entity).Group == bullet.DamageGroup)
                            { //Do damage
                                (fix.Body.UserData as Entity).GetComponent<Health>().SetHealth(bullet.Firer,
                                    (fix.Body.UserData as Entity).GetComponent<Health>().CurrentHealth - bullet.Damage);
                                e.Delete(); //Remove bullet

                                if (bullet.OnBulletHit != null)
                                {
                                    //Do bullet effects here........... Maybe a call back?{
                                    bullet.OnBulletHit(fix.Body.UserData as Entity);
                                }
                            }
                        }
                        return 0;
                    }, particle.Position,  particle.Position + particle.LinearVelocity * new Vector2(World.Delta * .001f));
        }
    }
}