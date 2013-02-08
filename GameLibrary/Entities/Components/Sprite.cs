using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components.Physics;

namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// A sprite component
    /// </summary>
    public struct Sprite : Component
    {
        public Sprite(SpriteSheet spriteSheet, string spriteKey, Vector2 origin, float scale, Color color, float layer)
        {
            this.Source = spriteSheet.Animations[spriteKey];
            this.SpriteSheet = spriteSheet;
            this.Origin = origin;
            this.Scale = scale;
            this.Color = color;
            this.Layer = layer;
            this.index = 0;
        }

        public Sprite(SpriteSheet spriteSheet, string spriteKey, Body body, float scale, Color color, float layer) :
            this(spriteSheet, spriteKey, AssetCreator.CalculateOrigin(body) / scale, scale, color, layer)
        {
        }

        public Sprite(SpriteSheet spriteSheet, string spriteKey) : this(
            spriteSheet, 
            spriteKey, 
            new Vector2(
                spriteSheet[spriteKey][0].Width/2f, spriteSheet[spriteKey][0].Height/2f),
            1,
            Color.White,
            0f)
        {
        }

        public float Layer;
        public Color Color;
        public float Scale;
        public SpriteSheet SpriteSheet;
        public Vector2 Origin;
        public Rectangle[] Source;

        int index;
        public int FrameIndex
        {
            get { return index; }
            set
            {
                if (index < 0)
                    index = 0;

                index = value % (Source.Count() - 1);
            }
        }

        public Rectangle CurrentRectangle
        {
            get { return Source[FrameIndex]; }
        }
    }
}
