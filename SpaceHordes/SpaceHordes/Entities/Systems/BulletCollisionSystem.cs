using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using SpaceHordes.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Physics.Dynamics;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    class BulletCollisionSystem : EntityProcessingSystem
    {
        ComponentMapper<Bullet> bulletMapper;
        ComponentMapper<Particle> particleMapper;
        public BulletCollisionSystem() : base(typeof(Bullet), typeof(Particle))
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
            if(bullet.collisionChecked <3)
                world.RayCast(
                    delegate(Fixture fix, Vector2 point, Vector2 normal, float fraction) //On hit
                    {
                        bullet.collisionChecked++;
                        if (fix.Body.UserData is Entity)
                        {
                            if ((fix.Body.UserData as Entity).HasComponent<Health>()
                                && (fix.Body.UserData as Entity).Group == bullet.DamageGroup)
                            { //Do damage
                                (fix.Body.UserData as Entity).GetComponent<Health>().CurrentHealth -= bullet.Damage;
                                e.Delete(); //Remove bullet

                                if (bullet.OnBulletHit != null) //Do bullet effects here........... Maybe a call back?
                                    bullet.OnBulletHit(fix.Body.UserData as Entity);
                            }
                        }
                        return 0;
                    }, particle.Position, particle.Position + particle.LinearVelocity * (new Microsoft.Xna.Framework.Vector2(world.Delta / 1000f)));

        }
    }
}
