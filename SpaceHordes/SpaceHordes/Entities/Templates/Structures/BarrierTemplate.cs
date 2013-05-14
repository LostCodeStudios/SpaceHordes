using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates.Objects
{
    public class BarrierTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;

        private float distFromPlayer = 20f;
        public static int barriers = 0;

        public BarrierTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            barriers = 0;
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            #region Origin

            e.AddComponent<Origin>(new Origin(args[1] as Entity));

            #endregion

            #region Body

            Body Body = e.AddComponent<Body>(new Body(_World, e));
            {
                FixtureFactory.AttachEllipse(//Add a basic bounding box (rectangle status)
                    ConvertUnits.ToSimUnits(_SpriteSheet.Animations["barrier"][0].Width / 2f),
                    ConvertUnits.ToSimUnits(_SpriteSheet.Animations["barrier"][0].Height / 2f),
                    5,
                    1,
                    Body);

                Body blarg = (args[1] as Entity).GetComponent<Body>();
                Vector2 dist = Body.Position - blarg.Position;
                dist.Normalize();
                dist *= distFromPlayer;

                Body.Position = ConvertUnits.ToSimUnits((Vector2)args[0] - dist);
                Body.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Static;
                Body.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1;
                Body.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat4 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat5;
                Body.FixedRotation = false;

                Body.RotateTo((Body.Position));

                Body.SleepingAllowed = false;
            }

            #endregion Body

            #region Sprite

            Sprite s = new Sprite(_SpriteSheet, "barrier", 0.6f + barriers/10000);
            e.AddComponent<Sprite>(s);

            #endregion Sprite

            #region Health

            e.AddComponent<Health>(new Health(5)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntity("Explosion", 0.5f, poss, ent, 3, Vector2.Zero).Refresh();

                    int splodeSound = 1;
                    SoundManager.Play("Explosion" + splodeSound.ToString());
                };

            #endregion Health

            ++barriers;
            e.Group = "Structures";
            return e;
        }
    }
}