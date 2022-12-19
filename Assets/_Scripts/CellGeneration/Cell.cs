using System.Collections.Generic;
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

        // Needs to be in constructors???
        public bool Collidable = false;
        public bool Interactable = false;
        
        public enum AssetType
        {
            None,
            Rock,
            Tree,
            Bush,
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

        // collision
        public CellAsset Asset; // e.g. a tree stands on this cell

        // neighbours
        public List<Cell> Neighbours;

        public Cell()
        {
            Debug.LogWarning("Cell created without index!");
            CellIndex = new Vector2Int();
            Neighbours = new List<Cell>();
        }

        public Cell(int x, int y)
        {
            CellIndex = new Vector2Int(x, y);
            Neighbours = new List<Cell>();
        }

        public Cell(Vector2Int cellPos)
        {
            CellIndex = cellPos;
            Neighbours = new List<Cell>();
        }
    }
}