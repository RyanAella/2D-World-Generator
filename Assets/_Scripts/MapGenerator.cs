using _Scripts._GradientNoise;
using _Scripts._GradientNoise.TilemapGeneration;
using _Scripts._GradientNoise.ValueGeneration;
using _Scripts.CellGeneration;
using UnityEngine;

namespace _Scripts
{
    /**
     * This class controls the entire generation process.
     * All required parameters are collected in it and passed to the corresponding methods.
     */
    public class MapGenerator : MonoBehaviour
    {
        //resolution default 16:9
        [SerializeField] private Vector2Int resolution = new(128, 72);

        private CellMapGenerator _cellMapGenerator;
        [SerializeField] private TilemapGenerator tilemapGenerator;

        // Settings for the layer determining if a tile is in or outdoors
        [SerializeField] private ValueGenerationSettings baseLayerSettings;

        // Settings for determining if an indoor tile is massive rock or a cavity
        [SerializeField] private ValueGenerationSettings mountainLayerSettings;

        // Settings for determining if an outdoor tile is meadows or woods
        [SerializeField] private ValueGenerationSettings outdoorBiomSettings;
        
        // Settings for determining how many percent of woods are trees, bushes and gras
        [SerializeField] private AssetGenerationSettings assetGenerationSettings;
        
        // Settings for 
        // [SerializeField] private CellGenerationSettings cellGenerationSettings;
        
        // For the use of OnValidate()
        private bool _scriptLoaded = false;
        
        // [SerializeField] private Cell cell;

        // For Debugging
        // private CellDebugger _debugger;

        void Start()
        {
            // initialization
            _cellMapGenerator = new CellMapGenerator();
            tilemapGenerator.Setup();

            // cell map generation
            Cell[,] cellMap = _cellMapGenerator.GenerateCellMap(resolution, baseLayerSettings,
                mountainLayerSettings, outdoorBiomSettings, assetGenerationSettings);

            // tilemap generation
            tilemapGenerator.GenerateTilemap(cellMap);

            // For Debugging
            // _debugger = new CellDebugger(randomGenerator);
            // _debugger.PlotNeighbours(_cellMap[0,0]);

            _scriptLoaded = true;
        }

        private void OnValidate()
        {
            if (!_scriptLoaded) return;

            // initialization
            _cellMapGenerator = new CellMapGenerator();
            tilemapGenerator.Setup();

            // cell map generation
            Cell[,] cellMap = _cellMapGenerator.GenerateCellMap(resolution, baseLayerSettings, mountainLayerSettings,
                outdoorBiomSettings, assetGenerationSettings);

            // tilemap generation
            tilemapGenerator.GenerateTilemap(cellMap);

            // For Debugging
            // _debugger = new CellDebugger(randomGenerator);
            // _debugger.PlotNeighbours(_cellMap[0,0]);
        }
    }
}