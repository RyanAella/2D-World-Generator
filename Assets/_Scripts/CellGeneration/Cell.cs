using System;
using System.Collections.Generic;
using _Scripts.ScriptableObjects;
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
        public Vector2Int CellIndex;
        public bool Indoors = false; // in- or outdoor
        // Needed to create the Woods Biom (outdoors)
        System.Random prng = new System.Random();

        // biom
        public Biom Biom;

        // asset
        public CellAsset Asset; // e.g. a tree stands on this cell

        // neighbours
        public List<Cell> Neighbours;

        // Tile
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

        public Cell()
        {
            Debug.LogWarning("Cell created without index!");
            CellIndex = new Vector2Int();
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, Tile>();
        }

        public Cell(int x, int y)
        {
            CellIndex = new Vector2Int(x, y);
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
            Tiles = new Dictionary<TilemapTypes, Tile>();
        }

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
            var cave = tilePalette["Cave"].tiles;
            var woods = tilePalette["Woods"].tiles;
            var meadows = tilePalette["Meadows"].tiles;
            var massiveRock = tilePalette["MassiveRock"].tiles;
            var wall = tilePalette["Wall"].tiles;
            var tree = tilePalette["Tree"].tiles;
            var bush = tilePalette["Bush"].tiles;

            var prngCave = prng.Next(cave.Count);
            var prngWoods = prng.Next(woods.Count);
            var prngMeadows = prng.Next(meadows.Count);
            var prngRock = prng.Next(massiveRock.Count);
            var prngWall = prng.Next(wall.Count);
            var prngTree = prng.Next(tree.Count);
            var prngBush = prng.Next(bush.Count);

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