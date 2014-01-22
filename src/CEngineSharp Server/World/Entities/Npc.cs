using CEngineSharp_Server;
using CEngineSharp_Utilities;
using System;
using System.IO;

namespace CEngineSharp_Server.World.Entities
{
    public class Npc : IEntity
    {
        public string Name { get; set; }

        public int TextureNumber { get; set; }

        public Vector Position { get; set; }

        public int Level { get; set; }

        private readonly int[] _stats;

        public Npc()
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


        public void Attack(IEntity attacker)
        {
            throw new NotImplementedException();
        }

        public void Interact(IEntity interactor)
        {
            throw new NotImplementedException();
        }

        public void Die(IEntity murderer)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(Vector vector, byte direction)
        {
            throw new NotImplementedException();
        }

        public int GetDamage()
        {
            throw new NotImplementedException();
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
    }
}