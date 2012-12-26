using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Entities;
using Microsoft.Xna.Framework;


namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// A physical component is a body. Physical components are always coupled with Transform & Velocity
    /// </summary>
    public class Physical : Body, Component
    {
        public Physical(World world, Entity e, string ComponentName)
            : base(world, e)
        {
            //Create transform && velocity,
            e.AddComponent(ComponentName, new Transform());
            e.AddComponent(ComponentName, new Velocity());
            e.Refresh();
            
        }
        ~Physical()
        {
            this.World.RemoveBody(this);
            
        }

        #region Helpers
        public override string ToString()
        {
            return "[(Pos=" + this.Position
                + "),\n                (LVel=" + this.LinearVelocity
                + "),\n                (AVel=" + this.AngularVelocity
                + "),\n                (Ent=" + this.UserData + ")]";
        }
        #endregion
    }
}
