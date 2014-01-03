using System;
using System.IO;

namespace CEngineSharp_Editor.World
{
    public class Npc
    {
        public string Name { get; set; }

        public int TextureNumber { get; set; }

        public static Npc Load(string filePath)
        {
            Npc npc = new Npc();

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    npc.Name = binaryReader.ReadString();
                    npc.TextureNumber = binaryReader.ReadInt32();
                }
            }

            return npc;
        }

        public void Save(string filePath)
        {
            using (FileStream fs = new FileStream(filePath + "/Npcs/" + this.Name + ".dat", FileMode.OpenOrCreate))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fs))
                {
                    binaryWriter.Write(this.Name);
                    binaryWriter.Write(this.TextureNumber);
                }
            }
        }
    }
}