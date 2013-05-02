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

        private static float _Pitch;
        public static float Pitch
        {
            get { return _Pitch; }
            set
            {
                _Pitch = MathHelper.Clamp(value, -1f, 1f);
            }
        }

        public static void Play(string soundKey)
        {
            Play(soundKey, 1);
        }

        public static void Play(string soundKey, float volume)
        {
            if (sounds.ContainsKey(soundKey) && Volume > 0f)
                sounds[soundKey].Play(Volume * volume, Pitch, 0f);
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