using CEngineSharp_Server.Utilities;
using System;
using System.IO;

namespace CEngineSharp_Server.World
{
    public class NPC : Entity
    {
        public override void Attack(Entity attacker)
        {
            if (this.IsDead())
            {
                // Execute the ondeath script.

                // Distrubute the drops.

                // Cleanup
            }
        }

        public override void Interact(Entity interactor)
        {
            // todo... execute interaction script.
        }

        public override void Save()
        {
            try
            {
                using (var fileStream = new FileStream(Constants.FilePath_Npcs + this.Name + ".dat", FileMode.OpenOrCreate))
                {
                    using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                    {
                        binaryWriter.Write(this.Name);
                        binaryWriter.Write(this.Level);

                        foreach (Entity.Vitals vital in Enum.GetValues(typeof(Entity.Vitals)))
                        {
                            binaryWriter.Write(this.GetVital(vital));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // An npc didn't save correctly... we're going to have some trouble when we try to load it agian.
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.Low);

                // Move the npc to a directory containing corrupted npc files.
                // todo
            }
        }

        public override void Load(string fileName)
        {
            try
            {
                using (var fileStream = new FileStream(Constants.FilePath_Npcs + fileName + ".dat", FileMode.OpenOrCreate))
                {
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                    {
                        this.Name = binaryReader.ReadString();
                        this.Level = binaryReader.ReadUInt16();

                        foreach (Entity.Vitals vital in Enum.GetValues(typeof(Entity.Vitals)))
                        {
                            this.SetVital(vital, binaryReader.ReadUInt16());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // An npc didn't load correctly... we're fucked (for now).
                ErrorHandler.HandleException(ex, ErrorHandler.ErrorLevels.High);
            }
        }
    }
}