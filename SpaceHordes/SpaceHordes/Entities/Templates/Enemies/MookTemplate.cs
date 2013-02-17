using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Physics.Factories;
using SpaceHordes.Entities.Components;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Dynamics.Contacts;

namespace SpaceHordes.Entities.Templates.Enemies
{
    public class MookTemplate : IEntityTemplate
    {
        SpriteSheet _SpriteSheet;
        EntityWorld _World;
        static Random rbitch = new Random();
        public MookTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            int type = (int)args[0];
           
            #region Sprite
            string spriteKey = "";
            switch (type)
            {
                case 0:
                    spriteKey = "squidship";
                    break;
                case 1:
                    spriteKey = "brownfangship";
                    break;
                case 2:
                    spriteKey = "blueshipredexhaust";
                    break;
                case 3:
                    spriteKey = "grayshipwithtwoprongs";
                    break;
                case 4:
                    spriteKey = "grayshipwithtwowings";
                    break;
                case 5:
                    spriteKey = "brownplane";
                    break;
                case 6:
                    spriteKey = "bluecrystalship";
                    break;
                case 7:
                    spriteKey = "brownthingwithbluelight";
                    break;
                case 8:
                    spriteKey = "graytriangleship";
                    break;
                case 9:
                    spriteKey = "eyeshot";
                    break;
            }
            #endregion

            #region Body
            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.5f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.CollisionCategories = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat2;
            bitch.CollidesWith = GameLibrary.Dependencies.Physics.Dynamics.Category.Cat1;
            bitch.Mass++;

            Vector2 pos = new Vector2((float)(rbitch.NextDouble() * 2) - 1, (float)(rbitch.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= ScreenHelper.Viewport.Width;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;

            #endregion

            #region Animation
            if (s.Source.Count() > 1)
                e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 10));
            #endregion

            #region Crystal

            Color crystalColor = Color.Green;
            int colorchance = rbitch.Next(100);
            int amount = 3;
            if (colorchance > 50)
            {
                crystalColor = Color.Blue;
                amount = 2;
            }
            if (colorchance > 70)
            {
                crystalColor = Color.Red;
                amount = 1;
            }
            if (colorchance > 80)
            {
                crystalColor = Color.Yellow;
                amount = 5;
            }
            if (colorchance > 90)
            {
                crystalColor = Color.Gray;
                amount = 2;
            }
            e.AddComponent<Crystal>(new Crystal(crystalColor, amount));      

            #endregion

            #region AI/Health
            e.AddComponent<AI>(new AI((args[1] as Body)));

            e.AddComponent<Health>(new Health(1)).OnDeath +=
                ent =>
                {
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    _World.CreateEntity("Explosion", 0.5f, poss, ent).Refresh();
                    e.Delete();

                    int splodeSound = rbitch.Next(1, 5);
                    SoundManager.Play("Explosion" + splodeSound.ToString());
                    _World.CreateEntity("Crystal", e.GetComponent<ITransform>().Position, e.GetComponent<Crystal>().Color, e.GetComponent<Crystal>().Amount, ent);
                };

            #endregion

          

            e.Group = "Enemies";
            return e;
        }

    }
}
