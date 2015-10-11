﻿using ICities;

namespace CitiesZombieMod
{
    class LoadingExtention : LoadingExtensionBase
    {
        public class LoadingExtension : LoadingExtensionBase
        {
            MonitorHelper _helper;

            public override void OnCreated(ILoading loading)
            {
                _helper = MonitorHelper.Instance;

                _helper.GameLoaded = loading.loadingComplete;
            }

            public override void OnLevelLoaded(LoadMode mode)
            {
                if (mode != LoadMode.NewGame && mode != LoadMode.LoadGame) return;

                if (ThreadingExtension.Instance != null)
                {
                    ThreadingExtension.Instance.OnLevelLoaded(mode);
                }

                if (_helper != null)
                {
                    _helper.GameLoaded = true;
                }
            }

            public override void OnLevelUnloading()
            {
                if (ThreadingExtension.Instance != null)
                {
                    ThreadingExtension.Instance.OnLevelUnloading();
                }

                if (_helper != null)
                {
                    _helper.GameLoaded = false;
                }
            }
        }
    }
}