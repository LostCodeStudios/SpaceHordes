namespace GameLibrary.Helpers.Drawing
{
    public struct RectangleF
    {
        public float X;
        public float Y;

        public float Width;
        public float Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}