using System;
using _Scripts._GradientNoise.OpenSimplex;
using UnityEngine;

namespace _Scripts._GradientNoise.ValueGeneration
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
    [Serializable] // With this it can be shown in the Inspector
    public class ValueGenerationSettings
    {
        // general
        [Header("General")] public NoiseType noiseType = NoiseType.Perlin;
        [Range(0, 100)] public int thresholdPercentage = 45;

        // seed
        [Header("Seed")] public bool useRandomSeed = true;
        private bool _seedLocked = false;
        [SerializeField] private string seed = "Hello World!";
        [Range(1000.0f, 1000000.0f)] public float seedScale = 100000.0f;

        // gradient noise settings
        [Header("Gradient Noise")] [Range(0.0f, 1.0f)]
        public float noiseScale = 0.033f;

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

    /**
     * This class generates a value per cell (x-, y- Coordinate pair)
     */
    public static class ValueGenerator
    {
        public static int Evaluate(int x, int y, ValueGenerationSettings settings)
        {
            float threshold;
            double noiseValue;

            if (settings.useRandomSeed)
            {
                settings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
            }

            float seedOffset = settings.GetSeed().GetHashCode() / settings.seedScale;
            var sampleX = (x + seedOffset) * settings.noiseScale;
            var sampleY = (y + seedOffset) * settings.noiseScale;

            switch (settings.noiseType)
            {
                case NoiseType.Perlin:

                    threshold = Mathf.Lerp(0.0f, 1.0f, (float)settings.thresholdPercentage / 100);
                    noiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                    break;

                case NoiseType.OpenSimplex:

                    OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise(settings.GetSeed().GetHashCode());
                    threshold = Mathf.Lerp(-1.0f, 1.0f, (float)settings.thresholdPercentage / 100);
                    noiseValue = (float)openSimplexNoise.Evaluate(sampleX, y * sampleY);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return noiseValue < threshold ? 1 : 0;
        }
    }
}