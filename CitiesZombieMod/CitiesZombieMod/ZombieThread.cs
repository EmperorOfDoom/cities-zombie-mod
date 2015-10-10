using ICities;

namespace CitiesZombieMod
{
    class ZombieThread : ThreadingExtensionBase
    {
        public static ZombieThread Instance { get; private set; }

        private bool threadActive = false;

        public void OnLevelLoaded(LoadMode mode)
        {
            threadActive = true;
            Logger.Log("Thread has been activated.");
        }

        public void OnLevelUnloading()
        {
            threadActive = false;
            Logger.Log("Thread has been deactivated.");
        }

        public override void OnCreated(IThreading threading)
        {
            Logger.LogClassAndMethodName(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Instance = this;
            Logger.Log("Thread has been created.");
        }

        public override void OnReleased()
        {
            Logger.Log("Thread has been released.");
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            Logger.LogClassAndMethodName(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.Log("Thread OnUpdate.");
            if (!threadActive) return;
            Logger.Log("Thread OnUpdate.");
        }
    }
}
