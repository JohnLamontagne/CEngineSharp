using CEngineSharp_World;

namespace CEngineSharp_Server.World.Entities
{
    public interface IEntity
    {
        void Attack(IEntity attacker);

        void Interact(IEntity interactor);

        void Die(IEntity murderer);

        void MoveTo(Vector vector, byte direction);

        int GetDamage();
    }
}