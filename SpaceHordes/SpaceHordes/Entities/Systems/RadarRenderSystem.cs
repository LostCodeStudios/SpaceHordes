using GameLibrary.Dependencies.Entities;
using GameLibrary.Entities.Components.Physics;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceHordes.Entities.Systems
{
    class RadarRenderSystem : IntervalEntityProcessingSystem
    {
        Rectangle _RadarBounds;
        Rectangle _ScanBounds;

        public RadarRenderSystem(Rectangle radarBounds, Rectangle scanBounds)
            : base(30, typeof(Body))
        {
            _RadarBounds = radarBounds;
            _ScanBounds = scanBounds;

            _Projection = Matrix.CreateOrthographicOffCenter(-radarBounds.X, radarBounds.Width, radarBounds.Height,
                                                                  -radarBounds.Y, 0f, 1f);
            _View = Matrix.Identity;


            _PrimBatch = new PrimitiveBatch(ScreenHelper.GraphicsDevice);
        }

        public override void Process(Entity e)
        {
            
            
        }

        public override void Process()
        {
            _PrimBatch.Begin(ref _Projection, ref _View);
            DrawCircle(new Vector2(0), 20, Color.Red);
            base.Process();
            _PrimBatch.End();
        }

        #region Drawing
        PrimitiveBatch _PrimBatch;
        Matrix _Projection;
        Matrix _View;

        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            if (!_PrimBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / 32;
            double theta = 0.0;

            for (int i = 0; i < 32; i++)
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

        #endregion

    }
}
