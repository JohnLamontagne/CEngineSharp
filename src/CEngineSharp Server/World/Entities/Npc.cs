using CEngineSharp_World;
using CEngineSharp_World.Entities;
using System;

namespace CEngineSharp_Server.World.Entities
{
    public class Npc : BaseNpc, IEntity
    {
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
    }
}