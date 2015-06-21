using Lidgren.Network;

namespace CEngineSharp_Utilities
{
    public static class LidgrenExtensions
    {
        public static void Write(this NetBuffer nb, Vector vector)
        {
            nb.Write(vector.X);
            nb.Write(vector.Y);
        }

        public static Vector ReadVector(this NetBuffer db)
        {
            var x = db.ReadInt32();
            var y = db.ReadInt32();

            return new Vector(x, y);
        }
    }
}