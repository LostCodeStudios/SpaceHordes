using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using GameLibrary.Helpers.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHordes.Entities.Components;
using GameLibrary.Entities.Components;
using GameLibrary.Entities.Components.Physics;

namespace SpaceHordes.Entities.Systems
{
    public class HUDRenderSystem : GroupSystem
    {
        private SpriteBatch _SpriteBatch;
        private ImageFont _Font;
        private Texture2D _Hud;

        #region Locations

        private static Vector2 hudDimmensions;
        //private static Vector2 radarDimmensions;
        private static Vector2 warningDimmensions;
        private static Rectangle[] hudLocations;
        //private static Rectangle radarLocation;
        //public static Rectangle RadarScreenLocation;
        private static Rectangle warningLocation;

        //private static Rectangle radarSource;
        private static Rectangle hudSource;
        private static Rectangle buildMenuSource;
        private static Rectangle selectionSource;
        private static Rectangle warningSource;

        private static Vector2[] boxOffsets;
        private static RectangleF box = new RectangleF(0, 0, 21, 21);

        #endregion Locations

        public static string SurgeWarning = "";

        public HUDRenderSystem()
            : base("Players")
        {
            SurgeWarning = "";
            ApplyScaling();
        }

        public static void ApplyScaling()
        {
            #region HUD Specifications

            hudDimmensions = new Vector2(96, 51);
            //float radarScale = 2.6f;
            //radarDimmensions = new Vector2(86, 50) * radarScale;
            warningDimmensions = new Vector2(173, 76);

            hudLocations = new Rectangle[]
            {
                new Rectangle(ScreenHelper.TitleSafeArea.X, ScreenHelper.TitleSafeArea.Y, (int)hudDimmensions.X, (int)hudDimmensions.Y),
                new Rectangle(ScreenHelper.TitleSafeArea.Right - (int)hudDimmensions.X, ScreenHelper.TitleSafeArea.Y, (int)hudDimmensions.X, (int)hudDimmensions.Y),
                new Rectangle(ScreenHelper.TitleSafeArea.X, ScreenHelper.TitleSafeArea.Bottom - (int)hudDimmensions.Y, (int)hudDimmensions.X, (int)hudDimmensions.Y),
                new Rectangle(ScreenHelper.TitleSafeArea.Right - (int)hudDimmensions.X, ScreenHelper.TitleSafeArea.Bottom - (int)hudDimmensions.Y, (int)hudDimmensions.X, (int)hudDimmensions.Y)
            };

            //float topRadarPadding = 4f * radarScale;
            //float leftRadarPadding = topRadarPadding;
            //float rightRadarPadding = topRadarPadding;
            //float bottomRadarPadding = 5f * radarScale;

            //radarLocation = new Rectangle((int)ScreenHelper.Center.X - (int)radarDimmensions.X / 2, ScreenHelper.Viewport.Height - (int)radarDimmensions.Y, (int)radarDimmensions.X, (int)radarDimmensions.Y);
            //RadarScreenLocation = new Rectangle((int)(radarLocation.Location.X + leftRadarPadding), (int)(radarLocation.Location.Y + topRadarPadding), (int)(radarLocation.Width - (rightRadarPadding + leftRadarPadding)), (int)(radarLocation.Height - (bottomRadarPadding + topRadarPadding)));
            warningLocation = new Rectangle((int)(ScreenHelper.Center.X - warningDimmensions.X / 2), (int)(ScreenHelper.Center.Y - warningDimmensions.Y * 2), (int)warningDimmensions.X, (int)warningDimmensions.Y);

            //radarSource = new Rectangle(0, 0, 86, 50);
            hudSource = new Rectangle(86, 0, 96, 51);
            buildMenuSource = new Rectangle(181, 0, 96, 51);
            selectionSource = new Rectangle(277, 0, 26, 26);
            warningSource = new Rectangle(305, 0, (int)warningDimmensions.X, (int)warningDimmensions.Y);

            boxOffsets = new Vector2[8];
            boxOffsets[0] = new Vector2(3, 3);
            boxOffsets[1] = new Vector2(26, 3);
            boxOffsets[2] = new Vector2(49, 3);
            boxOffsets[3] = new Vector2(72, 3);
            boxOffsets[4] = new Vector2(3, 26);
            boxOffsets[5] = new Vector2(26, 26);
            boxOffsets[6] = new Vector2(49, 26);
            boxOffsets[7] = new Vector2(72, 26);

            #endregion HUD Specifications
        }

        public void LoadContent(ImageFont font, Texture2D texture)
        {
            _SpriteBatch = new SpriteBatch(ScreenHelper.GraphicsDevice);
            _Font = font;
            _Hud = texture;
        }

        public override void Process()
        {
            _SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            base.Process();
            //_SpriteBatch.Draw(_Hud, radarLocation, radarSource, Color.White);

            SpaceWorld w = world as SpaceWorld;

            for (int i = 0; i < w.enemySpawnSystem.RespawnTime.Length; ++i)
            {
                int time = w.enemySpawnSystem.RespawnTime[i];
                if (time > 0)
                {
                    int amount = (int)(time / 1000) + 1;
                    amount = (int)MathHelper.Clamp(amount, 1, 3);
                    Rectangle loc = hudLocations[i];
                    RectangleF locF = new RectangleF(loc.X, loc.Y, loc.Width, loc.Height);
                    _Font.DrawString(_SpriteBatch, locF, amount.ToString());
                }
            }

            if (!string.IsNullOrEmpty(SurgeWarning))
            {
                if (SurgeWarning.Equals("Surge"))
                    _SpriteBatch.Draw(_Hud, warningLocation, warningSource, Color.White);
                else
                {
                    Vector2 warnDim = _Font.MeasureString(SurgeWarning);
                    RectangleF loc = new RectangleF(ScreenHelper.Center.X - warnDim.X / 2, ScreenHelper.Center.Y - warnDim.Y * 6, warnDim.X, warnDim.Y);
                    _Font.DrawString(_SpriteBatch, loc, SurgeWarning);
                }
            }

            _SpriteBatch.End();
        }

        public override void Process(Entity e)
        {
            Inventory i = e.GetComponent<Inventory>();
            if (i._type != InvType.Player)
                return;

            int playerIndex = int.Parse(e.Tag.Replace("P", "")) - 1;
            int playerNum = playerIndex + 1;

            Vector2 topLeft = new Vector2(hudLocations[playerIndex].X, hudLocations[playerIndex].Y);

            string label = "P" + playerNum.ToString();
            Vector2 labelSize = _Font.MeasureString(label);
            Vector2 labelLoc = topLeft + new Vector2(hudDimmensions.X / 2 - labelSize.X / 2, -labelSize.Y);

            Vector2 selectionOffset = new Vector2(-2, -3);

            _Font.DrawString(_SpriteBatch, labelLoc, label);
            
            if (!i.BuildMode)
            {
                _SpriteBatch.Draw(_Hud, hudLocations[playerIndex], hudSource, Color.White);

                int blue = i.BLUE.Ammunition;
                int green = i.GREEN.Ammunition;
                int red = i.RED.Ammunition;

                box.X = topLeft.X + boxOffsets[4].X;
                box.Y = topLeft.Y + boxOffsets[4].Y;
                _Font.DrawString(_SpriteBatch, box, blue.ToString());

                box.X = topLeft.X + boxOffsets[5].X;
                box.Y = topLeft.Y + boxOffsets[5].Y;
                _Font.DrawString(_SpriteBatch, box, green.ToString());

                box.X = topLeft.X + boxOffsets[6].X;
                box.Y = topLeft.Y + boxOffsets[6].Y;
                _Font.DrawString(_SpriteBatch, box, red.ToString());

                if (i.CurrentGun == i.BLUE)
                    _SpriteBatch.Draw(_Hud, topLeft + boxOffsets[0] + selectionOffset, selectionSource, Color.White);

                if (i.CurrentGun == i.GREEN)
                    _SpriteBatch.Draw(_Hud, topLeft + boxOffsets[1] + selectionOffset, selectionSource, Color.White);

                if (i.CurrentGun == i.RED)
                    _SpriteBatch.Draw(_Hud, topLeft + boxOffsets[2] + selectionOffset, selectionSource, Color.White);
            }
            else
            {
                int X = (int)ScreenHelper.Center.X;
                int Y = (int)ScreenHelper.Center.Y;
                float Width = e.GetComponent<Sprite>().CurrentRectangle.Width;
                float Height = e.GetComponent<Sprite>().CurrentRectangle.Height;

                Body body = e.GetComponent<Body>();

                Vector2 loc = new Vector2(
                    X + ConvertUnits.ToDisplayUnits(body.Position.X) - Width / 2,
                    Y + ConvertUnits.ToDisplayUnits(body.Position.Y) - Height / 2 - _Font.MeasureString(i.YELLOW.ToString()).Y);

                //Draw backing
                _Font.DrawString(_SpriteBatch, loc, i.YELLOW.ToString());

                _SpriteBatch.Draw(_Hud, hudLocations[playerIndex], buildMenuSource, Color.White);
                _SpriteBatch.Draw(_Hud, topLeft + boxOffsets[3] + selectionOffset, selectionSource, Color.White);

                box.X = topLeft.X + boxOffsets[4].X;
                box.Y = topLeft.Y + boxOffsets[4].Y;
                _Font.DrawString(_SpriteBatch, box, PlayerControlSystem.BarrierPrice.ToString());

                box.X = topLeft.X + boxOffsets[5].X;
                box.Y = topLeft.Y + boxOffsets[5].Y;
                _Font.DrawString(_SpriteBatch, box, PlayerControlSystem.TurretPrice.ToString());

                box.X = topLeft.X + boxOffsets[6].X;
                box.Y = topLeft.Y + boxOffsets[6].Y;
                _Font.DrawString(_SpriteBatch, box, PlayerControlSystem.MinePrice.ToString());
            }

            int yellow = (int)i.YELLOW;
            box.X = topLeft.X + boxOffsets[7].X;
            box.Y = topLeft.Y + boxOffsets[7].Y;
            _Font.DrawString(_SpriteBatch, box, yellow.ToString());
        }
    }
}