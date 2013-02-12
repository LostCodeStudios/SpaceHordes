using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Helpers.Drawing;

namespace SpaceHordes
{
    public class ImageFont
    {
        Texture2D texture;
        Dictionary<char, Rectangle> letters = new Dictionary<char, Rectangle>();

        int spaceWidth = 1;
        public int SpaceWidth
        {
            get { return spaceWidth; }
            set { spaceWidth = Math.Max(value, 1); }
        }

        float charSpacing = 0;
        public float CharSpaceWidth
        {
            get { return charSpacing; }
            set { charSpacing = Math.Max(value, 0); }
        }

        public Rectangle this[char key]
        {
            get { return letters[key]; }
        }

        public void LoadContent(ContentManager content, string filename, Color boundingColor)
        {
            texture = content.Load<Texture2D>(filename);

            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);

            //Get the data in 2D array form
            Color[,] colors = new Color[texture.Width, texture.Height];

            int z = 0;

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++, z++)
                {
                    colors[x, y] = data[z];
                }
            }

            //Now get the rectangles
            int y1 = 1;
            int x1 = 1;
            int rectCount = 0;
            int lastXBound = 0;
            int nextXBound = 0;
            int height = texture.Height - 2;

            Rectangle[] rects = new Rectangle[41];
            while (rectCount < rects.Count())
            {
                while (colors[x1, 1] != boundingColor)
                {
                    x1++;
                }
                nextXBound = x1;

                Rectangle rect = new Rectangle(lastXBound + 1, y1, nextXBound - lastXBound - 1, height);
                rects[rectCount] = rect;
                rectCount++;
                lastXBound = nextXBound;
                x1++;
            }

            #region Adding chars
            int charNext = 65;
            int i = 0;
            while (charNext <= 90)
            {
                letters.Add((char)charNext++, rects[i++]);
            }

            charNext = 49;

            while (charNext <= 57)
            {
                letters.Add((char)charNext++, rects[i++]);
            }

            letters.Add('0', rects[i++]);
            letters.Add('%', rects[i++]);
            letters.Add(':', rects[i++]);
            letters.Add('!', rects[i++]);
            letters.Add('?', rects[i++]);
            letters.Add('.', rects[i]);
            #endregion
        }

        public void LoadContent(ContentManager content, string filename)
        {
            LoadContent(content, filename, new Color(0, 255, 0));
        }

        public void DrawChar(SpriteBatch spriteBatch, Vector2 position, char c)
        {
            Rectangle source = letters[c];
            spriteBatch.Draw(
                texture, 
                new Rectangle((int)position.X, (int)position.Y, source.Width, source.Height), 
                source, 
                Color.White);
        }

        public void DrawChar(SpriteBatch spriteBatch, Vector2 position, char c, float scale)
        {
            Rectangle source = letters[c];
            spriteBatch.Draw(
                texture,
                new Rectangle((int)position.X, (int)position.Y, (int)(source.Width * scale), (int)(source.Height * scale)),
                source,
                Color.White);
        }

        public void DrawString(SpriteBatch spriteBatch, Vector2 position, string s)
        {
            char[] ch = s.ToUpper().ToCharArray();

            Vector2 pos = position;

            foreach (char c in ch)
            {
                if (c != ' ')
                {
                    DrawChar(spriteBatch, pos, c);
                    pos.X += letters[c].Width + charSpacing;
                }
                else
                {
                    pos.X += spaceWidth;
                }
            }
        }

        public void DrawString(SpriteBatch spriteBatch, Vector2 position, string s, float scale)
        {
            char[] ch = s.ToUpper().ToCharArray();

            Vector2 pos = position;

            foreach (char c in ch)
            {
                if (c != ' ')
                {
                    DrawChar(spriteBatch, pos, c, scale);
                    pos.X += (int)((letters[c].Width + charSpacing) * scale);
                }
                else
                {
                    pos.X += (int)(spaceWidth * scale);
                }
            }
        }

        public bool DrawString(SpriteBatch spriteBatch, RectangleF bounds, string s)
        {
            float scale = 1f;
            Vector2 size;

            while (scale > 0)
            {
                size = MeasureString(s, scale);

                if (size.X <= bounds.Width && size.Y <= bounds.Height)
                {
                    Vector2 drawTo = new Vector2(bounds.X, bounds.Y);

                    drawTo.X += bounds.Width / 2 - size.X / 2;
                    drawTo.Y += bounds.Height / 2 - size.Y / 2;

                    DrawString(spriteBatch, drawTo, s, scale);

                    return true;
                }

                scale -= 0.1f;
            }

            return false;
        }

        public Vector2 MeasureString(string s, float scale)
        {
            Vector2 toReturn = new Vector2();

            char[] ch = s.ToUpper().ToCharArray();

            foreach (char c in ch)
            {
                if (c != ' ')
                {
                    toReturn.X += (letters[c].Width + charSpacing) * scale;
                }
                else
                {
                    toReturn.X += spaceWidth * scale;
                }
            }

            toReturn.X -= charSpacing * scale;
            toReturn.Y = letters['A'].Height * scale;

            return toReturn;
        }

        public Vector2 MeasureString(string s)
        {
            return MeasureString(s, 1f);
        }
    }
}
