using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Library.Model
{
    /// <summary>
    /// The type of animation used by the sprite.
    /// </summary>
    public enum AnimationType
    {
        Loop,
        Once,
        State,
        Increment,
        Decrement,
        Static
    }


    public class Sprite
    {
        /// <summary>
        /// Initializes the sprite without a specified animation type.
        /// </summary>
        /// <param name="assetName">The name of the asset within the spritesheet from which the sprite will be drawn.</param>
        /// <param name="offset">The offset from the origin</param>
        /// <param name="spriteSheet">The sprite sheet from which the sprite will get it's specific texture and asset data.</param>
        public Sprite(string assetName, Vector2 offset, Spritesheet spriteSheet)
        {
            Origin = offset;
            SpriteSheet = spriteSheet;
            SpriteName = assetName;
            Color = Color.White;
            _TickCount = 0;
            AnimationIndex = 0;
            AnimationPattern = AnimationType.Static;
            AnimationTickRate = 0;

            data = new Color[Frames.Length][];

            for (int x = 0; x < data.Length; x++)
            {
                data[x] = new Color[Source.Width * Source.Height];
            }

            //Set the color data for each frame, to be used for pixel collision.
            for (int x = 0; x < Frames.Length; x++)
            {
                SpriteSheet.Texture.GetData(0,
                    Frames[x],
                    Data, 0,
                    Frames[x].Width *
                    Frames[x].Height);
            }
        }

        /// <summary>
        /// Initializes the sprite with a specified default animation type.
        /// </summary>
        /// <param name="assetName">The name of the asset within the spritesheet from which the sprite will be drawn.</param>
        /// <param name="offset">The offset from the origin</param>
        /// <param name="spriteSheet">The sprite sheet from which the sprite will get it's specific texture and asset data.</param>
        /// <param name="animationPattern">The pattern under which the sprites animation will occurr.</param>
        public Sprite(string assetName, Vector2 offset, Spritesheet spriteSheet, AnimationType animationPattern, Color color)
            : this(assetName, offset, spriteSheet)
        {
            AnimationPattern = animationPattern;
            Color = color;
        }


        #region Functioning Loop

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The current gametime</param>
        public void Update(GameTime gameTime)
        {
            _TickCount++;
            if (_TickCount > AnimationTickRate)//Only animate when appropriate to do so.
            {
                AdvanceFrame();
                _TickCount = 0;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 centerPosition, float rotation)
        {
            //TODO: ADD SCALE AND COLOR TO SPRITES LOL. XD LOL. LOL. FRSRS.
            spriteBatch.Draw(SpriteSheet.Texture, centerPosition, Source, Color, rotation, Origin, 1f, SpriteEffects.None, 0f);
        }


        #endregion

        #region Fields

        Color[][] data;

        #endregion

        #region Properties

        /// <summary>
        /// The source rectangle of the current frame.
        /// </summary>
        public Rectangle Source
        {
            get { return Frames[AnimationIndex]; }
        }

        /// <summary>
        /// The array of frames for this sprite.
        /// </summary>
        public Rectangle[] Frames
        {
            get { return SpriteSheet.Animations[SpriteName]; }
        }

        /// <summary>
        /// The color data used for checking collision.
        /// </summary>
        public Color[] Data
        {
            get { return data[AnimationIndex]; }
        }

        /// <summary>
        /// The sprite sheet from which the sprite will get it's specific texture and asset data.
        /// </summary>
        public Spritesheet SpriteSheet
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the asset within the spritesheet from which the sprite will be drawn.
        /// </summary>
        public string SpriteName
        {
            get;
            set;
        }

        /// <summary>
        /// The origin of the sprite (namely the negative offset from the center to the left corner)
        /// </summary>
        public Vector2 Origin
        {
            set;
            get;
        }

        /// <summary>
        /// The color of the sprite.
        /// </summary>
        public Color Color
        {
            set;
            get;
        }
        #endregion

        //TODO: Move animation to a seperate subclass.
        #region Animation 
        /// <summary>
        /// The index at which the sprite will render the texture from the spritesheet
        /// </summary>
        public int AnimationIndex
        {
            set;
            get;
        }

        /// <summary>
        /// The rate at which the sprite will be animated.
        /// </summary>
        public int AnimationTickRate
        {
            set;
            get;
        }
        private int _TickCount;
        
        /// <summary>
        /// The pattern whereby the sprite will animate.
        /// </summary>
        public AnimationType AnimationPattern
        {
            set;
            get;
        }

        /// <summary>
        /// If the animation is in State pattern, this value will be the animation index of the sprite.
        /// </summary>
        public int State
        {
            set;
            get;
        }

        /// <summary>
        /// Animates the sprite.
        /// </summary>
        protected virtual void AdvanceFrame()
        {
            //If the sprite is animated, animate
            if (this.AnimationPattern != AnimationType.Static)
            {
                switch (this.AnimationPattern)
                {
                    case AnimationType.Loop:// Loops the animation
                        AnimationIndex = (this.SpriteSheet.Animations[this.SpriteName].Count() % (AnimationIndex + 1));
                        break;

                    case AnimationType.Once: //Moves the frame up one.
                        AnimationIndex++;
                        if (AnimationIndex >= this.SpriteSheet.Animations[this.SpriteName].Count())
                        {
                            AnimationIndex = this.SpriteSheet.Animations[this.SpriteName].Count()-1;
                            AnimationPattern = AnimationType.Static; //End looping
                        }
                        break;

                    case AnimationType.State: //Set the frame to a certain state value.
                        AnimationIndex = (int)State;
                        break;

                    case AnimationType.Increment: //Moves the frame up one.
                        AnimationIndex++;
                        if (AnimationIndex >= this.SpriteSheet.Animations[this.SpriteName].Count())
                            AnimationIndex = 0;
                        AnimationPattern = AnimationType.Static;
                        break;

                    case AnimationType.Decrement: // Moves the frame back one.
                         AnimationIndex--;
                        if (AnimationIndex < 0)
                            AnimationIndex = this.SpriteSheet.Animations[this.SpriteName].Count()-1;
                        AnimationPattern = AnimationType.Static;
                        break;
                }

            }

        }
        #endregion
    }
}
