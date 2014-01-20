using System.IO;

namespace CEngineSharp_World.Entities
{
    public class BaseNpc : IBaseEntity
    {
        public string Name { get; set; }

        public int TextureNumber { get; set; }

        public Vector Position { get; set; }

        public int Level { get; set; }

        private readonly int[] _stats;

        public BaseNpc()
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
            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (var binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(this.Name);
                    binaryWriter.Write(this.TextureNumber);
                    binaryWriter.Write(this.Position.X);
                    binaryWriter.Write(this.Level);

                    binaryWriter.Write(_stats.Length);
                    foreach (var stat in _stats)
                    {
                        binaryWriter.Write(stat);
                    }
                }
            }
        }

        public static BaseNpc LoadNpc(string filePath)
        {
            var baseNpc = new BaseNpc() { Position = new Vector() };

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    baseNpc.Name = binaryReader.ReadString();
                    baseNpc.TextureNumber = binaryReader.ReadInt32();
                    baseNpc.Position.X = binaryReader.ReadInt32();
                    baseNpc.Position.Y = binaryReader.ReadInt32();

                    var statCount = binaryReader.ReadInt32();
                    for (int i = 0; i < statCount; i++)
                        baseNpc.SetStat((Stats)i, binaryReader.ReadInt32());
                }
            }

            return baseNpc;
        }
    }
}