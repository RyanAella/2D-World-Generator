using _Scripts.Helper;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.old
{
    // [RequireComponent(typeof(Tilemap))]
    // [RequireComponent(typeof(TilemapRenderer))]
    public class TilemapGenerator : MonoBehaviour
    {
        // resolution default 16:9
        [SerializeField] private Vector2Int resolution = new Vector2Int(256, 144);
        [SerializeField] private int smoothSteps = 5;

        [Header("Base Generation")] [Range(0, 100)] [SerializeField]
        private int indoorsPercentage = 35;

        [Header("Tilemaps")] [SerializeField] private Tilemap floorTilemap;

        [SerializeField] private Tilemap wallTilemap;

        // private properties
        // private int[,] _valueMap;
        private Cell[,] _cells;

        // public properties
        [Header("Seed")] [SerializeField] private string seed;
        [SerializeField] private bool useRandomSeed;

        void Start()
        {
            // init cell debugger
            CellDebugger debugger = new CellDebugger(gameObject);

            _cells = new Cell[resolution.x, resolution.y];

            // generate base map
            /*_valueMap =*/ GenerateBaseMap();
            SmoothBaseMap();
            DrawFloorMap();

            // debugger.PlotNeighbours(_cells[11, 3]);
            debugger.PlotNeighbours(_cells[10, 3]);
            debugger.PlotNeighbours(_cells[3, 0]);
            // debugger.PlotNeighbours(_cells[4, 0]);

            // generate wall map
            GenerateWallMap();
        }

        /**
         * Create a Sprite
         */
        private Sprite CreateSprite(float pixelPerUnit = 1)
        {
            // create texture and rect for Sprite
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, -1, true);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);

            // create Sprite
            var sprite = Sprite.Create(texture, rect, Vector2.up, pixelPerUnit);

            return sprite;
        }

        /**
         * Generate the BaseMap, which contains information about the tile being in- or outdoors
         */
        private /*int[,]*/ void GenerateBaseMap()
        {
            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }

            System.Random prng = new System.Random(seed.GetHashCode());

            // init result array
            // int[,] result = new int[resolution.x, resolution.y];

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    var cell = new Cell(x, y);
                    
                    // Next(n) -> value between inclusive 0 and exclusive n
                    if (prng.Next(101) < indoorsPercentage)
                    {
                        // result[x, y] = 1;
                        cell.indoors = true;
                    }
                    else
                    {
                        // result[x, y] = 0;
                        cell.indoors = false;
                    }

                    _cells[x, y] = cell;
                }
            }

            // return result;
        }

        /**
         * Smooth the Base Map
         */
        private void SmoothBaseMap()
        {
            if (/*_valueMap*/ _cells == null) return;

            for (int i = 0; i < smoothSteps; i++)
            {
                var xDimension = _cells.GetLength(0);
                var yDimension = _cells.GetLength(1);
                
                Cell[,] tempCells = new Cell[xDimension, yDimension];

                for (int x = 0; x < xDimension; x++)
                {
                    for (int y = 0; y < yDimension; y++)
                    {
                        tempCells[x, y] = ApplyBaseRules(_cells[x, y]);
                    }
                }

                _cells = tempCells;
            }
        }

        /**
         * Apply the rules on the BaseMap
         */
        private /*int*/ Cell ApplyBaseRules(/*int xIndex, int yIndex*/ Cell cell)
        {
            Cell tempCell = cell;

            var neighbours = GetSimilarNeighbours(cell, out tempCell);

            // check if neighbours fits rule
            if (neighbours > 4)
            {
                tempCell.indoors = true;
                return tempCell;
            }

            if (neighbours < 4)
            {
                tempCell.indoors = false;
                return tempCell;
            }
            
            return tempCell;
        }

        /**
         * Get the number of similar neighbours and save the status (in-/outdoors) of the neighbours
         */
        private int GetSimilarNeighbours(Cell inCell, out Cell outCell)
        {
            Cell cell = new Cell(inCell.cellIndex);
            
            bool myVal = inCell.indoors;
            int similarNeighbours = 0;

            Debug.Log("");
            Debug.Log("My position: " + inCell.cellIndex.x + ", " + inCell.cellIndex.y);
            Debug.Log("MyVal: " + myVal);

            // clear cell.neighbours to rewrite them
            // cell.neighbours.Clear();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var xPos = inCell.cellIndex.x + x;
                    var yPos = inCell.cellIndex.y + y;

                    Debug.Log("Neighbour offset, x: " + x + ", y: " + y);
                    Debug.Log("Neighbour position, x: " + xPos + ", y: " + yPos);
                    
                    // skip x- and yIndex, as well as map border out of bounds
                    if ((xPos == inCell.cellIndex.x && yPos == inCell.cellIndex.y) || xPos < 0 || xPos >= resolution.x || yPos < 0 ||
                        yPos >= resolution.y)
                    {
                        Debug.Log("Skip position");
                        Debug.Log("----------------------------");
                        continue;
                    }

                    var neighbourVal = _cells[xPos, yPos].indoors;
                    
                    Debug.Log("Neighbour value: " + neighbourVal);
                    
                    // increment neighbours if similar
                    if (neighbourVal == myVal)
                    {
                        Debug.Log("Similar neighbour");
                        Debug.Log("----------------------------");
                        // add neighbour as true to dictionary
                        cell.neighbours.Add(_cells[xPos, yPos]);
                        similarNeighbours++;
                    }
                    else
                    {
                        Debug.Log("Different neighbour");
                        Debug.Log("----------------------------");
                        // add neighbour as false to dictionary
                        cell.neighbours.Add(_cells[xPos, yPos]);
                    }
                }
            }

            outCell = cell;
            return similarNeighbours;
        }

        /**
         * For Development: Set the BaseTilemap
         */
        void DrawFloorMap()
        {
            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    if (tempTile != null)
                    {
                        tempTile.sprite = CreateSprite();
                        if (_cells[x, y].indoors /*_valueMap[x, y] == 1*/ )
                        {
                            tempTile.sprite.texture.SetPixel(0, 0, new Color(0.7960785f, 0.7960785f, 0.7960785f, 1f));
                            tempTile.sprite.texture.Apply();
                        }
                        else
                        {
                            tempTile.sprite.texture.SetPixel(0, 0, new Color(0.3662f, 0.6415f, 0.1361f, 1f));
                            tempTile.sprite.texture.Apply();
                        }

                        floorTilemap.SetTile(new Vector3Int(x, y, 0), tempTile);
                    }
                }
            }
        }

        /**
         * Generate the IndoorWallMap
         */
        void GenerateWallMap()
        {
            // var xDimension = _cells.GetLength(0);
            // var yDimension = _cells.GetLength(1);
            // // Currently generate walls between in- and outdoor
            // for (int x = 0; x < xDimension; x++)
            // {
            //     for (int y = 0; y < yDimension; y++)
            //     {
            //         if (_cells[x, y].indoors && _cells[x, y].neighbours.ContainsValue(false)) // indoors
            //         {
            //             // Debug.Log(_cells[x, y].neighbours.Count);
            //             // foreach (KeyValuePair<Vector2Int, bool> kvp in _cells[x, y].neighbours)
            //             // {
            //             //     Debug.Log("Key = " + kvp.Key + ", Value = " + kvp.Value);
            //             // }
            //
            //             Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            //             tempTile.sprite = CreateSprite(1.5f);
            //             tempTile.sprite.texture.SetPixel(0, 0, Color.black);
            //             tempTile.sprite.texture.Apply();
            //
            //             wallTilemap.SetTile(new Vector3Int(x, y, 0), tempTile);
            //         }
            //     }
            // }

            // foreach (var cell in _cells)
            // {
            //     if (cell.indoors && cell.neighbours.ContainsValue(false)) // indoors
            //     {
            //         // Debug.Log(_cells[x, y].neighbours.Count);
            //         // foreach (KeyValuePair<Vector2Int, bool> kvp in _cells[x, y].neighbours)
            //         // {
            //         //     Debug.Log("Key = " + kvp.Key + ", Value = " + kvp.Value);
            //         // }
            //
            //         Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            //         tempTile.sprite = CreateSprite(1.5f);
            //         tempTile.sprite.texture.SetPixel(0, 0, Color.black);
            //         tempTile.sprite.texture.Apply();
            //
            //         wallTilemap.SetTile(new Vector3Int(cell.cellIndex.x, cell.cellIndex.y, 0), tempTile);
            //     }
            // }
        }
    }
}