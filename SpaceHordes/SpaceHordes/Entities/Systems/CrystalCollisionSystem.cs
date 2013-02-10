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
    class CrystalCollisionSystem : EntityProcessingSystem
    {
        ComponentMapper<Crystal> crystalMapper;
        ComponentMapper<Particle> particleMapper;

        public CrystalCollisionSystem()
            : base(typeof(Crystal), typeof(Particle))
        {
        }

        public override void Initialize()
        {
            crystalMapper = new ComponentMapper<Crystal>(world);
            particleMapper = new ComponentMapper<Particle>(world);
        }


        public override void Process(Entity e)
        {
            Particle particle = particleMapper.Get(e);
            Crystal crystal = crystalMapper.Get(e);

            //Check collision with physical world.
            world.RayCast(
                delegate(Fixture fix, Vector2 point, Vector2 normal, float fraction) //On hit
                {
                    if (fix.Body.UserData is Entity)
                    {
                        if ((fix.Body.UserData as Entity).HasComponent<Inventory>()
                            && (fix.Body.UserData as Entity).Group == crystal.AmmoGroup)
                        { //Give ammo
                            Inventory i = (fix.Body.UserData as Entity).GetComponent<Inventory>();

                            if (crystal.Color == Color.Red)
                            {
                                i.RED.Ammunition += crystal.Amount;
                            }

                            if (crystal.Color == Color.Blue)
                            {
                                i.BLUE.Ammunition += crystal.Amount;
                            }

                            if (crystal.Color == Color.Green)
                            {
                                i.GREEN.Ammunition += crystal.Amount;
                            }

                            if (crystal.Color == Color.Yellow)
                            {
                                i.YELLOW += (uint)crystal.Amount;
                            }

                            if (crystal.Color == Color.Gray) //power up
                            {
                                i.CurrentGun.PowerUp(10000, crystal.Amount);
                            }

                            e.Delete(); //Remove crystal
                        }
                    }
                    return 0;
                }, particle.Position, particle.Position + particle.LinearVelocity * (new Microsoft.Xna.Framework.Vector2(world.Delta / 1000f)));

        }
    }
}
