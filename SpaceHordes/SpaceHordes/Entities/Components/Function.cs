using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Components
{
    public class Function : Component
    {
        public Func<Body, bool> Behavior;

        public Function(Func<Body, bool> b)
        {
            Behavior = b;
        }
    }
}
