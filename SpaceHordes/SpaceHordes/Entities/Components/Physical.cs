using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Physics.Dynamics;
using GameLibrary.Entities;


namespace SpaceHordes.Entities.Components
{
    /// <summary>
    /// A physical component is a body ;)
    /// </summary>
    public class Physical : Body, Component
    {

        public Physical(EntityWorld world, Entity e)
            : base(world, e)
        {
        }

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Events
        #endregion
    }
}
