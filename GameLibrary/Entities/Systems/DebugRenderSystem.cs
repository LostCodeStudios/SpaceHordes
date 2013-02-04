using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Dependencies.Entities;
using GameLibrary.Helpers;
using GameLibrary.Helpers.Debug;

namespace GameLibrary.Entities.Systems
{
    public class DebugRenderSystem  : IntervalEntitySystem
    {
        DebugView _debugView;
        Camera _Camera;
        public DebugRenderSystem(Camera camera) : base(33)
        {
            this._Camera = camera;
        }
        public override void Initialize()
        {
            _debugView = new DebugView(world, _Camera);
        }

        public void LoadContent(GraphicsDevice device, ContentManager content, params KeyValuePair<string, object>[] userData)
        {

                _debugView.LoadContent(device, content, userData);

        }

        public override void Process()
        {
            Matrix projection = _Camera.SimProjection;
            Matrix view = _Camera.SimView;
            _debugView.RenderDebugData(ref projection, ref view);
        }
    }
}
