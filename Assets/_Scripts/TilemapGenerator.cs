using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts
{
    public class TilemapGenerator
    {
        /**
         * Generate the Tilemap
         */
        public void GenerateTilemap(Cell[,] cellMap, Tilemap floorTilemap)
        {
            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;

            for (int x = 0; x < cellMap.GetLength(0); x++)
            {
                for (int y = 0; y < cellMap.GetLength(1); y++)
                {
                    if (tempTile != null)
                    {
                        tempTile.sprite = CreateSprite();

                        if (cellMap[x, y].indoors)
                        {
                            tempTile.sprite.texture.SetPixel(0, 0, Color.gray);
                            tempTile.sprite.texture.Apply();
                        }
                        else if (!cellMap[x, y].indoors)
                        {
                            tempTile.sprite.texture.SetPixel(0, 0, Color.green);
                            tempTile.sprite.texture.Apply();
                        }
                        else
                        {
                            Debug.LogError("No indoor value: " + x + ", " + y);
                        }

                        floorTilemap.SetTile(new Vector3Int(x, y, 0), tempTile);
                    }
                }
            }
        }

        /**
         * Create a Sprite
         */
        private Sprite CreateSprite(float pixelPerUnit = 1)
        {
            // create texture and rect for Sprite
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, -1, true);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);

            // create Sprite
            var sprite = Sprite.Create(texture, rect, Vector2.up, pixelPerUnit);

            return sprite;
        }
    }
}