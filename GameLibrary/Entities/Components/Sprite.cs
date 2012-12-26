using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Helpers;

namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// A sprite component
    /// </summary>
    public struct Sprite : Component
    {
        public Sprite(Texture2D spriteSheet, Rectangle source, Vector2 origin, float scale, Color color, float layer)
        {
            this.Source = source;
            this.SpriteSheet = spriteSheet;
            this.Origin = origin;
            this.Scale = scale;
            this.Color = color;
            this.Layer = layer;
        }

        public Sprite(Texture2D spriteSheet, Rectangle source, Body body, float scale, Color color, float layer) :
            this(spriteSheet, source, AssetCreator.CalculateOrigin(body) / scale, scale, color, layer)
        {
        }

        public Sprite(Texture2D spriteSheet, Rectangle source) : this(spriteSheet, source, new Vector2(source.Width/2f, source.Height/2f),1,Color.White,0f)
        {
        }

        public float Layer;
        public Color Color;
        public float Scale;
        public Texture2D SpriteSheet;
        public Vector2 Origin;
        public Rectangle Source;
    }
}
