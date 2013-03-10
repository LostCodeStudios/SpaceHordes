using GameLibrary.Dependencies.Entities;
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
    public class TurretTemplate : IEntityTemplate
    {
        private SpriteSheet _SpriteSheet;
        private EntityWorld _World;
        public TurretTemplate(SpriteSheet spriteSheet, EntityWorld world)
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
                    ConvertUnits.ToSimUnits(_SpriteSheet.Animations["miniturret"][0].Width / 2f),
                    ConvertUnits.ToSimUnits(_SpriteSheet.Animations["miniturret"][0].Height / 2f),
                    20,
                    1,
                    Body);
                Body.Position = ConvertUnits.ToSimUnits(new Vector2(0, 0));
                Body.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Static;
                Body.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1;

                Body.SleepingAllowed = false;
            }
            #endregion Body

            #region Sprite
            Sprite s = new Sprite(_SpriteSheet, "miniturret");
            e.AddComponent<Sprite>(s);
            #endregion Sprite

            #region AI
            AI AI = e.AddComponent<AI>(new AI(null,
                (turret, target) =>
                {
                    Body.RotateTo(target.Position); //TODO: Account for percentage of linear velocity + bullet travel time?

                    
                    return true;
                },
                "Enemies",
                40));

            #endregion AI

            #region Health
            e.AddComponent<Health>(new Health(1)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntity("Explosion", 0.5f, poss, ent, 3).Refresh();

                    int splodeSound = 1;
                    SoundManager.Play("Explosion" + splodeSound.ToString());
                };
            #endregion Health

            e.Group = "Structures";
            return e;
        }
    }
}
