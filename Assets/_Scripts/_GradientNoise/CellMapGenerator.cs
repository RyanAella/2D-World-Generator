using System.Collections.Generic;
using _Scripts._GradientNoise.ValueGeneration;
using _Scripts.CellGeneration;
using _Scripts.ScriptableObjects;
using _Scripts.ScriptableObjects.AssetGenerationSettings;
using _Scripts.ScriptableObjects.ValueGenerationSettings;
using UnityEngine;

namespace _Scripts._GradientNoise
{
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

        /**
         * Generate a CellMap with the given settings.
         */
        public Cell[,] GenerateCellMap(Vector2Int resolution, ValueGenerationSettings baseLayerSettings,
                ValueGenerationSettings mountainLayerSettings, ValueGenerationSettings outdoorBiomSetting,
                ValueGenerationSettings waterLayerSettings, AssetGenerationSettings meadowsAssetSettings,
                AssetGenerationSettings woodsAssetSettings)
            // public Cell[,] GenerateCellMap(Vector2Int resolution,  List<ValueGenerationSettings> valueGenerationSettings, AssetGenerationSettings assetGenerationSettings2)
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

            // Mountain layer generation
            foreach (var cell in _indoorCells)
            {
                // All indoor cells are cave
                cell.Biom = Biom.Mountain;

                // Can be generated with Open Simplex Noise, Perlin Noise
                var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, mountainLayerSettings);

                if (val == 1)
                {
                    cell.Asset = new CellAsset(CellAsset.AssetType.MassiveRock);
                }
                else
                {
                    cell.Asset = new CellAsset(CellAsset.AssetType.Cave);

                    var value = prng.Next(101);
                    var stone = mountainLayerSettings.stonePercentage;

                    if (value <= stone)
                    {
                        cell.Asset = new CellAsset(CellAsset.AssetType.Stone);
                    }
                }
            }
            
            // Generate walls
            // Get the values of the neighbours
            // If one or more neighbours are different from the current cell, make the current cell a wall
            foreach (var cell in _indoorCells)
            {
                // Get the coordinates of all 8 neighbours
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int xPos = cell.CellIndex.x + x;
                        int yPos = cell.CellIndex.y + y;

                        // Skip the incoming cell, and cell coordinates that are not in the map
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
                            neighbourAsset.Type is CellAsset.AssetType.Cave or CellAsset.AssetType.Stone)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Wall);
                        }
                    }
                }
            }

            // Open terrain layer generation
            foreach (var cell in _outdoorCells)
            {
                // Can be generated with Open Simplex Noise, Perlin Noise
                var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, outdoorBiomSetting);

                if (val == 1)
                {
                    // Cell is Meadows
                    cell.Biom = Biom.Meadows;

                    var value = prng.Next(101);
                    var trees = meadowsAssetSettings.treePercentage;
                    var bushes = meadowsAssetSettings.bushPercentage;
                    var grass = meadowsAssetSettings.grassPercentage;
                    var stone = meadowsAssetSettings.stonePercentage;

                    if (trees + bushes + grass + stone > 100)
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
                        else if (value <= trees + bushes + grass)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Grass);
                        }
                        else if (value <= trees + bushes + grass + stone)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Stone);
                        }
                    }
                }
                else
                {
                    // cell is Woods
                    cell.Biom = Biom.Woods;

                    var value = prng.Next(101);
                    var trees = woodsAssetSettings.treePercentage;
                    var bushes = woodsAssetSettings.bushPercentage;
                    var grass = woodsAssetSettings.grassPercentage;
                    var stone = woodsAssetSettings.stonePercentage;

                    if (trees + bushes + grass + stone > 100)
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
                        else if (value <= trees + bushes + grass)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Grass);
                        }
                        else if (value <= trees + bushes + grass + stone)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Stone);
                        }
                    }
                }
            }

            // Water layer generation
            foreach (var cell in _cellMap)
            {
                if (cell.Biom == Biom.Meadows)
                {
                    // Can be generated with Open Simplex Noise, Perlin Noise
                    var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, waterLayerSettings);

                    if (val == 1)
                    {
                        cell.Asset = new CellAsset(CellAsset.AssetType.Water);
                    }
                }

                if (cell.Biom == Biom.Woods)
                {
                    // Can be generated with Open Simplex Noise, Perlin Noise
                    var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, waterLayerSettings);

                    if (val == 1)
                    {
                        cell.Asset = new CellAsset(CellAsset.AssetType.Water);
                    }
                }

                if (cell.Asset.Type == CellAsset.AssetType.Cave)
                {
                    // Can be generated with Open Simplex Noise, Perlin Noise
                    var val = ValueGenerator.Evaluate(cell.CellIndex.x, cell.CellIndex.y, waterLayerSettings);

                    if (val == 1)
                    {
                        cell.Asset = new CellAsset(CellAsset.AssetType.Water);
                    }
                }
            }

            return _cellMap;
        }
    }
}