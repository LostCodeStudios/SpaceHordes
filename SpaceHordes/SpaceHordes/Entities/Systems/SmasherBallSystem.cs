using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using Microsoft.Xna.Framework;
using SpaceHordes.Entities.Components;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    public class SmasherBallSystem : IntervalTagSystem
    {
        float elapsedSeconds = 0f;
        float orbitTime = 1f;
        public static float Radius = 10f;

        public SmasherBallSystem()
            : base(16, "SmasherBall")
        {
        }

        public override void Process(Entity e)
        {
            Body b = e.GetComponent<Body>();
            Entity parent = e.GetComponent<Origin>().Parent;

            if (!parent.HasComponent<Body>() || parent.GetComponent<Health>().IsAlive)
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


            Vector2 oldPosition = origin + new Vector2((float)(ConvertUnits.ToSimUnits(Radius) * (Math.Cos(MathHelper.ToRadians(360/orbitTime) * lastTime))), (float)(ConvertUnits.ToSimUnits(Radius) * (Math.Sin(MathHelper.ToRadians(360/orbitTime) * lastTime))));
            Vector2 newPosition = origin + new Vector2((float)(ConvertUnits.ToSimUnits(Radius) * (Math.Cos(MathHelper.ToRadians(360/orbitTime) * elapsedSeconds))), (float)(ConvertUnits.ToSimUnits(Radius) * (Math.Sin(MathHelper.ToRadians(360/orbitTime) * elapsedSeconds))));

            b.LinearVelocity = parent.GetComponent<Body>().LinearVelocity + (newPosition - oldPosition);
        }
    }
}
