using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Components
{
    public class Origin : Component
    {
        public Entity Parent;

        public Origin(Entity p)
        {
            Parent = p;
        }
    }
}
