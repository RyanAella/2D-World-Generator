using System;
using _Scripts._GradientNoise.OpenSimplex;
using _Scripts.ScriptableObjects;
using _Scripts.ScriptableObjects.ValueGenerationSettings;
using UnityEngine;

namespace _Scripts._GradientNoise.ValueGeneration
{
    /**
     * This class generates a value per cell (x-, y-coordinate pair)
     */
    public static class ValueGenerator
    {
        public static int Evaluate(int x, int y, ValueGenerationSettings settings)
        {
            float threshold;
            double noiseValue;

            // Check if a random seed is wanted
            if (settings.useRandomSeed)
            {
                settings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
            }

            // Get the coordinates
            float seedOffset = settings.GetSeed().GetHashCode() / settings.seedScale;
            var sampleX = (x + seedOffset) * settings.noiseScale;
            var sampleY = (y + seedOffset) * settings.noiseScale;

            switch (settings.noiseType)
            {
                case NoiseType.Perlin:
                    
                    // Interpolate between 0.0 and 1.0 by settings.percentage / 100
                    threshold = Mathf.Lerp(0.0f, 1.0f, (float)settings.percentage / 100);
                    noiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                    break;

                case NoiseType.OpenSimplex:

                    // Interpolate between -1.0 and 1.0 by settings.percentage / 100
                    OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise(settings.GetSeed().GetHashCode());
                    threshold = Mathf.Lerp(-1.0f, 1.0f, (float)settings.percentage / 100);
                    noiseValue = (float)openSimplexNoise.Evaluate(sampleX, sampleY);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return noiseValue < threshold ? 1 : 0;
        }
    }
}