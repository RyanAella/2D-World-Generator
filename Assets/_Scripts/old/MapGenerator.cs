using _Scripts.CellGeneration;
using _Scripts.Helper;
using UnityEngine;

namespace _Scripts.old
{
    /**
     * This class controls the entire generation process.
     * All required parameters are collected in it and passed to the corresponding methods.
     */
    public class MapGenerator : MonoBehaviour
    {
        // Resolution
        [SerializeField] private Vector2Int resolution = new Vector2Int(256, 144);

        [SerializeField] private int smoothSteps = 7;
        [Range(0, 100)] [SerializeField] private int indoorsPercentage = 35;

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
            _valueMap = GenerateMap();
            SmoothMap();
            _display.UpdateMapDisplay(_valueMap);

            // Initialize cell array
            _cells = new Cell[resolution.x, resolution.y];

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    // Generate new cell
                    Cell cell = new Cell(x, y);

                    // Determine if the cell is in- or outdoors
                    cell.Indoors = _valueMap[x, y] == 1;

                    // Add the cell to cells
                    _cells[x, y] = cell;
                }
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SmoothMap();

                _display.UpdateMapDisplay(_valueMap);
            }
        }

        /**
         * Generate the map.
         */
        private int[,] GenerateMap()
        {
            // Initialize result array
            int[,] result = new int[resolution.x, resolution.y];
            System.Random prng = new System.Random();

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    // Next(n) -> value between inclusive 0 and exclusive n
                    result[x, y] = prng.Next(101) < indoorsPercentage ? 1 : 0;
                }
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
            var neighbours = GetSimilarNeighboursCount(xIndex, yIndex);

            switch (neighbours)
            {
                case > 4:
                    return 1; // Wall
                case < 4:
                    return 0; // Empty tile
                default:
                    return _valueMap[xIndex, yIndex];
            }
        }

        /**
         * Get the number of similar neighbours.
         */
        private int GetSimilarNeighboursCount(int xIndex, int yIndex)
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

                    // increment neighbours if wall
                    if (_valueMap[xPos, yPos] == 1) neighbours++;
                }
            }

            return neighbours;
        }
    }
}