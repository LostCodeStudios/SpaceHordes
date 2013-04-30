using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using System;
using GameLibrary.Helpers;
using GameLibrary.Entities.Components;
using System.Collections.Generic;

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
                            b.RotateTo(distance);
                        else if( ent.Group != null && ent.Group != "Boss")
                            b.AngularVelocity = (float)Math.PI * 4; 
                    }
                    return false;
                };
        }

        public static Func<Body, bool> CreateSinDownards(Entity ent, float speed, float sideTime, EntityWorld world)
        {
            float time = 0f;

            return
                (target) =>
                {
                    if (time > sideTime)
                    {
                        
                        time = 0f;
                    }

                    Body b = ent.GetComponent<Body>();
                    float x = (float)(ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width/2) * (Math.Cos(MathHelper.ToRadians(360 / sideTime) * time)));

                    if (!ent.HasComponent<Slow>())
                    {
                        b.Position = new Vector2(x, b.Position.Y + speed * world.Delta/1000);

                    
                        time += (float)world.Delta / 1000;
                    }
                    return false;
                };
        }

        public static Func<Body, bool> CreateKiller(Entity ent, float speed, float sideTime, EntityWorld world)
        {
            float time = 0f;

            return
                (target) =>
                {
                    if (time > sideTime)
                    {
                        time = 0f;
                    }

                    Body b = ent.GetComponent<Body>();
                    float x = (float)(ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width / 2) * (Math.Cos(MathHelper.ToRadians(360 / sideTime) * time)));

                    if (!ent.HasComponent<Slow>())
                    {
                        b.Position = new Vector2(x, b.Position.Y + speed * world.Delta / 1000);

                    
                        time += (float)world.Delta / 1000;
                        ent.GetComponent<Children>().CallChildren(b);
                    }

                    return false;
                };
        }

        public static Func<Body, bool> CreateFlamer(Entity ent, float speed, Body bitch, Sprite s, EntityWorld _World)
        {
            int times = 0;
            return
                (target) =>
                {
                    ++times;
                    Body b = ent.GetComponent<Body>();
                    Vector2 distance = target.Position - b.Position;

                    if (distance != Vector2.Zero)
                        distance.Normalize();
                    distance *= speed;

                    if (target != null && target.LinearVelocity != distance && !ent.HasComponent<Slow>())
                    {
                        b.LinearVelocity = distance;
                    }

                    if (times % 10 == 0)
                    {
                        int range = s.CurrentRectangle.Width / 2;
                        float posx = -range;
                        Vector2 pos1 = bitch.Position + ConvertUnits.ToSimUnits(new Vector2(posx, 0));
                        Vector2 pos2 = bitch.Position - ConvertUnits.ToSimUnits(new Vector2(posx, 0));
                        float x = posx / range;

                        float y = 1;

                        Vector2 velocity1 = new Vector2(x, y);
                        velocity1.Normalize();
                        velocity1 *= 7;
                        Vector2 velocity2 = new Vector2(-velocity1.X, velocity1.Y);

                        _World.CreateEntity("Fire", pos1, velocity1).Refresh();
                        _World.CreateEntity("Fire", pos2, velocity2).Refresh();
                    }
                    return false;
                };
        }

        public static Func<Body, bool> CreateKillerGun(Entity ent, Entity origin, Vector2 offs, float shotTime, float shootTime, float nonShot, Sprite s, EntityWorld _World, bool shit = false)
        {
            bool shot = false;
            float shotttt = 0f;
            float time = 0f;

            return
                (target) =>
                {
                    Body b = ent.GetComponent<Body>();
                    Body b2 = origin.GetComponent<Body>();

                    b.Position = b2.Position + offs;

                    if (shot)
                    {
                        if (shotttt > shotTime)
                        {
                            shotttt = 0f;

                            Vector2 offset = ConvertUnits.ToSimUnits(new Vector2(0, 50));

                            if (shit)
                                offset.X = ConvertUnits.ToSimUnits(6);
                            float rot = (float)Math.PI / 2;

                            Transform fireAt = new Transform(b.Position + offset, rot);

                            _World.CreateEntity("KillerBullet", fireAt).Refresh();
                        }
                        if (time > shootTime)
                        {
                            time = 0f;
                            shot = false;
                        }
                    }
                    else
                    {
                        if (time > nonShot)
                        {
                            time = 0f;
                            shot = true;
                        }
                    }

                    if (!ent.HasComponent<Slow>())
                    {
                        shotttt += (float)_World.Delta / 1000;
                        time += (float)_World.Delta / 1000;
                    }
                    return false;
                };
        }

        public static Func<Body, bool> CreateWarMachine(Entity ent, float speed, Body bitch, float sideTime, float shootTime, Sprite s, EntityWorld _World)
        {
            float shotTime = 0f;
            float time = 0f;

            return
                (target) =>
                {
                    if (time > sideTime)
                        time = 0f;

                    Body b = ent.GetComponent<Body>();
                    float x = (float)(ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width / 2) * (Math.Cos(MathHelper.ToRadians(360 / sideTime) * time)));

                    

                    if (shotTime > shootTime)
                    {
                        shotTime = 0f;
                        Vector2 velocity1 = new Vector2(0, 1);
                        velocity1 *= 8f;

                        _World.CreateEntity("ExplosiveBullet", bitch.Position, velocity1, 1, "reddownmissile").Refresh();
                    }

                    if (!ent.HasComponent<Slow>())
                    {
                        b.Position = new Vector2(x, b.Position.Y + speed * _World.Delta / 1000);
                        shotTime += (float)_World.Delta / 1000;
                        time += (float)_World.Delta / 1000;
                    }
                    return false;
                };
        }

        public static Func<Body, bool> CreateBigGreen(Entity ent, float speed, float sideTime, float shootTime, float shotTime, float nonShoot, Sprite s, EntityWorld _World)
        {
            bool shot = false;
            float shotttt = 0f;
            float time = 0f;
            float ttttime = 0f;

            return
                (target) =>
                {
                    if (ttttime > sideTime)
                    {
                        ttttime = 0f;
                    }

                    Body b = ent.GetComponent<Body>();
                    float x = (float)(ConvertUnits.ToSimUnits(ScreenHelper.Viewport.Width / 2) * (Math.Cos(MathHelper.ToRadians(360 / sideTime) * ttttime)));

                    

                    time += (float)_World.Delta / 1000;

                    if (shot)
                    {
                        if (shotttt > shotTime)
                        {
                            shotttt = 0f;

                            Vector2 offset = ConvertUnits.ToSimUnits(new Vector2(0, 25));

                            float rot = (float)Math.PI / 2;

                            Transform fireAt = new Transform(b.Position + offset, rot);

                            _World.CreateEntity("BigGreenBullet", fireAt).Refresh();
                        }
                        if (time > shootTime)
                        {
                            time = 0f;
                            shot = false;
                        }
                    }
                    else
                    {
                        if (time > nonShoot)
                        {
                            time = 0f;
                            shot = true;
                        }
                    }

                    if (!ent.HasComponent<Slow>())
                    {
                        b.Position = new Vector2(x, b.Position.Y + speed * _World.Delta / 1000);
                        shotttt += (float)_World.Delta / 1000;
                        time += (float)_World.Delta / 1000;
                        ttttime += (float)_World.Delta / 1000;
                    }
                    return false;
                };
        }

        public static Func<Body, bool> CreateCannon(Entity ent, bool rotateTo = true)
        {
            float shootDistance = ConvertUnits.ToSimUnits(700);

            return
                (target) =>
                {
                    Body b = ent.GetComponent<Body>();
                    float distance = Vector2.Distance(b.Position, target.Position);

                    if (distance < shootDistance)
                    {
                        Vector2 direction = target.Position - b.Position;
                        direction.Normalize();
                        if (rotateTo)
                        {
                            b.RotateTo(direction);
                            ent.GetComponent<Inventory>().CurrentGun.BulletsToFire = true;
                        }
                    }

                    b.LinearVelocity = ent.GetComponent<Origin>().Parent.GetComponent<Body>().LinearVelocity;

                    ent.Refresh();
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
                        b.LinearVelocity = direction;
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
    }
        #endregion Behaviors
}