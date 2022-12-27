using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.CellGeneration
{
    public class CellDebugger
    {
        private readonly Tilemap _tilemap;

        private const float PixelPerUnit = 3f;
        
        private enum CellType
        {
            Owned,
            Similar,
            Different,
        }

        public CellDebugger(GameObject root)
        {
            if (root.GetComponent(typeof(Grid)) as Grid == null)
            {
                Debug.LogError("Parent for CellDebugTilemap has no Grid Component!");
                return;
            }
            
            _tilemap = GameObject.Instantiate(Resources.Load("Prefabs/CellDebugTilemap"), root.transform.position,
                Quaternion.identity, root.transform).GetComponent(typeof(Tilemap)) as Tilemap;
        }

        public void PlotNeighbours(Cell cell)
        {
            if (cell.neighbours.Count <= 0)
            {
                Debug.LogWarning("Cell has no Neighbours set!");
                return;
            }
            
            // Draw center cell
            DrawTile(cell.cellIndex, CellType.Owned);

            foreach (var neighbour in cell.neighbours)
            {
                if (neighbour.indoors == cell.indoors)
                {
                    DrawTile(neighbour.cellIndex, CellType.Similar);
                }
                else
                {
                    DrawTile(neighbour.cellIndex, CellType.Different);
                }
            }
        }

        private void DrawTile(int xPos, int yPos, CellType type)
        {
            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            // create texture and rect for Sprite
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, -1, true);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);

            // create Sprite
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
        
        private void DrawTile(Vector2Int cellPos, CellType type)
        {
            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            // create texture and rect for Sprite
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, -1, true);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);

            // create Sprite
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