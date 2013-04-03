using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceHordes.Entities.Templates.Objects
{
    public class ExplosiveBulletTemplate : IEntityTemplate
    {
        World _World;
        SpriteSheet _SS;
        public ExplosiveBulletTemplate(World world, SpriteSheet ss)
        {
            this._World = world;
            this._SS = ss;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args">The position and velocity of the bullet + the health/damage</param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Enemies";
            
            #region Sprite
            //Builds a sprite using "redshot3" (arbitrary). TODO: FIX DIS BITCH HELLA D
            Sprite bulletSprite = e.AddComponent<Sprite>(new Sprite(_SS, "redspikeball"));
            #endregion

            #region Body
            //Creates a body based off of the position and velocity supplied in the args list.
            Body bitch = e.AddComponent<Body>(new Body(_World, e));

            bitch.BodyType = BodyType.Dynamic;
            bitch.SleepingAllowed = false;

            bitch.Position = (Vector2)args[0];
            bitch.LinearVelocity = (Vector2)args[1];

            FixtureFactory.AttachEllipse(
                ConvertUnits.ToSimUnits(bulletSprite.CurrentRectangle.Width),
                ConvertUnits.ToSimUnits(bulletSprite.CurrentRectangle.Height),
                6, 1, bitch);

            bitch.CollisionCategories = Category.Cat3;
            bitch.CollidesWith = Category.Cat2;
            #endregion

            #region Health
            //Creates health based off of the third parameter of the build entity.
            Health bulletHealth = e.AddComponent<Health>(new Health((int)args[2]));
            #endregion

            return e;
        }
    }
}
