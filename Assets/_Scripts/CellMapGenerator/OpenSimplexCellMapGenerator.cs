using UnityEngine;

namespace _Scripts
{
    public class OpenSimplexCellMapGenerator
    {
        // private properties
        private Cell[,] _cellMap;

        /**
         * Generate a CellMap
         */
        public Cell[,] GenerateCellMap(Vector2Int resolution, bool useRandomSeed, MapGenerator.NoiseType noiseType,
            float noiseScale, string seed, int indoorsPercentage)
        {
            _cellMap = new Cell[resolution.x, resolution.y];

            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }

            OpenSimplexNoise openSimplexNoise = new OpenSimplexNoise(seed.GetHashCode());
            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Cell cell = new Cell(x, y);
                    double value = openSimplexNoise.Evaluate(x * noiseScale, y * noiseScale);

                    if (value < 0)
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