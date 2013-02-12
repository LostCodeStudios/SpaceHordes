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
    public class AnimationSystem : EntityProcessingSystem
    {
        public AnimationSystem()
            : base(typeof(Animation))
        {
        }

        public override void Process(Entity e)
        {
            Sprite sprite = e.GetComponent<Sprite>();
            Animation anim = e.GetComponent<Animation>();

            if (anim.Type != AnimationType.None)
            {
                anim._Tick++;
                anim._Tick %= anim.FrameRate;
                if (anim._Tick == 0) //If time to animate.
                {
                    switch (anim.Type)
                    {
                        case AnimationType.Loop:
                            sprite.FrameIndex++;
                            break;
                        case AnimationType.ReverseLoop:
                            sprite.FrameIndex--;
                            break;
                        case AnimationType.Increment:
                            sprite.FrameIndex++;
                            anim.Type = AnimationType.None;
                            break;
                        case AnimationType.Decrement:
                            sprite.FrameIndex--;
                            anim.Type = AnimationType.None;
                            break;
                        case AnimationType.Bounce:
                            sprite.FrameIndex += anim.FrameInc;
                            if (sprite.FrameIndex == sprite.Source.Count() - 1)
                                anim.FrameInc = -1;

                            if (sprite.FrameIndex == 0)
                                anim.FrameInc = 1;
                            break;
                        case AnimationType.Once:
                            if (sprite.FrameIndex < sprite.Source.Count() - 1)
                                sprite.FrameIndex++;
                            else
                                anim.Type = AnimationType.None;
                            break;
                    }
                    e.RemoveComponent<Sprite>(e.GetComponent<Sprite>());
                    e.AddComponent<Sprite>(sprite);
                }
            }
        }
    }
}
