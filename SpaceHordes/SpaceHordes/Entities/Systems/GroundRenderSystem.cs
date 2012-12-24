using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Entities;
using Microsoft.Xna.Framework.Input;
using SpaceHordes.Entities.Components;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Systems
{
    class GroundRenderSystem : TagSystem
    {
        ComponentMapper<Physical> physicalMapper;
        Camera camera;
        GraphicsDevice graphicsDevice;
        LineBatch LineBatch;

        public GroundRenderSystem(Camera camera, GraphicsDevice graphics)
            : base("Ground")
        {
            this.camera = camera;
            this.graphicsDevice = graphics;
            this.LineBatch = new LineBatch(graphicsDevice);
        }

        public override void Initialize()
        {
            physicalMapper = new ComponentMapper<Physical>(world);
        }

        public override void Process(Entity e)
        {
            Physical ground = physicalMapper.Get(e, "Ground");

            this.LineBatch.Begin(camera.SimProjection, camera.SimView);
            // draw ground
            for (int i = 0; i < ground.FixtureList.Count; ++i)
            {
                this.LineBatch.DrawLineShape(ground.FixtureList[i].Shape, Color.Black);
            }
            this.LineBatch.End();

        }
    }
}
