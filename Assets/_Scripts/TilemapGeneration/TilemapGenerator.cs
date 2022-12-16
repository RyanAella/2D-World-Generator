using System;
using _Scripts.CellGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.TilemapGeneration
{
    [Serializable]
    public class TilemapGenerator
    {
        [SerializeField] private GameObject floorLayer; // floor base level showing biom color
        private Tilemap _floorTileMap;
        [SerializeField] private GameObject mountainLayer; // tilemap containing either nothing, or solid rock
        private Tilemap _mountainTileMap;
        [SerializeField] private GameObject openTerrainLayer; // tilemap containing either trees, or grass

        private Tilemap _openTerrainTileMap;
        // [SerializeField] private GameObject TreeLayer;    // tilemap containing all trees
        // [SerializeField] private GameObject BushLayer;    // tilemap containing all bushes


        public void Setup()
        {
            _floorTileMap = floorLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _mountainTileMap = mountainLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _openTerrainTileMap = openTerrainLayer.GetComponent(typeof(Tilemap)) as Tilemap;
        }


        /**
         * Generate the Tilemap
         */
        public void GenerateTilemap(Cell[,] cellMap)
        {
            _floorTileMap.ClearAllTiles();
            _mountainTileMap.ClearAllTiles();
            _openTerrainTileMap.ClearAllTiles();

            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;

            // foreach cell, TileMapGenerator.GenerateTiles(cell)
            foreach (var cell in cellMap)
            {
                if (tempTile != null)
                {
                    GenerateTiles(cell, tempTile);
                }
            }

            // generate base/floor tilemap
            // Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            //
            // for (int x = 0; x < cellMap.GetLength(0); x++)
            // {
            //     for (int y = 0; y < cellMap.GetLength(1); y++)
            //     {
            //         if (tempTile != null)
            //         {
            //             tempTile.sprite = CreateSprite();
            //
            //             if (cellMap[x, y].Indoors)
            //             {
            //                 tempTile.sprite.texture.SetPixel(0, 0, Color.gray);
            //                 
            //                 tempTile.sprite.texture.Apply();
            //             }
            //             else if (!cellMap[x, y].Indoors)
            //             {
            //                 tempTile.sprite.texture.SetPixel(0, 0, Color.green);
            //                 tempTile.sprite.texture.Apply();
            //             }
            //             // else
            //             // {
            //             //     Debug.LogError("No indoor value: " + x + ", " + y);
            //             // }
            //
            //             _floorTileMap.SetTile(new Vector3Int(x, y, 0), tempTile);
            //         }
            //     }
            // }

            _floorTileMap.CompressBounds();
            _mountainTileMap.CompressBounds();
            _openTerrainTileMap.CompressBounds();
            floorLayer.SetActive(true);
            mountainLayer.SetActive(true);
            openTerrainLayer.SetActive(true);
        }

        private void GenerateTiles(Cell cell, Tile tempTile)
        {
            GenerateBaseTile(cell, tempTile);

            if (cell.Indoors)
            {
                GenerateMountainTile(cell, tempTile);
            }
            else
            {
                GenerateOpenTerrainTile(cell, tempTile);
            }
        }

        private void GenerateBaseTile(Cell cell, Tile tempTile)
        {
            // foreach cell create floor tile based on biom
            // meadows light green
            // woods dark green
            // cave grey

            tempTile.sprite = CreateSprite(1);

            if (cell.Biom == Biom.Cave)
            {
                tempTile.sprite.texture.SetPixel(0, 0, Color.gray);
                tempTile.sprite.texture.Apply();
            }
            else if (cell.Biom == Biom.Meadows)
            {
                tempTile.sprite.texture.SetPixel(0, 0, new Color(0.3f, 0.8f, 0.5f, 1));
                tempTile.sprite.texture.Apply();
            }
            else if(cell.Biom == Biom.Woods)
            {
                tempTile.sprite.texture.SetPixel(0, 0, new Color(0.1f, 0.5f, 0.2f, 1));
                tempTile.sprite.texture.Apply();
            }

            _floorTileMap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tempTile);
        }

        private void GenerateMountainTile(Cell cell, Tile tempTile)
        {
            // foreach indoor cell create asset tile if such is rock
            // each asset on the correct tilemap

            if (cell.Asset.Type == CellAsset.AssetType.Rock)
            {
                tempTile.sprite = CreateSprite(2);
                tempTile.sprite.texture.SetPixel(0, 0, Color.black);
                tempTile.sprite.texture.Apply();
                _mountainTileMap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tempTile);
            }
        }

        private void GenerateOpenTerrainTile(Cell cell, Tile tempTile)
        {
            // foreach indoor cell create asset tile if such is tree
            // each asset on the correct tilemap

            if (cell.Asset.Type == CellAsset.AssetType.Tree)
            {
                tempTile.sprite = CreateSprite(2);
                tempTile.sprite.texture.SetPixel(0, 0, Color.red);
                tempTile.sprite.texture.Apply();
                _openTerrainTileMap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tempTile);
            }
            else if (cell.Asset.Type == CellAsset.AssetType.Bush)
            {
                tempTile.sprite = CreateSprite(2);
                tempTile.sprite.texture.SetPixel(0, 0, Color.magenta);
                tempTile.sprite.texture.Apply();
                _openTerrainTileMap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tempTile);
            }
        }

        /**
         * Create a Sprite
         */
        private Sprite CreateSprite(float pixelPerUnit)
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