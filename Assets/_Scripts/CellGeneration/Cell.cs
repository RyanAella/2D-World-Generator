using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.CellGeneration
{
    public enum Biom
    {
        Meadows,
        Woods,
        Cave,
    }

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

    public class CellAsset
    {
        public AssetType Type;

        public bool Collidable = false;
        
        public enum AssetType
        {
            None,
            Rock,
            Tree,
            Bush,
        }
    }
}