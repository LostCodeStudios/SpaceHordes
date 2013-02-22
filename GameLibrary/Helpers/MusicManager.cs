using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace GameLibrary.Helpers
{
    public static class MusicManager
    {
        static Dictionary<string, Song> songs = new Dictionary<string, Song>();

        public static float Volume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = MathHelper.Clamp(value, 0f, 1f); }
        }

        public static void AddSong(string key, Song value)
        {
            songs.Add(key, value);
        }

        public static void RemoveSong(string key)
        {
            songs.Remove(key);
        }

        public static void PlaySong(string key)
        {
            if (songs.ContainsKey(key))
                MediaPlayer.Play(songs[key]);
        }

        public static void Pause()
        {
            MediaPlayer.Pause();
        }

        public static void Resume()
        {
            MediaPlayer.Resume();
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
