using ColossalFramework;
using System;
using UnityEngine;

namespace CitiesZombieMod
{
    class ZombieManager : SimulationManagerBase<ZombieManager, ZombieProperties>, IAudibleManager, IRenderableManager, ISimulationManager {
        [NonSerialized]
        public int ID_Speed;

        [NonSerialized]
        public int ID_State;

        [NonSerialized]
        public int ID_Color;

        [NonSerialized]
        public int[] ID_ZombieColor;

        [NonSerialized]
        public int[] ID_ZombieLocation;

        [NonSerialized]
        public Array32<Zombie> m_zombies;

        [NonSerialized]
        public Array16<ZombieInstance> m_instances;

        [NonSerialized]
        public ushort[] m_zombieGrid;

        [NonSerialized]
        public MaterialPropertyBlock m_materialBlock;

        [NonSerialized]
        public int m_zombieLayer;

        [NonSerialized]
        public AudioGroup m_audioGroup;

        private ulong[] m_renderBuffer;


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

    }
}
