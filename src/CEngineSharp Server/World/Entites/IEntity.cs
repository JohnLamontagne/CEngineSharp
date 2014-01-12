using CEngineSharp_Server.Utilities;
using System;

namespace CEngineSharp_Server.World.Entities
{
    public interface IEntity
    {
        string Name
        {
            get;
            set;
        }

        int Level
        {
            get;
            set;
        }

        int TextureNumber
        {
            get;
            set;
        }

        Vector2i Position
        {
            get;
            set;
        }

        int GetVital(Vitals vital);

        void SetVital(Vitals vital, int value);

        void Attack(IEntity attacker);

        void Interact(IEntity interactor);

        void Die(IEntity murderer);

        void MoveTo(Vector2i vector, byte direction);

        int GetDamage();
    }
}