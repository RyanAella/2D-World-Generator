using System;
using _Scripts._GradientNoise.OpenSimplex;
using _Scripts.CellGeneration;
using _Scripts.Helper;
using UnityEngine;

namespace _Scripts.old
{
    /**
     * The type of Noise which should be used.
     */
    public enum NoiseType
    {
        PseudoRandom,
        OpenSimplex,
        Perlin,
    }

    /**
     * This class controls the entire generation process.
     * All required parameters are collected in it and passed to the corresponding methods.
     */
    public class MapGenerator : MonoBehaviour
    {
        [Header("General")] public NoiseType noiseType = NoiseType.PseudoRandom;
        [SerializeField] private Vector2Int resolution = new Vector2Int(256, 144);
        [Range(0, 100)] [SerializeField] private int indoorsPercentage = 35;

        // Seed
        [Header("Seed")] public bool useRandomSeed = true;
        [SerializeField] private string seed = "Hello World!";
        [Range(1000.0f, 1000000.0f)] public float seedScale = 100000.0f;

        // Gradient noise settings
        [Header("Gradient Noise")] [Range(0.0f, 1.0f)]
        public float noiseScale = 0.033f;

        [Header("Pseudo Random")] [SerializeField]
        private int smoothSteps = 7;


        // private properties
        private int[,] _valueMap;
        private MapDisplay _display;
        private Cell[,] _cells;

        void Start()
        {
            // Initialization
            var mapPos = transform.position;
            _display = new MapDisplay(mapPos, resolution, gameObject);

            // Generate and display valueMap
            _valueMap = GenerateMap(seed, noiseType, seedScale, noiseScale, indoorsPercentage);
            // if (noiseType == NoiseType.PseudoRandom) SmoothMap();
            _display.UpdateMapDisplay(_valueMap);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && noiseType == NoiseType.PseudoRandom)
            {
                SmoothMap();

                _display.UpdateMapDisplay(_valueMap);
            }
        }

        /**
         * Generate the map.
         */
        private int[,] GenerateMap(string seedIn, NoiseType noiseTypeIn, float seedScaleIn, float noiseScaleIn,
            int indoorsPercentageIn)
        {
            // Initialize result array
            int[,] result = new int[resolution.x, resolution.y];

            float threshold;
            double noiseValue;

            // Get the coordinates
            float seedOffset = seedIn.GetHashCode() / seedScaleIn;
            float sampleX;
            float sampleY;

            switch (noiseTypeIn)
            {
                case NoiseType.Perlin:

                    for (int x = 0; x < resolution.x; x++)
                    {
                        for (int y = 0; y < resolution.y; y++)
                        {
                            // Get the coordinates
                            sampleX = (x + seedOffset) * noiseScaleIn;
                            sampleY = (y + seedOffset) * noiseScaleIn;

                            threshold = Mathf.Lerp(0.0f, 1.0f, (float)indoorsPercentageIn / 100);
                            noiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                            result[x, y] = noiseValue < threshold ? 1 : 0;
                        }
                    }

                    break;

                case NoiseType.OpenSimplex:

                    for (int x = 0; x < resolution.x; x++)
                    {
                        for (int y = 0; y < resolution.y; y++)
                        {
                            // Get the coordinates
                            sampleX = (x + seedOffset) * noiseScaleIn;
                            sampleY = (y + seedOffset) * noiseScaleIn;

                            OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise(seedIn.GetHashCode());
                            threshold = Mathf.Lerp(-1.0f, 1.0f, (float)indoorsPercentageIn / 100);
                            noiseValue = (float)openSimplexNoise.Evaluate(sampleX, sampleY);
                            result[x, y] = noiseValue < threshold ? 1 : 0;
                        }
                    }

                    break;

                case NoiseType.PseudoRandom:

                    System.Random prng = new System.Random(seedIn.GetHashCode());

                    for (int x = 0; x < resolution.x; x++)
                    {
                        for (int y = 0; y < resolution.y; y++)
                        {
                            // Next(n) -> value between inclusive 0 and exclusive n
                            result[x, y] = prng.Next(101) < indoorsPercentageIn ? 1 : 0;
                        }
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }


            return result;
        }

        /**
         * Smooth the map.
         */
        private void SmoothMap()
        {
            if (_valueMap == null) return;

            for (int i = 0; i < smoothSteps; i++)
            {
                int[,] tempMap = new int[resolution.x, resolution.y];

                for (int x = 0; x < resolution.x; x++)
                {
                    for (int y = 0; y < resolution.y; y++)
                    {
                        tempMap[x, y] = ApplyRules(x, y);
                    }
                }

                _valueMap = tempMap;
            }
        }

        /**
         * Apply the rules.
         */
        private int ApplyRules(int xIndex, int yIndex)
        {
            var neighbours = GetLivingNeighboursCount(xIndex, yIndex);

            switch (neighbours)
            {
                case > 4:
                    return 1;
                case < 4:
                    return 0;
                default:
                    return _valueMap[xIndex, yIndex];
            }
        }

        /**
         * Get the number of neighbours with value 1.
         */
        private int GetLivingNeighboursCount(int xIndex, int yIndex)
        {
            int neighbours = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var xPos = xIndex + x;
                    var yPos = yIndex + y;

                    // skip x and y index value
                    if ((x == 0 && y == 0) || xPos < 0 || xPos >= resolution.x || yPos < 0 ||
                        yPos >= resolution.y) continue;

                    // increment neighbours if 1
                    if (_valueMap[xPos, yPos] == 1) neighbours++;
                }
            }

            return neighbours;
        }
    }
}