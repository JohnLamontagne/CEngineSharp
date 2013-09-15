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
            return _vitals[(int)vital];
        }

        public virtual void SetVital(Vitals vital, ushort value)
        {
            _vitals[(int)vital] = value;
        }

        public abstract void Kill(Entity killer);
    }
}