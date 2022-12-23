using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

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

        private bool _collidable = false;
        private bool _interactable = false;
        private bool _collidableInteractable = false;
        
        public enum AssetType
        {
            None,
            Rock,
            Tree,
            Bush,
        }

        public CellAsset(AssetType type)
        {
            switch (type)
            {
                case AssetType.None:
                    Type = type;
                    _collidable = false;
                    _interactable = false;
                    _collidableInteractable = false;
                    break;
                case AssetType.Rock:
                    Type = type;
                    _collidable = false;
                    _interactable = false;
                    _collidableInteractable = true;
                    break;
                case AssetType.Tree:
                    Type = type;
                    _collidable = false;
                    _interactable = false;
                    _collidableInteractable = true;
                    break;
                case AssetType.Bush:
                    Type = type;
                    _collidable = false;
                    _interactable = false;
                    _collidableInteractable = false;
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

        public Cell()
        {
            Debug.LogWarning("Cell created without index!");
            CellIndex = new Vector2Int();
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
        }

        public Cell(int x, int y)
        {
            CellIndex = new Vector2Int(x, y);
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
        }

        public Cell(Vector2Int cellPos)
        {
            CellIndex = cellPos;
            Neighbours = new List<Cell>();
            Asset = new CellAsset(CellAsset.AssetType.None);
        }
    }
}