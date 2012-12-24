using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Input
{
    public interface IHandlesInput
    {
        void HandleInput(InputState input);
    }
}
