using CEngineSharp_Server;
using CEngineSharp_Utilities;

namespace CEngineSharp_Server.World.Entities
{
    public interface IEntity
    {
        void Attack(IEntity attacker);

        void Interact(IEntity interactor);

        void Die(IEntity murderer);

        void MoveTo(Vector vector, byte direction);

        int GetDamage();

        string Name { get; set; }

        int TextureNumber { get; set; }

        int Level { get; set; }

        Vector Position { get; set; }

        int GetStat(Stats stat);

        void SetStat(Stats stat, int value);
    }
}