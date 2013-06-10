using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using System;

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