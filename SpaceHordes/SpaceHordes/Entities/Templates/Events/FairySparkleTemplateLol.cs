using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates
{
    public class FairySparkleTemplateLol : Explosive, IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private World _World;

        public FairySparkleTemplateLol(World world, SpriteSheet sheet)
            : base(world)
        {
            _SpriteSheet = sheet;
            this._World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            Entity ent = args[0] as Entity;
            Vector2 offset = (Vector2)args[1];
            Vector2 velocity = -ent.GetComponent<Body>().LinearVelocity;
            string spriteKey = "greenspark";

            Vector2 center = new Vector2(_SpriteSheet[spriteKey][0].Center.X, _SpriteSheet[spriteKey][0].Center.Y);

            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, 1));
            Animation a = e.AddComponent<Animation>(new Animation(AnimationType.Once, 5));
            ITransform i = new Transform(ent.GetComponent<Body>().Position + offset, 0f);
            IVelocity v = new Velocity(velocity, 0f);
            e.AddComponent<Particle>(new Particle(e, i.Position, i.Rotation, v.LinearVelocity, v.AngularVelocity));
            e.Group = "Explosions";

            return e;
        }
    }
}