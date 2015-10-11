using ICities;

namespace CitiesZombieMod
{
    public class ThreadingExtension : ThreadingExtensionBase
    {
        public static ThreadingExtension Instance { get; private set; }
        bool loadingLevel = false;

        public void OnLevelUnloading()
        {
            loadingLevel = true;
        }

        public void OnLevelLoaded(LoadMode mode)
        {
            loadingLevel = false;
        }

        public override void OnCreated(IThreading threading)
        {
            Instance = this;
        }

        public override void OnReleased()
        {
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (loadingLevel) return;
        }
    }
}
