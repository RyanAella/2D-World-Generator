using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.CellGeneration
{
    /*
     * This class shows the value of a cell and its neighbouring cells.
     */
    public class CellDebugger
    {
        private readonly Tilemap _tilemap;

        private const float PixelPerUnit = 3f;

        // The cell types
        private enum CellType
        {
            Owned,
            Similar,
            Different,
        }

        // Constructor
        public CellDebugger(GameObject root)
        {
            if (root.GetComponent(typeof(Grid)) as Grid == null)
            {
                Debug.LogError("Parent for CellDebugTilemap has no Grid Component!");
                return;
            }

            _tilemap = Object.Instantiate(Resources.Load("Prefabs/CellDebugTilemap"), root.transform.position,
                Quaternion.identity, root.transform).GetComponent(typeof(Tilemap)) as Tilemap;
        }

        /*
         * Plot the cell and its neighbours.
         */
        public void PlotNeighbours(Cell cell)
        {
            if (cell.Neighbours.Count <= 0)
            {
                Debug.LogWarning("Cell has no Neighbours set!");
                return;
            }

            // Draw center cell
            DrawTile(cell.CellIndex, CellType.Owned);

            foreach (var neighbour in cell.Neighbours)
            {
                DrawTile(neighbour.CellIndex,
                    neighbour.Indoors == cell.Indoors ? CellType.Similar : CellType.Different);
            }
        }

        /*
         * Draw the needed tile.
         */
        private void DrawTile(int xPos, int yPos, CellType type)
        {
            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;

            // Create texture and rect for Sprite
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, -1, true);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);

            // create Sprite
            if (tempTile == null) return;
            
            tempTile.sprite = Sprite.Create(texture, rect, Vector2.up, PixelPerUnit);

            var color = Color.clear;

            switch (type)
            {
                case CellType.Owned:
                    color = Color.yellow;
                    break;
                case CellType.Different:
                    color = Color.red;
                    break;
                case CellType.Similar:
                    color = Color.blue;
                    break;
            }

            tempTile.sprite.texture.SetPixel(0, 0, color);
            tempTile.sprite.texture.Apply();

            _tilemap.SetTile(new Vector3Int(xPos, yPos, 0), tempTile);
        }

        /*
         * Draw the needed tile.
         */
        private void DrawTile(Vector2Int cellPos, CellType type)
        {
            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            
            // create texture and rect for Sprite
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, -1, true);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);

            // create Sprite
            if (tempTile == null) return;
            
            tempTile.sprite = Sprite.Create(texture, rect, Vector2.up, PixelPerUnit);

            var color = Color.clear;

            switch (type)
            {
                case CellType.Owned:
                    color = Color.yellow;
                    break;
                case CellType.Different:
                    color = Color.red;
                    break;
                case CellType.Similar:
                    color = Color.blue;
                    break;
            }

            tempTile.sprite.texture.SetPixel(0, 0, color);
            tempTile.sprite.texture.Apply();

            _tilemap.SetTile(new Vector3Int(cellPos.x, cellPos.y, 0), tempTile);
        }
    }
}