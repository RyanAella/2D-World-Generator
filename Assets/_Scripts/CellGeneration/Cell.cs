using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.CellGeneration
{
    // /**
    //  * This class stores the parameters for each generation step.
    //  */
    // [Serializable] // With this it can be showed in the Inspector
    // public class CellGenerationSettings
    // {
    //     public Tiles _tiles;
    // }
    
    /**
     * The Bioms of the map.
     */
    public enum Biom
    {
        Meadows,
        Woods,
        Cave,
    }

    /**
     * The Assets a cell can hold and if they are collidable or interactable.
     */
    public class CellAsset
    {
        public AssetType Type;

        public bool Collidable = false;
        public bool Interactable = false;
        public bool CollidableInteractable = false;

        public enum AssetType
        {
            None,
            MassiveRock,
            Cavity,
            Wall,
            Tree,
            Bush
        }

        public CellAsset(AssetType type)
        {
            switch (type)
            {
                case AssetType.None:
                    Type = type;
                    Collidable = false;
                    Interactable = false;
                    CollidableInteractable = false;
                    break;
                case AssetType.MassiveRock:
                    Type = type;
                    Collidable = false;
                    Interactable = false;
                    CollidableInteractable = true;
                    break;
                case AssetType.Cavity:
                    Type = type;
                    Collidable = false;
                    Interactable = false;
                    CollidableInteractable = false;
                    break;
                case AssetType.Wall:
                    Type = type;
                    Collidable = true;
                    Interactable = false;
                    CollidableInteractable = false;
                    break;
                case AssetType.Tree:
                    Type = type;
                    Collidable = false;
                    Interactable = false;
                    CollidableInteractable = true;
                    break;
                case AssetType.Bush:
                    Type = type;
                    Collidable = false;
                    Interactable = false;
                    CollidableInteractable = false;
                    break;
            }
        }
    }

    /**
     * This class holds information about the Biom, the Asset and the neighbouring cells.
     */
    public class Cell
    {
        // general
        public Vector2Int cellIndex;
        public bool indoors = false; // in- or outdoor

        // biom
        public Biom biom;

        // asset
        public CellAsset Asset; // e.g. a tree stands on this cell

        // neighbours
        public List<Cell> neighbours;

        // Tile
        public Dictionary<TilemapTypes, Tile> BiomTiles;
        public Dictionary<TilemapTypes, string> Tiles;

        public enum TilemapTypes
        {
            BiomLayer,
            MassiveRockLayer,
            WallLayer,
            TreeLayer,
            BushLayer
        }

        // [SerializeField] public Tiles biomTiles;

        public Cell()
        {
            Debug.LogWarning("Cell created without index!");
            cellIndex = new Vector2Int();
            neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, string>();
            BiomTiles = new Dictionary<TilemapTypes, Tile>();
        }

        public Cell(int x, int y)
        {
            cellIndex = new Vector2Int(x, y);
            neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, string>();
            BiomTiles = new Dictionary<TilemapTypes, Tile>();
        }

        public Cell(Vector2Int cellPos)
        {
            cellIndex = cellPos;
            neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, string>();
            BiomTiles = new Dictionary<TilemapTypes, Tile>();
        }

        public void GenerateTiles()
        {
            // CellGenerationSettings tiles = new CellGenerationSettings();
            // if (tiles._tiles != null) Debug.Log(tiles._tiles.tiles.Count);

            switch (biom)
            {
                case Biom.Cave:
                    Tiles.Add(TilemapTypes.BiomLayer, "Tiles/Biom/CaveFloor");
                    // Debug.Log("biomTiles.tiles[0]: " + biomTiles.tiles[0]);
                    // BiomTiles.Add(TilemapTypes.BiomLayer, tiles._tiles.tiles[0]);
                    break;
                case Biom.Meadows:
                    // Debug.Log("biomTiles.tiles[0]: " + biomTiles.tiles[1]);
                    // BiomTiles.Add(TilemapTypes.BiomLayer, tiles._tiles.tiles[1]);
                    Tiles.Add(TilemapTypes.BiomLayer, "Tiles/Biom/MeadowsFloor");
                    break;
                case Biom.Woods:
                    // Debug.Log("biomTiles.tiles[0]: " + biomTiles.tiles[2]);
                    // BiomTiles.Add(TilemapTypes.BiomLayer, tiles._tiles.tiles[2]);
                    Tiles.Add(TilemapTypes.BiomLayer, "Tiles/Biom/WoodsFloor");
                    break;
            }

            switch (Asset.Type)
            {
                case CellAsset.AssetType.MassiveRock:
                    Tiles.Add(TilemapTypes.MassiveRockLayer, "Tiles/Indoor/MassiveRock");
                    break;
                case CellAsset.AssetType.Cavity:
                    break;
                case CellAsset.AssetType.Wall:
                    Tiles.Add(TilemapTypes.WallLayer, "Tiles/Indoor/Wall");
                    break;
                case CellAsset.AssetType.Tree:
                    Tiles.Add(TilemapTypes.TreeLayer, "Tiles/Outdoor/Tree");
                    break;
                case CellAsset.AssetType.Bush:
                    Tiles.Add(TilemapTypes.BushLayer, "Tiles/Outdoor/Bush");
                    break;
                case CellAsset.AssetType.None:
                    break;
            }
        }
    }
}