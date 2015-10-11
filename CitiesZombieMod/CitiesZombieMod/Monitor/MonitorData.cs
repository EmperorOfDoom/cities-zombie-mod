using System.Collections.Generic;
using System.Linq;

namespace CitiesZombieMod
{
    public sealed class MonitorData
    {
        private MonitorData()
        {
            // Humans
            _Humans = new HashSet<uint>();
            _HumansUpdated = new HashSet<uint>();
            _HumansRemoved = new HashSet<uint>();

            _Residents = new HashSet<uint>();
            _ServicePersons = new HashSet<uint>();
            _Tourists = new HashSet<uint>();
            _HumanOther = new HashSet<uint>();
        }

        private static readonly MonitorData _Instance = new MonitorData();
        public static MonitorData Instance { get { return _Instance; } }

        // Humans
        internal HashSet<uint> _Humans;
        internal HashSet<uint> _HumansUpdated;
        internal HashSet<uint> _HumansRemoved;

        internal HashSet<uint> _Residents;
        internal HashSet<uint> _ServicePersons;
        internal HashSet<uint> _Tourists;
        internal HashSet<uint> _HumanOther;

        public uint[] Humans { get { return _Humans.ToArray<uint>(); } }
        public uint[] HumansUpdated { get { return _HumansUpdated.ToArray<uint>(); } }
        public uint[] HumansRemoved { get { return _HumansRemoved.ToArray<uint>(); } }

        public uint[] Residents { get { return _Residents.ToArray<uint>(); } }
        public uint[] ServicePersons { get { return _ServicePersons.ToArray<uint>(); } }
        public uint[] Tourists { get { return _Tourists.ToArray<uint>(); } }
        public uint[] HumanOther { get { return _HumanOther.ToArray<uint>(); } }

        public bool IsHuman(uint id) { return _Humans.Contains(id); }
        public bool IsHumanUpdated(uint id) { return _HumansUpdated.Contains(id); }
        public bool IsHumanRemoved(uint id) { return _HumansRemoved.Contains(id); }

        public bool IsResident(uint id) { return _Residents.Contains(id); }
        public bool IsServicePerson(uint id) { return _ServicePersons.Contains(id); }
        public bool IsTourist(uint id) { return _Tourists.Contains(id); }
        public bool IsHumanOther(uint id) { return _HumanOther.Contains(id); }
    }
}