using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHordes.Entities.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Entities;
using GameLibrary.Physics.Common;
using GameLibrary.Physics.Collision.Shapes;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Physics.Dynamics.Joints;

namespace SpaceHordes.Entities.Templates
{
    public class PlayerTemplate : IEntityTemplate
    {
        private EntityWorld world;
        public PlayerTemplate(EntityWorld world)
        {
            this.world = world;
        }

        public Entity BuildEntity(Entity e, params object[] args)
        {
            e.Group = "PLAYERS";
            e.Tag = "Player";

            return e;
        }


    }
}
