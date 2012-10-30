using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Library.Model.Entities
{
    public interface IPhysical
    {
        /// <summary>
        /// Handles collision between two physical objects.
        /// </summary>
        /// <param name="collidingWith"></param>
        void HandleCollision(IPhysical collidingWith); 

    }
}
