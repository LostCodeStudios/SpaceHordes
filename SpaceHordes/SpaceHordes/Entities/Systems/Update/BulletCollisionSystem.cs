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

            //Check collision with physical world.
            if (bullet.collisionChecked < 3)
            {
                //Range
                Vector2 expectedRange = particle.LinearVelocity * (new Microsoft.Xna.Framework.Vector2(world.Delta / 1000f));

                world.RayCast(
                    delegate(Fixture fix, Vector2 point, Vector2 normal, float fraction) //On hit
                    {
                        //Check if in collision range.
                        if((point - particle.Position).Length() < (expectedRange).Length())
                        {
                            bullet.collisionChecked++;
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
                        } 
                        else //If premptive
                        {


                        return 0;
                    }, particle.Position, particle.Position + particle.LinearVelocity);


            }



            //Long range preemptive


        }
    }
}