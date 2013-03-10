using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using System;

namespace SpaceHordes.Entities.Systems
{
    public class SmasherBallSystem : IntervalTagSystem
    {
        private float elapsedSeconds = 0f;
        private float orbitTime = 1f;
        public static float Radius = 100f;

        public SmasherBallSystem()
            : base(16, "SmasherBall")
        {
        }

        public override void Process(Entity e)
        {
            Body b = e.GetComponent<Body>();
            Origin o = e.GetComponent<Origin>();
            if (o != null)
            {
                Entity parent = o.Parent;
                if (!parent.HasComponent<Body>() || !parent.GetComponent<Health>().IsAlive)
                {
                    e.GetComponent<Health>().SetHealth(e, 0);
                    return;
                }

                Vector2 origin = parent.GetComponent<Body>().Position;

                float lastTime = elapsedSeconds;
                elapsedSeconds += 16 / 1000f;

                if (elapsedSeconds > orbitTime)
                {
                    elapsedSeconds = 0f;
                }

                Vector2 oldPosition = origin + new Vector2((float)(ConvertUnits.ToSimUnits(Radius) * (Math.Cos(MathHelper.ToRadians(360 / orbitTime) * lastTime))), (float)(ConvertUnits.ToSimUnits(Radius) * (Math.Sin(MathHelper.ToRadians(360 / orbitTime) * lastTime))));
                Vector2 newPosition = origin + new Vector2((float)(ConvertUnits.ToSimUnits(Radius) * (Math.Cos(MathHelper.ToRadians(360 / orbitTime) * elapsedSeconds))), (float)(ConvertUnits.ToSimUnits(Radius) * (Math.Sin(MathHelper.ToRadians(360 / orbitTime) * elapsedSeconds))));

                //b.LinearVelocity = parent.GetComponent<Body>().LinearVelocity + (newPosition - oldPosition);
                b.Position = newPosition;
            }
        }
    }
}