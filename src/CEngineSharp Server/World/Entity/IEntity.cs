﻿using CEngineSharp_Server.Utilities;
using System;

namespace SharedGameData.World.Entities
{
    public interface IEntity
    {
        string Name
        {
            get;
            set;
        }

        ushort Level
        {
            get;
            set;
        }

        ushort GetVital(Vitals vital);

        void SetVital(Vitals vital, ushort value);

        void Attack(IEntity attacker);

        void Interact(IEntity interactor);

        void Die(IEntity murderer);

        void MoveTo(Vector2i vector, byte direction);

        int GetDamage();
    }
}