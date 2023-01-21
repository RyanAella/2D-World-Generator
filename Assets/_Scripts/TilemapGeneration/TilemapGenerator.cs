using System;
using System.Collections.Generic;
using _Scripts.CellGeneration;
using _Scripts.ScriptableObjects.TilePalettes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.TilemapGeneration
{
    /**
     * This class generates the tilemaps.
     */
    [Serializable]
    public class TilemapGenerator
    {
        [Header("Layer Prefabs")]
        // Floor base level tilemap showing biom color
        [SerializeField] private GameObject biomLayerPrefab;
        private GameObject _biomLayer;
        private Tilemap _biomTilemap;
        
        // Tilemap containing massive rock and walls
        [SerializeField] private GameObject mountainLayerPrefab;
        private GameObject _mountainLayer;
        private Tilemap _mountainTilemap;

        // Tilemap containing trees
        [SerializeField] private GameObject treeLayerPrefab;
        private GameObject _treeLayer;
        private Tilemap _treeTilemap;
        
        // Tilemap containing bushes
        [SerializeField] private GameObject bushLayerPrefab;
        private GameObject _bushLayer;
        private Tilemap _bushTilemap;
        
        // Tilemap containing grass
        [SerializeField] private GameObject grassLayerPrefab;
        private GameObject _grassLayer;
        private Tilemap _grassTilemap;
        
        // Tilemap containing stones
        [SerializeField] private GameObject stoneLayerPrefab;
        private GameObject _stoneLayer;
        private Tilemap _stoneTilemap;
        
        // Tilemap containing water
        [SerializeField] private GameObject waterLayerPrefab;
        private GameObject _waterLayer;
        private Tilemap _waterTilemap;
        
        // List containing all GameObjects
        private List<GameObject> _gameObjects;

        // List containing all Tilemaps
        private List<Tilemap> _tilemaps;

        // ScriptableObject lists containing the tiles for each tilemap
        [Header("TilePalettes (ScriptableObjects)")]
        [SerializeField] public TilePaletteScriptableObject mountainPalette;
        [SerializeField] public TilePaletteScriptableObject meadowsPalette;
        [SerializeField] public TilePaletteScriptableObject woodsPalette;
        [SerializeField] public TilePaletteScriptableObject massiveRockPalette;
        [SerializeField] public TilePaletteScriptableObject wallPalette;
        [SerializeField] public TilePaletteScriptableObject treePalette;
        [SerializeField] public TilePaletteScriptableObject bushPalette;
        [SerializeField] public TilePaletteScriptableObject grassPalette;
        [SerializeField] public TilePaletteScriptableObject stonePalette;
        [SerializeField] public TilePaletteScriptableObject waterPalette;

        // Dictionary containing all ScriptableObject lists
        private Dictionary<string, TilePaletteScriptableObject> _tilePalettes;

        public void Setup(GameObject tilemapParent)
        {
            _biomLayer = GameObject.Instantiate(biomLayerPrefab, tilemapParent.transform);
            _mountainLayer = GameObject.Instantiate(mountainLayerPrefab, tilemapParent.transform);
            _treeLayer = GameObject.Instantiate(treeLayerPrefab, tilemapParent.transform);
            _bushLayer = GameObject.Instantiate(bushLayerPrefab, tilemapParent.transform);
            _grassLayer = GameObject.Instantiate(grassLayerPrefab, tilemapParent.transform);
            _stoneLayer = GameObject.Instantiate(stoneLayerPrefab, tilemapParent.transform);
            _waterLayer = GameObject.Instantiate(waterLayerPrefab, tilemapParent.transform);
            
            // Creating the List
            _gameObjects = new List<GameObject>()
            {
                _biomLayer,
                _mountainLayer,
                _treeLayer,
                _bushLayer,
                _grassLayer,
                _stoneLayer,
                _waterLayer
            };

            // Getting the tilemap components
            _biomTilemap = _biomLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _mountainTilemap = _mountainLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _treeTilemap = _treeLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _bushTilemap = _bushLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _grassTilemap = _grassLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _stoneTilemap = _stoneLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _waterTilemap = _waterLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            
            // Creating the List
            _tilemaps = new List<Tilemap>()
            {
                _biomTilemap,
                _mountainTilemap,
                _treeTilemap,
                _bushTilemap,
                _grassTilemap,
                _stoneTilemap,
                _waterTilemap
            };

            // Creating the Dictionary
            _tilePalettes = new Dictionary<string, TilePaletteScriptableObject>
            {
                { "Mountain", mountainPalette },
                { "Woods", woodsPalette },
                { "Meadows", meadowsPalette },
                { "MassiveRock", massiveRockPalette },
                { "Wall", wallPalette },
                { "Tree", treePalette },
                { "Bush", bushPalette },
                { "Grass", grassPalette },
                { "Stone", stonePalette },
                { "Water", waterPalette}
            };
        }

        /**
         * Generate the Tilemap.
         */
        public void GenerateTilemap(Cell[,] cellMap)
        {
            // Clear all tilemaps
            foreach (var tilemap in _tilemaps)
            {
                tilemap.ClearAllTiles();
            }
            
            foreach (var cell in cellMap)
            {
                // generate the tiles for each cell
                cell.GenerateTiles(_tilePalettes);
                
                // Add the tiles to the tilemaps at cellIndex position
                foreach (var tile in cell.Tiles)
                {
                    switch (tile.Key)
                    {
                        case Cell.TilemapTypes.BiomLayer:
                            _biomTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tile.Value);
                            break;
                        case Cell.TilemapTypes.MountainLayer:
                            _mountainTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0),
                                tile.Value);
                            break;
                        case Cell.TilemapTypes.TreeLayer:
                            _treeTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tile.Value);
                            break;
                        case Cell.TilemapTypes.BushLayer:
                            _bushTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tile.Value);
                            break;
                        case Cell.TilemapTypes.GrassLayer:
                            _grassTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tile.Value);
                            break;
                        case Cell.TilemapTypes.StoneLayer:
                            _stoneTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tile.Value);
                            break;
                        case Cell.TilemapTypes.WaterLayer:
                            _waterTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tile.Value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        
            // Compress the size of the tilemaps to bounds where tiles exist
            foreach (var tilemap in _tilemaps)
            {
                tilemap.CompressBounds();
            }
        
            // Set the tilemaps active
            foreach (var gameObject in _gameObjects)
            {
                gameObject.SetActive(true);
            }
        }
    }
}