using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Helpers
{
    /// <summary>
    /// Contains a Texture2D and a dictionary of source rectangle arrays. Each animation sequence can be accessed using a string key.
    /// </summary>
    public class SpriteSheet
    {
        #region Fields

        Texture2D texture;

        Dictionary<string, Rectangle[]> animations = new Dictionary<string, Rectangle[]>();

        #endregion

        #region Properties

        /// <summary>
        /// The spritesheet's texture.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// The animations dictionary.
        /// </summary>
        public Dictionary<string, Rectangle[]> Animations
        {
            get { return animations; }
            set { animations = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Makes a new Spritesheet, loading in the texture.
        /// </summary>
        public SpriteSheet(ContentManager content, string filename)
        {
            texture = content.Load<Texture2D>(filename);
        }

        #endregion
    }
}
