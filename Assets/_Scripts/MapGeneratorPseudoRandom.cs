using _Scripts._PseudoRandom;
using _Scripts.CellGeneration;
using _Scripts.TilemapGeneration;
using UnityEngine;

namespace _Scripts
{
    /**
     * This class controls the entire generation process.
     * All required parameters are collected in it and passed to the corresponding methods.
     */
    public class MapGeneratorPseudoRandom : MonoBehaviour
    {
        // Resolution: default 16:9
        [SerializeField] private Vector2Int resolution = new(128, 72);

        // Script access
        private CellMapGeneratorPseudoRandom _cellMapGeneratorPseudoRandom;
        [SerializeField] private TilemapGenerator tilemapGenerator;

        // Settings for the base layer determining if a tile is in or outdoors
        [SerializeField] private MapGenerationSettings baseLayerSettings;

        // Settings for determining if an indoor tile is massive rock or a cavity
        [SerializeField] private MapGenerationSettings mountainLayerSettings;

        // Settings for determining if an outdoor tile is meadows or woods
        [SerializeField] private MapGenerationSettings outdoorBiomSettings;

        // Settings for determining how many percent of woods are trees, bushes and gras
        [SerializeField] private AssetGenerationSettings assetGenerationSettings;

        // For the use of OnValidate()
        private bool _scriptLoaded;

        // For Debugging
        // private CellDebugger _debugger;

        void Start()
        {
            // Initialization
            _cellMapGeneratorPseudoRandom = new CellMapGeneratorPseudoRandom();
            tilemapGenerator.Setup();

            // Cell map generation
            Cell[,] cellMap = _cellMapGeneratorPseudoRandom.GenerateCellMap(resolution, baseLayerSettings,
                mountainLayerSettings, outdoorBiomSettings, assetGenerationSettings);

            // Tilemap generation
            tilemapGenerator.GenerateTilemap(cellMap);

            // For Debugging
            // _debugger = new CellDebugger(randomGenerator);
            // _debugger.PlotNeighbours(_cellMap[0,0]);

            _scriptLoaded = true;
        }

        private void OnValidate()
        {
            if (!_scriptLoaded) return;

            // Initialization
            _cellMapGeneratorPseudoRandom = new CellMapGeneratorPseudoRandom();
            tilemapGenerator.Setup();

            // Cell map generation
            Cell[,] cellMap = _cellMapGeneratorPseudoRandom.GenerateCellMap(resolution, baseLayerSettings,
                mountainLayerSettings, outdoorBiomSettings, assetGenerationSettings);

            // Tilemap generation
            tilemapGenerator.GenerateTilemap(cellMap);

            // For Debugging
            // _debugger = new CellDebugger(randomGenerator);
            // _debugger.PlotNeighbours(_cellMap[0,0]);
        }
    }
}