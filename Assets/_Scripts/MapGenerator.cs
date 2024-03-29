using _Scripts._GradientNoise;
using _Scripts.CellGeneration;
using _Scripts.ScriptableObjects.AssetGenerationSettings;
using _Scripts.ScriptableObjects.ValueGenerationSettings;
using _Scripts.TilemapGeneration;
using UnityEngine;

namespace _Scripts
{
    /**
     * This class controls the entire generation process.
     * All required parameters are collected in it and passed to the corresponding methods.
     */
    public class MapGenerator : MonoBehaviour
    {
        // Resolution: default 16:9
        [SerializeField] private Vector2Int resolution = new(128, 72);

        // Script access
        private CellMapGenerator _cellMapGenerator;
        [SerializeField] private TilemapGenerator tilemapGenerator;

        [Header("Settings for each Layer/Tilemap")]
        // Settings for the base layer determining if a tile is in or outdoors
        [Tooltip("What percentage is indoors.")]
        [SerializeField] private ValueGenerationSettings baseLayerSettings;
        
        // Settings for determining if an indoor tile is massive rock or a cavity
        [Tooltip("What percentage is massive rock.")]
        [SerializeField] private ValueGenerationSettings mountainLayerSettings;
        
        // Settings for determining if an outdoor tile is meadows or woods
        [Tooltip("What percentage is meadows.")]
        [SerializeField] private ValueGenerationSettings outdoorBiomSettings;
        
        // Settings for determining if a meadows tile is water
        [Tooltip("What percentage is water.")]
        [SerializeField] private ValueGenerationSettings waterLayerSettings;
        
        [Header("Settings for Meadows/Woods Assets")]
        // Settings for determining how many percent of meadows are trees, bushes and gras
        [SerializeField] private AssetGenerationSettings meadowsAssetSettings;
        
        // Settings for determining how many percent of woods are trees, bushes and gras
        [SerializeField] private AssetGenerationSettings woodsAssetSettings;

        // For the use of OnValidate()
        private bool _scriptLoaded;

        // For Debugging
        // private CellDebugger _debugger;

        void Start()
        {
            // Initialization
            _cellMapGenerator = new CellMapGenerator();
            tilemapGenerator.Setup(gameObject);

            // Cell map generation
            Cell[,] cellMap = _cellMapGenerator.GenerateCellMap(resolution, baseLayerSettings,
                mountainLayerSettings, outdoorBiomSettings, waterLayerSettings, meadowsAssetSettings, woodsAssetSettings);

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
            _cellMapGenerator = new CellMapGenerator();
            tilemapGenerator.Setup(gameObject);

            // Cell map generation
            Cell[,] cellMap = _cellMapGenerator.GenerateCellMap(resolution, baseLayerSettings,
                mountainLayerSettings, outdoorBiomSettings, waterLayerSettings, meadowsAssetSettings, woodsAssetSettings);
            
            // Tilemap generation
            tilemapGenerator.GenerateTilemap(cellMap);

            // For Debugging
            // _debugger = new CellDebugger(randomGenerator);
            // _debugger.PlotNeighbours(_cellMap[0,0]);
        }
    }
}