using ColossalFramework;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CitiesZombieMod
{ 
        public class ZombieProperties : MonoBehaviour
        {
        public Shader m_undergroundShader;

        private void Awake()
        {
           if (Application.isPlaying)
           {
               Singleton<LoadingManager>.instance.QueueLoadingAction(this.InitializeProperties());
           }
        }

        [DebuggerHidden]
        private IEnumerator InitializeProperties()
        {
            return (IEnumerator) new ZombieProperties();
        }

        private void OnDestroy()
       {
           if (Application.isPlaying)
           {
               Singleton<LoadingManager>.instance.m_loadingProfilerMain.BeginLoading("ZombieProperties");
               Singleton<ZombieManager>.instance.DestroyProperties(this);
               Singleton<LoadingManager>.instance.m_loadingProfilerMain.EndLoading();
           }
       }
    }

}
