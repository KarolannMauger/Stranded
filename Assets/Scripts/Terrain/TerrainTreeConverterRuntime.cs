using System.Collections.Generic;
using UnityEngine;

public class TerrainTreeConverterRuntime : MonoBehaviour
{
    // Replaces terrain trees with prefab instances when the player is nearby
    public Terrain terrain;
    public Transform player;
    public float activationRadius = 20f;

    private TreeInstance[] _originalTrees;
    private bool[] _converted;
    private TerrainData _terrainData;

    void Start()
    {
        if (terrain == null)
            terrain = Terrain.activeTerrain;

        if (terrain == null)
        {
            Debug.LogError("[TreeConverter] No Terrain found.");
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
            Debug.LogError("[TreeConverter] No Player found.");
            enabled = false;
            return;
        }

        _terrainData = terrain.terrainData;

        _originalTrees = (TreeInstance[])_terrainData.treeInstances.Clone();
        _converted = new bool[_originalTrees.Length];
    }

    void Update()
    {
        // Skip if something critical is missing
        if (player == null || _terrainData == null) return;

        var trees = _terrainData.treeInstances;
        var prototypes = _terrainData.treePrototypes;

        bool changed = false;

        for (int i = 0; i < trees.Length; i++)
        {
            if (_converted[i]) continue;

            TreeInstance t = trees[i];

            Vector3 worldPos =
                Vector3.Scale(t.position, _terrainData.size) + terrain.transform.position;

            float dist = Vector3.Distance(player.position, worldPos);

            if (dist <= activationRadius)
            {
                // Spawn the runtime prefab and hide the terrain tree instance
                GameObject prefab = prototypes[t.prototypeIndex].prefab;
                if (prefab != null)
                {
                    Instantiate(prefab, worldPos, Quaternion.identity);
                }

                t.heightScale = 0f;
                t.widthScale = 0f;
                trees[i] = t;

                _converted[i] = true;
                changed = true;
            }
        }

        if (changed)
        {
            _terrainData.treeInstances = trees;
        }
    }

    private void OnDisable()
    {
        // Restore original tree instances when this manager is turned off
        if (_terrainData != null && _originalTrees != null)
        {
            _terrainData.treeInstances = _originalTrees;
        }
    }
}
