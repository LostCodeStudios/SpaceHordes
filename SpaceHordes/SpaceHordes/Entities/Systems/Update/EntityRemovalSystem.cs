using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using GameLibrary.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    /// <summary>
    /// Checks if Transforms have gone off of the screen.
    /// </summary>
    internal class EntityRemovalSystem : ParallelEntityProcessingSystem
    {
        private Camera camera;
        private ComponentMapper<ITransform> TransformMapper;

        public EntityRemovalSystem(Camera camera)
            : base(typeof(ITransform))
        {
            this.camera = camera;
        }

        public override void Initialize()
        {
            TransformMapper = new ComponentMapper<ITransform>(world);
        }

        public override void Process(Entity e)
        {
            ITransform t = TransformMapper.Get(e);
            if(Vector2.Distance(t.Position, Vector2.Zero) > ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width)*2 + 1)
            {
                e.Delete();
            }
        }
    }
}