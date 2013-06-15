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
    public class ExplosiveBulletTemplate : IEntityTemplate
    {
        private World _World;
        private SpriteSheet _SS;

        private static int num = 0;

        public ExplosiveBulletTemplate(World world, SpriteSheet ss)
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
            e.Group = "Enemies";

            #region Sprite

            //Builds a sprite using "redshot3" (arbitrary). TODO: FIX DIS BITCH HELLA D
            string spriteKey = "redspikeball";
            if (args.Length > 3)
            {
                spriteKey = (string)args[3];
            }
            Sprite bulletSprite = e.AddComponent<Sprite>(new Sprite(_SS, spriteKey, 0.54f + (float)num / 1000000f));

            #endregion Sprite

            #region Body

            //Creates a body based off of the position and velocity supplied in the args list.
            Body bitch = e.AddComponent<Body>(new Body(_World, e));

            bitch.BodyType = BodyType.Dynamic;

            bitch.Position = (Vector2)args[0];
            bitch.LinearVelocity = (Vector2)args[1];

            FixtureFactory.AttachEllipse(
                ConvertUnits.ToSimUnits(bulletSprite.CurrentRectangle.Width / 2),
                ConvertUnits.ToSimUnits(bulletSprite.CurrentRectangle.Height / 2),
                6, 1, bitch);

            bitch.CollisionCategories = Category.Cat4;
            bitch.CollidesWith = Category.Cat1 | Category.Cat3;

            #endregion Body

            #region Health

            //Creates health based off of the third parameter of the build entity.
            Health bulletHealth = e.AddComponent<Health>(new Health((int)args[2]));
            bulletHealth.OnDeath +=
            ent =>
            {
                _World.CreateEntityGroup("BigExplosion", "Explosions", bitch.Position, 10, e, e.GetComponent<IVelocity>().LinearVelocity);

                SoundManager.Play("Explosion1");
            };

            bitch.OnCollision +=
                (f1, f2, c) =>
                {
                    bulletHealth.SetHealth(f2.UserData as Entity, 0.0);
                    return true;
                };

            #endregion Health

            return e;
        }
    }
}