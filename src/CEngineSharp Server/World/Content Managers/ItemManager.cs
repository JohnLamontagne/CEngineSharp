using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Server.World.Content_Managers
{
    public static class ItemManager
    {
        private static List<Item> items;

        public static Item GetItem(int itemIndex)
        {
            return items[itemIndex];
        }

        public static void LoadItems()
        {
            ItemManager.items = new List<Item>();

            Console.WriteLine("Loading items...");

            DirectoryInfo dI = new DirectoryInfo(Constants.FILEPATH_ITEMS);

            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
            {
                items.Add(ItemManager.LoadItem(file.FullName));
            }

            Console.WriteLine("Loaded {0} items!", ItemManager.items.Count);
        }

        public static Item LoadItem(string filePath)
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
    }
}