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
        // Floor base level tilemap showing biom color
        [SerializeField] private GameObject biomLayer;
        private Tilemap _biomTilemap;
        
        // Tilemap containing massive rock
        [SerializeField] private GameObject massiveRockLayer; 
        private Tilemap _massiveRockTilemap;
        
        // Tilemap containing walls
        [SerializeField] private GameObject wallLayer; 
        private Tilemap _wallTilemap;
        
        // Tilemap containing trees
        [SerializeField] private GameObject treeLayer; 
        private Tilemap _treeTilemap;
        
        // Tilemap containing bushes
        [SerializeField] private GameObject bushLayer; 
        private Tilemap _bushTilemap;
        
        // Tilemap containing grass
        [SerializeField] private GameObject grassLayer; 
        private Tilemap _grassTilemap;
        
        // Tilemap containing stones
        [SerializeField] private GameObject stoneLayer; 
        private Tilemap _stoneTilemap;
        
        // Tilemap containing water
        [SerializeField] private GameObject waterLayer; 
        private Tilemap _waterTilemap;
        
        // List containing all GameObjects
        private List<GameObject> _gameObjects;

        // List containing all Tilemaps
        private List<Tilemap> _tilemaps;

        // ScriptableObject lists containing the tiles for each tilemap
        [SerializeField] public TilePaletteScriptableObject mountainScriptableObject;
        [SerializeField] public TilePaletteScriptableObject meadowsScriptableObject;
        [SerializeField] public TilePaletteScriptableObject woodsScriptableObject;
        [SerializeField] public TilePaletteScriptableObject massiveRockScriptableObject;
        [SerializeField] public TilePaletteScriptableObject wallScriptableObject;
        [SerializeField] public TilePaletteScriptableObject treeScriptableObject;
        [SerializeField] public TilePaletteScriptableObject bushScriptableObject;
        [SerializeField] public TilePaletteScriptableObject grassScriptableObject;
        [SerializeField] public TilePaletteScriptableObject stoneScriptableObject;
        [SerializeField] public TilePaletteScriptableObject waterScriptableObject;

        // Dictionary containing all ScriptableObject lists
        private Dictionary<string, TilePaletteScriptableObject> _tilePalettes;

        public void Setup()
        {
            // Creating the Dictionary
            _gameObjects = new List<GameObject>()
            {
                biomLayer,
                massiveRockLayer,
                wallLayer,
                treeLayer,
                bushLayer,
                grassLayer,
                stoneLayer,
                waterLayer
            };
            
            // Getting the tilemap components
            _biomTilemap = biomLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _massiveRockTilemap = massiveRockLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _wallTilemap = wallLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _treeTilemap = treeLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _bushTilemap = bushLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _grassTilemap = grassLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _stoneTilemap = stoneLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _waterTilemap = waterLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            
            // Creating the Dictionary
            _tilemaps = new List<Tilemap>()
            {
                _biomTilemap,
                _massiveRockTilemap,
                _wallTilemap,
                _treeTilemap,
                _bushTilemap,
                _grassTilemap,
                _stoneTilemap,
                _waterTilemap
            };

            // Creating the Dictionary
            _tilePalettes = new Dictionary<string, TilePaletteScriptableObject>
            {
                { "Mountain", mountainScriptableObject },
                { "Woods", woodsScriptableObject },
                { "Meadows", meadowsScriptableObject },
                { "MassiveRock", massiveRockScriptableObject },
                { "Wall", wallScriptableObject },
                { "Tree", treeScriptableObject },
                { "Bush", bushScriptableObject },
                { "Grass", grassScriptableObject },
                { "Stone", stoneScriptableObject },
                { "Water", waterScriptableObject}
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
                        case Cell.TilemapTypes.MassiveRockLayer:
                            _massiveRockTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0),
                                tile.Value);
                            break;
                        case Cell.TilemapTypes.WallLayer:
                            _wallTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tile.Value);
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