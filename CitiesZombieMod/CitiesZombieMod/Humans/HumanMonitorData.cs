using System.Collections.Generic;
using System.Linq;

namespace CitiesZombieMod
{
    public sealed class HumanMonitorData
    {
        private HumanMonitorData()
        {
            _Humans = new HashSet<uint>();
            _HumansUpdated = new HashSet<uint>();
            _HumansRemoved = new HashSet<uint>();
        }

        private static readonly HumanMonitorData _Instance = new HumanMonitorData();
        public static HumanMonitorData Instance { get { return _Instance; } }

        internal HashSet<uint> _Humans;
        internal HashSet<uint> _HumansUpdated;
        internal HashSet<uint> _HumansRemoved;

        public uint[] Humans { get { return _Humans.ToArray<uint>(); } }
        public uint[] HumansUpdated { get { return _HumansUpdated.ToArray<uint>(); } }
        public uint[] HumansRemoved { get { return _HumansRemoved.ToArray<uint>(); } }

        public bool IsHuman(uint id) { return _Humans.Contains(id); }
        public bool IsHumanUpdated(uint id) { return _HumansUpdated.Contains(id); }
        public bool IsHumanRemoved(uint id) { return _HumansRemoved.Contains(id); }
    }
}