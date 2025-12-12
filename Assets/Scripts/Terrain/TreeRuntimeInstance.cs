using UnityEngine;

public class TreeRuntimeInstance : MonoBehaviour
{
    // Metadata used by TerrainTreeDynamicManager to track spawned trees
    [HideInInspector] public TerrainTreeDynamicManager manager;
    [HideInInspector] public int treeIndex;
}
