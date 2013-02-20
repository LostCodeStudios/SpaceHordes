using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    class BaseAnimationSystem : TagSystem
    {
        int _MaxH = 20;
        float _X = 0;
        float _Speed = 0.25f;
        float _Direction;
        
        public BaseAnimationSystem(float speed, int maxHeight)
            : base("Base")
        {
            _MaxH = maxHeight;
            _Speed = speed;
            _Direction = _Speed;
        }

        public override void Process(Entity e)
        {
            Body b = e.GetComponent<Body>();
  

            if (ConvertUnits.ToDisplayUnits(b.Position.Y) >= _MaxH)
                _Direction = -_Speed;
            if (ConvertUnits.ToDisplayUnits(b.Position.Y) <= 0)
                _Direction = _Speed;


            b.Position += new Microsoft.Xna.Framework.Vector2(b.Position.X - _X,ConvertUnits.ToSimUnits(_Direction));
        }
    }
}
