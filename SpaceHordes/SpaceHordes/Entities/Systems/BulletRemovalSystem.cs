using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Systems
{
    /// <summary>
    /// Checks if Transforms have gone off of the screen.
    /// </summary>
    class BulletRemovalSystem : ParallelEntityProcessingSystem
    {
        Camera camera;
        ComponentMapper<Particle> TransformMapper;
        public BulletRemovalSystem(Camera camera)
            : base(typeof(Particle))
        {
            this.camera = camera;
            //this.EntitiesToProcessEachFrame = 10;
        }

        public override void Initialize()
        {
            TransformMapper = new ComponentMapper<Particle>(world);
        }
        public override void Process(Entity e)
        {
            if (e.Group == "Bullets") //If bullet has component
            {
                Particle t = TransformMapper.Get(e);
                if ((t.Position.X > camera.ConvertScreenToWorld(new Vector2(1280, 720)).X
                    || t.Position.Y > camera.ConvertScreenToWorld(new Vector2(1280, 720)).Y ||
                    (t.Position.X < camera.ConvertScreenToWorld(new Vector2(0, 0)).X
                    || t.Position.Y < camera.ConvertScreenToWorld(new Vector2(0, 0)).Y)))
                {
                    e.Delete();
                }
            }
        }
    }
}
