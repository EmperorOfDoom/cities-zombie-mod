using ColossalFramework;
using ColossalFramework.Math;
using UnityEngine;

namespace CitiesZombieMod
{
    public struct ZombieInstance
    {
        public enum Flags {
            None = 0,
            Created = 1,
            Deleted = 2,
            AtTarget = 4,
            Character = 8,
            InsideBuilding = 16,
            WaitingPath = 32,
            OnPath = 64,
            All = -1
        }

        public struct Frame {
            public Vector3 m_velocity;
            public Vector3 m_position;
            public Quaternion m_rotation;
            public bool m_underground;
            public bool m_insideBuilding;
            public bool m_transition;
        }

        public Frame m_frame0;
        public Frame m_frame1;
        public Frame m_frame2;
        public Frame m_frame3;
        public Vector4 m_targetPos;
        public Vector2 m_targetDir;
        public Flags m_flags;
        public uint m_zombie;
        public uint m_path;
        public ushort m_sourceBuilding;
        public ushort m_targetBuilding;
        public ushort m_nextGridInstance;
        public ushort m_nextSourceInstance;
        public ushort m_nextTargetInstance;
        public ushort m_infoIndex;
        public byte m_lastFrame;
        public byte m_pathPositionIndex;
        public byte m_lastPathOffset;
        public byte m_waitCounter;
        public byte m_targetSeed;

        private static Mesh asseteditorDrawMesh;

        public ZombieInfo Info {
            get { return PrefabCollection<ZombieInfo>.GetPrefab((uint)this.m_infoIndex); }
            set { this.m_infoIndex = (ushort)Mathf.Clamp(value.m_prefabDataIndex, 0, 65535); }
        }

        public static void RenderInstance(RenderManager.CameraInfo cameraInfo, ZombieInfo info, Vector3 position)
        {
            if (info.m_prefabInitialized)
            {
                float maxDistance = Mathf.Min(RenderManager.LevelOfDetailFactor * 800f, info.m_maxRenderDistance + cameraInfo.m_height * 0.5f);
                if (cameraInfo == null || info.m_lodMesh == null || cameraInfo.CheckRenderDistance(position, maxDistance))
                {
                    Matrix4x4 matrix = default(Matrix4x4);
                    matrix.SetTRS(position, Quaternion.identity, new Vector3(20f, 20f, 20f));
                    if (asseteditorDrawMesh == null)
                    {
                        asseteditorDrawMesh = new Mesh();
                    }
                    info.m_skinRenderer.BakeMesh(asseteditorDrawMesh);
                    Graphics.DrawMesh(asseteditorDrawMesh, matrix, info.m_skinRenderer.sharedMaterial, LayerMask.NameToLayer("Citizens"));
                }
            }
        }

        public void Spawn(ushort instanceID)
        {
            if ((this.m_flags & ZombieInstance.Flags.Character) == ZombieInstance.Flags.None)
            {
                ZombieInstance mFlags = this;
                mFlags.m_flags = mFlags.m_flags | ZombieInstance.Flags.Character;
                Singleton<ZombieManager>.instance.AddToGrid(instanceID, ref this);
            }
        }

        public void Unspawn(ushort instanceID)
        {
            if ((this.m_flags & ZombieInstance.Flags.Character) != ZombieInstance.Flags.None)
            {
                Singleton<ZombieManager>.instance.RemoveFromGrid(instanceID, ref this);
                ZombieInstance mFlags = this;
                mFlags.m_flags = mFlags.m_flags & (ZombieInstance.Flags.Created | ZombieInstance.Flags.Deleted | ZombieInstance.Flags.InsideBuilding | ZombieInstance.Flags.WaitingPath | ZombieInstance.Flags.OnPath | ZombieInstance.Flags.AtTarget);
            }
        }

        public ZombieInstance.Frame GetLastFrameData()
        {
            switch (this.m_lastFrame)
            {
                case 0:
                    return this.m_frame0;
                case 1:
                    return this.m_frame1;
                case 2:
                    return this.m_frame2;
                case 3:
                    return this.m_frame3;
                default:
                    return this.m_frame0;
            }
        }

        public void SetFrameData(uint simulationFrame, ZombieInstance.Frame data)
        {
            this.m_lastFrame = (byte)(simulationFrame >> 4 & 3u);
            switch (this.m_lastFrame)
            {
                case 0:
                    this.m_frame0 = data;
                    return;
                case 1:
                    this.m_frame1 = data;
                    return;
                case 2:
                    this.m_frame2 = data;
                    return;
                case 3:
                    this.m_frame3 = data;
                    return;
                default:
                    return;
            }
        }
    }
}
