using System;
using _Scripts.CellGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts._GradientNoise.TilemapGeneration
{
    /**
     * This class generates the tilemaps
     */
    [Serializable]
    public class TilemapGenerator
    {
        [SerializeField] private GameObject biomLayer; // floor base level showing biom color
        private Tilemap _biomTilemap;
        [SerializeField] private GameObject massiveRockLayer; // tilemap containing massive rock
        private Tilemap _massiveRockTilemap;
        [SerializeField] private GameObject wallLayer; // tilemap containing walls
        private Tilemap _wallTilemap;
        [SerializeField] private GameObject treeLayer; // tilemap containing trees
        private Tilemap _treeTilemap;
        [SerializeField] private GameObject bushLayer; // tilemap containing bushes
        private Tilemap _bushTilemap;

        public void Setup()
        {
            _biomTilemap = biomLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _massiveRockTilemap = massiveRockLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _wallTilemap = wallLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _treeTilemap = treeLayer.GetComponent(typeof(Tilemap)) as Tilemap;
            _bushTilemap = bushLayer.GetComponent(typeof(Tilemap)) as Tilemap;
        }

        /**
         * Generate the Tilemap
         */
        public void GenerateTilemap(Cell[,] cellMap)
        {
            _biomTilemap.ClearAllTiles();
            _massiveRockTilemap.ClearAllTiles();
            _wallTilemap.ClearAllTiles();
            _treeTilemap.ClearAllTiles();
            _bushTilemap.ClearAllTiles();

            Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;


            // foreach cell, TileMapGenerator.GenerateTiles(cell)
            foreach (var cell in cellMap)
            {
                if (tempTile == null) continue;

                cell.GenerateTiles();

                foreach (var tile in cell.Tiles)
                {
                    tempTile.sprite = Resources.Load(tile.Value) as Sprite;

                    switch (tile.Key)
                    {
                        case Cell.TilemapTypes.BiomLayer:
                            _biomTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tempTile);
                            break;
                        case Cell.TilemapTypes.MassiveRockLayer:
                            _massiveRockTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0),
                                tempTile);
                            break;
                        case Cell.TilemapTypes.WallLayer:
                            _wallTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tempTile);
                            break;
                        case Cell.TilemapTypes.TreeLayer:
                            _treeTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tempTile);
                            break;
                        case Cell.TilemapTypes.BushLayer:
                            _bushTilemap.SetTile(new Vector3Int(cell.CellIndex.x, cell.CellIndex.y, 0), tempTile);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            _biomTilemap.CompressBounds();
            _massiveRockTilemap.CompressBounds();
            _wallTilemap.CompressBounds();
            _treeTilemap.CompressBounds();
            _bushTilemap.CompressBounds();

            biomLayer.SetActive(true);
            massiveRockLayer.SetActive(true);
            wallLayer.SetActive(true);
            treeLayer.SetActive(true);
            bushLayer.SetActive(true);
        }
    }
}