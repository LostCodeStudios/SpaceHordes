using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    /// <summary>
    /// Checks if Transforms have gone off of the screen.
    /// </summary>
    internal class EntityRemovalSystem : ParallelEntityProcessingSystem
    {
        private Camera camera;
        private ComponentMapper<Particle> TransformMapper;

        public EntityRemovalSystem(Camera camera)
            : base(typeof(Particle))
        {
            this.camera = camera;
        }

        public override void Initialize()
        {
            TransformMapper = new ComponentMapper<Particle>(world);
        }

        public override void Process(Entity e)
        {
            Particle t = TransformMapper.Get(e);
            if(Vector2.Distance(t.Position, Vector2.Zero) > ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width)*2 + 1)
            {
                e.Delete();
            }
        }
    }
}