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
        public MookTemplate(SpriteSheet spriteSheet, EntityWorld world)
        {
            _SpriteSheet = spriteSheet;
            _World = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            int type = (int)args[0];
            string spriteKey = "";
            Random rbitch = new Random();
            Vector2 pos = new Vector2((float)(rbitch.NextDouble() * 2) - 1, (float)(rbitch.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= ScreenHelper.Viewport.Width;
            pos = ConvertUnits.ToSimUnits(pos);

            switch (type)
            {
                case 1:
                    spriteKey = "squidship";
                    break;
                case 2:
                    spriteKey = "brownfangship";
                    break;
                case 3:
                    spriteKey = "blueshipredexhaust";
                    break;
                case 4:
                    spriteKey = "grayshipwithtwoprongs";
                    break;
                case 5:
                    spriteKey = "grayshipwithtwowings";
                    break;
                case 6:
                    spriteKey = "brownplane";
                    break;
                case 7:
                    spriteKey = "bluecrystalship";
                    break;
                case 8:
                    spriteKey = "brownthingwithbluelight";
                    break;
                case 9:
                    spriteKey = "graytriangleship";
                    break;
                case 10:
                    spriteKey = "eyeshot";
                    break;
            }

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Width / 2), ConvertUnits.ToSimUnits(_SpriteSheet[spriteKey][0].Height / 2), 5, 1f, bitch);
            Sprite s = e.AddComponent<Sprite>(new Sprite(_SpriteSheet, spriteKey, bitch, 1f, Color.White, 0.5f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.Position = pos;
            e.AddComponent<AI>(new AI((args[1] as Body)));
            
            
            return e;
        }

    }
}
