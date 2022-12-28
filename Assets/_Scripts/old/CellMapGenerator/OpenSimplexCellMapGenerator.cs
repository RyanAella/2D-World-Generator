using _Scripts._GradientNoise.OpenSimplex;
using _Scripts.CellGeneration;
using _Scripts.Helper;
using UnityEngine;

namespace _Scripts.old.CellMapGenerator
{
    public class OpenSimplexCellMapGenerator
    {
        // private properties
        private Cell[,] _cellMap;

        /**
         * Generate a CellMap
         */
        public Cell[,] GenerateCellMap(Vector2Int resolution, bool useRandomSeed, float noiseScale, string seed,
            int indoorsPercentage)
        {
            _cellMap = new Cell[resolution.x, resolution.y];

            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }

            OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise(seed.GetHashCode());

            float threshold = Mathf.Lerp(-1.0f, 1.0f, (float)indoorsPercentage / 100);

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Cell cell = new Cell(x, y);
                    double value = openSimplexNoise.Evaluate(x * noiseScale, y * noiseScale);

                    if (value < threshold)
                    {
                        cell.indoors = true;
                    }
                    else
                    {
                        cell.indoors = false;
                    }

                    _cellMap[x, y] = cell;
                }
            }


            return _cellMap;
        }
    }
}