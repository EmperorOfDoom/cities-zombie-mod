using ColossalFramework;
using ColossalFramework.Math;
using System;
using System.Diagnostics;
using UnityEngine;

namespace CitiesZombieMod
{
    class ZombieManager : SimulationManagerBase<ZombieManager, ZombieProperties>, IAudibleManager, IRenderableManager, ISimulationManager {
        [NonSerialized] public int ID_Speed;
        [NonSerialized] public int ID_State;
        [NonSerialized] public int ID_Color;
        [NonSerialized] public int[] ID_ZombieColor;
        [NonSerialized] public int[] ID_ZombieLocation;
        [NonSerialized] public Array32<Zombie> m_zombies;
        [NonSerialized] public Array16<ZombieInstance> m_instances;
        [NonSerialized] public ushort[] m_zombieGrid;
        [NonSerialized] public MaterialPropertyBlock m_materialBlock;
        [NonSerialized] public int m_zombieLayer;
        [NonSerialized] public AudioGroup m_audioGroup;
        [NonSerialized] public int m_tempOldestOriginalResident;
        [NonSerialized] public int m_finalOldestOriginalResident;

        public int m_zombieCount;
        public int m_instanceCount;

        private ulong[] m_renderBuffer;
        private FastList<ushort>[] m_groupZombies;
        private bool m_citizensRefreshed;

        protected override void Awake()
        {
            base.Awake();
            this.m_zombies = new Array32<Zombie>(1048576u);
            this.m_instances = new Array16<ZombieInstance>(65536u);
            this.m_zombieGrid = new ushort[4665600];
            this.m_renderBuffer = new ulong[1024];
            this.m_materialBlock = new MaterialPropertyBlock();
            this.ID_Color = Shader.PropertyToID("_Color");
            this.ID_Speed = Animator.StringToHash("Speed");
            this.ID_State = Animator.StringToHash("State");
            this.ID_ZombieLocation = new int[16];
            this.ID_ZombieColor = new int[16];
            for (int i = 0; i < 16; i++)
            {
                this.ID_ZombieLocation[i] = Shader.PropertyToID("_CitizenLocation" + i);
                this.ID_ZombieColor[i] = Shader.PropertyToID("_CitizenColor" + i);
            }
            this.m_zombieLayer = LayerMask.NameToLayer("Citizens");
            this.m_audioGroup = new AudioGroup(5, new SavedFloat(Settings.effectAudioVolume, Settings.gameSettingsFile, DefaultSettings.effectAudioVolume, true));
            uint num;
            this.m_zombies.CreateItem(out num);
            ushort num2;
            this.m_instances.CreateItem(out num2);
        }

        public bool CreateZombie(out uint zombie, ref Randomizer r)
        {
            uint zombieId;
            if (this.m_zombies.CreateItem(out zombieId, ref r))
            {
                zombie = zombieId;
                Zombie newZombie = default(Zombie);
                newZombie.m_flags = Zombie.Flags.Created;
                newZombie.m_health = 50;
                Logger.Log("Zombie id: " + (UIntPtr)zombie);
                this.m_zombies.m_buffer[(int)((UIntPtr)zombie)] = newZombie;
                this.m_zombieCount = (int)(this.m_zombies.ItemCount() - 1u);
                return true;
            }
            zombie = 0u;
            return false;
        }

        public bool CreateZombieInstance(out ushort instanceID, ref Randomizer randomizer, ZombieInfo info, uint zombie)
        {
            ushort newInstanceId;
            if (this.m_instances.CreateItem(out newInstanceId, ref randomizer))
            {
                instanceID = newInstanceId;
                ZombieInstance.Frame frame;
                frame.m_velocity = Vector3.zero;
                frame.m_position = Vector3.zero;
                frame.m_rotation = Quaternion.identity;
                frame.m_underground = false;
                frame.m_insideBuilding = false;
                frame.m_transition = false;
                this.m_instances.m_buffer[(int)instanceID].m_flags = ZombieInstance.Flags.Created;
                this.m_instances.m_buffer[(int)instanceID].Info = info;
                this.m_instances.m_buffer[(int)instanceID].m_zombie = zombie;
                this.m_instances.m_buffer[(int)instanceID].m_frame0 = frame;
                this.m_instances.m_buffer[(int)instanceID].m_frame1 = frame;
                this.m_instances.m_buffer[(int)instanceID].m_frame2 = frame;
                this.m_instances.m_buffer[(int)instanceID].m_frame3 = frame;
                this.m_instances.m_buffer[(int)instanceID].m_targetPos = Vector3.zero;
                this.m_instances.m_buffer[(int)instanceID].m_targetDir = Vector2.zero;
                this.m_instances.m_buffer[(int)instanceID].m_sourceBuilding = 0;
                this.m_instances.m_buffer[(int)instanceID].m_targetBuilding = 0;
                this.m_instances.m_buffer[(int)instanceID].m_nextGridInstance = 0;
                this.m_instances.m_buffer[(int)instanceID].m_nextSourceInstance = 0;
                this.m_instances.m_buffer[(int)instanceID].m_nextTargetInstance = 0;
                this.m_instances.m_buffer[(int)instanceID].m_lastFrame = 0;
                this.m_instances.m_buffer[(int)instanceID].m_pathPositionIndex = 0;
                this.m_instances.m_buffer[(int)instanceID].m_lastPathOffset = 0;
                this.m_instances.m_buffer[(int)instanceID].m_waitCounter = 0;
                this.m_instances.m_buffer[(int)instanceID].m_targetSeed = 0;
                if (zombie != 0u)
                {
                    this.m_zombies.m_buffer[(int)((UIntPtr)zombie)].m_instance = instanceID;
                }
                Logger.Error("InstanceID:"+instanceID);
                Logger.Error("Buffer:"+ this.m_instances.m_buffer[(int)instanceID]);
                info.m_zombieAI.CreateInstance(instanceID, ref this.m_instances.m_buffer[(int)instanceID]);
                this.m_instanceCount = (int)(this.m_instances.ItemCount() - 1u);
                Logger.Log("Zombie Instance created with instance id : " + instanceID);
                return true;
            }
            instanceID = 0;
            return false;
        }

        public void AddToGrid(ushort instance, ref ZombieInstance data)
        {
            ZombieInstance.Frame lastFrameData = data.GetLastFrameData();
            int gridX = Mathf.Clamp((int)(lastFrameData.m_position.x / 8f + 1080f), 0, 2159);
            int gridZ = Mathf.Clamp((int)(lastFrameData.m_position.z / 8f + 1080f), 0, 2159);
            this.AddToGrid(instance, ref data, gridX, gridZ);
        }

        public void AddToGrid(ushort instance, ref ZombieInstance data, int gridX, int gridZ)
        {
            int num = gridZ * 2160 + gridX;
            data.m_nextGridInstance = this.m_zombieGrid[num];
            this.m_zombieGrid[num] = instance;
        }

        public void RemoveFromGrid(ushort instance, ref ZombieInstance data)
        {
            ZombieInstance.Frame lastFrameData = data.GetLastFrameData();
            int gridX = Mathf.Clamp((int)(lastFrameData.m_position.x / 8f + 1080f), 0, 2159);
            int gridZ = Mathf.Clamp((int)(lastFrameData.m_position.z / 8f + 1080f), 0, 2159);
            this.RemoveFromGrid(instance, ref data, gridX, gridZ);
        }

        public void RemoveFromGrid(ushort instance, ref ZombieInstance data, int gridX, int gridZ)
        {
            int num = gridZ * 2160 + gridX;
            ushort num2 = 0;
            ushort num3 = this.m_zombieGrid[num];
            int num4 = 0;
            while (num3 != 0)
            {
                if (num3 == instance)
                {
                    if (num2 == 0)
                    {
                        this.m_zombieGrid[num] = data.m_nextGridInstance;
                    }
                    else
                    {
                        this.m_instances.m_buffer[(int)num2].m_nextGridInstance = data.m_nextGridInstance;
                    }
                    break;
                }
                num2 = num3;
                num3 = this.m_instances.m_buffer[(int)num3].m_nextGridInstance;
                if (++num4 > 65536)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                    break;
                }
            }
            data.m_nextGridInstance = 0;
        }

        protected override void SimulationStepImpl(int subStep)
        {
            Logger.LogClassAndMethodName(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (subStep != 0)
            {
                int num = (int)(Singleton<SimulationManager>.instance.m_currentFrameIndex & 4095u);
                int num2 = num * 256;
                int num3 = (num + 1) * 256 - 1;
                for (int i = num2; i <= num3; i++)
                {
                    if ((this.m_zombies.m_buffer[i].m_flags & Zombie.Flags.Created) != Zombie.Flags.None)
                    {
                        ZombieInfo zombieInfo = this.m_zombies.m_buffer[i].getZombieInfo((uint)i);
                        if (zombieInfo == null)
                        {
                            this.ReleaseZombie((uint)i);
                        }
                        else
                        {
                            zombieInfo.m_zombieAI.SimulationStep((uint)i, ref this.m_zombies.m_buffer[i]);
                        }
                    }
                }
                if (num == 4095)
                {
                    this.m_finalOldestOriginalResident = this.m_tempOldestOriginalResident;
                    this.m_tempOldestOriginalResident = 0;
                }
            }
            if (subStep != 0)
            {
                SimulationManager instance = Singleton<SimulationManager>.instance;
                Vector3 physicsLodRefPos = instance.m_simulationView.m_position + instance.m_simulationView.m_direction * 200f;
                int num7 = (int)(Singleton<SimulationManager>.instance.m_currentFrameIndex & 15u);
                int num8 = num7 * 4096;
                int num9 = (num7 + 1) * 4096 - 1;
                for (int k = num8; k <= num9; k++)
                {
                    if ((this.m_instances.m_buffer[k].m_flags & ZombieInstance.Flags.Created) != ZombieInstance.Flags.None)
                    {
                        ZombieInfo info = this.m_instances.m_buffer[k].Info;
                        info.m_zombieAI.SimulationStep((ushort)k, ref this.m_instances.m_buffer[k], physicsLodRefPos);
                    }
                }
            }
        }

        public void ReleaseZombie(uint zombie)
        {
            this.ReleaseZombieImplementation(zombie, ref this.m_zombies.m_buffer[(int)((UIntPtr)zombie)]);
        }

        public void ReleaseZombieInstance(ushort instance)
        {
            this.ReleaseZombieInstanceImplementation(instance, ref this.m_instances.m_buffer[(int)instance]);
        }

        private void ReleaseZombieImplementation(uint citizen, ref Zombie data)
        {
            InstanceID id = default(InstanceID);
            id.Citizen = citizen;
            Singleton<InstanceManager>.instance.ReleaseInstance(id);
            if (data.m_instance != 0)
            {
                this.ReleaseZombieInstance(data.m_instance);
                data.m_instance = 0;
            }
            data = default(Zombie);
            this.m_zombies.ReleaseItem(citizen);
            this.m_zombieCount = (int)(this.m_zombies.ItemCount() - 1u);
        }

        private void ReleaseZombieInstanceImplementation(ushort instance, ref ZombieInstance data)
        {
            ZombieInfo info = data.Info;
            if (info != null)
            {
                info.m_zombieAI.ReleaseInstance(instance, ref this.m_instances.m_buffer[(int)instance]);
            }
            data.Unspawn(instance);
            InstanceID id = default(InstanceID);
            id.CitizenInstance = instance;
            Singleton<InstanceManager>.instance.ReleaseInstance(id);
            if (data.m_path != 0u)
            {
                Singleton<PathManager>.instance.ReleasePath(data.m_path);
                data.m_path = 0u;
            }
            if (data.m_zombie != 0u)
            {
                this.m_zombies.m_buffer[(int)((UIntPtr)data.m_zombie)].m_instance = 0;
                data.m_zombie = 0u;
            }
            data.m_flags = ZombieInstance.Flags.None;
            this.m_instances.ReleaseItem(instance);
            this.m_instanceCount = (int)(this.m_instances.ItemCount() - 1u);
        }

        public ZombieInfo GetGroupZombieInfo(ref Randomizer r, ItemClass.Service service, Citizen.Gender gender, Citizen.SubCulture subCulture)
        {
            if (!this.m_citizensRefreshed)
            {
                CODebugBase<LogChannel>.Error(LogChannel.Core, "Random citizens not refreshed yet!");
                return null;
            }
            int num = this.GetGroupIndex(service, gender, subCulture);
            FastList<ushort> fastList = this.m_groupZombies[num];
            if (fastList == null)
            {
                return null;
            }
            if (fastList.m_size == 0)
            {
                return null;
            }
            num = r.Int32((uint)fastList.m_size);
            return PrefabCollection<ZombieInfo>.GetPrefab((uint)fastList.m_buffer[num]);
        }

        private int GetGroupIndex(ItemClass.Service service, Citizen.Gender gender, Citizen.SubCulture subCulture)
        {
            int num;
            if (subCulture != Citizen.SubCulture.Generic)
            {
                num = subCulture + 20 - Citizen.SubCulture.Hippie;
            }
            else
            {
                num = service - ItemClass.Service.Residential;
            }
            num = (int)(num * 2 + gender);
            return (int)(num * 14);
        }

        private int GetGroupIndex(ItemClass.Service service, ItemClass.SubService subService)
        {
            int result;
            if (subService != ItemClass.SubService.None)
            {
                result = subService + 20 - ItemClass.SubService.ResidentialLow;
            }
            else
            {
                result = service - ItemClass.Service.Residential;
            }
            return result;
        }

        public void ReleaseCitizen(uint citizen)
        {
	        this.ReleaseZombieImplementation(citizen, ref this.m_zombies.m_buffer[(int)((UIntPtr)citizen)]);
        }

    }
}
