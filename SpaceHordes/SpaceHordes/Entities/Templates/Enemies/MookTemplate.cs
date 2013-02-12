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


            Vector2 pos = new Vector2((float)(rbitch.NextDouble() * 2) - 1, (float)(rbitch.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= ScreenHelper.Viewport.Width / 1.5f;
            pos = ConvertUnits.ToSimUnits(pos);
            bitch.Position = pos;

            #endregion

            #region AI/Health
            e.AddComponent<AI>(new AI((args[1] as Body)));
            if (s.Source.Count() > 1)
                e.AddComponent<Animation>(new Animation(AnimationType.Bounce, 20));

            e.AddComponent<Health>(new Health(1));

            #endregion

            #region Crystal

            Color crystalColor = Color.Green;
            int colorchance = rbitch.Next(100);
            if (colorchance > 50)
                crystalColor = Color.Blue;
            if (colorchance > 70)
                crystalColor = Color.Red;
            if (colorchance > 80)
                crystalColor = Color.Gray;
            e.AddComponent<Crystal>(new Crystal(crystalColor, rbitch.Next(2), "ASD"));
           

            #endregion

            e.Group = "Enemies";
            return e;
        }

    }
}
