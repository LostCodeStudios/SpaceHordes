using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Library.Model
{
    /// <summary>
    /// An animated sprite to be drawn during gameplay.
    /// </summary>
    public class Sprite
    {
        #region Fields

        private Texture2D Texture;
        private Color tintColor = Color.White;

        private Vector2 location = Vector2.Zero;
        private double velocity = 0;
        private float rotation = 0.0f;

        private List<Rectangle> frames = new List<Rectangle>();
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;
        
        private int boundingXPadding = 0;
        private int boundingYPadding = 0;

        protected bool expired = false;
        private bool animate = true;
        private bool animateWhenStopped = true;
        private bool collidable = true;
        private int collisionRadius = 0;

        #endregion

        #region Drawing and Animation Properties

        /// <summary>
        /// Returns the width of the sprite's frames.
        /// </summary>
        public int FrameWidth
        {
            get { return frames[0].Width; }
        }

        /// <summary>
        /// Return's the height of the sprite's frames.
        /// </summary>
        public int FrameHeight
        {
            get { return frames[0].Height; }
        }

        /// <summary>
        /// The color used in the sprite's spriteBatch.Draw() call.
        /// </summary>
        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }

        /// <summary>
        /// The rotation used in the sprite's spriteBatch.Draw() call, in radians.
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        /// <summary>
        /// The frame the sprite currently needs to draw.
        /// </summary>
        public int Frame
        {
            get { return currentFrame; }
            set
            {
                // Make sure the frame actually exists
                currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1);
            }
        }

        /// <summary>
        /// The seconds for each frame, as a decimal.
        /// </summary>
        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }//Make sure frametime is positive
        }

        /// <summary>
        /// The source rectangle of the current frame. Used for drawing
        /// the sprite.
        /// </summary>
        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }

        /// <summary>
        /// How many pixels separate each frame horizontally on the spritesheet.
        /// </summary>
        public int BoundingXPadding
        {
            get { return boundingXPadding; }
            set { boundingXPadding = Math.Max(0, value); }
        }

        /// <summary>
        /// How many pixels separate each frame vertically on the spritesheet.
        /// </summary>
        public int BoundingYPadding
        {
            get { return boundingXPadding; }
            set { boundingYPadding = Math.Max(0, value); }
        }

        /// <summary>
        /// Does the sprite still cycle animations when velocity is at 0?
        /// </summary>
        public bool AnimateWhenStopped
        {
            get { return animateWhenStopped; }
            set { animateWhenStopped = value; }
        }

        /// <summary>
        /// Whether or not the sprite should still draw itself.
        /// </summary>
        public bool Expired
        {
            get { return expired; }
            set { expired = value; }
        }

        #endregion

        #region Positional Properties

        /// <summary>
        /// The sprite's location.
        /// </summary>
        public Vector2 Location
        {
            get { return location; }
            set { location = value+Offset; }
        }

        /// <summary>
        /// The offset from a location value.
        /// </summary>
        public Vector2 Offset { set; get; }


        /// <summary>
        /// The sprite's velocity, used for updating position.
        /// </summary>
        public double Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// The sprite's bounding rectangle in the larger game world.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    FrameWidth,
                    FrameHeight);
            }
        }

        /// <summary>
        /// The sprite's center, expressed as a point relative to its
        /// top left corner.
        /// </summary>
        public Vector2 RelativeCenter
        {
            get { return new Vector2(FrameWidth / 2, FrameHeight / 2); }
        }

        /// <summary>
        /// The sprite's center, in terms of the larger game world.
        /// </summary>
        public Vector2 Center
        {
            get { return Location + RelativeCenter; }
            set { Location = new Vector2(value.X - FrameWidth / 2, value.Y - FrameHeight / 2); }
        }

        #endregion

        #region Collision Properties

        /// <summary>
        /// Returns the sprite's hit box, taking padding into account.
        /// </summary>
        public Rectangle HitBox
        {
            get
            {
                return new Rectangle(
                    (int)Location.X + BoundingXPadding,
                    (int)Location.Y + BoundingYPadding,
                    FrameWidth - (BoundingXPadding * 2),
                    FrameHeight - (BoundingYPadding * 2));
            }
        }

        /// <summary>
        /// The radius of the sprite's collision circle.
        /// </summary>
        public int CollisionRadius
        {
            get { return collisionRadius; }
            set { collisionRadius = value; }
        }
        
        /// <summary>
        /// Returns whether or not this sprite can collide with other sprites.
        /// </summary>
        public bool Collidable
        {
            get { return collidable; }
            set { collidable = true; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new Sprite.
        /// </summary>
        public Sprite(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 offset)
        {
            Offset = offset;
            Location = location;
            Texture = texture;
            Velocity = 0;
            frames.Add(initialFrame);
        }

        #endregion

        #region Collision Detection Methods

        /// <summary>
        /// Checks whether the sprite is colliding with another hit box.
        /// </summary>
        public bool IsBoxColliding(Rectangle otherBox)
        {
            if ((collidable) && (!expired))
                return HitBox.Intersects(otherBox);

            return false; //If the sprite is expired or non-collidable,
                          //Don't bother with the check.
        }

        /// <summary>
        /// Checks whether the sprite is colliding with another sprite's
        /// collision circle.
        /// </summary>
        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if ((collidable) && (!expired))
            {
                if (Vector2.Distance(Center, otherCenter) < (collisionRadius + otherRadius))
                    return true;
            }

            return false;
        }

        #endregion

        #region Animation Methods

        /// <summary>
        /// Adds a frame to the sprite's frames list.
        /// </summary>
        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        /// <summary>
        /// Changes the sprite's rotation to face a specified direction.
        /// </summary>
        public void RotateTo(Vector2 direction)
        {
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        #endregion

        #region Update & Draw

        /// <summary>
        /// Runs the sprite's logic.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            //Only update if sprite is still valid.
            if (!expired)
            {
                //See how much time has passed.
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Update the frame's timer.
                timeForCurrentFrame += elapsed;

                if (animate)
                {
                    if (timeForCurrentFrame >= FrameTime)
                    {
                        //As long as the sprite should be animating
                        if ((animateWhenStopped) || (velocity != 0))
                        {
                            currentFrame = (currentFrame + 1) % (frames.Count);
                            timeForCurrentFrame = 0.0f;
                        }
                    }
                }

                //Movement
                location.X += (float)(velocity * Math.Cos((double)Rotation));
                location.Y += (float)(velocity * Math.Sin((double)Rotation));
            }
        }

        /// <summary>
        /// Draws the sprite to the screen using an active SpriteBatch.
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch sprB)
        {
            //Only draw if sprite is still valid
            if (!expired)
            {
                sprB.Draw(
                    Texture,
                    Center,
                    Source,
                    tintColor,
                    rotation,
                    RelativeCenter,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }

        #endregion
    }
}
