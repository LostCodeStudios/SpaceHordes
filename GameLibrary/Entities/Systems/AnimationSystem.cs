using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using GameLibrary.Entities.Systems;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;



namespace SpaceHordes.Entities.Systems
{
    public class SpriteAnimationSystem : IntervalEntityProcessingSystem
    {
        public SpriteAnimationSystem()
            : base(1, typeof(Sprite))
        {
        }

        public override void Process(Entity e)
        {
            Sprite s = e.GetComponent<Sprite>();

            //if (s.FrameDelay == 0 || s.Source.Count() == 1) //If frameTime is zero or we only have one frame, we don't have to update.
            //    return;

            ////s.Elapsed += gameTime.ElapsedGameTime; //Find a way to fix this. Couldn't figure out how to get in a GameTime
            //s.Elapsed += TimeSpan.FromSeconds(.005);
            //if (s.Elapsed.TotalSeconds >= s.FrameDelay)
            //{
            //    s.Elapsed = TimeSpan.Zero;

            //    s.FrameIndex = ++s.FrameIndex % (s.Source.Count() - 1);
            //}
        }
    }
}
