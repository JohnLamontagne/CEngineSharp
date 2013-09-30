using System;

namespace CEngineSharp_Server.World
{
    public abstract class Entity
    {
        public enum Vitals
        {
            HitPoints,
            ManaPoints,
            Energy,
            Count
        }

        private string _name;
        private ushort _level;
        private ushort[] _vitals;

        public Entity()
        {
            _vitals = new ushort[(int)Vitals.Count];
        }

        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        public ushort Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public virtual ushort GetVital(Vitals vital)
        {
            if (vital != Vitals.Count)
                return _vitals[(int)vital];
            else
                return 0;
        }

        public virtual void SetVital(Vitals vital, ushort value)
        {
            if (vital != Vitals.Count)
                _vitals[(int)vital] = value;
        }

        public abstract void Attack(Entity attacker);

        public abstract void Interact(Entity interactor);

        public abstract void Save();

        public abstract void Load(string fileName);

        public virtual bool IsDead()
        {
            return (_vitals[(int)Vitals.HitPoints] <= 0);
        }
    }
}