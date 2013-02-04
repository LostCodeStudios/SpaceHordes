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
    /// <summary>
    /// Handles all debug tools.
    /// </summary>
    public class DebugSystem : IntervalEntitySystem, IDisposable
    {
        public DebugSystem(World world) : base(33)
        {
            this._Camera = world.Camera;
            this.Console = new DebugConsole(world);
        }

        #region Initialization
        /// <summary>
        /// Initializes the system
        /// </summary>
        public override void Initialize()
        {
            View = new DebugView(world, _Camera);
            Console.Start();
        }

        /// <summary>
        /// Loads the systems content.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="content"></param>
        /// <param name="userData"></param>
        public void LoadContent(GraphicsDevice device, ContentManager content, params KeyValuePair<string, object>[] userData)
        {
            View.LoadContent(device, content, userData);
        }

        public override void Dispose()
        {
            Console.Stop();
            base.Dispose();
        }
        #endregion

        #region Functioning Loop
        public override void Process()
        {
            Matrix projection = _Camera.SimProjection;
            Matrix view = _Camera.SimView;
            View.RenderDebugData(ref projection, ref view);
        }

        #endregion

        #region Fields
        /// <summary>
        /// The DebugConsole which the DebugConsoleSystem is writing too
        /// </summary>
        public DebugConsole Console;

        public DebugView View;
        Camera _Camera;
        #endregion
    }
}
