using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates.Objects
{
    public class MineTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;

        public MineTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            #region Body

            Body Body = e.AddComponent<Body>(new Body(_World, e));
            {
                FixtureFactory.AttachEllipse(//Add a basic bounding box (rectangle status)
                    ConvertUnits.ToSimUnits(_SpriteSheet.Animations["rotatinglightball"][0].Width * 2),
                    ConvertUnits.ToSimUnits(_SpriteSheet.Animations["rotatinglightball"][0].Height * 2),
                    5,
                    1,
                    Body);
                Body.Position = ConvertUnits.ToSimUnits((Vector2)args[0]);
                Body.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
                Body.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat3 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat16;
                Body.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat4 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat5;
                Body.FixedRotation = false;

                Body.SleepingAllowed = false;
            }

            #endregion Body

            #region Sprite

            Sprite s = new Sprite(_SpriteSheet, "rotatinglightball", 0.4f);
            e.AddComponent<Sprite>(s);

            #endregion Sprite

            #region Animation

            e.AddComponent<Animation>(new Animation(AnimationType.Loop, 10));

            #endregion Animation

            #region Health

            e.AddComponent<Health>(new Health(1)).OnDeath += LambdaComplex.BigEnemyDeath(e, _World as SpaceWorld, 0);

            #endregion Health

            e.Group = "Structures";
            return e;
        }
    }
}