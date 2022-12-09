using _Scripts.Helper;
using UnityEngine;

namespace _Scripts.old
{
    public class MapGenerator : MonoBehaviour
    {
        // [SerializeField] private int width;
        // [SerializeField] private int height;
        [SerializeField] private Vector2Int resolution = new Vector2Int(256, 144);

        [SerializeField] private int smoothSteps = 5;
        [Range(0, 100)] [SerializeField] private int indoorsPercentage = 35;

        // private properties
        private int[,] _valueMap;
        private MapDisplay _display;
        private Cell[,] _cells;

        void Start()
        {
            // // init MapDisplay
            var mapPos = transform.position;
            _display = new MapDisplay(mapPos, resolution, gameObject);

            // generate valueMap
            _valueMap = GenerateMap();
            SmoothMap();
            _display.UpdateMapDisplay(_valueMap);

            // init cell array
            _cells = new Cell[resolution.x, resolution.y];

            // init random generator
            System.Random pRandom = new System.Random();

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    // generate new cell
                    Cell cell = new Cell(x, y);
                    cell.cellIndex = new Vector2Int(x, y);

                    // determine if cell is in- or outdoors
                    cell.indoors = _valueMap[x, y] == 1;

                    // add cell to cells
                    _cells[x, y] = cell;
                }
            }
        }

        void Update()
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     SmoothMap();
            //
            //     _display.UpdateMapDisplay(_valueMap);
            // }
        }

        private int[,] GenerateMap()
        {
            // init result array
            int[,] result = new int[resolution.x, resolution.y];
            System.Random prng = new System.Random();

            // for (int x = 0; x < resolution.x; x++)
            // {
            //     for (int y = 0; y < resolution.y; y++)
            //     {
            //         if (x == 0 || x == resolution.x - 1 || y == 0 || y == resolution.y - 1)
            //         {
            //             result[x, y] = 1; // Wall
            //         }
            //         else
            //         {
            //             // Next(n) -> value between inclusive 0 and exclusive n
            //             result[x, y] = prng.Next(101) < indoorsPercentage ? 1 : 0;
            //         }
            //     }
            // }

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

        private int ApplyRules(int xIndex, int yIndex)
        {
            var neighbours = GetSurroundingWallCount(xIndex, yIndex);

            if (neighbours > 4)
            {
                return 1; // Wall
            }

            if (neighbours < 4)
            {
                return 0; // Empty tile
            }

            return _valueMap[xIndex, yIndex];
        }

        private int GetSurroundingWallCount(int xIndex, int yIndex)
        {
            int wallCount = 0;

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
                    if (_valueMap[xPos, yPos] == 1) wallCount++;
                }
            }

            return wallCount;
        }
    }
}