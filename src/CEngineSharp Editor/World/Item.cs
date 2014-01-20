using System.IO;

namespace CEngineSharp_Editor.World
{
    public class Item
    {
        public string Name { get; set; }

        public int TextureNumber { get; set; }

        public static Item Load(string filePath)
        {
            Item item = new Item();

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    item.Name = binaryReader.ReadString();
                    item.TextureNumber = binaryReader.ReadInt32();
                }
            }

            return item;
        }

        public void Save(string filePath)
        {
            using (FileStream fs = new FileStream(filePath + "/Items/" + this.Name + ".dat", FileMode.OpenOrCreate))
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