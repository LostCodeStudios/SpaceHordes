using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace GameLibrary.Helpers
{
    public static class SoundManager
    {
        static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        public static bool Enabled = true;

        public static void Play(string soundKey)
        {
            if (Enabled)
                sounds[soundKey].Play();
        }

        public static void Add(string soundKey, SoundEffect sound)
        {
            sounds.Add(soundKey, sound);
        }

        public static void Remove(string soundKey)
        {
            sounds.Remove(soundKey);
        }
    }
}
