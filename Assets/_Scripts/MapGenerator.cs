using _Scripts.Helper;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts
{
    public class MapGenerator : MonoBehaviour
    {
        //resolution default 16:9
        [SerializeField] private Vector2Int resolution = new Vector2Int(128, 72);

        [Header("Base Generation")] [Range(0, 100)] [SerializeField]
        private int indoorsPercentage = 35;

        [SerializeField] private int smoothSteps = 5;

        // public properties
        [Header("Seed")] [SerializeField] private string seed;
        [SerializeField] private bool useRandomSeed;
        
        [Header("Tilemaps")] 
        [SerializeField] private Tilemap floorTilemap;
        [SerializeField] private Tilemap newfloorTilemap;

        void Start()
        {
            CellMapGenerator cellMapGenerator = new CellMapGenerator();
            Cell[,] cellMap = cellMapGenerator.GenerateCellMap(resolution, useRandomSeed, seed, indoorsPercentage);
            Cell[,] newCellMap = cellMapGenerator.SmoothCellMap(cellMap, smoothSteps, resolution);
            
            TilemapGenerator tilemapGenerator = new TilemapGenerator();
            tilemapGenerator.GenerateTilemap(cellMap, floorTilemap);
            tilemapGenerator.GenerateTilemap(newCellMap, newfloorTilemap);
            
            CellDebugger debugger = new CellDebugger(gameObject);
            debugger.PlotNeighbours(newCellMap[4, 0]);
        }
    }
}
