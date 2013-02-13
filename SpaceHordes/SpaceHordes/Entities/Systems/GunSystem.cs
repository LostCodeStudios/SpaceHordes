using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using GameLibrary;
using SpaceHordes.Entities.Components;
using GameLibrary.Entities.Components;
using Microsoft.Xna.Framework;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework.Input;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    class GunSystem : IntervalEntityProcessingSystem
    {
        ComponentMapper<ITransform> transformMapper;
        ComponentMapper<Inventory> invMapper;

        static Random r = new Random();

        int elapsedMilli = 16;

        public GunSystem()
            : base(16, typeof(Inventory), typeof(ITransform))
        {
        }

        public override void Initialize()
        {
            invMapper = new ComponentMapper<Inventory>(world);
            transformMapper = new ComponentMapper<ITransform>(world);
        }

        public override void Process()
        {
            base.Process();
        }

        public override void Process(Entity e)
        {
            //Process guns
            Inventory inv = invMapper.Get(e);
            Gun gun = inv.CurrentGun;

            ITransform transform = transformMapper.Get(e);

            gun.Elapsed += elapsedMilli;

            //Update power
            if (gun.Power > 1)
            {
                gun.UpdatePower(elapsedMilli);
            }

            if (e.Group.Equals("Players"))
            {
                gun.BulletsToFire = false;

                int index = int.Parse(e.Tag.Replace("P", "")) - 1;
                PlayerIndex playerIndex = (PlayerIndex)index;
                GamePadState padState = GamePad.GetState(playerIndex);
                KeyboardState keyState = Keyboard.GetState();

                if (padState.IsConnected)
                {
                    if (padState.IsButtonDown(Buttons.RightTrigger))
                        gun.BulletsToFire = true;
                }

                else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    gun.BulletsToFire = true;
            }


            //Fire bullets bro
            if (gun.Elapsed > gun.Interval && gun.BulletsToFire && gun.Ammunition > 0)
            {
                gun.BulletsToFire = false;
                gun.Elapsed = 0;
                gun.Ammunition--;
                Entity bullet = world.CreateEntity(gun.BulletTemplateTag, transform);
                bullet.Refresh();

                int shot = r.Next(1, 3);
                SoundManager.Play("Shot" + shot.ToString());
            }
        }
    }
}