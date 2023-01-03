using System;
using System.Collections.Generic;
using _Scripts._GradientNoise.ValueGeneration;
using _Scripts.CellGeneration;
using UnityEngine;

namespace _Scripts._GradientNoise
{
    /**
     * This class stores the parameters for each generation step.
     */
    [Serializable] // With this it can be showed in the Inspector
    public class AssetGenerationSettings
    {
        // trees, bushes
        [Range(1, 100)] public int treePercentage;
        [Range(1, 100)] public int bushPercentage;
        [Range(1, 100)] public int grassPercentage;
    }

    /**
     * This class generates the cell map.
     * First: The Base Layer (in-/outdoors).
     * Second: The Mountain Layers and its Bioms.
     * Third: The Open Terrain Layer and its Bioms.
     */
    public class CellMapGenerator
    {
        private Cell[,] _cellMap;
        private List<Cell> _indoorCells;
        private List<Cell> _outdoorCells;

        public Cell[,] GenerateCellMap(Vector2Int resolution, ValueGenerationSettings baseLayerSettings,
            ValueGenerationSettings mountainLayerSettings, ValueGenerationSettings outdoorBiomSetting,
            AssetGenerationSettings assetGenerationSettings)
        {
            _cellMap = new Cell[resolution.x, resolution.y];
            _indoorCells = new List<Cell>();
            _outdoorCells = new List<Cell>();

            // Needed to create the Woods Biom (outdoors)
            System.Random prng = new System.Random(outdoorBiomSetting.GetSeed().GetHashCode());

            // Generate CellMap for indoor/outdoor
            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Cell cell = new Cell(new Vector2Int(x, y));

                    // Can be generated with Open Simplex Noise, Perlin Noise
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

            // mountain layer generation
            foreach (var cell in _indoorCells)
            {
                // all indoor cells are cave
                cell.Biom = Biom.Cave;
            
                // Can be generated with Open Simplex Noise, Perlin Noise
                var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, mountainLayerSettings);
            
                cell.Asset = val == 1
                    ? new CellAsset(CellAsset.AssetType.MassiveRock)
                    : new CellAsset(CellAsset.AssetType.Cavity);
            }
            
            // open terrain layer generation
            foreach (var cell in _outdoorCells)
            {
                // Can be generated with Open Simplex Noise, Perlin Noise
                var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, outdoorBiomSetting);
            
                if (val == 1)
                {
                    // cell is Meadows
                    cell.Biom = Biom.Meadows;
                }
                else
                {
                    // cell is Woods
                    cell.Biom = Biom.Woods;
            
                    var value = prng.Next(101);
                    var trees = assetGenerationSettings.treePercentage;
                    var bushes = assetGenerationSettings.bushPercentage;
                    var gras = assetGenerationSettings.grassPercentage;
            
                    if (trees + bushes + gras > 100)
                    {
                        Debug.LogError("More than 100% Assets.");
                    }
                    else
                    {
                        if (value <= trees)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Tree);
                        }
                        else if (value <= trees + bushes)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Bush);
                        }
                    }
                }
            }
            
            // Get the values of the neighbours
            // If one or more neighbours are different from the current cell, make the current cell a wall
            foreach (var cell in _indoorCells)
            {
                // get the coordinates of all 8 neighbours
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int xPos = cell.CellIndex.x + x;
                        int yPos = cell.CellIndex.y + y;
            
                        // skip the incoming cell, and cell coordinates that are not in the map
                        if ((xPos == cell.CellIndex.x && yPos == cell.CellIndex.y) || xPos < 0 || yPos < 0 ||
                            xPos >= resolution.x || yPos >= resolution.y)
                        {
                            continue;
                        }
            
                        var neighbourVal = _cellMap[xPos, yPos].Indoors;
                        var neighbourAsset = _cellMap[xPos, yPos].Asset;
            
                        if (neighbourVal == false)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Wall);
                        }
            
                        if (cell.Asset.Type == CellAsset.AssetType.MassiveRock &&
                            neighbourAsset.Type == CellAsset.AssetType.Cavity)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Wall);
                        }
                    }
                }
            }

            return _cellMap;
        }
    }
}