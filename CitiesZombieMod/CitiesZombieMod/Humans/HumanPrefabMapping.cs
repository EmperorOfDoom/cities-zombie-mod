using System.Collections.Generic;

namespace CitiesZombieMod
{
    public class HumanPrefabMapping
    {
        private MonitorHelper _helper;
        private HumanMonitorData _data;

        private PrefabMapping<uint> _mapping;

        public HumanPrefabMapping()
        {
            _helper = MonitorHelper.Instance;
            _data = HumanMonitorData.Instance;

            _mapping = new PrefabMapping<uint>();
        }

        public List<HashSet<uint>> GetMapping(CitizenInfo human)
        {
            int prefabID = human.m_prefabDataIndex;

            if (!_mapping.PrefabMapped(prefabID))
                CategorizePrefab(human);

            return _mapping.GetMapping(prefabID);
        }

        public void AddTurnedMapping(CitizenInfo human) {
            int prefabID = human.m_prefabDataIndex;
            if (!_mapping.PrefabMapped(prefabID))
                _mapping.AddMapping(prefabID, _data._TurnedHumans);
        }

        private void CategorizePrefab(CitizenInfo human)
        {
            CitizenAI ai = human.m_citizenAI;
            int prefabID = human.m_prefabDataIndex;

            /*
             * Create a blank entry. This way, even if this prefab does not belong here
             * for some bizarre reason, we will have a record of it. This eliminates
             * the chance of a prefab getting evaluated more than once, ever.
             */
            _mapping.AddEntry(prefabID);

            if (ai is HumanAI)
            {
                _mapping.AddMapping(prefabID, _data._Humans);
            }
        }
    }
}