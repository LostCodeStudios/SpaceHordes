using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using SpaceHordes.Entities.Components;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    public class CrystalMovementSystem : GroupSystem
    {
        public CrystalMovementSystem() : base("Crystals")
        {
        }

        public override void Added(Entity e)
        {
            e.GetComponent<Body>().OnCollision +=
                (f1, f2, c) =>
                {
                    if ((f2.Body.UserData as Entity).Group == "Players")
                    {
                        (f2.Body.UserData as Entity).GetComponent<Inventory>().GiveCrystals((f1.Body.UserData as Entity).GetComponent<Crystal>());
                        (f1.Body.UserData as Entity).Delete();
                        SoundManager.Play("Pickup1");
                    }
                    return true;
                };
            
            base.Added(e);
        }

        public override void Process(Entity e)
        {
            
        }
    }
}
