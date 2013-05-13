using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Templates
{
    public class ExplosionTemplate : Explosive, IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private World _World;

        public ExplosionTemplate(World world, SpriteSheet sheet)
            : base(world)
        {
            _SpriteSheet = sheet;
            this._World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            float magnitude = (float)args[0];

            int power = (int)args[3];
            string spriteKey = "splosion" + power.ToString();

            Vector2 center = new Vector2(_SpriteSheet[spriteKey][0].Center.X, _SpriteSheet[spriteKey][0].Center.Y);

            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, 1));
            Animation a = e.AddComponent<Animation>(new Animation(AnimationType.Once, 5));
            //ITransform i = e.AddComponent<ITransform>(new Transform((Vector2)args[1], 0f));

            if (power == 1)
                e.AddComponent<Origin>(new Origin(args[5] as Entity));

            Particle p = e.AddComponent<Particle>(new Particle(e, (Vector2)args[1], 0f, (args.Length > 4) ? (Vector2)args[4] : Vector2.Zero, 0f));
            e.Group = "Explosions";
            
            Explode((Entity)args[2], 1, p.Position, 3, magnitude);

            return e;
        }
    }
}