using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Templates.Objects
{
    public class CrystalTemplate : IEntityTemplate
    {
        private EntityWorld _World;
        private SpriteSheet _SpriteSheet;

        private static int crystals = 0;

        public CrystalTemplate(EntityWorld World, SpriteSheet spriteSheet)
        {
            crystals = 0;
            this._World = World;
            this._SpriteSheet = spriteSheet;
        }

        /// <summary>
        /// Builds the crystal at a specified position and a color.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args">[0] = position; [1] = color; [2] amount</param>
        /// <returns></returns>
        public Entity BuildEntity(Entity e, params object[] args)
        {
            Vector2 pos = (Vector2)args[0];
            Color color = (Color)args[1];
            string source = "redcrystal";

            e.AddComponent<Components.Timer>(new Components.Timer(10));

            if (color == Color.Red)
                source = "redcrystal";
            if (color == Color.Blue)
                source = "bluecrystal";
            if (color == Color.Yellow)
                source = "yellowcrystal";
            if (color == Color.Green)
                source = "greencrystal";
            if (color == Color.Gray)
                source = "graycrystal";

            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, source, 0.2f + (float)crystals / 10000f));
            Body b = e.AddComponent<Body>(new Body(_World, e));
            b.IsBullet = true;
            FixtureFactory.AttachEllipse((float)ConvertUnits.ToSimUnits(s.CurrentRectangle.Width / 1.5), (float)ConvertUnits.ToSimUnits(s.CurrentRectangle.Height / 1.5), 3, 1f, b);

            b.Position = pos;
            b.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;

            e.GetComponent<Body>().OnCollision += LambdaComplex.CrystalCollision();

            if (args.Length > 4)
            {
                e.AddComponent<Crystal>(new Crystal(color, (int)args[2], true));
                e.AddComponent<AI>(new AI((args[3] as Entity).GetComponent<Body>(), AI.CreateFollow(e, 5f, false)));
            }
            else if (args.Length > 3)
            {
                e.AddComponent<Crystal>(new Crystal(color, (int)args[2]));
                e.AddComponent<AI>(new AI((args[3] as Entity).GetComponent<Body>(), AI.CreateFollow(e, 5f, false)));
            }

            else
            {
                e.AddComponent<Crystal>(new Crystal(color, (int)args[2]));
            }
            e.Group = "Crystals";


            e.AddComponent<AI>(new AI((args[3] as Entity).GetComponent<Body>(), //AI was severely lagging the game.
                (target) =>
                {
                    if ((target.UserData as Entity).HasComponent<Health>() && (target.UserData as Entity).GetComponent<Health>().IsAlive && target.Position != b.Position && (target.UserData as Entity).Group == "Players")
                    {
                        Vector2 distance = target.Position - b.Position;
                        distance.Normalize();
                        b.LinearVelocity = distance * new Vector2(14);
                        return false;
                    }
                    else
                    {
                        e.Delete();
                        return false;
                    }
                }));

            e.Refresh();
            return e;
        }
    }
}