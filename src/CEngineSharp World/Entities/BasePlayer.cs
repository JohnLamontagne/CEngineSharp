using System.IO;

namespace CEngineSharp_World.Entities
{
    public class BasePlayer : IBaseEntity
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public Vector Position { get; set; }

        public int Level { get; set; }

        public int TextureNumber { get; set; }

        public int MapNum { get; protected set; }

        private readonly int[] _stats;

        public BasePlayer()
        {
            _stats = new int[(int)Stats.STAT_COUNT];
            this.Position = new Vector();
        }

        public int GetStat(Stats stat)
        {
            return _stats[(int)stat];
        }

        public void SetStat(Stats stat, int value)
        {
            _stats[(int)stat] = value;
        }

        public void Save(string filePath)
        {
            using (var fileStream = new FileStream(filePath + this.Name + ".dat", FileMode.OpenOrCreate))
            {
                using (var binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(this.Name);
                    binaryWriter.Write(this.Password);
                    binaryWriter.Write(this.Position.X);
                    binaryWriter.Write(this.Position.Y);

                    binaryWriter.Write(_stats.Length);
                    foreach (var stat in _stats)
                    {
                        binaryWriter.Write(stat);
                    }
                }
            }
        }

        public static BasePlayer LoadPlayer(string filePath)
        {
            var basePlayer = new BasePlayer { Position = new Vector() };

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    basePlayer.Name = binaryReader.ReadString();
                    basePlayer.Password = binaryReader.ReadString();
                    basePlayer.TextureNumber = binaryReader.ReadInt32();
                    basePlayer.Position.X = binaryReader.ReadInt32();
                    basePlayer.Position.Y = binaryReader.ReadInt32();

                    int statCount = binaryReader.ReadInt32();
                    for (int i = 0; i < statCount; i++)
                        basePlayer.SetStat((Stats)i, binaryReader.ReadInt32());
                }
            }

            return basePlayer;
        }
    }
}