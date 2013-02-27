using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using System;

namespace SpaceHordes.Entities.Components
{
    public enum Behavior
    {
        Follow,          //Follows until hits target.
        FollowAndGun,    //Follows and shoots untill hits target
        CareFollow,      //(Careful mode: keeps a distance) Follows until hits target.
        CareFollowAndGun //(Careful mode: keeps a distance) Follows and shoots untill hits target
    }

    public class AI : Component
    {
        private Body target;
        private Behavior behavior;

        public event Action TargetChangedEvent;

        public Body Target
        {
            get { return target; }
            set
            {
                if (TargetChangedEvent != null)
                    TargetChangedEvent();
                target = value;
            }
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