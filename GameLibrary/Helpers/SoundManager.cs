using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace GameLibrary.Helpers
{
    public static class SoundManager
    {
        static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        static float volume;
        public static float Volume
        {
            get { return volume; }
            set
            {
                volume = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        public static void Play(string soundKey)
        {
            if (sounds.ContainsKey(soundKey) && Volume > 0f)
                sounds[soundKey].Play(Volume, 0f, 0f);
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
