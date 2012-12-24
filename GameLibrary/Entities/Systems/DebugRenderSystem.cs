using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Entities;
using GameLibrary.Helpers;

namespace GameLibrary.Entities.Systems
{
    public class DebugRenderSystem  : IntervalEntitySystem
    {
        DebugViewXNA _debugView;
        Camera _Camera;
        bool contentLoaded = false;
        public DebugRenderSystem(Camera camera) : base(33)
        {
            this._Camera = camera;
        }
        public override void Initialize()
        {
            _debugView = new DebugViewXNA(world);
        }

        public void LoadContent(GraphicsDevice device, ContentManager content, params KeyValuePair<string, object>[] userData)
        {
            if (!contentLoaded)
            {
                _debugView.LoadContent(device, content, userData);
                contentLoaded = true;
            }
        }

        public override void Process()
        {
            Matrix projection = _Camera.SimProjection;
            Matrix view = _Camera.SimView;
            _debugView.RenderDebugData(ref projection, ref view);
        }
    }
}
