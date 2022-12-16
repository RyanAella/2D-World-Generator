using System;
using System.Collections.Generic;
using _Scripts.CellGeneration;
using _Scripts.ValueGeneration;
using UnityEngine;

namespace _Scripts
{
    public class CellMapGenerator
    {
        private Cell[,] _cellMap;
        private List<Cell> _indoorCells;
        private List<Cell> _outdoorCells;

        public Cell[,] GenerateCellMap(Vector2Int resolution, ValueGenerationSettings baseLayerSettings,
            ValueGenerationSettings mountainLayerSettings, ValueGenerationSettings outdoorBiomSetting)
        {
            _cellMap = new Cell[resolution.x, resolution.y];
            _indoorCells = new List<Cell>();
            _outdoorCells = new List<Cell>();
            
            System.Random prng = new System.Random(outdoorBiomSetting.GetSeed().GetHashCode());
            Debug.Log("outdoorBiomSetting.GetSeed().GetHashCode(): " + outdoorBiomSetting.GetSeed().GetHashCode());

            // Generate CellMap for indoor/outdoor
            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Cell cell = new Cell(new Vector2Int(x, y));
                    // cell.CellIndex = new Vector2Int(x, y);

                    var val = ValueGenerator.Evaluate(x, y, baseLayerSettings);

                    if (val == 1)
                    {
                        cell.Indoors = true;
                        _indoorCells.Add(cell);
                    }
                    else
                    {
                        cell.Indoors = false;
                        _outdoorCells.Add(cell);
                    }

                    _cellMap[x, y] = cell;
                }
            }

            // Get the values of the neighbours
            // If one or more neighbours is different from the current cell, make the current cell a wall
            //  foreach (var cell in _cellMap)
            // {
            //     // get the coordinates of all 8 neighbours
            //     for (int x = -1; x <= 1; x++)
            //     {
            //         for (int y = -1; y <= 1; y++)
            //         {
            //             int xPos = cell.CellIndex.x + x;
            //             int yPos = cell.CellIndex.y + y;
            //
            //             // skip the incoming cell, and cell coordinates that are not in the map
            //             if ((xPos == cell.CellIndex.x && yPos == cell.CellIndex.y) || xPos < 0 || yPos < 0 ||
            //                 xPos >= resolution.x || yPos >= resolution.y)
            //             {
            //                 continue;
            //             }
            //
            //             bool neighbourVal = _cellMap[xPos, yPos].Indoors;
            //
            //             if (neighbourVal != cell.Indoors)
            //             {
            //                 // cell.isWall = true;
            //             }
            //
            //             cell.Neighbours.Add(_cellMap[xPos, yPos]);
            //         }
            //     }
            // }


            // mountain layer generation
            foreach (var cell in _indoorCells)
            {
                // all indoor cells are currently cave
                cell.Biom = Biom.Cave;

                var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, mountainLayerSettings);

                CellAsset asset = new CellAsset();

                if (val == 1)
                {
                    // cell is massive rock
                    asset.Collidable = true;
                    asset.Type = CellAsset.AssetType.Rock;
                }
                else
                {
                    // cell is a cavity
                    asset.Collidable = false;
                    asset.Type = CellAsset.AssetType.None;
                }

                cell.Asset = asset;
            }

            // open terrain layer generation
            foreach (var cell in _outdoorCells)
            {
                var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, outdoorBiomSetting);

                CellAsset asset = new CellAsset();

                if (val == 1)
                {
                    // cell is Meadow
                    cell.Biom = Biom.Meadows;
                    asset.Collidable = false;
                }
                else
                {
                    // cell is Woods
                    cell.Biom = Biom.Woods;

                    var value = prng.Next(11);
                    Debug.Log("prng.Next(0, 11): " + value);
                    
                    if (value <= 5)
                    {
                        asset.Collidable = true;
                        asset.Type = CellAsset.AssetType.Tree;
                    }
                    else if (value <= 8)
                    {
                        asset.Collidable = true;
                        asset.Type = CellAsset.AssetType.Bush;
                    }
                    else
                    {
                        asset.Collidable = false;
                        asset.Type = CellAsset.AssetType.None;
                    }
                }

                cell.Asset = asset;
            }

            return _cellMap;
        }
    }
}