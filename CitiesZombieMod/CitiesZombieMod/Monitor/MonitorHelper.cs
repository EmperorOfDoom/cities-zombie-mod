namespace CitiesZombieMod
{
    public sealed class MonitorHelper
    {
        private MonitorHelper()
        {
            GameLoaded = false;

            _BuildingMonitorSpun = false;
            _VehicleMonitorSpun = false;
            _HumanMonitorSpun = false;
            _AnimalMonitorSpun = false;
        }

        private static readonly MonitorHelper _Instance = new MonitorHelper();
        public static MonitorHelper Instance { get { return _Instance; } }

        internal bool GameLoaded;

        private bool _BuildingMonitorSpun;
        private bool _VehicleMonitorSpun;
        private bool _HumanMonitorSpun;
        private bool _AnimalMonitorSpun;

        internal bool BuildingMonitorSpun
        {
            get { return BuildingMonitorSpinnable && _BuildingMonitorSpun; }
            set { _BuildingMonitorSpun = BuildingMonitorSpinnable ? value : false; }
        }

        internal bool VehicleMonitorSpun
        {
            get { return VehicleMonitorSpinnable && _VehicleMonitorSpun; }
            set { _VehicleMonitorSpun = VehicleMonitorSpinnable ? value : false; }
        }

        internal bool HumanMonitorSpun
        {
            get { return HumanMonitorSpinnable && _HumanMonitorSpun; }
            set { _HumanMonitorSpun = HumanMonitorSpinnable ? value : false; }
        }

        internal bool AnimalMonitorSpun
        {
            get { return AnimalMonitorSpinnable && _AnimalMonitorSpun; }
            set { _AnimalMonitorSpun = AnimalMonitorSpinnable ? value : false; }
        }

        internal bool BuildingMonitorSpinnable { get { return GameLoaded; } }
        internal bool VehicleMonitorSpinnable { get { return GameLoaded; } }
        internal bool HumanMonitorSpinnable { get { return GameLoaded; } }
        internal bool AnimalMonitorSpinnable { get { return BuildingMonitorSpun; } }

        internal HumanMonitor HumanMonitor;

        public void RequestHumanRemoval(uint id)
        {
            if (HumanMonitor != null)
                HumanMonitor.RequestRemoval(id);
        }
    }
}