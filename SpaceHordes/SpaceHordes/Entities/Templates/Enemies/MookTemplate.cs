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
            Rectangle source = new Rectangle();
            Random rbitch = new Random();
            Vector2 pos = new Vector2((float)(rbitch.NextDouble() * 2) - 1, (float)(rbitch.NextDouble() * 2) - 1);
            pos.Normalize();
            pos *= ScreenHelper.Viewport.Width;
            pos = ConvertUnits.ToSimUnits(pos);

            switch (type)
            {
                case 1:
                    source = _SpriteSheet.Animations["squidship"][0];
                    break;
                case 2:
                    source = _SpriteSheet.Animations["brownfangship"][0];
                    break;
                case 3:
                    source = _SpriteSheet.Animations["blueshipredexhaust"][0];
                    break;
                case 4:
                    source = _SpriteSheet.Animations["grayshipwithtwoprongs"][0];
                    break;
                case 5:
                    source = _SpriteSheet.Animations["grayshipwithtwowings"][0];
                    break;
                case 6:
                    source = _SpriteSheet.Animations["brownplane"][0];
                    break;
                case 7:
                    source = _SpriteSheet.Animations["bluecrystalship"][0];
                    break;
                case 8:
                    source = _SpriteSheet.Animations["brownthingwithbluelight"][0];
                    break;
                case 9:
                    source = _SpriteSheet.Animations["graytriangleship"][0];
                    break;
                case 10:
                    source = _SpriteSheet.Animations["eyeshot"][0];
                    break;
            }

            Body bitch = e.AddComponent<Body>(new Body(_World, e));
            FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(source.Center.X),1f,bitch);
            e.AddComponent<Sprite>(new Sprite(_SpriteSheet.Texture, source, bitch, 1f, Color.White, 0.5f));
            bitch.BodyType = GameLibrary.Dependencies.Physics.Dynamics.BodyType.Dynamic;
            bitch.Position = pos;


            return e;
        }

    }
}
