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
                ITransform t = e.GetComponent<ITransform>();

                int index;
                try
                {
                    index = int.Parse(e.Tag.Replace("P", "")) - 1;
                }
                catch
                {
                    return;
                }
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

                if (!inv.BuildMode && 
                    (GamePad.GetState((PlayerIndex)index).IsButtonDown(Buttons.LeftTrigger) || 
                    (!GamePad.GetState((PlayerIndex)index).IsConnected && Mouse.GetState().RightButton == ButtonState.Pressed)))
                {
                    world.CreateEntityGroup("BaseShot", "Bullets", e);
                    if (gun.Ammunition <= 0)
                        inv.ChangeGun(e, GunType.WHITE);
                }
            }

            //Fire bullets bro
            if (!inv.BuildMode && gun.Elapsed > gun.Interval / gun.Power && gun.BulletsToFire && gun.Ammunition > 0)
            {
                if (inv._type == InvType.Cannon)
                {
                    ITransform t = e.GetComponent<ITransform>();
                    world.CreateEntity("ExplosiveBullet", t.Position, new Vector2((float)Math.Cos(t.Rotation) * 8, (float)Math.Sin(t.Rotation) * 8), 1).Refresh();
                }
                else
                {
                    foreach (Vector2 offset in gun.GunOffsets)
                    {
                        float rotation = transform.Rotation;
                        float r_o = (float)Math.Atan2(offset.X, offset.Y);
                        float r_a = (float)Math.Atan2(offset.Y, offset.X);

                        Vector2 rotatedOffset = ConvertUnits.ToSimUnits(new Vector2((float)Math.Cos(r_a + rotation) * offset.Length(), (float)Math.Sin(r_a + rotation) * offset.Length()));
                        Transform fireAt = new Transform(transform.Position + rotatedOffset, rotation);

                        Entity bullet = world.CreateEntity(gun.BulletTemplateTag, fireAt);
                        gun.BulletVelocity = bullet.GetComponent<IVelocity>().LinearVelocity;
                        Bullet bb = bullet.GetComponent<Bullet>();
                        bb.Firer = e;
                        bullet.RemoveComponent<Bullet>(bullet.GetComponent<Bullet>());
                        bullet.AddComponent<Bullet>(bb);
                        bullet.Refresh();

                        int shot = r.Next(1, 3);
                        if (e.Group == "Structures" || e.Group == "Enemies")
                            SoundManager.Play("Shot" + shot.ToString(), .25f);
                        else
                            SoundManager.Play("Shot" + shot.ToString());
                    }


                    --gun.Ammunition;
                    if (gun.Ammunition == 0)
                    {
                        inv.CurrentGun = inv.WHITE;
                    }
                }

                gun.BulletsToFire = false;
                gun.Elapsed = 0;
            }
        }
    }
}