using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Library.Model
{
    /// <summary>
    /// Manager Framework headclass (Manager paradigm is really stupid but okay, I'm doing this)
    /// </summary>
    public abstract class Manager<TKey, TValue> :  Dictionary<TKey, TValue>
    {
        #region Functioning Loop
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch sprB);
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Adds another subscriber to the manager.
        /// </summary>
        /// <param name="key">The key at which the new subscriber will be stored.</param>
        /// <param name="toAdd">The value of the entry which will be added.</param>
        public virtual void Add(TKey key, TValue toAdd)
        {
            base.Add(key, toAdd);
        }

        /// <summary>
        /// Removes a subscriber from the manager.
        /// </summary>
        /// <param name="key">The key at which the subscriber is stored.</param>
        public virtual void Remove(TKey key)
        {
            base.Remove(key);
        }

        /// <summary>
        /// Attempts to get a subscriber based on a key.
        /// </summary>
        /// <param name="k">The key at which the subscriber is supposedly stored.</param>
        /// <param name="tryTo">The out variable in which the subscriber may be stored.</param>
        /// <returns></returns>
        public virtual bool TryGetValue(TKey key, out TValue tryTo)
        {
            return base.TryGetValue(key, out tryTo);
        }

        /// <summary>
        /// Sets a key inside the mnanager
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        public virtual void Set(TKey key, TValue newValue)
        {
            if (!base.ContainsKey(key))
                base.Add(key, newValue);
            else
            {
                base.Remove(key);
                base.Add(key, newValue);
            }
        }
        #endregion

        #region Helpers

        #endregion
    }
}
