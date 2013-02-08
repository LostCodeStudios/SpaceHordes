using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Dependencies.Entities;

namespace SpaceHordes.Entities.Components
{
    public enum Behavior
    {
        Follow,
        FollowAndGun
    }

    public class AI : Component
    {
        Body target;
        Behavior behavior;

        public Body Target
        {
            get { return target; }
            set { target = value; }
        }

        public Behavior Behavior
        {
            get { return behavior; }
            set { behavior = value; }
        }

        public AI()
        {
        }

        public AI(Body target)
        {
            Target = target;
        }
    }
}
