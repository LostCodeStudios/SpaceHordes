﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Components
{
    public class Crystal : Component
    {
        public Color Color;

        public int Amount;

        public string AmmoGroup;

        public Crystal(Color color, int amount, string ammoGroup)
        {
            Color = color;
            Amount = amount;
            AmmoGroup = ammoGroup;
        }
    }
}