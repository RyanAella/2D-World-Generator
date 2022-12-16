using System;
using System.Collections.Generic;
using _Scripts.CellGeneration;
using _Scripts.Helper;
using _Scripts.OpenSimplex;
using UnityEngine;

namespace _Scripts.ValueGeneration
{
    public enum NoiseType
    {
        // PseudoRandom,
        OpenSimplex,
        Perlin,
    }

    [Serializable]
    public class ValueGenerationSettings
    {
        // general
        [Header("General")] public NoiseType noiseType = NoiseType.Perlin;
        [Range(0, 100)] public int thresholdPercentage = 49;

        // seed
        [Header("Seed")] public bool useRandomSeed = true;
        private bool _seedLocked = false;
        [SerializeField] private string seed = "Hello World!";

        [Range(1000.0f, 1000000.0f)] public float seedScale = 100000.0f;

        // pseudo random settings
        [Header("Pseudo Random")] public int smoothSteps = 7;

        // gradient noise settings
        [Header("Gradient Noise")] [Range(0.0f, 1.0f)]
        public float noiseScale = 0.033f;

        public void SetSeed(String seed)
        {
            if (!_seedLocked)
            {
                this.seed = seed;
                _seedLocked = true;
            }
        }

        public String GetSeed()
        {
            return seed;
        }
    }

    public static class ValueGenerator
    {
        // public static Cell[,] GenerateValueMap(Vector2Int resolution, ValueGenerationSettings settings)
        // {
        //     Cell[,] cellMap = new Cell[resolution.x, resolution.y];
        //
        //     if (settings.useRandomSeed)
        //     {
        //         settings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
        //     }
        //
        //     float threshold = 0.0f;
        //
        //     switch (settings.noiseType)
        //     {
        //         case NoiseType.PseudoRandom:
        //
        //             System.Random prng = new System.Random(settings.GetSeed().GetHashCode());
        //
        //             for (int x = 0; x < resolution.x; x++)
        //             {
        //                 for (int y = 0; y < resolution.y; y++)
        //                 {
        //                     Cell cell = new Cell(x, y);
        //
        //                     // Next(n) -> value between inclusive 0 and exclusive n
        //                     if (prng.Next(101) < settings.thresholdPercentage)
        //                     {
        //                         cell.indoors = true;
        //                     }
        //                     else
        //                     {
        //                         cell.indoors = false;
        //                     }
        //
        //                     cellMap[x, y] = cell;
        //                 }
        //             }
        //
        //             // smooth cell map
        //             cellMap = SmoothCellMap(cellMap, settings.smoothSteps, resolution);
        //
        //             break;
        //
        //         case NoiseType.OpenSimplex:
        //
        //             OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise(settings.GetSeed().GetHashCode());
        //
        //             threshold = Mathf.Lerp(-1.0f, 1.0f, (float)settings.thresholdPercentage / 100);
        //
        //             for (int x = 0; x < resolution.x; x++)
        //             {
        //                 for (int y = 0; y < resolution.y; y++)
        //                 {
        //                     Cell cell = new Cell(x, y);
        //                     double value = openSimplexNoise.Evaluate(x * settings.noiseScale, y * settings.noiseScale);
        //
        //                     if (value < threshold)
        //                     {
        //                         Debug.Log("added to indoors");
        //                         cell.indoors = true;
        //                     }
        //                     else
        //                     {
        //                         Debug.Log("added to outdoors");
        //                         cell.indoors = false;
        //                     }
        //
        //                     cellMap[x, y] = cell;
        //                 }
        //             }
        //
        //             break;
        //
        //         case NoiseType.Perlin:
        //
        //             float seedOffset = settings.GetSeed().GetHashCode() / settings.seedScale;
        //
        //             threshold = Mathf.Lerp(0.0f, 1.0f, (float)settings.thresholdPercentage / 100);
        //
        //             for (int x = 0; x < resolution.x; x++)
        //             {
        //                 for (int y = 0; y < resolution.y; y++)
        //                 {
        //                     var sampleX = (x + seedOffset) * settings.noiseScale;
        //                     var sampleY = (y + seedOffset) * settings.noiseScale;
        //
        //                     Cell cell = new Cell(x, y);
        //                     float value = Mathf.PerlinNoise(sampleX, sampleY);
        //
        //                     if (value < threshold)
        //                     {
        //                         cell.indoors = true;
        //                     }
        //                     else
        //                     {
        //                         cell.indoors = false;
        //                     }
        //
        //                     cellMap[x, y] = cell;
        //                 }
        //             }
        //
        //             break;
        //     }
        //
        //     return cellMap;
        // }

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

                    noiseValue = (float) openSimplexNoise.Evaluate(x * settings.noiseScale, y * settings.noiseScale);
                    break;
            }

            return noiseValue < threshold ? 1 : 0;
        }

        /**
         * Smooth the cellMap
         */
        private static Cell[,] SmoothCellMap(Cell[,] cellMap, int smoothSteps, Vector2Int resolution)
        {
            if (cellMap == null)
            {
                Debug.LogError("CellMap to smooth equals null.");
                return null;
            }

            for (int i = 0; i < smoothSteps; i++)
            {
                var xDimension = cellMap.GetLength(0);
                var yDimension = cellMap.GetLength(1);

                Cell[,] tempCellMap = new Cell[xDimension, yDimension];

                for (int x = 0; x < xDimension; x++)
                {
                    for (int y = 0; y < yDimension; y++)
                    {
                        tempCellMap[x, y] = ApplyFloorRules(cellMap, cellMap[x, y], resolution);
                    }
                }

                cellMap = tempCellMap;
            }


            GetNeighbours(cellMap);

            return cellMap;
        }


        /**
         * Apply rules to the cell
         */
        private static Cell ApplyFloorRules(Cell[,] cellMap, Cell cell, Vector2Int resolution)
        {
            Cell tempCell = cell;

            int neighbours = GetSimilarNeighbours(cellMap, cell, resolution);

            if (neighbours >= 4)
            {
                tempCell.Indoors = cell.Indoors == true;
            }
            else
            {
                tempCell.Indoors = cell.Indoors != true;
            }

            cell = tempCell;
            return cell;
        }

        /**
         * Get the number of similar neighbours
         */
        private static int GetSimilarNeighbours(Cell[,] cellMap, Cell tempCell, Vector2Int resolution)
        {
            bool myVal = tempCell.Indoors;
            int similarNeighbourCount = 0;

            // get the coordinates of all 8 neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int xPos = tempCell.CellIndex.x + x;
                    int yPos = tempCell.CellIndex.y + y;

                    // skip the incoming cell, and cell coordinates that are not in the map
                    if ((xPos == tempCell.CellIndex.x && yPos == tempCell.CellIndex.y) || xPos < 0 || yPos < 0 ||
                        xPos >= resolution.x || yPos >= resolution.y)
                    {
                        continue;
                    }

                    bool neighbourVal = cellMap[xPos, yPos].Indoors;

                    if (neighbourVal == myVal)
                    {
                        similarNeighbourCount++;
                    }

                    // cell.neighbours.Add(_cellMap[xPos, yPos]);
                }
            }

            // outCell = cell;
            return similarNeighbourCount;
        }

        private static void GetNeighbours(Cell[,] cellMap)
        {
            for (int x = 0; x < cellMap.GetLength(0); x++)
            {
                for (int y = 0; y < cellMap.GetLength(1); y++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int xPos = cellMap[x, y].CellIndex.x + i;
                            int yPos = cellMap[x, y].CellIndex.y + j;

                            // skip the incoming cell, and cell coordinates that are not in the map
                            if ((xPos == cellMap[x, y].CellIndex.x && yPos == cellMap[x, y].CellIndex.y) || xPos < 0 ||
                                yPos < 0 ||
                                xPos >= cellMap.GetLength(0) || yPos >= cellMap.GetLength(1))
                            {
                                continue;
                            }

                            cellMap[x, y].Neighbours.Add(cellMap[xPos, yPos]);
                        }
                    }
                }
            }
        }
    }
}