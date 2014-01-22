namespace CEngineSharp_Utilities
{
    public sealed class Vector
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Vector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector(Vector copy)
        {
            this.X = copy.X;
            this.Y = copy.Y;
        }

        public Vector()
        {
        }

    }
}