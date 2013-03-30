using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceHordes.Entities.Components;
using System;

namespace SpaceHordes.Entities.Systems
{
    internal class RadarRenderSystem : EntityProcessingSystem
    {
        private Rectangle _RadarBounds;
        private Vector2 _RadarCenter;
        private Rectangle _ScanBounds;

        public RadarRenderSystem(Rectangle radarBounds, Rectangle scanBounds)
            : base(typeof(Body))
        {
            _RadarBounds = radarBounds;
            _RadarCenter = new Vector2(radarBounds.X + radarBounds.Width / 2f, radarBounds.Y + radarBounds.Height / 2f);
            _ScanBounds = scanBounds;

            _Projection = Matrix.CreateOrthographicOffCenter(0, ScreenHelper.GraphicsDevice.Viewport.Width, ScreenHelper.GraphicsDevice.Viewport.Height,
                0f, 0f, 1);
            _View = Matrix.Identity;

            _PrimBatch = new PrimitiveBatch(ScreenHelper.GraphicsDevice);
            _SpriteBatch = new SpriteBatch(ScreenHelper.GraphicsDevice);
        }

        public override void Process(Entity e)
        {
            Body b = e.GetComponent<Body>(); //Sim => Display => %[Scan] => *<width,height> + <x,y>
            if (b != null)
            {
                Vector2 position = ConvertUnits.ToDisplayUnits(b.Position) - ScreenHelper.Center - ScreenHelper.Center;
                if (_ScanBounds.Contains((int)position.X, (int)position.Y))
                {
                    position -= new Vector2(_ScanBounds.X, _ScanBounds.Y);
                    position /= new Vector2(_ScanBounds.Width, _ScanBounds.Height);
                    position *= new Vector2(_RadarBounds.Width, _RadarBounds.Height);
                    position += new Vector2(_RadarBounds.X, _RadarBounds.Y);

                    //DRAW
                    Color drawColor = Color.White;
                    if (e.HasComponent<Crystal>())
                        drawColor = e.GetComponent<Crystal>().Color;
                    DrawSolidCircle(position, 0.5f * b.Mass, Vector2.Zero, drawColor);

                    //PLAYER ID
                    if (e.Group != null && e.Group == "Players")
                        DrawString(e.Tag, position, Color.Tan);

//#if DEBUG
//                    DrawString(position.ToString(), position, Color.Red);
//#endif
                }
            }
        }

        public override void Process()
        {
            _PrimBatch.Begin(ref _Projection, ref _View);
            _SpriteBatch.Begin();
            base.Process();
            _PrimBatch.End();
            _SpriteBatch.End();
        }

        public void LoadContent(ContentManager Content)
        {
            _RadarFont = Content.Load<SpriteFont>("Fonts/debugfont");
        }

        #region Drawing

        private SpriteBatch _SpriteBatch;
        private SpriteFont _RadarFont;

        private PrimitiveBatch _PrimBatch;
        private Matrix _Projection;
        private Matrix _View;

        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            if (!_PrimBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / 32;
            double theta = 0.0;

            for (int i = 0; i < 32; ++i)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center +
                             radius *
                             new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                _PrimBatch.AddVertex(v1, color, PrimitiveType.LineList);
                _PrimBatch.AddVertex(v2, color, PrimitiveType.LineList);

                theta += increment;
            }
        }

        public void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
        {
            if (!_PrimBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / 32;
            double theta = 0.0;

            Color colorFill = color * 0.5f;

            Vector2 v0 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            theta += increment;

            for (int i = 1; i < 32 - 1; ++i)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center +
                             radius *
                             new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                _PrimBatch.AddVertex(v0, colorFill, PrimitiveType.TriangleList);
                _PrimBatch.AddVertex(v1, colorFill, PrimitiveType.TriangleList);
                _PrimBatch.AddVertex(v2, colorFill, PrimitiveType.TriangleList);

                theta += increment;
            }
            DrawCircle(center, radius, color);

            DrawSegment(center, center + axis * radius, color);
        }

        public void DrawSegment(Vector2 start, Vector2 end, Color color)
        {
            if (!_PrimBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            _PrimBatch.AddVertex(start, color, PrimitiveType.LineList);
            _PrimBatch.AddVertex(end, color, PrimitiveType.LineList);
        }

        public void DrawString(string text, Vector2 position, Color color)
        {
            _SpriteBatch.DrawString(_RadarFont, text, position, color, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
        }

        #endregion Drawing
    }
}