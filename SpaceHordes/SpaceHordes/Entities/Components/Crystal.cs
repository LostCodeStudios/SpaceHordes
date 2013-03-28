using GameLibrary.Dependencies.Entities;
using Microsoft.Xna.Framework;

namespace SpaceHordes.Entities.Components
{
    public class Crystal : Component
    {
        public Color Color;
        public bool SurgeCrystal;

        public int Amount;

        public Crystal(Color color, int amount, bool surgeCrystal = false)
        {
            Color = color;
            Amount = amount;
            SurgeCrystal = surgeCrystal;
        }
    }
}