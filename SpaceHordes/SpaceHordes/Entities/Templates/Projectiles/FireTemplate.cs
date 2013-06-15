using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates.Objects
{
    public class FireTemplate : IEntityTemplate
    {
        private World _World;
        private SpriteSheet _SS;

        private static int num = 0;

        public FireTemplate(World world, SpriteSheet ss)
        {
            num = 0;
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
            e.Group = "Fire";

            #region Sprite

            Sprite fireSprite = e.AddComponent<Sprite>(new Sprite(_SS, "minifire", 0.55f + (float)num / 1000000f));

            #endregion Sprite

            #region Body

            //Creates a body based off of the position and velocity supplied in the args list.
            Body bitch = e.AddComponent<Body>(new Body(_World, e));

            bitch.BodyType = BodyType.Dynamic;

            bitch.Position = (Vector2)args[0];
            bitch.LinearVelocity = (Vector2)args[1];
            bitch.RotateTo(bitch.LinearVelocity);
            bitch.IsBullet = true;
            bitch.SleepingAllowed = false;

            FixtureFactory.AttachEllipse(
                ConvertUnits.ToSimUnits(fireSprite.CurrentRectangle.Width / 2),
                ConvertUnits.ToSimUnits(fireSprite.CurrentRectangle.Height / 2),
                6, 1, bitch);

            bitch.CollisionCategories = Category.Cat4;
            bitch.CollidesWith = Category.Cat1 | Category.Cat3;
            bitch.OnCollision +=
                (f1, f2, c) =>
                {
                    if (f2.Body.UserData != null && f2.Body.UserData is Entity)
                        if ((f2.Body.UserData as Entity).Group != "Crystals")
                        {
                            try
                            {
                                (f2.Body.UserData as Entity).GetComponent<Health>().SetHealth(f1.Body.UserData as Entity,
                                    (f2.Body.UserData as Entity).GetComponent<Health>().CurrentHealth
                                    - 3);
                            }
                            catch { } //Lol nullreferenceexception bitch
                        }
                    return false;
                };

            #endregion Body

            return e;
        }
    }
}