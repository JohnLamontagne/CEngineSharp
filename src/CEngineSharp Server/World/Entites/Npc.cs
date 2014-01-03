using CEngineSharp_Server.Utilities;
using CEngineSharp_Server.World.Entities;
using System;

namespace CEngineSharp_Server.World.Entities
{
    public class Npc : IEntity
    {
        public string Name
        {
            get;
            set;
        }

        public ushort Level
        {
            get;
            set;
        }

        public int TextureNumber
        {
            get;
            set;
        }


        public ushort GetVital(Vitals vital)
        {
            throw new NotImplementedException();
        }

        public void SetVital(Vitals vital, ushort value)
        {
            throw new NotImplementedException();
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

        public void MoveTo(Vector2i vector, byte direction)
        {
            throw new NotImplementedException();
        }

        public int GetDamage()
        {
            throw new NotImplementedException();
        }
    }
}