using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    /// <summary>
    /// Checks if Transforms have gone off of the screen.
    /// </summary>
    internal class BulletRemovalSystem : EntityProcessingSystem
    {
        private Camera camera;
        private ComponentMapper<Particle> TransformMapper;

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
                if ((t.Position.X > camera.ConvertScreenToWorld(new Vector2(2) * ScreenHelper.Center).X
                    || t.Position.Y > camera.ConvertScreenToWorld(new Vector2(2) * ScreenHelper.Center).Y ||
                    (t.Position.X < camera.ConvertScreenToWorld(new Vector2(0, 0)).X
                    || t.Position.Y < camera.ConvertScreenToWorld(new Vector2(0, 0)).Y)))
                {
                    e.Delete();
                }
            }
        }
    }
}