using System;
using System.Collections.Generic;
using _Scripts.CellGeneration;
using _Scripts.ValueGeneration;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts
{
    public class PseudoRandomCellMapGenerator
    {
        private List<Cell> _indoorCells;
        private List<Cell> _outdoorCells;

        public static Cell[,] Evaluate(Vector2Int resolution, ValueGenerationSettings settings, out List<Cell> indoorCells, out List<Cell> outdoorCells)
        {
            Cell[,] cellMap = new Cell[resolution.x, resolution.y];
            
            System.Random prng = new System.Random(settings.GetSeed().GetHashCode());

            if (settings.useRandomSeed)
            {
                settings.SetSeed(Time.realtimeSinceStartupAsDouble.ToString());
            }

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Cell cell = new Cell(x, y);

                    if (prng.Next(101) <= settings.thresholdPercentage)
                    {
                        cell.Indoors = true;
                    }
                    else
                    {
                        cell.Indoors = false;
                    }

                    cellMap[x, y] = cell;
                }
            }

            cellMap = SmoothCellMap(cellMap, settings, resolution);
            GetInAndOutdoorCells(cellMap, out indoorCells, out outdoorCells, resolution);
            
            return cellMap;
        }

        // Smooth the Cell Map
        private static Cell[,] SmoothCellMap(Cell[,] cellMap, ValueGenerationSettings settings, Vector2Int resolution)
        {
            if (cellMap == null)
            {
                // Debug.LogError("CellMap to smooth equals null.");
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
                        tempCellMap[x, y] = ApplyFloorRules(cellMap, cellMap[x, y], resolution);
                    }
                }

                cellMap = tempCellMap;
            }

            return cellMap;
        }


        // Apply rules to the cell
        private static Cell ApplyFloorRules(Cell[,] cellMap, Cell cell, Vector2Int resolution)
        {
            Cell tempCell = cell;

            int neighbours = GetSimilarNeighbours(cellMap, cell, resolution);

            if (neighbours >= 4)
            {
                tempCell.Indoors = cell.Indoors == true;
            }
            else
            {
                tempCell.Indoors = cell.Indoors != true;
            }

            cell = tempCell;
            return cell;
        }


        // Get the number of similar neighbours
        private static int GetSimilarNeighbours(Cell[,] cellMap, Cell tempCell, Vector2Int resolution)
        {
            bool myVal = tempCell.Indoors;
            int similarNeighbourCount = 0;

            // get the coordinates of all 8 neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int xPos = tempCell.CellIndex.x + x;
                    int yPos = tempCell.CellIndex.y + y;

                    // skip the incoming cell, and cell coordinates that are not in the map
                    if ((xPos == tempCell.CellIndex.x && yPos == tempCell.CellIndex.y) || xPos < 0 || yPos < 0 ||
                        xPos >= resolution.x || yPos >= resolution.y)
                    {
                        continue;
                    }

                    bool neighbourVal = cellMap[xPos, yPos].Indoors;

                    if (neighbourVal == myVal)
                    {
                        similarNeighbourCount++;
                    }

                    // cell.neighbours.Add(_cellMap[xPos, yPos]);
                }
            }

            // outCell = cell;
            return similarNeighbourCount;
        }

        private static void GetInAndOutdoorCells(Cell[,] cellMap, out  List<Cell> indoorCells, out List<Cell> outdoorCells, Vector2Int resolution)
        {
            indoorCells = new List<Cell>();
            outdoorCells = new List<Cell>();
            
            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    if (cellMap[x,y].Indoors)
                    {
                        indoorCells.Add(cellMap[x,y]);
                    }
                    else
                    {
                        outdoorCells.Add(cellMap[x,y]);
                    }
                }
            }
        }
    }
}