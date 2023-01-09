using System;
using System.Collections.Generic;
using _Scripts._GradientNoise.ValueGeneration;
using _Scripts.CellGeneration;
using _Scripts.ScriptableObjects;
using _Scripts.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;
using ValueGenerationSettings = _Scripts.Settings.ValueGenerationSettings;

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
        
        // Tilemap containing water
        [SerializeField] private GameObject waterLayer; 
        private Tilemap _waterTilemap;
        
        // ScriptableObject lists containing the tiles for each tilemap
        [SerializeField] public TilePaletteScriptableObject caveScriptableObject;
        [SerializeField] public TilePaletteScriptableObject meadowsScriptableObject;
        [SerializeField] public TilePaletteScriptableObject woodsScriptableObject;
        [SerializeField] public TilePaletteScriptableObject massiveRockScriptableObject;
        [SerializeField] public TilePaletteScriptableObject wallScriptableObject;
        [SerializeField] public TilePaletteScriptableObject treeScriptableObject;
        [SerializeField] public TilePaletteScriptableObject bushScriptableObject;
        [SerializeField] public TilePaletteScriptableObject waterScriptableObject;
        // [SerializeField] private List<TilePaletteScriptableObject> _tilePaletteScriptableObjects;

        // Dictionary containing all ScriptableObject lists
        private Dictionary<string, TilePaletteScriptableObject> _tilePalettes;

        // private Dictionary<string, Tilemap> tilemaps;

        // public void Setup(GameObject gameObject, List<ValueGenerationSettings> valueGenerationSettings, List<AssetGenerationSettings> assetGenerationSettings)
        // {
        //     foreach (var setting in valueGenerationSettings)
        //     {
        //         var tilemap = Resources.Load("Prefabs/Tilemap.prefab") as Tilemap;
        //         tilemaps.Add(setting.name, tilemap);
        //     }
        // }
        
        public void Setup()
        {
            // Getting the tilemap components
            _biomTilemap = biomLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _massiveRockTilemap = massiveRockLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _wallTilemap = wallLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _treeTilemap = treeLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _bushTilemap = bushLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _waterTilemap = waterLayer.GetComponent(typeof(Tilemap)) as Tilemap;
        
            // Creating the Dictionary
            _tilePalettes = new Dictionary<string, TilePaletteScriptableObject>
            {
                { "Cave", caveScriptableObject },
                { "Woods", woodsScriptableObject },
                { "Meadows", meadowsScriptableObject },
                { "MassiveRock", massiveRockScriptableObject },
                { "Wall", wallScriptableObject },
                { "Tree", treeScriptableObject },
                { "Bush", bushScriptableObject },
                { "Water", waterScriptableObject}
            };
        }

        /**
         * Generate the Tilemap.
         */
        public void GenerateTilemap(Cell[,] cellMap)
        {
            // Clear all tilemaps
            _biomTilemap.ClearAllTiles();
            _massiveRockTilemap.ClearAllTiles();
            _wallTilemap.ClearAllTiles();
            _treeTilemap.ClearAllTiles();
            _bushTilemap.ClearAllTiles();
            _waterTilemap.ClearAllTiles();
            
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
                        case Cell.TilemapTypes.WaterLayer:
                            _waterTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tile.Value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            // Compress the size of the tilemaps to bounds where tiles exist
            _biomTilemap.CompressBounds();
            _massiveRockTilemap.CompressBounds();
            _wallTilemap.CompressBounds();
            _treeTilemap.CompressBounds();
            _bushTilemap.CompressBounds();
            _waterTilemap.CompressBounds();

            // Set the tilemaps active
            biomLayer.SetActive(true);
            massiveRockLayer.SetActive(true);
            wallLayer.SetActive(true);
            treeLayer.SetActive(true);
            bushLayer.SetActive(true);
            waterLayer.SetActive(true);
        }
    }
}