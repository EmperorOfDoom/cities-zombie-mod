using ColossalFramework;
using System;
using UnityEngine;

namespace CitiesZombieMod
{
    public class ZombieInfo : PrefabInfo
    {
        public Citizen.Gender m_gender;
        public ItemClass m_class;
        public ItemClass.Placement m_placementStyle;
        public ItemClass.Availability m_availableIn = ItemClass.Availability.All;
        public SkinnedMeshRenderer m_skinRenderer;
        public GameObject m_lodObject;
        public float m_walkSpeed = 3f;
        public float m_radius = 0.4f;
        public float m_height = 1.8f;

        [NonSerialized]
        public Mesh m_lodMesh;

        [NonSerialized]
        public Material m_lodMaterial;

        [NonSerialized]
        public Color m_color0;

        [NonSerialized]
        public Color m_color1;

        [NonSerialized]
        public Color m_color2;

        [NonSerialized]
        public Color m_color3;

        [NonSerialized]
        public float m_maxRenderDistance;

        [NonSerialized]
        public float m_lodRenderDistance;

        [NonSerialized]
        public ZombieAI m_zombieAI;

        [NonSerialized]
        private Material m_material;

        [NonSerialized]
        private Renderer m_renderer;

        [NonSerialized]
        private Animator m_animator;

        [NonSerialized]
        public Mesh m_lodMeshCombined1;

        [NonSerialized]
        public Mesh m_lodMeshCombined4;

        [NonSerialized]
        public Mesh m_lodMeshCombined8;

        [NonSerialized]
        public Mesh m_lodMeshCombined16;

        [NonSerialized]
        public Material m_lodMaterialCombined;

        [NonSerialized]
        public Matrix4x4[] m_lodLocations;

        [NonSerialized]
        public Color[] m_lodColors;

        [NonSerialized]
        public int m_lodCount;

        [NonSerialized]
        public Vector3 m_lodMin;

        [NonSerialized]
        public Vector3 m_lodMax;

        public override void InitializePrefab()
        {
            base.InitializePrefab();
            if (this.m_class == null)
            {
                throw new PrefabException(this, "Class missing");
            }
            if (this.m_skinRenderer != null)
            {
                Material sharedMaterial = this.m_skinRenderer.sharedMaterial;
                this.m_color0 = sharedMaterial.GetColor("_ColorV0");
                this.m_color1 = sharedMaterial.GetColor("_ColorV1");
                this.m_color2 = sharedMaterial.GetColor("_ColorV2");
                this.m_color3 = sharedMaterial.GetColor("_ColorV3");
            }
            else
            {
                this.m_color0 = Color.white;
                this.m_color1 = Color.white;
                this.m_color2 = Color.white;
                this.m_color3 = Color.white;
            }
            if (this.m_lodObject != null)
            {
                MeshFilter component = this.m_lodObject.GetComponent<MeshFilter>();
                this.m_lodMesh = component.sharedMesh;
                this.m_lodMaterial = this.m_lodObject.GetComponent<Renderer>().sharedMaterial;
                this.GenerateCombinedLodMesh();
            }
            else
            {
                CODebugBase<LogChannel>.Warn(LogChannel.Core, "LOD missing: " + base.gameObject.name, base.gameObject);
            }
            this.RefreshLevelOfDetail();
            if (this.m_zombieAI == null)
            {
                this.m_zombieAI = base.GetComponent<ZombieAI>();
                this.m_zombieAI.m_info = this;
                this.m_zombieAI.InitializeAI();
            }
            this.m_lodLocations = new Matrix4x4[16];
            this.m_lodColors = new Color[16];
            this.m_lodCount = 0;
            this.m_lodMin = new Vector3(100000f, 100000f, 100000f);
            this.m_lodMax = new Vector3(-100000f, -100000f, -100000f);
        }

        public override void DestroyPrefab()
        {
            if (this.m_lodMeshCombined1 != null)
            {
                UnityEngine.Object.Destroy(this.m_lodMeshCombined1);
                this.m_lodMeshCombined1 = null;
            }
            if (this.m_lodMeshCombined4 != null)
            {
                UnityEngine.Object.Destroy(this.m_lodMeshCombined4);
                this.m_lodMeshCombined4 = null;
            }
            if (this.m_lodMeshCombined8 != null)
            {
                UnityEngine.Object.Destroy(this.m_lodMeshCombined8);
                this.m_lodMeshCombined8 = null;
            }
            if (this.m_lodMeshCombined16 != null)
            {
                UnityEngine.Object.Destroy(this.m_lodMeshCombined16);
                this.m_lodMeshCombined16 = null;
            }
            if (this.m_lodMaterialCombined != null)
            {
                UnityEngine.Object.Destroy(this.m_lodMaterialCombined);
                this.m_lodMaterialCombined = null;
            }
            this.m_zombieAI.ReleaseAI();
            base.DestroyPrefab();
        }

        public override void RefreshLevelOfDetail()
        {
            float levelOfDetailFactor = RenderManager.LevelOfDetailFactor;
            if (this.m_lodObject != null)
            {
                this.m_lodRenderDistance = levelOfDetailFactor * 150f;
            }
            else
            {
                this.m_lodRenderDistance = 10000f;
            }
            this.m_maxRenderDistance = levelOfDetailFactor * 400f;
        }

        public override void InitializePrefabInstance()
        {
            base.InitializePrefabInstance();
            this.m_animator = base.gameObject.GetComponent<Animator>();
            this.m_zombieAI = base.gameObject.GetComponent<ZombieAI>();
            if (this.m_zombieAI != null)
            {
                this.m_zombieAI.m_info = this;
            }
            this.m_renderer = base.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (this.m_renderer == null)
            {
                for (int i = 0; i < base.gameObject.transform.childCount; i++)
                {
                    Transform child = base.gameObject.transform.GetChild(i);
                    this.m_renderer = child.GetComponent<Renderer>();
                    if (this.m_renderer != null)
                    {
                        break;
                    }
                }
            }
            if (this.m_renderer != null)
            {
                this.m_skinRenderer = (this.m_renderer as SkinnedMeshRenderer);
                this.m_material = new Material(this.m_renderer.sharedMaterial);
                this.m_renderer.sharedMaterial = this.m_material;
            }
        }

        public override void DestroyPrefabInstance()
        {
            if (this.m_material != null)
            {
                UnityEngine.Object.Destroy(this.m_material);
                this.m_material = null;
            }
            this.m_renderer = null;
            this.m_animator = null;
            base.DestroyPrefabInstance();
        }

        public override void RenderMesh(RenderManager.CameraInfo cameraInfo)
        {
            Vector3 position = new Vector3(0f, 60f, 0f);
            ZombieInstance.RenderInstance(cameraInfo, this, position);
        }

        public void SetRenderParameters(Vector3 position, Quaternion rotation, Vector3 velocity, Color color, int state, bool underground)
        {
            ZombieManager instance = Singleton<ZombieManager>.instance;
            ZombieManager expr_0C_cp_0 = instance;
            expr_0C_cp_0.m_drawCallData.m_defaultCalls = expr_0C_cp_0.m_drawCallData.m_defaultCalls + 1;
            Transform transform = base.transform;
            transform.position = position;
            transform.rotation = rotation;
            float magnitude = velocity.magnitude;
            this.m_animator.SetFloat(instance.ID_Speed, magnitude);
            this.m_animator.SetInteger(instance.ID_State, state);
            if (this.m_instanceChanged)
            {
                this.m_instanceChanged = false;
                switch (state)
                {
                    case 0:
                        if (magnitude > 0.1f)
                        {
                            this.m_animator.Play("walk 0", -1, UnityEngine.Random.value);
                        }
                        else
                        {
                            this.m_animator.Play("idle", -1, UnityEngine.Random.value);
                        }
                        break;
                    case 1:
                        if (magnitude > 0.1f)
                        {
                            this.m_animator.Play("walk 0", -1, UnityEngine.Random.value);
                        }
                        else
                        {
                            this.m_animator.Play("panic", -1, UnityEngine.Random.value);
                        }
                        break;
                    case 2:
                        this.m_animator.Play("sitting idle", -1, UnityEngine.Random.value);
                        break;
                    case 3:
                        if (magnitude > 0.1f)
                        {
                            this.m_animator.Play("bike-ride", -1, UnityEngine.Random.value);
                        }
                        else
                        {
                            this.m_animator.Play("bike-idle", -1, UnityEngine.Random.value);
                        }
                        break;
                    case 4:
                        if (magnitude > 0.1f)
                        {
                            this.m_animator.Play("walk 0", -1, UnityEngine.Random.value);
                        }
                        else
                        {
                            this.m_animator.Play("idle2", -1, UnityEngine.Random.value);
                        }
                        break;
                }
            }
            if (magnitude < 0.1f)
            {
                this.m_animator.speed = (1f - magnitude * 9f) * Singleton<SimulationManager>.instance.m_simulationTimeSpeed;
            }
            else
            {
                this.m_animator.speed = magnitude * Singleton<SimulationManager>.instance.m_simulationTimeSpeed;
            }

            if (this.m_renderer != null)
            {
                if (this.m_material != null)
                {
                    this.m_renderer.sharedMaterial = this.m_material;
                }
                this.m_renderer.gameObject.layer = Singleton<CitizenManager>.instance.m_citizenLayer;
            } else if (this.m_material != null)
            {
                this.m_material.color = color;
            }
        }

        private void GenerateCombinedLodMesh()
        {
            if (this.m_lodMeshCombined1 == null)
            {
                this.m_lodMeshCombined1 = new Mesh();
            }
            if (this.m_lodMeshCombined4 == null)
            {
                this.m_lodMeshCombined4 = new Mesh();
            }
            if (this.m_lodMeshCombined8 == null)
            {
                this.m_lodMeshCombined8 = new Mesh();
            }
            if (this.m_lodMeshCombined16 == null)
            {
                this.m_lodMeshCombined16 = new Mesh();
            }
            if (this.m_lodMaterialCombined == null)
            {
                string[] shaderKeywords;
                if (this.m_lodMaterial != null)
                {
                    this.m_lodMaterialCombined = new Material(this.m_lodMaterial);
                    shaderKeywords = this.m_lodMaterial.shaderKeywords;
                }
                else
                {
                    this.m_lodMaterialCombined = new Material(this.m_material);
                    shaderKeywords = this.m_material.shaderKeywords;
                }
                for (int i = 0; i < shaderKeywords.Length; i++)
                {
                    this.m_lodMaterialCombined.EnableKeyword(shaderKeywords[i]);
                }
                this.m_lodMaterialCombined.EnableKeyword("MULTI_INSTANCE");
            }
            Vector3[] vertices = this.m_lodMesh.vertices;
            Vector3[] normals = this.m_lodMesh.normals;
            Vector4[] tangents = this.m_lodMesh.tangents;
            Vector2[] uv = this.m_lodMesh.uv;
            Color32[] array = this.m_lodMesh.colors32;
            int[] triangles = this.m_lodMesh.triangles;
            if (array.Length != vertices.Length)
            {
                array = new Color32[vertices.Length];
                for (int j = 0; j < array.Length; j++)
                {
                    array[j] = new Color32(255, 255, 255, 255);
                }
            }
            int num = vertices.Length;
            int num2 = triangles.Length;
            RenderGroup.MeshData meshData = new RenderGroup.MeshData(RenderGroup.VertexArrays.Vertices | RenderGroup.VertexArrays.Normals | RenderGroup.VertexArrays.Tangents | RenderGroup.VertexArrays.Uvs | RenderGroup.VertexArrays.Colors, num * 1, num2 * 1);
            RenderGroup.MeshData meshData2 = new RenderGroup.MeshData(RenderGroup.VertexArrays.Vertices | RenderGroup.VertexArrays.Normals | RenderGroup.VertexArrays.Tangents | RenderGroup.VertexArrays.Uvs | RenderGroup.VertexArrays.Colors, num * 4, num2 * 4);
            RenderGroup.MeshData meshData3 = new RenderGroup.MeshData(RenderGroup.VertexArrays.Vertices | RenderGroup.VertexArrays.Normals | RenderGroup.VertexArrays.Tangents | RenderGroup.VertexArrays.Uvs | RenderGroup.VertexArrays.Colors, num * 8, num2 * 8);
            RenderGroup.MeshData meshData4 = new RenderGroup.MeshData(RenderGroup.VertexArrays.Vertices | RenderGroup.VertexArrays.Normals | RenderGroup.VertexArrays.Tangents | RenderGroup.VertexArrays.Uvs | RenderGroup.VertexArrays.Colors, num * 16, num2 * 16);
            int num3 = 0;
            int num4 = 0;
            for (int k = 0; k < 16; k++)
            {
                byte a = (byte)(k * 16);
                for (int l = 0; l < num2; l++)
                {
                    if (k < 1)
                    {
                        meshData.m_triangles[num4] = triangles[l] + num3;
                    }
                    if (k < 4)
                    {
                        meshData2.m_triangles[num4] = triangles[l] + num3;
                    }
                    if (k < 8)
                    {
                        meshData3.m_triangles[num4] = triangles[l] + num3;
                    }
                    if (k < 16)
                    {
                        meshData4.m_triangles[num4] = triangles[l] + num3;
                    }
                    num4++;
                }
                for (int m = 0; m < num; m++)
                {
                    Color32 color = array[m];
                    color.a = a;
                    if (k < 1)
                    {
                        meshData.m_vertices[num3] = vertices[m];
                        meshData.m_normals[num3] = normals[m];
                        meshData.m_tangents[num3] = tangents[m];
                        meshData.m_uvs[num3] = uv[m];
                        meshData.m_colors[num3] = color;
                    }
                    if (k < 4)
                    {
                        meshData2.m_vertices[num3] = vertices[m];
                        meshData2.m_normals[num3] = normals[m];
                        meshData2.m_tangents[num3] = tangents[m];
                        meshData2.m_uvs[num3] = uv[m];
                        meshData2.m_colors[num3] = color;
                    }
                    if (k < 8)
                    {
                        meshData3.m_vertices[num3] = vertices[m];
                        meshData3.m_normals[num3] = normals[m];
                        meshData3.m_tangents[num3] = tangents[m];
                        meshData3.m_uvs[num3] = uv[m];
                        meshData3.m_colors[num3] = color;
                    }
                    if (k < 16)
                    {
                        meshData4.m_vertices[num3] = vertices[m];
                        meshData4.m_normals[num3] = normals[m];
                        meshData4.m_tangents[num3] = tangents[m];
                        meshData4.m_uvs[num3] = uv[m];
                        meshData4.m_colors[num3] = color;
                    }
                    num3++;
                }
            }
            meshData.PopulateMesh(this.m_lodMeshCombined1);
            meshData2.PopulateMesh(this.m_lodMeshCombined4);
            meshData3.PopulateMesh(this.m_lodMeshCombined8);
            meshData4.PopulateMesh(this.m_lodMeshCombined16);
        }

        public override PrefabAI GetAI()
        {
            return this.m_zombieAI;
        }

        public override ItemClass.Service GetService()
        {
            return (this.m_class == null) ? base.GetService() : this.m_class.m_service;
        }

        public override ItemClass.SubService GetSubService()
        {
            return (this.m_class == null) ? base.GetSubService() : this.m_class.m_subService;
        }
    }
}
