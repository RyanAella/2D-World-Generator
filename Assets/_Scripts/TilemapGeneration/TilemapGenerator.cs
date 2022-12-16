using System;
using _Scripts.CellGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.TilemapGeneration
{
    [Serializable]
    public class TilemapGenerator
    {
        [SerializeField] private GameObject FloorLayer;   // floor base level showing biome color
        private Tilemap _floorTileMap;
        [SerializeField] private GameObject MountainLayer;    // tilemap containing either wall (indoor/outdoor), or solid rock
        private Tilemap _mountainTileMap;
        // [SerializeField] private GameObject TreeLayer;    // tilemap containing all trees
        // [SerializeField] private GameObject BushLayer;    // tilemap containing all bushes


        public void Setup()
        {
            _floorTileMap = FloorLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _mountainTileMap = MountainLayer.GetComponent(typeof(Tilemap)) as Tilemap;
        }


        /**
         * Generate the Tilemap
         */
        public void GenerateTilemap(Cell[,] cellMap)
        {
            // foreach cell, TileMapGenerator.GenerateTiles(cell)
            
            
            _floorTileMap.ClearAllTiles();
            
            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;

            for (int x = 0; x < cellMap.GetLength(0); x++)
            {
                for (int y = 0; y < cellMap.GetLength(1); y++)
                {
                    if (tempTile != null)
                    {
                        tempTile.sprite = CreateSprite();

                        if (cellMap[x, y].Indoors)
                        {
                            // if (cellMap[x, y].isWall)
                            // {
                            //     tempTile.sprite.texture.SetPixel(0, 0, Color.black);
                            // }
                            // else
                            // {
                                tempTile.sprite.texture.SetPixel(0, 0, Color.gray);
                            // }
                            
                            tempTile.sprite.texture.Apply();
                        }
                        else if (!cellMap[x, y].Indoors)
                        {
                            tempTile.sprite.texture.SetPixel(0, 0, Color.green);
                            tempTile.sprite.texture.Apply();
                        }
                        // else
                        // {
                        //     Debug.LogError("No indoor value: " + x + ", " + y);
                        // }

                        _floorTileMap.SetTile(new Vector3Int(x, y, 0), tempTile);
                    }
                }
            }
            
            _floorTileMap.CompressBounds();
        }

        private void GenerateTiles(Cell cell)
        {
            // if (cell.)
        }

        private void GenerateBaseTile()
        {
        }

        private void GenerateMountainTile()
        {
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