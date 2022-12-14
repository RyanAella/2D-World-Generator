using _Scripts.Helper;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts
{
    public class MapGenerator : MonoBehaviour
    {
        public enum NoiseType
        {
            PseudoRandom,
            OpenSimplex,
            Perlin,
            // Voronoise
        }

        //resolution default 16:9
        [SerializeField] private Vector2Int resolution = new(128, 72);

        [SerializeField] private NoiseType noiseType;

        [Header("System Random")] [SerializeField]
        private string seed = "Dennis";

        [SerializeField] private bool useRandomSeed;
        [Range(0, 100)] [SerializeField] private int indoorsPercentage = 49;
        [SerializeField] private int smoothSteps = 7;

        [Header("Noise")] [Range(0.0f, 1.0f)] [SerializeField]
        private float noiseScale = 0.033f;

        [Header("Map Generation")] [SerializeField]
        private GameObject randomGenerator;

        [SerializeField] private Tilemap randomFloorTilemap;
        [SerializeField] private GameObject openSimplexGenerator;
        [SerializeField] private Tilemap openFloorTilemap;
        [SerializeField] private GameObject perlinGenerator;
        [SerializeField] private Tilemap perlinFloorTilemap;

        private PseudoRandomCellMapGenerator _randomCellMapGenerator;
        private OpenSimplexCellMapGenerator _openSimplexCellMapGenerator;
        private PerlinCellMapGenerator _perlinCellMapGenerator;

        // private CellDebugger _debugger;
        private Cell[,] _cellMap;
        private TilemapGenerator _tilemapGenerator;

        private bool _scriptLoaded = false;

        void Start()
        {
            _tilemapGenerator = new TilemapGenerator();

            switch (noiseType)
            {
                case NoiseType.PseudoRandom:
                    randomGenerator.SetActive(true);
                    _randomCellMapGenerator = new PseudoRandomCellMapGenerator();

                    _cellMap = _randomCellMapGenerator.GenerateCellMap(resolution, useRandomSeed, seed, indoorsPercentage);
                    _cellMap = _randomCellMapGenerator.SmoothCellMap(_cellMap, smoothSteps, resolution);

                    _tilemapGenerator.GenerateTilemap(_cellMap, randomFloorTilemap);
                    break;
                case NoiseType.OpenSimplex:
                    openSimplexGenerator.SetActive(true);
                    _openSimplexCellMapGenerator = new OpenSimplexCellMapGenerator();

                    _cellMap = _openSimplexCellMapGenerator.GenerateCellMap(resolution, useRandomSeed, noiseType, noiseScale, seed, indoorsPercentage);

                    _tilemapGenerator.GenerateTilemap(_cellMap, openFloorTilemap);
                    break;
                case NoiseType.Perlin:
                    perlinGenerator.SetActive(true);
                    _perlinCellMapGenerator = new PerlinCellMapGenerator();

                    _cellMap = _perlinCellMapGenerator.GenerateCellMap(resolution, useRandomSeed, noiseType, noiseScale, seed, indoorsPercentage);

                    _tilemapGenerator.GenerateTilemap(_cellMap, perlinFloorTilemap);
                    break;
            }

            // _debugger = new CellDebugger(randomGenerator);
            // _debugger.PlotNeighbours(_cellMap[126, 70]);
            // _debugger.PlotNeighbours(_cellMap[15,8]);

            _scriptLoaded = true;
        }

        private void OnValidate()
        {
            if (!_scriptLoaded) return;
            
            randomGenerator.SetActive(false);
            openSimplexGenerator.SetActive(false);
            perlinGenerator.SetActive(false);

            _tilemapGenerator = new TilemapGenerator();
                
            switch (noiseType)
            {
                case NoiseType.PseudoRandom:
                    randomGenerator.SetActive(true);
                    _randomCellMapGenerator = new PseudoRandomCellMapGenerator();

                    _cellMap = _randomCellMapGenerator.GenerateCellMap(resolution, useRandomSeed, seed, indoorsPercentage);
                    _cellMap = _randomCellMapGenerator.SmoothCellMap(_cellMap, smoothSteps, resolution);

                    _tilemapGenerator.GenerateTilemap(_cellMap, randomFloorTilemap);
                    break;
                case NoiseType.OpenSimplex:
                    openSimplexGenerator.SetActive(true);
                    _openSimplexCellMapGenerator = new OpenSimplexCellMapGenerator();

                    _cellMap = _openSimplexCellMapGenerator.GenerateCellMap(resolution, useRandomSeed, noiseType, noiseScale, seed, indoorsPercentage);

                    _tilemapGenerator.GenerateTilemap(_cellMap, openFloorTilemap);
                    break;
                case NoiseType.Perlin:
                    perlinGenerator.SetActive(true);
                    _perlinCellMapGenerator = new PerlinCellMapGenerator();

                    _cellMap = _perlinCellMapGenerator.GenerateCellMap(resolution, useRandomSeed, noiseType, noiseScale, seed, indoorsPercentage);

                    _tilemapGenerator.GenerateTilemap(_cellMap, perlinFloorTilemap);
                    break;
            }


            // _debugger = new CellDebugger(gameObject);
            // _debugger.PlotNeighbours(_cellMap[4, 0]);
        }
    }
}