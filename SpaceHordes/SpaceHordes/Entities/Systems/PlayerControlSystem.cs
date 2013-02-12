using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using GameLibrary.Helpers;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Dependencies.Physics.Dynamics;
using GameLibrary.Dependencies.Physics.Dynamics.Contacts;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;
using SpaceHordes.Entities.Components;

namespace SpaceHordes.Entities.Systems
{
    public class PlayerControlSystem : GroupSystem
    {
        ComponentMapper<Body> bodyMapper;
        float _Velocity;
        bool WasMoving = false;

        KeyboardState keyState;
        KeyboardState lastKeyState;

        GamePadState[] padState = new GamePadState[4];
        GamePadState[] lastPadState = new GamePadState[4];

        MouseState mouseState;

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

            for (int i = 0; i < 4; i++)
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

            if (WasMoving) //Stops movement
            {
                b.LinearDamping = (float)Math.Pow(_Velocity, _Velocity * 4);
                WasMoving = false;
            }
            else
                b.LinearDamping = 0;

            #region Gamepad

            int playerIndex = int.Parse(e.Tag.Replace("P", "")) - 1;
            PlayerIndex index = (PlayerIndex)playerIndex;
            GamePadState pad = padState[playerIndex];
            GamePadState lastPad = lastPadState[playerIndex];

            if (GamePad.GetState(index).IsConnected)
            {
                #region Movement

                target = new Vector2(pad.ThumbSticks.Left.X, -pad.ThumbSticks.Left.Y);

                #endregion

                #region Gun Swapping

                if (!inv.BuildMode)
                {
                    if (pad.IsButtonDown(Buttons.X) && lastPad.IsButtonUp(Buttons.X))
                    {
                        if (inv.CurrentGun == inv.BLUE)
                            inv.ChangeGun(e, GunType.WHITE);
                        else
                            inv.ChangeGun(e, GunType.BLUE);
                    }

                    if (pad.IsButtonDown(Buttons.A) && lastPad.IsButtonUp(Buttons.A))
                    {
                        if (inv.CurrentGun == inv.GREEN)
                            inv.ChangeGun(e, GunType.WHITE);
                        else
                            inv.ChangeGun(e, GunType.GREEN);
                    }

                    if (pad.IsButtonDown(Buttons.B) && lastPad.IsButtonUp(Buttons.B))
                    {
                        if (inv.CurrentGun == inv.RED)
                            inv.ChangeGun(e, GunType.WHITE);
                        else
                            inv.ChangeGun(e, GunType.RED);
                    }
                }

                #endregion

                #region Building

                if (pad.IsButtonDown(Buttons.Y) && lastPad.IsButtonUp(Buttons.Y))
                {
                    inv.BuildMode = !inv.BuildMode;
                }

                #endregion

                #region Shooting

                if (pad.ThumbSticks.Right != Vector2.Zero)
                {
                    Vector2 aiming = new Vector2(pad.ThumbSticks.Right.X, -pad.ThumbSticks.Right.Y);
                    b.RotateTo(aiming);
                    g.BulletsToFire = true;
                }

                #endregion
            }

            #endregion

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

                #endregion

                #region Gun Swapping

                if (!inv.BuildMode)
                {
                    if (keyState.IsKeyDown(Keys.D1) && lastKeyState.IsKeyUp(Keys.D1))
                    {
                        if (inv.CurrentGun == inv.BLUE)
                            inv.ChangeGun(e, GunType.WHITE);
                        else
                            inv.ChangeGun(e, GunType.BLUE);
                    }

                    if (keyState.IsKeyDown(Keys.D2) && lastKeyState.IsKeyUp(Keys.D2))
                    {
                        if (inv.CurrentGun == inv.GREEN)
                            inv.ChangeGun(e, GunType.WHITE);
                        else
                            inv.ChangeGun(e, GunType.GREEN);
                    }

                    if (keyState.IsKeyDown(Keys.D3) && lastKeyState.IsKeyUp(Keys.D3))
                    {
                        if (inv.CurrentGun == inv.RED)
                            inv.ChangeGun(e, GunType.WHITE);
                        else
                            inv.ChangeGun(e, GunType.RED);
                    }
                }

                #endregion

                #region Building

                if (keyState.IsKeyDown(Keys.D4) && lastKeyState.IsKeyUp(Keys.D4))
                {
                    inv.BuildMode = !inv.BuildMode;
                }

                #endregion

                #region Shooting

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 mouseLoc = new Vector2(mouseState.X, mouseState.Y);
                    Vector2 mouseWorldLoc = mouseLoc - ScreenHelper.Center;
                    Vector2 aiming = b.Position - ConvertUnits.ToSimUnits(mouseWorldLoc);
                    b.RotateTo(-aiming);
                    g.BulletsToFire = true;
                }

                #endregion
            }
#endif

            #endregion

            if (target != Vector2.Zero) //If being moved by player
            {
                target.Normalize();
                WasMoving = true;
                b.LinearDamping = _Velocity * 2;
            }

            //Rotation
            if (b.LinearVelocity != Vector2.Zero)
                b.Rotation = (float)Math.Atan2(b.LinearVelocity.Y, b.LinearVelocity.X) + 0;// (float)Math.PI / 2f;

            //update position
            b.ApplyLinearImpulse((target) * new Vector2(_Velocity));
        }
    }
}