using ICities;

namespace CitiesZombieMod
{
    class ModLoader : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            Logger.LogClassAndMethodName(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.Log("Level Loaded.");
           
            if (ZombieThread.Instance != null)
            {
                Logger.Log("Zombie thread started.");
                ZombieThread.Instance.OnLevelLoaded(mode);
            }
         
        }

        public override void OnLevelUnloading()
        {
           if (ZombieThread.Instance != null)
           {
                ZombieThread.Instance.OnLevelUnloading();
           }
        }
    }
}
