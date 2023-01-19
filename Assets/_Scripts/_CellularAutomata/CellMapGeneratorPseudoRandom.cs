using System;
using System.Collections.Generic;
using _Scripts.CellGeneration;
using UnityEngine;

namespace _Scripts._PseudoRandom
{
    /**
     * This class stores the parameters for each generation step.
     */
    [Serializable] // With this it can be shown in the Inspector
    public class AssetGenerationSettings
    {
        // trees, bushes
        [Range(1, 100)] public int treePercentage = 50;
        [Range(1, 100)] public int bushPercentage = 30;
        [Range(1, 100)] public int grassPercentage = 10;
        [Range(1, 100)] public int stonePercentage = 10;
    }

    /**
     * This class stores the parameters for each generation step.
     */
    [Serializable] // With this it can be shown in the Inspector
    public class MapGenerationSettings
    {
        // General
        [Header("General")] [Range(0, 100)] public int thresholdPercentage = 45;
        [Range(0, 100)] public int similarNeighboursPercentage = 45;

        // Seed
        [Header("Seed")] public bool useRandomSeed = true;
        private bool _seedLocked;
        [SerializeField] private string seed = "Hello World!";

        // Pseudo random settings
        [Header("Pseudo Random")] public int smoothSteps = 7;

        [Header("For Mountain Generation")] public int stonePercentage = 10;

        // Seed can only be changed if there is no seed.
        public void SetSeed(string inSeed)
        {
            if (!_seedLocked)
            {
                seed = inSeed;
                _seedLocked = true;
            }
        }

        public string GetSeed()
        {
            return seed;
        }
    }

    /**
     * This class generates the cell map.
     * First: The Base Layer (in-/outdoors).
     * Second: The Mountain Layers and its Bioms.
     * Third: The Open Terrain Layer and its Bioms.
     */
    public class CellMapGeneratorPseudoRandom
    {
        private Cell[,] _cellMap;
        private List<Cell> _indoorCells;
        private List<Cell> _outdoorCells;

        // Generate the map with the given settings
        public Cell[,] GenerateCellMap(Vector2Int resolution, MapGenerationSettings baseLayerSettings,
            MapGenerationSettings mountainLayerSettings, MapGenerationSettings outdoorBiomSettings,
            MapGenerationSettings waterLayerSettings, AssetGenerationSettings meadowsAssetSettings,
            AssetGenerationSettings woodsAssetSettings)
        {
            _cellMap = new Cell[resolution.x, resolution.y];
            _indoorCells = new List<Cell>();
            _outdoorCells = new List<Cell>();

            // Check if random seeds are wanted
            if (baseLayerSettings.useRandomSeed)
            {
                baseLayerSettings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
            }

            if (mountainLayerSettings.useRandomSeed)
            {
                mountainLayerSettings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
            }

            if (outdoorBiomSettings.useRandomSeed)
            {
                outdoorBiomSettings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
            }

            if (waterLayerSettings.useRandomSeed)
            {
                waterLayerSettings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
            }

            // Create PRNGs with the given seeds
            System.Random prng = new System.Random(baseLayerSettings.GetSeed().GetHashCode());
            System.Random prng1 = new System.Random(mountainLayerSettings.GetSeed().GetHashCode());
            System.Random prng2 = new System.Random(outdoorBiomSettings.GetSeed().GetHashCode());
            System.Random prng3 = new System.Random(waterLayerSettings.GetSeed().GetHashCode());

            // Generate the base layer and get the in- and outdoor cells
            _cellMap = GenerateBaseLayer(resolution, baseLayerSettings, prng);
            GetInAndOutdoorCells(_cellMap, out _indoorCells, out _outdoorCells, resolution);

            // Generate the mountain/indoor layer
            _cellMap = GenerateMountainLayer(_cellMap, _indoorCells, mountainLayerSettings, prng1);

            // Generate the open terrain/outdoor layer
            _cellMap = GenerateOpenTerrainLayer(_cellMap, _outdoorCells, outdoorBiomSettings,
                meadowsAssetSettings, woodsAssetSettings, prng2);

            // Generate walls
            _cellMap = GenerateWalls(_cellMap, resolution, _indoorCells);

            // Generate the water layer
            _cellMap = GenerateWaterLayer(_cellMap, waterLayerSettings, prng3);

            return _cellMap;
        }

        /*
         * Generate the Base Layer.
         * This contains the information whether a cell is in- or outdoors.
         */
        private Cell[,] GenerateBaseLayer(Vector2Int resolution, MapGenerationSettings baseLayerSettings,
            System.Random prng)
        {
            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Cell cell = new Cell(x, y);

                    cell.Indoors = prng.Next(101) < baseLayerSettings.thresholdPercentage;

                    _cellMap[x, y] = cell;
                }
            }

            // Smooth the map and return it
            return SmoothCellMap(_cellMap, baseLayerSettings, resolution,
                baseLayerSettings.similarNeighboursPercentage);
        }

        /*
         * Generate the Mountain (indoors) Layer.
         * This contains the information whether a cell is massive rock or a cavity.
         */
        private Cell[,] GenerateMountainLayer(Cell[,] cellMap, List<Cell> indoorCells,
            MapGenerationSettings mountainLayerSettings,
            System.Random prng)
        {
            foreach (var cell in indoorCells)
            {
                // All indoor cells are cave
                cell.Biom = Biom.Mountain;

                if (prng.Next(101) < mountainLayerSettings.thresholdPercentage)
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

                cellMap[cell.CellIndex.x, cell.CellIndex.y] = cell;
            }

            return cellMap;
        }

        /*
         * Generate the Open Terrain (outdoors) Layer.
         * This contains the information whether a cell is meadows or woods and whether has trees or bushes.
         */
        private Cell[,] GenerateOpenTerrainLayer(Cell[,] cellMap, List<Cell> outdoorCells,
            MapGenerationSettings outdoorBiomSettings, AssetGenerationSettings meadowsAssetSettings,
            AssetGenerationSettings woodsAssetSettings,
            System.Random prng)
        {
            foreach (var cell in outdoorCells)
            {
                if (prng.Next(101) < outdoorBiomSettings.thresholdPercentage)
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
                        Debug.LogError("More than 100% Trees and Bushes.");
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
                    // Cell is Woods
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


                cellMap[cell.CellIndex.x, cell.CellIndex.y] = cell;
            }

            return _cellMap;
        }

        /*
         * Generate the indoor cells containing walls.
         */
        private Cell[,] GenerateWalls(Cell[,] cellMap, Vector2Int resolution, List<Cell> indoorCells)
        {
            // Get the values of the neighbours
            // If one or more neighbours is different from the current cell, make the current cell a wall
            foreach (var cell in indoorCells)
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

                        bool neighbourVal = _cellMap[xPos, yPos].Indoors;
                        var neighbourAsset = _cellMap[xPos, yPos].Asset;

                        // If the neighbour is outdoors the cell becomes a wall
                        if (neighbourVal == false)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Wall);
                        }

                        // If the cell is indoors and massive rock and the neighbour is a cavity the cell becomes a wall
                        if (cell.Asset.Type == CellAsset.AssetType.MassiveRock &&
                            neighbourAsset.Type == CellAsset.AssetType.Cave)
                        {
                            cell.Asset = new CellAsset(CellAsset.AssetType.Wall);
                        }
                    }
                }

                cellMap[cell.CellIndex.x, cell.CellIndex.y] = cell;
            }

            return cellMap;
        }

        /*
         * Generate the Mountain (indoors) Layer.
         * This contains the information whether a cell is massive rock or a cavity.
         */
        private Cell[,] GenerateWaterLayer(Cell[,] cellMap, MapGenerationSettings waterLayerSettings,
            System.Random prng)
        {
            foreach (var cell in _cellMap)
            {
                if (cell.Biom == Biom.Meadows)
                {
                    if (prng.Next(101) < waterLayerSettings.thresholdPercentage)
                    {
                        cell.Asset = new CellAsset(CellAsset.AssetType.Water);
                    }
                }

                if (cell.Biom == Biom.Woods)
                {
                    if (prng.Next(101) < waterLayerSettings.thresholdPercentage)
                    {
                        cell.Asset = new CellAsset(CellAsset.AssetType.Water);
                    }
                }

                if (cell.Asset.Type == CellAsset.AssetType.Cave)
                {
                    if (prng.Next(101) < waterLayerSettings.thresholdPercentage)
                    {
                        cell.Asset = new CellAsset(CellAsset.AssetType.Water);
                    }
                }

                cellMap[cell.CellIndex.x, cell.CellIndex.y] = cell;
            }

            return cellMap;
        }

        /*
         * Smooth the cell map and apply the rules.
         */
        private static Cell[,] SmoothCellMap(Cell[,] cellMap, MapGenerationSettings settings,
            Vector2Int resolution, int similarNeighboursPercentage)
        {
            if (cellMap == null)
            {
                return null;
            }

            for (int i = 0; i < settings.smoothSteps; i++)
            {
                var xDimension = cellMap.GetLength(0);
                var yDimension = cellMap.GetLength(1);

                Cell[,] tempCellMap = new Cell[xDimension, yDimension];

                for (int x = 0; x < xDimension; x++)
                {
                    for (int y = 0; y < yDimension; y++)
                    {
                        tempCellMap[x, y] = ApplyFloorRules(cellMap, cellMap[x, y], resolution,
                            similarNeighboursPercentage);
                    }
                }

                cellMap = tempCellMap;
            }

            return cellMap;
        }


        /*
         * Apply the rule to the given cell.
         */
        private static Cell ApplyFloorRules(Cell[,] cellMap, Cell cell,
            Vector2Int resolution, int similarNeighboursPercentage)
        {
            Cell tempCell = cell;

            int neighbours = GetSimilarNeighbours(cellMap, cell, resolution);

            if (neighbours >= similarNeighboursPercentage)
            {
                tempCell.Indoors = cell.Indoors;
            }
            else
            {
                tempCell.Indoors = cell.Indoors != true;
            }

            cell = tempCell;
            return cell;
        }

        // // Apply rules to the mountain cell
        // private static CellPseudoRandom ApplyMountainRules(CellPseudoRandom[,] cellMap, CellPseudoRandom cell,
        //     Vector2Int resolution)
        // {
        //     CellPseudoRandom tempCell = cell;
        //
        //     int neighbours = GetSimilarNeighbours(cellMap, cell, resolution);
        //
        //     if (neighbours >= 4)
        //     {
        //         tempCell.Asset = cell.Asset;
        //     }
        //     else
        //     {
        //         if (cell.Asset.Type == CellAsset.AssetType.MassiveRock)
        //         {
        //             tempCell.Asset = new CellAsset(CellAsset.AssetType.Cavity);
        //         }
        //         else if (cell.Asset.Type == CellAsset.AssetType.Cavity)
        //         {
        //             tempCell.Asset = new CellAsset(CellAsset.AssetType.MassiveRock);
        //         }
        //     }
        //
        //     cell = tempCell;
        //     return cell;
        // }

        // Apply rules to the outdoor cell
        // private static CellPseudoRandom ApplyOutdoorRules(CellPseudoRandom[,] cellMap, CellPseudoRandom cell,
        //     Vector2Int resolution)
        // {
        //     CellPseudoRandom tempCell = cell;
        //
        //     int neighbours = GetSimilarNeighbours(cellMap, cell, resolution);
        //     int trees = 0;
        //     int bush = 0;
        //     int nothing = 0;
        //
        //     foreach (var neighbour in cell.neighbours)
        //     {
        //         if (neighbour.Asset.Type == CellAsset.AssetType.Tree)
        //         {
        //             trees++;
        //         }
        //         else if (neighbour.Asset.Type == CellAsset.AssetType.Bush)
        //         {
        //             bush++;
        //         }
        //         else if (neighbour.Asset.Type == CellAsset.AssetType.None)
        //         {
        //             nothing++;
        //         }
        //     }
        //
        //     if (neighbours >= 4)
        //     {
        //         tempCell.Asset = cell.Asset;
        //     }
        //     else
        //     {
        //         if (cell.Asset.Type == CellAsset.AssetType.Tree && bush >= 4)
        //         {
        //             tempCell.Asset = new CellAsset(CellAsset.AssetType.Bush);
        //         }
        //         else if (cell.Asset.Type == CellAsset.AssetType.Tree && nothing >= 4)
        //         {
        //             tempCell.Asset = new CellAsset(CellAsset.AssetType.None);
        //         }
        //         else if (cell.Asset.Type == CellAsset.AssetType.Bush && trees >= 4)
        //         {
        //             tempCell.Asset = new CellAsset(CellAsset.AssetType.Tree);
        //         }
        //         else if (cell.Asset.Type == CellAsset.AssetType.Bush && nothing >= 4)
        //         {
        //             tempCell.Asset = new CellAsset(CellAsset.AssetType.None);
        //         }
        //         else if (cell.Asset.Type == CellAsset.AssetType.None && trees >= 4)
        //         {
        //             tempCell.Asset = new CellAsset(CellAsset.AssetType.Tree);
        //         }
        //         else if (cell.Asset.Type == CellAsset.AssetType.None && bush >= 4)
        //         {
        //             tempCell.Asset = new CellAsset(CellAsset.AssetType.Bush);
        //         }
        //     }
        //
        //     cell = tempCell;
        //     return cell;
        // }


        /*
         * Get the number of similar neighbours.
         */
        private static int GetSimilarNeighbours(Cell[,] cellMap, Cell tempCell,
            Vector2Int resolution)
        {
            var myVal = tempCell.Indoors;
            int similarNeighbourCount = 0;

            // Get the coordinates of all 8 neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int xPos = tempCell.CellIndex.x + x;
                    int yPos = tempCell.CellIndex.y + y;

                    // Skip the incoming cell, and cell coordinates that are not in the map
                    if ((xPos == tempCell.CellIndex.x && yPos == tempCell.CellIndex.y) || xPos < 0 || yPos < 0 ||
                        xPos >= resolution.x || yPos >= resolution.y)
                    {
                        continue;
                    }

                    var neighbourVal = cellMap[xPos, yPos].Indoors;

                    if (neighbourVal == myVal)
                    {
                        similarNeighbourCount++;
                    }
                }
            }

            return similarNeighbourCount;
        }

        /*
         * Add all indoor cells to the indoorCells list and all outdoor cells to the outdoorCells list.
         */
        private static void GetInAndOutdoorCells(Cell[,] cellMap, out List<Cell> indoorCells,
            out List<Cell> outdoorCells, Vector2Int resolution)
        {
            indoorCells = new List<Cell>();
            outdoorCells = new List<Cell>();

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    if (cellMap[x, y].Indoors)
                    {
                        indoorCells.Add(cellMap[x, y]);
                    }
                    else
                    {
                        outdoorCells.Add(cellMap[x, y]);
                    }
                }
            }
        }
    }
}