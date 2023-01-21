using System;
using UnityEngine;

namespace _Scripts.ScriptableObjects.MapGenerationSettings
{
    /**
     * This class stores the parameters for each generation step.
     */
    [CreateAssetMenu]
    [Serializable] // With this it can be shown in the Inspector
    public class MapGenerationSettings : ScriptableObject
    {
        // General
        [Header("General")] [Range(0, 100)] public int thresholdPercentage = 45;
        [Range(0, 100)] public int similarNeighboursPercentage = 45;

        // Seed
        [Header("Seed")] public bool useRandomSeed = true;
        private bool _seedLocked;
        public string seed = "Hello World!";

        // Pseudo random settings
        [Header("Pseudo Random")] public int smoothSteps = 7;

        [Header("For Mountain Generation")] public int stonePercentage = 10;

        // Seed can only be changed if there is no seed.
        public void SetSeed(string inSeed)
        {
            if (!_seedLocked)
            {
                seed = inSeed;
                _seedLocked = true;
            }
        }

        public string GetSeed()
        {
            return seed;
        }
    }
}