using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Game_Library.Model.Entities
{
    /// <summary>
    /// A generic ship entity, which handles collision normally.
    /// </summary>
    public class Ship : Entity, IPhysical
    {
        public Rectangle test;

        #region Constructor

        public Ship(Vector2 position, float rotation, Sprite sprite)
            : base(position, rotation, sprite)
        {
        }

        #endregion

        #region Collision

        /// <summary>
        /// Handles collision between two physical objects.
        /// </summary>
        /// <param name="collidingWith"></param>
        public void HandleCollision(IPhysical collidingWith)
        {
            
        }

        /// <summary>
        /// Checks for collision between two physical objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsCollidingWith(IPhysical other)
        {
            return (IsRectangleCollidingWith(other));
        }

        /// <summary>
        /// Preliminary check for whether pixel collision should be tested. If so, runs a more in-depth test.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool IsRectangleCollidingWith(IPhysical other)
        {
            //Build this sprite's transform.
            Matrix thisTransform =
                    Matrix.CreateTranslation(new Vector3(-Sprite.Origin, 0.0f)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(new Vector3(Sprite.Origin, 0.0f));

            // Calculate the bounding rectangle of this entity in world space
            Rectangle thisRectangle = BoundingRectangle(
                     new Rectangle(0, 0, 
                         Sprite.SpriteSheet.Animations[Sprite.SpriteName][Sprite.AnimationIndex].Width, 
                         Sprite.SpriteSheet.Animations[Sprite.SpriteName][Sprite.AnimationIndex].Height),
                     thisTransform);

            thisRectangle.X = (int)Position.X - thisRectangle.Width / 2;
            thisRectangle.Y = (int)Position.Y - thisRectangle.Height / 2;

            test = thisRectangle;

            Entity otherEntity = other as Entity;

            //Calculate the other sprite's transform.
            Matrix otherTransform =
                    Matrix.CreateTranslation(new Vector3(-otherEntity.Sprite.Origin, 0.0f)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(new Vector3(otherEntity.Sprite.Origin, 0.0f));

            // Calculate the bounding rectangle of the other entity in world space
            Rectangle otherRectangle = BoundingRectangle(
                     new Rectangle(0, 0,
                         otherEntity.Sprite.SpriteSheet.Animations[Sprite.SpriteName][Sprite.AnimationIndex].Width,
                         otherEntity.Sprite.SpriteSheet.Animations[Sprite.SpriteName][Sprite.AnimationIndex].Height),
                     thisTransform);

            otherRectangle.X = (int)otherEntity.Position.X - otherRectangle.Width / 2;
            otherRectangle.Y = (int)otherEntity.Position.Y - otherRectangle.Height / 2;

            //If the two rectangles are intersecting, return true
            if (thisRectangle.Intersects(otherRectangle))
                return true;
                //return !PixelCollision(thisTransform, this.Sprite.Source.Width, this.Sprite.Source.Height, this.Sprite.Data,
                //    otherTransform, otherEntity.Sprite.Source.Width, otherEntity.Sprite.Source.Height, otherEntity.Sprite.Data);

            return false;
        }

        /// <summary>
        /// In-depth test for precise collision.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool PixelCollision(Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        #endregion
    }
}
