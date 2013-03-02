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

    public enum Targeting
    {
        Constant,
        Closest,
        Strongest,
        Weakest
    }

    public class AI : Component
    {
        private Body target;
        private Behavior behavior;
        private Targeting targeting;

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

        public Targeting Targeting
        {
            get { return targeting; }
            set { targeting = value; }
        }

        public float SearchRadius
        {
            get;
            set;
        }

        public string HostileGroup
        {
            get;
            set;
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