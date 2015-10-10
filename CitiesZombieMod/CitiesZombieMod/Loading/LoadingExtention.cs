using ICities;

namespace CitiesZombieMod
{
    class LoadingExtention : LoadingExtensionBase
    {
        public class LoadingExtension : LoadingExtensionBase
        {
            public override void OnLevelLoaded(LoadMode mode)
            {
                if (ThreadingExtension.Instance != null)
                {
                    ThreadingExtension.Instance.OnLevelLoaded(mode);
                }
            }

            public override void OnLevelUnloading()
            {
                if (ThreadingExtension.Instance != null)
                {
                    ThreadingExtension.Instance.OnLevelUnloading();
                }
            }
        }
    }
}
