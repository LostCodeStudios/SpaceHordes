using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Entities;


namespace GameLibrary.Entities.Components
{
    /// <summary>
    /// A physical component is a body ;)
    /// </summary>
    public class Physical : Body, Component
    {

        public Physical(World world, Entity e)
            : base(world, e)
        {
        }

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Events
        #endregion

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
