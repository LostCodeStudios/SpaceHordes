using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Physics.Factories;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates.Objects
{
    public class CrystalTemplate : IEntityTemplate
    {
        EntityWorld _World;
        SpriteSheet _SpriteSheet;
        public CrystalTemplate(EntityWorld World, SpriteSheet spriteSheet)
        {
            this._World = World;
            this._SpriteSheet = spriteSheet;
        }

        /// <summary>
        /// Builds the crystal at a specified position and a color.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args">[0] = position; [1] = color; [2] ammount</param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            Vector2 pos = (Vector2)args[0];
            Color color = (Color)args[1];
            string source = "redcrystal";
            if (color == Color.Red)
                source = "redcrystal";
            if (color == Color.Blue)
                source = "bluecrystal";
            if (color == Color.Yellow)
                source = "yellowcrystal";
            if (color == Color.Green)
                source = "greencrystal";
            if (color == Color.Gray)
                source = "graycrystal";

            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, source));
            Body b = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse((float)ConvertUnits.ToSimUnits(s.CurrentRectangle.Width/2), (float)ConvertUnits.ToSimUnits(s.CurrentRectangle.Height/2), 4, 1f, b);
            b.Position = pos;
            b.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            e.AddComponent<Crystal>(new Crystal(color, (int)args[2]));
            e.Group = "Crystals";
            e.Refresh();
            return e;
        }
    }
}
