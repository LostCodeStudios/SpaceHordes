using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game_Library.Input;

namespace Game_Library.Model.Entities
{
    /// <summary>
    /// An entity that responds to user input.
    /// </summary>
    interface IInput
    {
        void HandleInput(InputState input);
    }
}
