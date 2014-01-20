using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Server.World.Content_Managers
{
    public sealed class ItemManager
    {
        private List<Item> _items;

        public Item GetItem(int itemIndex)
        {
            return _items[itemIndex];
        }

        private Item LoadItem(string filePath)
        {
            var item = new Item();

            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (var binaryReader = new BinaryReader(fs))
                {
                    item.Name = binaryReader.ReadString();
                    item.TextureNumber = binaryReader.ReadInt32();
                }
            }

            return item;
        }

        public void LoadItems()
        {
            _items = new List<Item>();

            Console.WriteLine("Loading items...");

            var dI = new DirectoryInfo(Constants.FILEPATH_ITEMS);

            foreach (var file in dI.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
            {
                _items.Add(LoadItem(file.FullName));
            }

            Console.WriteLine("Loaded {0} items!", _items.Count);
        }

    }
}