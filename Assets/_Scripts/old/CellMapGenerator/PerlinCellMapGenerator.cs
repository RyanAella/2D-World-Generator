using System.Collections.Generic;
using _Scripts.CellGeneration;
using _Scripts.Helper;
using UnityEngine;

namespace _Scripts.old.CellMapGenerator
{
    public class PerlinCellMapGenerator
    {
        // private properties
        private Cell[,] _cellMap;
        private List<Cell> _outdoorMap;
        private List<Cell> _indoorMap;

        /**
         * Generate a CellMap
         */
        public Cell[,] GenerateCellMap(Vector2Int resolution, bool useRandomSeed, float noiseScale, string seed,
            float seedScale, int indoorsPercentage)
        {
            _cellMap = new Cell[resolution.x, resolution.y];

            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }

            float seedOffset = seed.GetHashCode() / seedScale;

            float threshold = Mathf.Lerp(0.0f, 1.0f, (float)indoorsPercentage / 100);

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    var sampleX = (x + seedOffset) * noiseScale;
                    var sampleY = (y + seedOffset) * noiseScale;

                    Cell cell = new Cell(x, y);
                    float value = Mathf.PerlinNoise(sampleX, sampleY);

                    if (value < threshold)
                    {
                        cell.Indoors = true;
                        _indoorMap.Add(cell);
                    }
                    else
                    {
                        cell.Indoors = false;
                        _outdoorMap.Add(cell);
                    }

                    _cellMap[x, y] = cell;
                }
            }

            return _cellMap;
        }
    }
}