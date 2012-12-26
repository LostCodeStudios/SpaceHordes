using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    /// <summary>
    /// Checks if Transforms have gone off of the screen.
    /// </summary>
    class BulletRemovalSystem : ParallelEntityProcessingSystem
    {
        Camera camera;
        ComponentMapper<Transform> TransformMapper;
        public BulletRemovalSystem(Camera camera)
            : base(typeof(Transform))
        {
            this.camera = camera;
            //this.EntitiesToProcessEachFrame = 10;
        }

        public override void Initialize()
        {
            TransformMapper = new ComponentMapper<Transform>(world);
        }
        public override void Process(Entity e)
        {
            if (e.Group == "Bullets" && TransformMapper.Get(e) != null) //If bullet has component
            {
                Transform t = TransformMapper.Get(e).Values.First();
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
