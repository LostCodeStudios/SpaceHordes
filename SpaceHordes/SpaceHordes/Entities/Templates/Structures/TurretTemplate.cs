using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using SpaceHordes.Entities.Systems;
using System.Collections.Generic;

namespace SpaceHordes.Entities.Templates.Objects
{
    public class TurretTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        private DirectorSystem _DirectorSystem;

        public static List<Entity> Turrets = new List<Entity>();

        public TurretTemplate(SpriteSheet spriteSheet, DirectorSystem directorSystem, EntityWorld world)
        {
            Turrets.Clear();
            _SpriteSheet = spriteSheet;
            _DirectorSystem = directorSystem;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            #region Body

            Body Body = e.AddComponent<Body>(new Body(_World, e));
            {
                FixtureFactory.AttachEllipse(//Add a basic bounding box (rectangle status)
                    ConvertUnits.ToSimUnits(_SpriteSheet.Animations["miniturret"][0].Width / 2f),
                    ConvertUnits.ToSimUnits(_SpriteSheet.Animations["miniturret"][0].Height / 2f),
                    5,
                    1,
                    Body);
                Body.Position = ConvertUnits.ToSimUnits((Vector2)args[0]);
                Body.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
                Body.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat16;
                Body.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat5 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat16;
                Body.FixedRotation = false;
                Body.RotateTo((Body.Position));

                Body.SleepingAllowed = false;
            }

            #endregion Body

            #region Sprite

            Sprite s = new Sprite(_SpriteSheet, "miniturret", 0.4f);
            e.AddComponent<Sprite>(s);

            #endregion Sprite

            #region AI/GUN

            Inventory inv = e.AddComponent<Inventory>(new Inventory());

            AI ai = e.AddComponent<AI>(new AI(null,
                (target) => //AI FUNCTION
                {
                    Gun g = inv.CurrentGun;
                    g.BulletsToFire = true;

                    if (_DirectorSystem.SpawnState == SpawnState.Surge && g.Power == 1)
                    {
                        g.PowerUp((int)(DirectorSystem.StateDurations[(int)SpawnState.Surge] * 1000) - DirectorSystem.ElapsedSurge, 3);
                    }

                    /* Aiming *\
                     * X = v*t + x_o therefore
                     *  let X_aim = v_tar*t + x_tar
                     *            = v_bul*t + x_bul
                     *  therefore
                     *      0 = v_tar*t + x_tar - (v_bul*t + x_bul) =>
                     *  therefore
                     *      x_bul - x_tar = t(v_tar - v_bul)
                     *  so that
                     *      t = (x_bul - x_tar)/(v_tar - v_bul)
                     *  therefore X = (v_tar)*(x_bul - x_tar)/(v_tar - v_bul) + x_tar
                     */

                    //Vector2 zero = target.LinearVelocity*time - inv.CurrentGun.BulletVelocity*time + target.Position - Body.Position;
                    Vector2 time = (Body.Position - target.Position) /
                        (target.LinearVelocity - inv.CurrentGun.BulletVelocity);

                    Vector2 XFinal = (target.LinearVelocity) * time + target.Position;
                    Body.RotateTo(XFinal - Body.Position);

                    return true;

                    // Console.WriteLine((target.UserData as Entity).Tag + ": " + time);
                },
                "Enemies",
                ConvertUnits.ToSimUnits(500)));
            ai.Targeting = Targeting.Closest;

            #endregion AI/GUN

            #region Health

            e.AddComponent<Health>(new Health(5)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntity("Explosion", 0.5f, poss, ent, 2, Vector2.Zero).Refresh();

                    int splodeSound = 1;
                    SoundManager.Play("Explosion" + splodeSound.ToString());
                    Turrets.Remove(e);
                };

            #endregion Health

            #region Origin

            //e.AddComponent<Origin>(new Origin(args[1] as Entity));

            #endregion Origin

            e.Group = "Structures";

            Turrets.Add(e);
            return e;
        }
    }
}