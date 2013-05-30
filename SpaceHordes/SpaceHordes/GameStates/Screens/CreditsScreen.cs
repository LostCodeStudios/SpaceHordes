﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameStates;
using GameLib.GameStates.Screens;

namespace SpaceHordes.GameStates.Screens
{
    public class CreditsScreen : TextScreen
    {
        

        public CreditsScreen()
            : base(
            "Credits", 
                new string[]
        {
            "Code",
            "Nathaniel Nelson",
            "William Guss",
            "",
            "Art",
            "Iron Plague art by Daniel Cook",
            "Tyrian art by Daniel Cook",
            "",
            "Music",
            "Space Fighter Loop Kevin MacLeod (incompetech.com)",
            "Cephalopod Kevin MacLeod (incompetech.com)",
            "In a Heartbeat Kevin MacLeod (incompetech.com)",
            "Additional music by DST"
        })
        {
        }
    }
}
