using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary;
using GameLibrary.Entities.Components.Physics;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates
{
    public class ExplosionTemplate : Explosive, IEntityTemplate
    {
        SpriteSheet _SpriteSheet;
        World _World;
        public ExplosionTemplate(World world, SpriteSheet sheet) : base(world)
        {
            _SpriteSheet = sheet;
            this._World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            float magnitude = (float)args[0];

            string spriteKey = "splosion" + "4";

            Vector2 center = new Vector2(_SpriteSheet[spriteKey][0].Center.X, _SpriteSheet[spriteKey][0].Center.Y);

            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, 1));
            Animation a = e.AddComponent<Animation>(new Animation(AnimationType.Once, 5));
            ITransform i = e.AddComponent<ITransform>(new Transform((Vector2)args[1], 0f));
            e.Group = "Explosions";

            Explode((Entity)args[2], 1, i.Position, 3, magnitude);

            return e;
        }
    }
}