using System.Collections.Generic;
using _Scripts.CellGeneration;
using _Scripts.TilemapGeneration;
using _Scripts.ValueGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts
{
    public class MapGenerator : MonoBehaviour
    {
        //resolution default 16:9
        [SerializeField] private Vector2Int resolution = new(128, 72);

        // private CellDebugger _debugger;
        private Cell[,] _cellMap;
        private List<Cell> _indoorCells;
        private List<Cell> _outdoorCells;
        
        [SerializeField] private TilemapGenerator _tilemapGenerator;

        private bool _scriptLoaded = false;

        // Settings for the layer determining if a tile is in or outdoors
        [SerializeField] private ValueGenerationSettings BaseLayerSettings;
        
        // Settings for determining if an indoor tile is massive rock or a cavity
        [SerializeField] private ValueGenerationSettings MountainLayerSettings;

        // Settings for determining if an outdoor tile is meadows or woods
        [SerializeField] private ValueGenerationSettings OutdoorBiomeSettings;
        
        void Start()
        {
            // initialization
            _tilemapGenerator.Setup();
            _cellMap = new Cell[resolution.x, resolution.y];
            _indoorCells = new List<Cell>();
            _outdoorCells = new List<Cell>();

            // Generate CellMap for indoor/outdoor
            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Cell cell = new Cell();
                    cell.CellIndex = new Vector2Int(x, y);

                    var val = ValueGenerator.Evaluate(x, y, BaseLayerSettings);

                    if (val == 1)
                    {
                        cell.Indoors = true;
                        _indoorCells.Add(cell);
                    }
                    else
                    {
                        cell.Indoors = false;
                        _outdoorCells.Add(cell);
                    }

                    _cellMap[x, y] = cell;
                }
            }
            
            // // Get the values of the neighbours
            // foreach (var cell in _cellMap)
            // {
            //     // get the coordinates of all 8 neighbours
            //     for (int x = -1; x <= 1; x++)
            //     {
            //         for (int y = -1; y <= 1; y++)
            //         {
            //             int xPos = cell.CellIndex.x + x;
            //             int yPos = cell.CellIndex.y + y;
            //
            //             // skip the incoming cell, and cell coordinates that are not in the map
            //             if ((xPos == cell.CellIndex.x && yPos == cell.CellIndex.y) || xPos < 0 || yPos < 0 ||
            //                 xPos >= resolution.x || yPos >= resolution.y)
            //             {
            //                 continue;
            //             }
            //
            //             bool neighbourVal = _cellMap[xPos, yPos].Indoors;
            //
            //             if (neighbourVal != cell.Indoors)
            //             {
            //                 // cell.isWall = true;
            //             }
            //
            //             cell.Neighbours.Add(_cellMap[xPos, yPos]);
            //         }
            //     }
            // }
            
            // mountain layer generation
            foreach (var cell in _indoorCells)
            {
                var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, MountainLayerSettings);

                if (val == 1)
                {
                    // cell is massive rock
                    cell.Asset = new CellAsset();
                    cell.Asset.Asset = CellAsset.AssetType.Rock;
                    cell.Asset.Collidable = true;
                }
                else
                {
                    // cell is a cavity
                    cell.Biom = Biom.Cave;
                    cell.Asset.Asset = CellAsset.AssetType.None;
                    cell.Asset.Collidable = false;
                }
            }

            // tilemap generation
            _tilemapGenerator.GenerateTilemap(_cellMap);

            // _debugger = new CellDebugger(randomGenerator);
            // _debugger.PlotNeighbours(_cellMap[126, 70]);
            // _debugger.PlotNeighbours(_cellMap[15,8]);

            _scriptLoaded = true;
        }

        // private void OnValidate()
        // {
        //     if (!_scriptLoaded) return;
        //
        //     // initialization
        //     _tilemapGenerator = new TilemapGenerator.TilemapGenerator();
        //     _cellMap = new Cell[resolution.x, resolution.y];
        //     _indoorCells = new List<Cell>();
        //     _outdoorCells = new List<Cell>();
        //
        //     // value map generation
        //     _cellMap = ValueGenerator.GenerateValueMap(resolution, BaseLayerSettings);
        //
        //     // maybe for x in for y
        //     foreach (var cell in _cellMap)
        //     {
        //         if (cell.indoors)
        //         {
        //             _indoorCells.Add(cell);
        //         }
        //         else
        //         {
        //             _outdoorCells.Add(cell);
        //         }
        //     }
        //
        //     // tilemap generation
        //
        //     floorTilemap.SetActive(true);
        //     _tilemapGenerator.GenerateTilemap(_cellMap, (floorTilemap.GetComponent(typeof(Tilemap)) as Tilemap));
        //
        //     // _debugger = new CellDebugger(gameObject);
        //     // _debugger.PlotNeighbours(_cellMap[4, 0]);
        // }
    }
}