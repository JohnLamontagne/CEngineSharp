using SharpNetty;

namespace CEngineSharp_Utilities
{
    public static class SharpNettyExtensions
    {
        public static void WriteVector(this DataBuffer db, Vector vector)
        {
            db.WriteInteger(vector.X);
            db.WriteInteger(vector.Y);
        }

        public static Vector ReadVector(this DataBuffer db)
        {
            var x = db.ReadInteger();
            var y = db.ReadInteger();

            return new Vector(x, y);
        }
    }
}