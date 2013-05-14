using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceHordes.Entities.Components;
using System;

namespace SpaceHordes.Entities.Systems
{
    public class PlayerControlSystem : GroupSystem
    {
        private ComponentMapper<Body> bodyMapper;
        private float _Velocity;
        private bool[] WasMoving = new bool[4] { false, false, false, false };

        private KeyboardState keyState;
        private KeyboardState lastKeyState;

        private GamePadState[] padState = new GamePadState[4];
        private GamePadState[] lastPadState = new GamePadState[4];

        private MouseState mouseState;

        public static uint TurretPrice = 50;
        public static uint BarrierPrice = 25;
        public static uint MinePrice = 10;

        public PlayerControlSystem(float velocity)
            : base("Players")
        {
            this._Velocity = velocity;
        }

        public override void Initialize()
        {
            bodyMapper = new ComponentMapper<Body>(world);
        }

        public override void Process()
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            for (int i = 0; i < 4; ++i)
                padState[i] = GamePad.GetState((PlayerIndex)i);

            base.Process();

            lastKeyState = keyState;
            lastPadState = (GamePadState[])padState.Clone();

            
        }

        public override void Process(Entity e)
        {
            Body b = bodyMapper.Get(e);
            Inventory inv = e.GetComponent<Inventory>();
            Gun g = inv.CurrentGun;

            Vector2 target = Vector2.Zero;

            #region Gamepad

            int playerIndex;
            try
            {
                playerIndex = int.Parse(e.Tag.Replace("P", "")) - 1;
            }
            catch
            {
                return;
            }
            PlayerIndex index = (PlayerIndex)playerIndex;
            GamePadState pad = padState[playerIndex];
            GamePadState lastPad = lastPadState[playerIndex];

            if (GamePad.GetState(index).IsConnected)
            {
                #region Movement

                target = new Vector2(pad.ThumbSticks.Left.X, -pad.ThumbSticks.Left.Y);

                #endregion Movement

                #region Gun Swapping

                if (!inv.BuildMode)
                {
                    if (pad.IsButtonDown(Buttons.X) && lastPad.IsButtonUp(Buttons.X))
                    {
                        if (inv.CurrentGun == inv.BLUE)
                            inv.ChangeGun(e, GunType.WHITE);
                        else if (inv.BLUE.Ammunition > 0)
                            inv.ChangeGun(e, GunType.BLUE);
                    }

                    if (pad.IsButtonDown(Buttons.A) && lastPad.IsButtonUp(Buttons.A))
                    {
                        if (inv.CurrentGun == inv.GREEN)
                            inv.ChangeGun(e, GunType.WHITE);
                        else if (inv.GREEN.Ammunition > 0)
                            inv.ChangeGun(e, GunType.GREEN);
                    }

                    if (pad.IsButtonDown(Buttons.B) && lastPad.IsButtonUp(Buttons.B))
                    {
                        if (inv.CurrentGun == inv.RED)
                            inv.ChangeGun(e, GunType.WHITE);
                        else if (inv.RED.Ammunition > 0)
                            inv.ChangeGun(e, GunType.RED);
                    }
                }

                #endregion Gun Swapping

                #region Building

                if (pad.IsButtonDown(Buttons.Y) && lastPad.IsButtonUp(Buttons.Y))
                {
                    inv.BuildMode = !inv.BuildMode;
                }

                if (inv.BuildMode)
                {
                    if (pad.IsButtonDown(Buttons.X) && lastPad.IsButtonUp(Buttons.X))
                    {
                        if (inv.YELLOW >= BarrierPrice)
                        {
                            SpaceWorld w = world as SpaceWorld;
                            Vector2 offset = (w.Base.GetComponent<Body>().Position - b.Position);
                            offset.Normalize();
                            //offset;
                             world.CreateEntity("Barrier", ConvertUnits.ToDisplayUnits(b.Position - offset), e).Refresh();
                            inv.YELLOW -= BarrierPrice;
                        }
                    }

                    if (pad.IsButtonDown(Buttons.A) && lastPad.IsButtonUp(Buttons.A))
                    {
                        if (inv.YELLOW >= TurretPrice)
                        {
                            world.CreateEntity("Turret", ConvertUnits.ToDisplayUnits(b.Position), e).Refresh();
                            inv.YELLOW -= TurretPrice;
                        }
                    }

                    if (pad.IsButtonDown(Buttons.B) && lastPad.IsButtonUp(Buttons.B))
                    {
                        if (inv.YELLOW >= MinePrice)
                        {
                            world.CreateEntity("Mine", ConvertUnits.ToDisplayUnits(b.Position), e).Refresh();
                            inv.YELLOW -= MinePrice;
                        }
                    }



                    if (inv.YELLOW < MinePrice)
                    {
                        inv.BuildMode = false;
                    }
                }

                #endregion Building

                //Rotation
                if (b.LinearVelocity != Vector2.Zero)
                {
                    if (!(Mouse.GetState().LeftButton == ButtonState.Pressed))
                        b.RotateTo(b.LinearVelocity);
                    WasMoving[playerIndex] = true;
                }

                #region Aiming

                if (pad.ThumbSticks.Right != Vector2.Zero)
                {
                    Vector2 aiming = new Vector2(pad.ThumbSticks.Right.X, -pad.ThumbSticks.Right.Y);
                    b.RotateTo(aiming);
                }

                #endregion Aiming


            }

            #endregion Gamepad

            #region Keyboard

#if WINDOWS
            else
            {
                #region Movement

                if (keyState.IsKeyDown(Keys.D))
                { //Right
                    target += Vector2.UnitX;
                }
                else if (keyState.IsKeyDown(Keys.A))
                { //Left
                    target += -Vector2.UnitX;
                }

                if (keyState.IsKeyDown(Keys.S))
                { //Down
                    target += Vector2.UnitY;
                }
                else if (keyState.IsKeyDown(Keys.W))
                { //Up?
                    target += -Vector2.UnitY;
                }

                #endregion Movement

                #region Gun Swapping

                if (!inv.BuildMode)
                {
                    if (keyState.IsKeyDown(Keys.D1) && lastKeyState.IsKeyUp(Keys.D1))
                    {
                        if (inv.CurrentGun == inv.BLUE)
                            inv.ChangeGun(e, GunType.WHITE);
                        else if (inv.BLUE.Ammunition > 0)
                            inv.ChangeGun(e, GunType.BLUE);
                    }

                    if (keyState.IsKeyDown(Keys.D2) && lastKeyState.IsKeyUp(Keys.D2))
                    {
                        if (inv.CurrentGun == inv.GREEN)
                            inv.ChangeGun(e, GunType.WHITE);
                        else if (inv.GREEN.Ammunition > 0)
                            inv.ChangeGun(e, GunType.GREEN);
                    }

                    if (keyState.IsKeyDown(Keys.D3) && lastKeyState.IsKeyUp(Keys.D3))
                    {
                        if (inv.CurrentGun == inv.RED)
                            inv.ChangeGun(e, GunType.WHITE);
                        else if (inv.RED.Ammunition > 0)
                            inv.ChangeGun(e, GunType.RED);
                    }
                }

                #endregion Gun Swapping

                #region Building

                if (keyState.IsKeyDown(Keys.D4) && lastKeyState.IsKeyUp(Keys.D4))
                {
                    inv.BuildMode = !inv.BuildMode;
                }
                if (inv.BuildMode)
                {
                    if (keyState.IsKeyDown(Keys.D1) && lastKeyState.IsKeyUp(Keys.D1))
                    {
                        if (inv.YELLOW >= BarrierPrice)
                        {
                            world.CreateEntity("Barrier", ConvertUnits.ToDisplayUnits(b.Position), e).Refresh();
                            inv.YELLOW -= BarrierPrice;
                        }
                    }

                    if (keyState.IsKeyDown(Keys.D2) && lastKeyState.IsKeyUp(Keys.D2))
                    {
                        if (inv.YELLOW >= TurretPrice)
                        {
                            world.CreateEntity("Turret", ConvertUnits.ToDisplayUnits(b.Position), e).Refresh();
                            inv.YELLOW -= TurretPrice;
                        }
                    }

                    if (keyState.IsKeyDown(Keys.D3) && lastKeyState.IsKeyUp(Keys.D3))
                    {
                        if (inv.YELLOW >= MinePrice)
                        {
                            world.CreateEntity("Mine", ConvertUnits.ToDisplayUnits(b.Position), e).Refresh();
                            inv.YELLOW -= MinePrice;
                        }
                    }

                    if (inv.YELLOW < MinePrice)
                    {
                        inv.BuildMode = false;
                    }
                }

                #endregion Building

                #region Aiming

                Vector2 mouseLoc = new Vector2(mouseState.X, mouseState.Y);
                Vector2 mouseWorldLoc = mouseLoc - ScreenHelper.Center;
                Vector2 aiming = b.Position - ConvertUnits.ToSimUnits(mouseWorldLoc);
                b.RotateTo(-aiming);

                #endregion Aiming


                //Rotation
                if (b.LinearVelocity != Vector2.Zero)
                {
                    if (!(Mouse.GetState().LeftButton == ButtonState.Pressed))
                        b.RotateTo(b.LinearVelocity);
                    WasMoving[playerIndex] = true;
                }
            }
#endif

            #endregion Keyboard

            if (WasMoving[playerIndex]) //Stops movement
            {
                b.LinearDamping = (float)Math.Pow(_Velocity, _Velocity * 4);
                WasMoving[playerIndex] = false;
            }
            else
                b.LinearDamping = 0;

            if (target != Vector2.Zero) //If being moved by player
            {
                target.Normalize();
                WasMoving[playerIndex] = true;
                b.LinearDamping = _Velocity * 2;
            }

            //update position
            b.ApplyLinearImpulse((target) * new Vector2(_Velocity));
        }
    }
}