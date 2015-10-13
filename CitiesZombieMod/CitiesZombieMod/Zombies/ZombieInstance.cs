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

        public ZombieInstance.Frame m_frame0;
        public ZombieInstance.Frame m_frame1;
        public ZombieInstance.Frame m_frame2;
        public ZombieInstance.Frame m_frame3;
        public Vector4 m_targetPos;
        public Vector2 m_targetDir;
        public ZombieInstance.Flags m_flags;
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


    }
}
