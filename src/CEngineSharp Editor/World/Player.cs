using System;
using System.IO;

namespace CEngineSharp_Editor.World
{
    public class Player
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public int TextureNumber { get; set; }

        public int Level { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int MapNum { get; private set; }


        public void Save(string directoryPath)
        {
            using (FileStream fs = new FileStream(directoryPath + this.Name + ".dat", FileMode.Open))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fs))
                {
                    binaryWriter.Write(this.Name);
                    binaryWriter.Write(this.Password);
                    binaryWriter.Write(this.Level);
                    binaryWriter.Write(this.TextureNumber);
                    binaryWriter.Write(this.HP);
                    binaryWriter.Write(this.MP);
                    binaryWriter.Write(this.MapNum);
                    binaryWriter.Write(this.X);
                    binaryWriter.Write(this.Y);
                }
            }
        }

        public static Player Load(string filePath)
        {
            Player player = new Player();

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    player.Name = binaryReader.ReadString();
                    player.Password = binaryReader.ReadString();
                    player.Level = binaryReader.ReadInt32();
                    player.TextureNumber = binaryReader.ReadInt32();
                    player.HP = binaryReader.ReadInt32();
                    player.MP = binaryReader.ReadInt32();
                    player.MapNum = binaryReader.ReadInt32();
                    player.X = binaryReader.ReadInt32();
                    player.Y = binaryReader.ReadInt32();
                }
            }

            return player;
        }
    }
}