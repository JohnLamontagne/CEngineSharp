namespace CEngineSharp_Server.Utilities
{
    public class Rect
    {
        private readonly int _left;
        private readonly int _top;
        private readonly int _height;
        private readonly int _width;

        public int Left
        {
            get { return _left; }
        }

        public int Top
        {
            get { return _top; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int Width
        {
            get { return _width; }
        }

        public Rect(int left, int top, int height, int width)
        {
            _left = left;
            _top = top;
            _height = height;
            _width = width;
        }
    }
}