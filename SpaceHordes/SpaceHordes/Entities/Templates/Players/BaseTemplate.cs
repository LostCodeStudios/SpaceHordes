using GameLibrary;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Factories;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework.Input;

namespace SpaceHordes.Entities.Templates
{
    internal class BaseTemplate : IEntityTemplate
    {
        private World world;
        private SpriteSheet spriteSheet;

        public BaseTemplate(World world, SpriteSheet spriteSheet)
        {
            this.world = world;
            this.spriteSheet = spriteSheet;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "Base";
            e.Tag = "Base";

            #region Body

            Body Body = e.AddComponent<Body>(new Body(world, e));
            {
                FixtureFactory.AttachEllipse(//Add a basic bounding box (rectangle status)
                    ConvertUnits.ToSimUnits(spriteSheet.Animations["base"][0].Width / 2f),
                    ConvertUnits.ToSimUnits(spriteSheet.Animations["base"][0].Height / 2f),
                    15,
                    1,
                    Body);
                Body.Position = ConvertUnits.ToSimUnits(new Vector2(0, 0));
                Body.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Static;
                Body.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat6 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat16;
                Body.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat4 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat5 | GameLibrary.Dependencies.Physics.Dynamics.Category.Cat16;

                Body.SleepingAllowed = false;
            }

            #endregion Body

            #region Sprite

            Sprite Sprite = e.AddComponent<Sprite>(
                new Sprite(spriteSheet, "base",
                    Body, 1, Color.White, 0.1f));

            #endregion Sprite

            e.AddComponent<Score>(new Score());

            int health = 30;
#if DEBUG
            health = 1000000;
#endif
            Health h = new Health(health);
            h.OnDeath += LambdaComplex.BigEnemyDeath(e, world as SpaceWorld, 0);

            h.OnDamage +=
                ent =>
                {
                    SoundManager.SetVibration(0.25f, 0.15f);

                    e.RemoveComponent<Sprite>(Sprite);

                    double healthFraction = (h.CurrentHealth / h.MaxHealth);

                    if (healthFraction >= 0.33 && Sprite.FrameIndex == 2)
                    {
                        Sprite.FrameIndex = 1;
                    }
                    else if (healthFraction < 0.33 && Sprite.FrameIndex == 1)
                    {
                        Sprite.FrameIndex = 2;
                        SoundManager.SetVibration(0.3f, 0.3f);
                    }
                    
                    else if (healthFraction >= 0.66 && Sprite.FrameIndex == 1)
                    {
                        Sprite.FrameIndex = 0;
                    }
                    else if (healthFraction < 0.66 && Sprite.FrameIndex == 0)
                    {
                        Sprite.FrameIndex = 1;
                        SoundManager.SetVibration(0.3f, 0.3f);
                    }
                    


                    e.AddComponent<Sprite>(Sprite);
                };

            e.AddComponent<Health>(h);

            e.AddComponent<HealthRender>(new HealthRender());

            return e;
        }
    }
}