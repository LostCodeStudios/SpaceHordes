using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceHordes.Entities.Components;
using System;

namespace SpaceHordes.Entities.Systems
{
    internal class GunSystem : IntervalEntityProcessingSystem
    {
        private ComponentMapper<ITransform> transformMapper;
        private ComponentMapper<Inventory> invMapper;

        private static Random r = new Random();

        private static int elapsedMilli = 16;

        public GunSystem()
            : base(elapsedMilli, typeof(Inventory), typeof(ITransform))
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
            if (gun.Elapsed > gun.Interval / gun.Power && gun.BulletsToFire && gun.Ammunition > 0)
            {
                gun.BulletsToFire = false;
                gun.Elapsed = 0;
                gun.Ammunition--;
                Entity bullet = world.CreateEntity(gun.BulletTemplateTag, transform);
                gun.BulletVelocity = bullet.GetComponent<IVelocity>().LinearVelocity;
                Bullet bb = bullet.GetComponent<Bullet>();
                bb.Firer = e;
                bullet.RemoveComponent<Bullet>(bullet.GetComponent<Bullet>());
                bullet.AddComponent<Bullet>(bb);
                bullet.Refresh();

                int shot = r.Next(1, 3);
                if (e.Group == "Structures")
                    SoundManager.Play("Shot" + shot.ToString(), .25f);
                else
                    SoundManager.Play("Shot" + shot.ToString());
            }
        }
    }
}