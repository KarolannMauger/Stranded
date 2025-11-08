using UnityEngine;

public class TerrainTreeDynamicManager : MonoBehaviour
{
    [Header("Références")]
    public Terrain terrain;
    public Transform player;

    [Header("Distances")]
    public float activationRadius = 25f;
    public float hysteresis = 5f;

    private TerrainData _terrainData;
    private TreeInstance[] _originalTrees;

    private GameObject[] _activeInstances;
    private bool[] _harvested;

    private bool _initialized;

    void Start()
    {
        if (terrain == null)
            terrain = Terrain.activeTerrain;

        if (terrain == null)
        {
            Debug.LogError("[TreeManager] Aucun Terrain trouve.");
            enabled = false;
            return;
        }

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player == null)
        {
            Debug.LogError("[TreeManager] Aucun joueur trouve (tag Player ou assignation manuelle).");
            enabled = false;
            return;
        }

        _terrainData = terrain.terrainData;

        _originalTrees = (TreeInstance[])_terrainData.treeInstances.Clone();

        int count = _originalTrees.Length;
        _activeInstances = new GameObject[count];
        _harvested = new bool[count];

        _initialized = true;
    }

    void Update()
    {
        if (!_initialized || player == null || _terrainData == null) return;

        var trees = _terrainData.treeInstances;
        bool changed = false;

        for (int i = 0; i < _originalTrees.Length; i++)
        {
            if (_harvested[i])
                continue;

            TreeInstance original = _originalTrees[i];

            Vector3 worldPos =
                Vector3.Scale(original.position, _terrainData.size) + terrain.transform.position;

            float dist = Vector3.Distance(player.position, worldPos);

            float spawnDist = activationRadius;
            float despawnDist = activationRadius + hysteresis;

            if (dist <= spawnDist)
            {
                if (_activeInstances[i] == null)
                {
                    GameObject prefab = _terrainData.treePrototypes[original.prototypeIndex].prefab;
                    if (prefab != null)
                    {
                        float yRotationDeg = original.rotation * Mathf.Rad2Deg;
                        Quaternion rot = Quaternion.Euler(0f, yRotationDeg, 0f);

                        GameObject inst = Instantiate(prefab, worldPos, rot, transform);

                        Vector3 baseScale = prefab.transform.localScale;
                        inst.transform.localScale = new Vector3(
                            baseScale.x * original.widthScale,
                            baseScale.y * original.heightScale,
                            baseScale.z * original.widthScale
                        );

                        var runtime = inst.AddComponent<TreeRuntimeInstance>();
                        runtime.manager = this;
                        runtime.treeIndex = i;

                        _activeInstances[i] = inst;

                        TreeInstance t = trees[i];
                        t.widthScale = 0f;
                        t.heightScale = 0f;
                        trees[i] = t;
                        changed = true;
                    }
                }
            }
            else if (dist >= despawnDist)
            {
                if (_activeInstances[i] != null)
                {
                    Destroy(_activeInstances[i]);
                    _activeInstances[i] = null;

                    TreeInstance t = trees[i];
                    t.widthScale = _originalTrees[i].widthScale;
                    t.heightScale = _originalTrees[i].heightScale;
                    trees[i] = t;
                    changed = true;
                }
            }
        }

        if (changed)
        {
            _terrainData.treeInstances = trees;
        }
    }

    public void HarvestTree(int index)
    {
        if (!_initialized) return;
        if (index < 0 || index >= _harvested.Length) return;
        if (_harvested[index]) return;

        _harvested[index] = true;

        if (_activeInstances[index] != null)
        {
            Destroy(_activeInstances[index]);
            _activeInstances[index] = null;
        }

        var trees = _terrainData.treeInstances;
        if (index < trees.Length)
        {
            TreeInstance t = trees[index];
            t.widthScale = 0f;
            t.heightScale = 0f;
            trees[index] = t;
            _terrainData.treeInstances = trees;
        }
    }

    private void OnDisable()
    {
        if (_terrainData != null && _originalTrees != null)
        {
            _terrainData.treeInstances = _originalTrees;
        }
    }
}
