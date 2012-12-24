using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Entities;
using GameLibrary.Helpers;

namespace SpaceHordes.Entities.Systems
{
    public class DebugRenderSystem  : IntervalEntitySystem
    {
        DebugViewXNA _debugView;
        Camera _Camera;

        public DebugRenderSystem(Camera camera) : base(33)
        {
            this._Camera = camera;
        }
        public override void Initialize()
        {
            _debugView = new DebugViewXNA(world);
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            _debugView.LoadContent(device, content);
        }

        public override void Process()
        {
            Matrix projection = _Camera.SimProjection;
            Matrix view = _Camera.SimView;
            _debugView.RenderDebugData(ref projection, ref view);
        }
    }
}
