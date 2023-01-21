using System;
using UnityEngine;

namespace _Scripts.ScriptableObjects.AssetGenerationSettings
{
    /**
     * This class stores the parameters for each generation step.
     */
    [CreateAssetMenu]
    [Serializable] // With this it can be showed in the Inspector
    public class AssetGenerationSettings : ScriptableObject
    {
        // trees, bushes
        [Range(1, 100)] public int treePercentage = 50;
        [Range(1, 100)] public int bushPercentage = 30;
        [Range(1, 100)] public int grassPercentage = 10;
        [Range(1, 100)] public int stonePercentage = 10;
    }
}