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
    internal class EntityRemovalSystem : EntityProcessingSystem//ParallelEntityProcessingSystem
    {
        private Camera camera;
        private ComponentMapper<ITransform> TransformMapper;
        float bound; 
        float bulletBound;

        public EntityRemovalSystem(Camera camera)
            : base(typeof(ITransform))
        {
            this.camera = camera;
        }

        public override void Initialize()
        {
            TransformMapper = new ComponentMapper<ITransform>(world);
            bound = ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width) * 2 + 1;
            bulletBound = ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width) / 1.5f;
        }

        public override void Process(Entity e)
        {
            ITransform t = TransformMapper.Get(e);
            if (t == null)
                return;
            if(Vector2.Distance(t.Position, Vector2.Zero) > (e.Group == "Bullets" || e.Group == "Fire" ? bulletBound : bound))
            {
                e.Delete();
                return;
            }

            if (e.HasComponent<Components.Timer>() && e.GetComponent<Components.Timer>().Update(world.Delta))
            {
                e.Delete();
            }
        }
    }
}