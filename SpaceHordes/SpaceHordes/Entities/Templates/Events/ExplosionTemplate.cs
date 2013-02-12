using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Templates
{
    public class ExplosionTemplate : IEntityTemplate
    {
        SpriteSheet _SpriteSheet;

        public ExplosionTemplate(SpriteSheet sheet)
        {
            _SpriteSheet = sheet;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            int magnitude = (int)args[0];

            string spriteKey = "splosion" + magnitude.ToString();
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey));
            Animation a = e.AddComponent<Animation>(new Animation(AnimationType.Once, 5));
            ITransform i = e.AddComponent<ITransform>(new Transform((Vector2)args[1], 0f));

            e.Group = "Explosions";

            return e;
        }
    }
}