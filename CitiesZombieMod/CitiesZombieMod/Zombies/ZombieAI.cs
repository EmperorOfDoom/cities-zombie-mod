using ColossalFramework;
using System;
using UnityEngine;

namespace CitiesZombieMod
{
    public class ZombieAI : PrefabAI
    {
        [NonSerialized]
        public ZombieInfo m_info;

        public virtual void InitializeAI()
        {
        }

        public virtual void ReleaseAI()
        {
        }

        public virtual void SimulationStep(uint citizenID, ref Zombie data)
        {
            Logger.LogClassAndMethodName(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.Log("1");
        }

        public virtual void SimulationStep(ushort instanceID, ref ZombieInstance data, Vector3 physicsLodRefPos)
        {
            Logger.LogClassAndMethodName(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.Log("2");
            if ((data.m_flags & ZombieInstance.Flags.Character) != ZombieInstance.Flags.None)
            {
                Logger.Log(data.m_flags.ToString());
                ZombieInstance.Frame lastFrameData = data.GetLastFrameData();
                int num = Mathf.Clamp((int)(lastFrameData.m_position.x / 8f + 1080f), 0, 2159);
                int num2 = Mathf.Clamp((int)(lastFrameData.m_position.z / 8f + 1080f), 0, 2159);
                bool lodPhysics = Vector3.SqrMagnitude(physicsLodRefPos - lastFrameData.m_position) >= 62500f;
                this.SimulationStep(instanceID, ref data, ref lastFrameData, lodPhysics);
                int num3 = Mathf.Clamp((int)(lastFrameData.m_position.x / 8f + 1080f), 0, 2159);
                int num4 = Mathf.Clamp((int)(lastFrameData.m_position.z / 8f + 1080f), 0, 2159);
                if ((num3 != num || num4 != num2) && (data.m_flags & ZombieInstance.Flags.Character) != ZombieInstance.Flags.None)
                {
                    Singleton<ZombieManager>.instance.RemoveFromGrid(instanceID, ref data, num, num2);
                    Singleton<ZombieManager>.instance.AddToGrid(instanceID, ref data, num3, num4);
                }
                if (data.m_flags != ZombieInstance.Flags.None)
                {
                    data.SetFrameData(Singleton<SimulationManager>.instance.m_currentFrameIndex, lastFrameData);
                }
            }
        }

        public virtual void SimulationStep(ushort instanceID, ref ZombieInstance citizenData, ref ZombieInstance.Frame frameData, bool lodPhysics)
        {
            Logger.LogClassAndMethodName(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.Log("3");
        }

        public virtual void CreateInstance(ushort instanceID, ref ZombieInstance data)
        {
        }

        public virtual void SetSource(ushort instanceID, ref ZombieInstance data, ushort sourceBuilding)
        {
        }

        public virtual void SetTarget(ushort instanceID, ref ZombieInstance data, ushort targetBuilding)
        {
        }

        public virtual void ReleaseInstance(ushort instanceID, ref ZombieInstance data)
        {
            this.SetSource(instanceID, ref data, 0);
            this.SetTarget(instanceID, ref data, 0);
        }

    }
}
