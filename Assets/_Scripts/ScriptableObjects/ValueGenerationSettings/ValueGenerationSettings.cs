using System;
using UnityEngine;

namespace _Scripts.ScriptableObjects.ValueGenerationSettings
{
    /**
     * The type of Noise which should be used.
     */
    public enum NoiseType
    {
        OpenSimplex,
        Perlin,
    }
    
    /**
     * This class stores the parameters for each generation step.
     */
    [CreateAssetMenu]
    [Serializable] // With this it can be shown in the Inspector
    public class ValueGenerationSettings : ScriptableObject
    {
        // General
        [Header("General")] public NoiseType noiseType = NoiseType.Perlin;
        [Range(0, 100)] public int percentage = 45;
    
        // Seed
        [Header("Seed")] public bool useRandomSeed = true;
        private bool _seedLocked;
        public string seed = "Hello World!";
        [Range(1000.0f, 1000000.0f)] public float seedScale = 100000.0f;
    
        // Gradient noise settings
        [Header("Gradient Noise")] [Range(0.0f, 1.0f)]
        public float noiseScale = 0.033f;

        [Header("Only For Mountain Generation")] public int stonePercentage = 10;
        
        // Seed can only be changed if there is no seed
        public void SetSeed(string inSeed)
        {
            if (_seedLocked) return;
    
            seed = inSeed;
            _seedLocked = true;
        }
    
        public string GetSeed()
        {
            return seed;
        }
    }
}