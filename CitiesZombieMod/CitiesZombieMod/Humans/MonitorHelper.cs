namespace CitiesZombieMod
{
    public sealed class MonitorHelper
    {
        private MonitorHelper()
        {
            GameLoaded = false;
            _HumanMonitorSpun = false;
        }

        private static readonly MonitorHelper _Instance = new MonitorHelper();
        public static MonitorHelper Instance { get { return _Instance; } }

        internal bool GameLoaded;

        private bool _HumanMonitorSpun;

        internal bool HumanMonitorSpun
        {
            get { return HumanMonitorSpinnable && _HumanMonitorSpun; }
            set { _HumanMonitorSpun = HumanMonitorSpinnable ? value : false; }
        }

        internal bool HumanMonitorSpinnable { get { return GameLoaded; } }

        internal HumanMonitor HumanMonitor;

        public void RequestHumanRemoval(uint id)
        {
            if (HumanMonitor != null)
                HumanMonitor.RequestRemoval(id);
        }
    }
}