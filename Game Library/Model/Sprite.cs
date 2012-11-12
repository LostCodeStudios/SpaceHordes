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

    #region OldSprite TODO: DELETE
    ///// <summary>
    ///// An animated sprite to be drawn during gameplay.
    ///// </summary>
    //public class Sprite
    //{
    //    #region Fields

    //    Spritesheet spritesheet;
    //    string filename;

    //    Color tintColor = Color.White;

    //    Vector2 location = Vector2.Zero;
    //    double velocity = 0;
    //    float rotation = 0.0f;

    //    List<Rectangle> frames = new List<Rectangle>();
    //    int currentFrame;
    //    float frameTime = 0.1f;
    //    float timeForCurrentFrame = 0.0f;
    //    int frameInc = 1;

    //    protected bool expired = false;
    //    AnimationType type = AnimationType.Static;
    //    State state = State.Normal;
    //    bool animateWhenStopped = true;
    //    bool collidable = true;
    //    int collisionRadius = 0;

    //    #endregion

    //    #region Drawing and Animation Properties

    //    /// <summary>
    //    /// Returns the width of the sprite's frames.
    //    /// </summary>
    //    public int FrameWidth
    //    {
    //        get { return frames[0].Width; }
    //    }

    //    /// <summary>
    //    /// Return's the height of the sprite's frames.
    //    /// </summary>
    //    public int FrameHeight
    //    {
    //        get { return frames[0].Height; }
    //    }

    //    /// <summary>
    //    /// The color used in the sprite's spriteBatch.Draw() call.
    //    /// </summary>
    //    public Color TintColor
    //    {
    //        get { return tintColor; }
    //        set { tintColor = value; }
    //    }

    //    /// <summary>
    //    /// The rotation used in the sprite's spriteBatch.Draw() call, in radians.
    //    /// </summary>
    //    public float Rotation
    //    {
    //        get { return rotation; }
    //        set { rotation = value % MathHelper.TwoPi; }
    //    }

    //    /// <summary>
    //    /// The frame the sprite currently needs to draw.
    //    /// </summary>
    //    public int Frame
    //    {
    //        get { return currentFrame; }
    //        set
    //        {
    //            // Make sure the frame actually exists
    //            currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1);
    //        }
    //    }

    //    /// <summary>
    //    /// The seconds for each frame, as a decimal.
    //    /// </summary>
    //    public float FrameTime
    //    {
    //        get { return frameTime; }
    //        set { frameTime = MathHelper.Max(0, value); }//Make sure frametime is positive
    //    }

    //    /// <summary>
    //    /// The source rectangle of the current frame. Used for drawing
    //    /// the sprite.
    //    /// </summary>
    //    public Rectangle Source
    //    {
    //        get { return frames[currentFrame]; }
    //    }

    //    /// <summary>
    //    /// Does the sprite still cycle animations when velocity is at 0?
    //    /// </summary>
    //    public bool AnimateWhenStopped
    //    {
    //        get { return animateWhenStopped; }
    //        set { animateWhenStopped = value; }
    //    }

    //    /// <summary>
    //    /// Whether or not the sprite should still draw itself.
    //    /// </summary>
    //    public bool Expired
    //    {
    //        get { return expired; }
    //        set { expired = value; }
    //    }

    //    /// <summary>
    //    /// The type of animation used by this sprite.
    //    /// </summary>
    //    public AnimationType AnimationType
    //    {
    //        get { return type; }
    //        set { type = value; }
    //    }

    //    /// <summary>
    //    /// The state of the sprite.
    //    /// </summary>
    //    public State State
    //    {
    //        get { return state; }
    //        set { state = value; }
    //    }

    //    #endregion

    //    #region Positional Properties

    //    /// <summary>
    //    /// The sprite's location.
    //    /// </summary>
    //    public Vector2 Location
    //    {
    //        get { return location; }
    //        set { location = value+Offset; }
    //    }

    //    /// <summary>
    //    /// The offset from a location value.
    //    /// </summary>
    //    public Vector2 Offset { set; get; }


    //    /// <summary>
    //    /// The sprite's velocity, used for updating position.
    //    /// </summary>
    //    public double Velocity
    //    {
    //        get { return velocity; }
    //        set { velocity = value; }
    //    }

    //    /// <summary>
    //    /// The sprite's bounding rectangle in the larger game world.
    //    /// </summary>
    //    public Rectangle Rectangle
    //    {
    //        get
    //        {
    //            return new Rectangle(
    //                (int)location.X,
    //                (int)location.Y,
    //                FrameWidth,
    //                FrameHeight);
    //        }
    //    }

    //    /// <summary>
    //    /// The sprite's center, expressed as a point relative to its
    //    /// top left corner.
    //    /// </summary>
    //    public Vector2 RelativeCenter
    //    {
    //        get { return new Vector2(FrameWidth / 2, FrameHeight / 2); }
    //    }

    //    /// <summary>
    //    /// The sprite's center, in terms of the larger game world.
    //    /// </summary>
    //    public Vector2 Center
    //    {
    //        get { return Location + RelativeCenter; }
    //        set { Location = new Vector2(value.X - FrameWidth / 2, value.Y - FrameHeight / 2); }
    //    }

    //    #endregion

    //    #region Collision Properties

    //    /// <summary>
    //    /// Returns the sprite's hit box, taking padding into account.
    //    /// </summary>
    //    public Rectangle HitBox
    //    {
    //        get
    //        {
    //            return new Rectangle(
    //                (int)Location.X,
    //                (int)Location.Y,
    //                FrameWidth,
    //                FrameHeight);
    //        }
    //    }

    //    /// <summary>
    //    /// The radius of the sprite's collision circle.
    //    /// </summary>
    //    public int CollisionRadius
    //    {
    //        get { return collisionRadius; }
    //        set { collisionRadius = value; }
    //    }
        
    //    /// <summary>
    //    /// Returns whether or not this sprite can collide with other sprites.
    //    /// </summary>
    //    public bool Collidable
    //    {
    //        get { return collidable; }
    //        set { collidable = true; }
    //    }

    //    #endregion

    //    #region Constructor

    //    /// <summary>
    //    /// Constructs a new Sprite.
    //    /// </summary>
    //    public Sprite(
    //        Vector2 location,
    //        Spritesheet sheet,
    //        string key,
    //        Vector2 offset,
    //        AnimationType type)
    //    {
    //        Offset = offset;
    //        Location = location;
    //        spritesheet = sheet;
    //        Velocity = 0;
    //        AnimationType = type;

    //        for (int x = 0; x < sheet.Animations[key].Length; x++)
    //        {
    //            frames.Add(sheet.Animations[key][x]);
    //        }
    //    }

    //    /// <summary>
    //    /// Constructs a new Sprite.
    //    /// </summary>
    //    public Sprite(
    //        Vector2 location,
    //        Spritesheet sheet,
    //        string key,
    //        Vector2 offset)
    //        : this(location, sheet, key, offset, AnimationType.Static)
    //    {
    //        if (sheet.Animations[key].Length > 1)
    //            AnimationType = AnimationType.Loop;
    //    }

    //    #endregion

    //    #region Collision Detection Methods

    //    /// <summary>
    //    /// Checks whether the sprite is colliding with another hit box.
    //    /// </summary>
    //    public bool IsBoxColliding(Rectangle otherBox)
    //    {
    //        if ((collidable) && (!expired))
    //            return HitBox.Intersects(otherBox);

    //        return false; //If the sprite is expired or non-collidable,
    //                      //Don't bother with the check.
    //    }

    //    /// <summary>
    //    /// Checks whether the sprite is colliding with another sprite's
    //    /// collision circle.
    //    /// </summary>
    //    public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
    //    {
    //        if ((collidable) && (!expired))
    //        {
    //            if (Vector2.Distance(Center, otherCenter) < (collisionRadius + otherRadius))
    //                return true;
    //        }

    //        return false;
    //    }

    //    #endregion

    //    #region Animation Methods

    //    /// <summary>
    //    /// Changes the sprite's rotation to face a specified direction.
    //    /// </summary>
    //    public void RotateTo(Vector2 direction)
    //    {
    //        Rotation = (float)Math.Atan2(direction.Y, direction.X);
    //    }

    //    /// <summary>
    //    /// For sprites that advance frames to reflect the status of an entity, this simply advances to the next frame and returns whether or not the sprite is expired as a result.
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool AdvanceFrame()
    //    {
    //        currentFrame++;

    //        if (currentFrame >= frames.Count)
    //        {
    //            Expired = true;
    //            return true;
    //        }

    //        return false;
    //    }

    //    #endregion

    //    #region Update & Draw

    //    /// <summary>
    //    /// Runs the sprite's logic.
    //    /// </summary>
    //    public virtual void Update(GameTime gameTime)
    //    {
    //        //Only update if sprite is still valid.
    //        if (!expired)
    //        {
    //            //See how much time has passed.
    //            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
    //            //Update the frame's timer.
    //            if (AnimationType != AnimationType.Static)
    //                timeForCurrentFrame += elapsed;

    //            if (timeForCurrentFrame >= FrameTime)
    //            {
    //                //As long as the sprite should be animating
    //                if ((animateWhenStopped) || (velocity != 0))
    //                {
    //                    //Handle animation updating for each sprite type
    //                    switch (AnimationType)
    //                    {
    //                        case AnimationType.Loop:
    //                            currentFrame = (currentFrame + frameInc) % (frames.Count);
    //                            timeForCurrentFrame = 0.0f;
    //                            break;

    //                        case AnimationType.Reverse:                              
    //                            currentFrame += frameInc;
    //                            if (currentFrame == frames.Count || currentFrame == 0)
    //                            {
    //                                frameInc *= -1;
    //                            }
    //                            break;

    //                        case AnimationType.Once:
    //                            currentFrame += frameInc;
    //                            if (currentFrame >= frames.Count)
    //                            {
    //                                currentFrame = frames.Count;
    //                                frameInc = 0;
    //                            }
    //                            break;

    //                        case AnimationType.State:
    //                            currentFrame = (int)State;
    //                            break;
    //                    }
    //                }
    //            }

    //            //Movement
    //            location.Y += (float)(velocity * Math.Cos((double)Rotation));
    //            location.X += (float)(velocity * Math.Sin((double)Rotation));
                
    //        }
    //    }

    //    /// <summary>
    //    /// Draws the sprite to the screen using an active SpriteBatch.
    //    /// </summary>
    //    public virtual void Draw(GameTime gameTime, SpriteBatch sprB)
    //    {
    //        //Only draw if sprite is still valid
    //        if (!expired)
    //        {
    //            sprB.Draw(
    //                spritesheet.Texture,
    //                Center,
    //                Source,
    //                tintColor,
    //                rotation,
    //                RelativeCenter,
    //                1.0f,
    //                SpriteEffects.None,
    //                0.0f);
    //        }
    //    }

    //    #endregion
    //}
    #endregion

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
            this.SpriteName = assetName;
            this.Origin = offset;
            this.SpriteSheet = spriteSheet;
            _TickCount = 0;
            AnimationIndex = 0;
            AnimationPattern = AnimationType.Static;
            AnimationTickRate = 0;
        }

        /// <summary>
        /// Initializes the sprite with a specified default animation type.
        /// </summary>
        /// <param name="assetName">The name of the asset within the spritesheet from which the sprite will be drawn.</param>
        /// <param name="offset">The offset from the origin</param>
        /// <param name="spriteSheet">The sprite sheet from which the sprite will get it's specific texture and asset data.</param>
        /// <param name="animationPattern">The pattern under which the sprites animation will occurr.</param>
        public Sprite(string assetName, Vector2 offset, Spritesheet spriteSheet, AnimationType animationPattern)
            : this(assetName, offset, spriteSheet)
        {
            AnimationPattern = animationPattern;
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 centerPosition, float rotation)
        {
            //TODO: ADD SCALE AND COLOR TO SPRITES LOL. XD LOL. LOL. FRSRS.
            spriteBatch.Draw(SpriteSheet.Texture, centerPosition, SpriteSheet.Animations[SpriteName].ElementAt(AnimationIndex), Color.White, rotation, Origin, 1f, SpriteEffects.None, 0f);
        }


        #endregion

        #region Properties

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
        protected void AdvanceFrame()
        {
            //If the sprite is animated, animate
            if (this.AnimationPattern != AnimationType.Static)
            {
                switch (this.AnimationPattern)
                {
                    case AnimationType.Loop:// Loops the animation
                        AnimationIndex = (AnimationIndex + 1) % (this.SpriteSheet.Animations[this.SpriteName].Count());
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
