using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// A sprite component
    /// </summary>
    public struct Sprite : Component
    {
        #region Constructor

        /// <summary>
        /// Creates a sprite with a spritesheet and a specified origin.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="spriteKey"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="color"></param>
        /// <param name="layer"></param>
        public Sprite(SpriteSheet spriteSheet, string spriteKey, Vector2 origin, float scale, Color color, float layer)
        {
            this.Source = spriteSheet.Animations[spriteKey];
            this.SpriteSheet = spriteSheet;
            this.Origin = origin;
            this.Scale = scale;
            this.Color = color;
            this.Layer = layer;
            this._Index = 0;
        }

        /// <summary>
        /// Creates a sprite with a spritesheet and abody.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="spriteKey"></param>
        /// <param name="body"></param>
        /// <param name="scale"></param>
        /// <param name="color"></param>
        /// <param name="layer"></param>
        public Sprite(SpriteSheet spriteSheet, string spriteKey, Body body, float scale, Color color, float layer) :
            this(spriteSheet, spriteKey, AssetCreator.CalculateOrigin(body) / scale, scale, color, layer)
        {
        }

        /// <summary>
        /// Creates a sprite with a spritesheet.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="spriteKey"></param>
        public Sprite(SpriteSheet spriteSheet, string spriteKey)
            : this(
                spriteSheet,
                spriteKey,
                new Vector2(
                spriteSheet[spriteKey][0].Width / 2f, spriteSheet[spriteKey][0].Height / 2f),
                1,
                Color.White,
                0f)
        {
        }

        /// <summary>
        /// Creates a new sprite using source and a texture with a specified origin.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="source"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="color"></param>
        /// <param name="layer"></param>
        public Sprite(Texture2D spriteSheet, Rectangle source, Vector2 origin, float scale, Color color, float layer)
        {
            this.Source = new Rectangle[] { source };
            this.Layer = layer;
            this.Origin = origin;
            this.Scale = scale;
            this.Color = color;
            this._Index = 0;
            this.SpriteSheet = new SpriteSheet(spriteSheet);
        }

        /// <summary>
        /// Creates a new sprite with a source and a texture using a body.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="source"></param>
        /// <param name="body"></param>
        /// <param name="scale"></param>
        /// <param name="color"></param>
        /// <param name="layer"></param>
        public Sprite(Texture2D spriteSheet, Rectangle source, Body body, float scale, Color color, float layer)
            : this(spriteSheet, source, AssetCreator.CalculateOrigin(body) / scale, scale, color, layer)
        {
        }

        /// <summary>
        /// Creates a new sprite supplied with a source and a texture.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="source"></param>
        public Sprite(Texture2D spriteSheet, Rectangle source)
            : this(spriteSheet, source, new Vector2(source.Width / 2f, source.Height / 2f), 1f, Color.White, 0f)
        {
        }

        /// <summary>
        /// Nat is a bitch
        /// </summary>
        /// <param name="_SpriteSheet"></param>
        /// <param name="spriteKey"></param>
        /// <param name="layer"></param>
        public Sprite(SpriteSheet _SpriteSheet, string spriteKey, float layer)
            : this(_SpriteSheet, spriteKey, new Vector2(_SpriteSheet[spriteKey][0].Width / 2f, _SpriteSheet[spriteKey][0].Height / 2f), 1f, Color.White, layer)
        {
        }

        #endregion Constructor

        #region Methods

        public void ApplySpriteEffect(int ticks, Sprite newSprite)
        {
        }

        #endregion Methods

        #region Fields

        public float Layer;
        public Color Color;
        public float Scale;
        public SpriteSheet SpriteSheet;
        public Vector2 Origin;
        public Rectangle[] Source;

        private int _Index;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Returns the index of the animation frame. Within the sprite sheet.
        /// </summary>
        public int FrameIndex
        {
            get
            {
                if (_Index < 0)
                    _Index = 0;
                return _Index;
            }
            set
            {
                if (_Index < 0)
                    _Index = 0;

                _Index = value % (Source.Count());
            }
        }

        /// <summary>
        /// Returns the current rectangle animated.
        /// </summary>
        public Rectangle CurrentRectangle
        {
            get { return Source[FrameIndex]; }
        }

        #endregion Properties
    }
}