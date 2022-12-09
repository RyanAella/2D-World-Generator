using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class Cell
    {
        public Vector2Int cellIndex;
        public bool indoors = false;
        
        // Vector2Int = cellIndex of neighbours, bool = neighbour indoor/outdoor | may change later to index val instead of vector and enum instead of bools
        // public Dictionary<Vector2Int, bool> neighbours;
        public List<Cell> neighbours;

        public Cell()
        {
            Debug.LogWarning("Cell created without index!");
            cellIndex = new Vector2Int();
            // neighbours = new Dictionary<Vector2Int, bool>();
            neighbours = new List<Cell>();
        }

        public Cell(int x, int y)
        {
            cellIndex = new Vector2Int(x, y);
            // neighbours = new Dictionary<Vector2Int, bool>();
            neighbours = new List<Cell>();
        }
        
        public Cell(Vector2Int cellPos)
        {
            cellIndex = cellPos;
            // neighbours = new Dictionary<Vector2Int, bool>();
            neighbours = new List<Cell>();
        }
    }
}
