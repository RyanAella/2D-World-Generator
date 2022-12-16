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

        private CellMapGenerator _cellMapGenerator;
        [SerializeField] private TilemapGenerator tilemapGenerator;

        // private bool _scriptLoaded = false;

        // Settings for the layer determining if a tile is in or outdoors
        [SerializeField] private ValueGenerationSettings baseLayerSettings;
        
        // Settings for determining if an indoor tile is massive rock or a cavity
        [SerializeField] private ValueGenerationSettings mountainLayerSettings;

        // Settings for determining if an outdoor tile is meadows or woods
        [SerializeField] private ValueGenerationSettings outdoorBiomSettings;
        
        void Start()
        {
            // initialization
            _cellMapGenerator = new CellMapGenerator();
            tilemapGenerator.Setup();

            // cell map generation
            Cell[,] cellMap = _cellMapGenerator.GenerateCellMap(resolution, baseLayerSettings, mountainLayerSettings, outdoorBiomSettings);

            // tilemap generation
            tilemapGenerator.GenerateTilemap(cellMap);

            // _debugger = new CellDebugger(randomGenerator);
            // _debugger.PlotNeighbours(_cellMap[126, 70]);
            // _debugger.PlotNeighbours(_cellMap[15,8]);

            // _scriptLoaded = true;
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