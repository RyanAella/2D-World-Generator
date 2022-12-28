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
    [Serializable] // With this it can be showed in the Inspector
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
        public void SetSeed(string seed)
        {
            if (_seedLocked) return;
            
            this.seed = seed;
            _seedLocked = true;
        }

        public String GetSeed()
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
            float threshold = 0.0f;

            if (settings.useRandomSeed)
            {
                settings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
            }

            double noiseValue = 0.0;

            switch (settings.noiseType)
            {
                // case NoiseType.PseudoRandom:
                //     System.Random prng = new System.Random(settings.GetSeed().GetHashCode());
                //     threshold = (float)settings.thresholdPercentage;
                //     noiseValue = prng.Next(101);
                //     break;

                case NoiseType.Perlin:

                    float seedOffset = settings.GetSeed().GetHashCode() / settings.seedScale;
                    threshold = Mathf.Lerp(0.0f, 1.0f, (float)settings.thresholdPercentage / 100);

                    var sampleX = (x + seedOffset) * settings.noiseScale;
                    var sampleY = (y + seedOffset) * settings.noiseScale;

                    noiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                    break;

                case NoiseType.OpenSimplex:

                    OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise(settings.GetSeed().GetHashCode());
                    threshold = Mathf.Lerp(-1.0f, 1.0f, (float)settings.thresholdPercentage / 100);

                    noiseValue = (float)openSimplexNoise.Evaluate(x * settings.noiseScale, y * settings.noiseScale);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return noiseValue < threshold ? 1 : 0;
        }
    }
}