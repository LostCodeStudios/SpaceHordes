using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GameLibrary.Helpers
{
    public class SoundManager
    {
        #region Fields

        Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        public bool Enabled;

        #endregion

        #region Initialization

        public SoundManager()
        {
            Enabled = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a given sound effect.
        /// </summary>
        /// <param name="key">The name by which the effect will be referenced.</param>
        /// <param name="value">The SoundEffect.</param>
        public void Add(string key, SoundEffect value)
        {
            try
            {
                sounds.Add(key, value);
            }

            catch (ArgumentException)
            {
            }
        }

        public void Remove(string key)
        {
            try
            {
                sounds.Remove(key);
            }

            catch (ArgumentNullException)
            {
            }
        }

        public void Play(string key)
        {
            if (Enabled)
            {
                try
                {
                    sounds[key].Play();
                }

                catch (ArgumentNullException)
                {
                }
            }
        }

        #endregion
    }
}
