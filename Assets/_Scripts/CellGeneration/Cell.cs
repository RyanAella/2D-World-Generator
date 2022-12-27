using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.CellGeneration
{
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
        public Vector2Int CellIndex;
        public bool Indoors = false; // in- or outdoor

        // biom
        public Biom Biom;

        // asset
        public CellAsset Asset; // e.g. a tree stands on this cell

        // neighbours
        public List<Cell> Neighbours;
        
        // Tile
        public Dictionary<TilemapTypes, string> Tiles;
        
        public enum TilemapTypes
        {
            BiomLayer,
            MassiveRockLayer,
            WallLayer,
            TreeLayer,
            BushLayer
        }

        public Cell()
        {
            Debug.LogWarning("Cell created without index!");
            CellIndex = new Vector2Int();
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, string>();
        }

        public Cell(int x, int y)
        {
            CellIndex = new Vector2Int(x, y);
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, string>();
        }

        public Cell(Vector2Int cellPos)
        {
            CellIndex = cellPos;
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, string>();
        }

        public void GenerateTiles()
        {
            switch (Biom)
            {
                case Biom.Cave:
                    Tiles.Add(TilemapTypes.BiomLayer, "Tiles/Biom/CaveFloor");
                    break;
                case Biom.Meadows:
                    Tiles.Add(TilemapTypes.BiomLayer, "Tiles/Biom/MeadowsFloor");
                    break;
                case Biom.Woods:
                    Tiles.Add(TilemapTypes.BiomLayer, "Tiles/Biom/WoodsFloor");
                    break;
            }

            switch (Asset.Type)
            {
                case CellAsset.AssetType.MassiveRock:
                    Tiles.Add(TilemapTypes.MassiveRockLayer, "Tiles/MassiveRock");
                    break;
                case CellAsset.AssetType.Wall:
                    Tiles.Add(TilemapTypes.WallLayer, "Tiles/Wall");
                    break;
                case CellAsset.AssetType.Tree:
                    // Tiles.Add(TilemapTypes.TreeLayer, "Tiles/Tree");
                    break;
                case CellAsset.AssetType.Bush:
                    Tiles.Add(TilemapTypes.BushLayer, "Tiles/Bush");
                    break;
                case CellAsset.AssetType.None:
                    break;
            }
        }
    }
}