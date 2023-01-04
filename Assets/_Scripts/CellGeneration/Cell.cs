using System.Collections.Generic;
using _Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.CellGeneration
{
    /**
     * The different types of bioms a cell can have.
     */
    public enum Biom
    {
        Cave, // Default
        Meadows,
        Woods
    }

    /**
     * The Assets a cell can hold.
     */
    public class CellAsset
    {
        public AssetType Type;

        // public bool Collidable = false;
        // public bool Interactable = false;
        // public bool CollidableInteractable = false;

        // The cell asset types
        public enum AssetType
        {
            None,
            MassiveRock,
            Cavity,
            Wall,
            Tree,
            Bush
        }

        // The cell asset and its values
        public CellAsset(AssetType type)
        {
            switch (type)
            {
                case AssetType.None:
                    Type = type;
                    // Collidable = false;
                    // Interactable = false;
                    // CollidableInteractable = false;
                    break;
                case AssetType.MassiveRock:
                    Type = type;
                    // Collidable = false;
                    // Interactable = false;
                    // CollidableInteractable = true;
                    break;
                case AssetType.Cavity:
                    Type = type;
                    // Collidable = false;
                    // Interactable = false;
                    // CollidableInteractable = false;
                    break;
                case AssetType.Wall:
                    Type = type;
                    // Collidable = true;
                    // Interactable = false;
                    // CollidableInteractable = false;
                    break;
                case AssetType.Tree:
                    Type = type;
                    // Collidable = false;
                    // Interactable = false;
                    // CollidableInteractable = true;
                    break;
                case AssetType.Bush:
                    Type = type;
                    // Collidable = false;
                    // Interactable = false;
                    // CollidableInteractable = false;
                    break;
            }
        }
    }

    /**
     * This class holds information about the Biom, the Asset and the neighbouring cells.
     */
    public class Cell
    {
        // General
        public Vector2Int CellIndex; // cell position x and y
        public bool Indoors = false; // in- or outdoor

        private System.Random _prng = new System.Random(); // Needed to create the Woods Biom (outdoors)

        // Biom
        public Biom Biom;

        // Asset
        public CellAsset Asset; // e.g. a tree stands on this cell

        // Neighbours
        public List<Cell> Neighbours;

        // Dictionary containing the biom and asset tiles of the cell
        public Dictionary<TilemapTypes, Tile> Tiles;

        // The tilemap types
        public enum TilemapTypes
        {
            BiomLayer,
            MassiveRockLayer,
            WallLayer,
            TreeLayer,
            BushLayer
        }

        // Constructor
        public Cell(int x, int y)
        {
            CellIndex = new Vector2Int(x, y);
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, Tile>();
        }

        // Constructor
        public Cell(Vector2Int cellPos)
        {
            CellIndex = cellPos;
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, Tile>();
        }

        /*
         * Each cell generates its own biom and asset
         */
        public void GenerateTiles(Dictionary<string, TilePaletteScriptableObject> tilePalette)
        {
            // Get the list of tiles from the dictionary
            var cave = tilePalette["Cave"].tiles;
            var woods = tilePalette["Woods"].tiles;
            var meadows = tilePalette["Meadows"].tiles;
            var massiveRock = tilePalette["MassiveRock"].tiles;
            var wall = tilePalette["Wall"].tiles;
            var tree = tilePalette["Tree"].tiles;
            var bush = tilePalette["Bush"].tiles;

            // Load the tiles randomly from the list
            var prngCave = _prng.Next(cave.Count);
            var prngWoods = _prng.Next(woods.Count);
            var prngMeadows = _prng.Next(meadows.Count);
            var prngRock = _prng.Next(massiveRock.Count);
            var prngWall = _prng.Next(wall.Count);
            var prngTree = _prng.Next(tree.Count);
            var prngBush = _prng.Next(bush.Count);

            // Add the Biom tile to a dictionary
            switch (Biom)
            {
                case Biom.Cave:
                    Tiles.Add(TilemapTypes.BiomLayer, cave[prngCave]);
                    break;
                case Biom.Meadows:
                    Tiles.Add(TilemapTypes.BiomLayer, meadows[prngMeadows]);
                    break;
                case Biom.Woods:
                    Tiles.Add(TilemapTypes.BiomLayer, woods[prngWoods]);
                    break;
            }

            // Add the Asset tile to the dictionary
            switch (Asset.Type)
            {
                case CellAsset.AssetType.MassiveRock:
                    Tiles.Add(TilemapTypes.MassiveRockLayer, massiveRock[prngRock]);
                    break;
                case CellAsset.AssetType.Cavity:
                    break;
                case CellAsset.AssetType.Wall:
                    Tiles.Add(TilemapTypes.WallLayer, wall[prngWall]);
                    break;
                case CellAsset.AssetType.Tree:
                    Tiles.Add(TilemapTypes.TreeLayer, tree[prngTree]);
                    break;
                case CellAsset.AssetType.Bush:
                    Tiles.Add(TilemapTypes.BushLayer, bush[prngBush]);
                    break;
                case CellAsset.AssetType.None:
                    break;
            }
        }
    }
}