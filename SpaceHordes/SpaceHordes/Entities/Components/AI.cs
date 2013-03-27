using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using System;

namespace SpaceHordes.Entities.Components
{
    public enum Targeting
    {
        None,
        Closest,
        Strongest,
        Weakest
    }

    /// <summary>
    /// The AI component class
    /// </summary>
    public class AI : Component
    {
        public AI(Body target, Func<Body, bool> behavior, string targetGroup = "", float searchRadius = 200f)
        {
            this.Target = target;
            this.Targeting = Targeting.Closest;
            this.TargetGroup = targetGroup;
            this.SearchRadius = searchRadius;
            this.Behavior = behavior;
            Notify = (x, y) => { return; };
        }

        #region Properties

        /// <summary>
        /// The radius in which the AISystem will search for a new target.
        /// </summary>
        public float SearchRadius
        {
            get;
            set;
        }

        /// <summary>
        /// The target entity group in which a close entity must be.
        /// </summary>
        public string TargetGroup
        {
            get;
            set;
        }

        /// <summary>
        /// The way with which targets are targeted
        /// </summary>
        public Targeting Targeting
        {
            get;
            set;
        }

        #endregion Properties

        #region Fields

        /// <summary>
        /// The target which the AI component will process.
        /// </summary>
        public Body Target;

        /// <summary>
        /// Specifies the behavior of the AI component.
        /// Returns true if the AI system is to search for a new target for this specific AI component.
        /// </summary>
        public Func<Body, bool> Behavior;

        /// <summary>
        /// Messages the AI sub system of this component.
        /// </summary>
        public Action<string, Entity> Notify;

        #endregion Fields

        #region Behaviors

        /// <summary>
        /// Creates a follow behavior
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="rotateTo"></param>
        /// <returns></returns>
        public static Func<Body, bool> CreateFollow(Entity ent, float speed, bool rotateTo = true)
        {
            return
                (target) =>///edward
                {
                    Body b = ent.GetComponent<Body>();
                    Vector2 distance = target.Position - b.Position;

                    if (distance != Vector2.Zero)
                        distance.Normalize();
                    distance *= speed;

                    if (target != null && target.LinearVelocity != distance && !ent.HasComponent<Slow>())
                    {
                        b.LinearVelocity = distance;
                        if (rotateTo)
                            b.RotateTo(distance) ;
                    }

                    Console.WriteLine("TARGET: " + (target.UserData as Entity).Group);
                    return false;
                };
        }

        public static Func<Body, bool> CreateShoot(Entity ent, float speed, float shootDistance, bool rotateTo = true)
        {
            return
                (target) =>
                {
                    Body b = ent.GetComponent<Body>();
                    float distance = Vector2.Distance(b.Position, target.Position);

                    Vector2 direction = target.Position - b.Position + ent.GetComponent<Inventory>().CurrentGun.GunOffsets[0];
                    direction.Normalize();
                    b.RotateTo(direction);
                    if (distance > shootDistance)
                    {
                        direction *= 5f;
                        b.LinearVelocity = direction + new Vector2((float)Math.Sin(distance) ); //Wavy motion

                    }

                    else
                    {
                        b.LinearVelocity = new Vector2(MathHelper.SmoothStep(b.LinearVelocity.X, 0, 0.1f), MathHelper.SmoothStep(b.LinearVelocity.Y, 0, 0.1f));
                        ent.GetComponent<Inventory>().CurrentGun.BulletsToFire = true;
                    }
                    ent.Refresh();
                    return false;
                };
        }

        #endregion Behaviors
    }

    ////TODO: DEPRICATE
    //public class AI : Component
    //{
    //    private Body target;
    //    private Behavior behavior;
    //    private Targeting targeting;

    //    public event Action TargetChangedEvent;

    //    public Body Target
    //    {
    //        get { return target; }
    //        set
    //        {
    //            if (TargetChangedEvent != null)
    //                TargetChangedEvent();
    //            target = value;
    //        }
    //    }

    //    public Behavior Behavior
    //    {
    //        get { return behavior; }
    //        set { behavior = value; }
    //    }

    //    public Targeting Targeting
    //    {
    //        get { return targeting; }
    //        set { targeting = value; }
    //    }

    //    public float SearchRadius
    //    {
    //        get;
    //        set;
    //    }

    //    public string HostileGroup
    //    {
    //        get;
    //        set;
    //    }

    //    public AI()
    //    {
    //    }

    //    public AI(Body target)
    //    {
    //        Target = target;
    //    }
    //}
}