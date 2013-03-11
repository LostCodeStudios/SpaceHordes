using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace GameLibrary.Helpers
{
    public static class SoundManager
    {
        private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        private static float volume;

        public static float Volume
        {
            get { return volume; }
            set
            {
                volume = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        public static void Play(string soundKey, float volume = 1f)
        {
            if (sounds.ContainsKey(soundKey) && Volume > 0f)
                sounds[soundKey].Play(Volume * volume, 0f, 0f);
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