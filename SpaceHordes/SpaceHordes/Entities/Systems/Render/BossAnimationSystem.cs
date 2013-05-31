using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using SpaceHordes.Entities.Components;
using System;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    public class BossAnimationSystem : EntityProcessingSystem
    {
        static Random rbitch = new Random();

        public BossAnimationSystem(EntityWorld world)
            : base(typeof(Sprite), typeof(Animation), typeof(Health))
        {
            this.world = world;
        }

        public override void Process(Entity e)
        {
            if (!e.Tag.Contains("Boss"))
                return;

            #region Sprite Animation

            Sprite s = e.GetComponent<Sprite>();
            Health h = e.GetComponent<Health>();
            Animation a = e.GetComponent<Animation>();

            if (h.IsAlive && a.Type == AnimationType.None)
            {
                e.RemoveComponent<Sprite>(s);

                double healthFraction = (h.CurrentHealth / h.MaxHealth);

                int frame = 0;
                int frames = s.Source.Length;

                frame = (int)(frames - (frames * healthFraction));

                if (frame != s.FrameIndex)
                {
                    int splodeSound = rbitch.Next(1, 5);
                    SoundManager.Play("Explosion" + splodeSound.ToString());
                    Vector2 poss = e.GetComponent<ITransform>().Position;
                    world.CreateEntityGroup("BigExplosion", "Explosions", poss, 15, e, e.GetComponent<IVelocity>().LinearVelocity);
                }
                s.FrameIndex = frame;

                e.AddComponent<Sprite>(s);
            }

            #endregion Sprite Animation
        }
    }
}