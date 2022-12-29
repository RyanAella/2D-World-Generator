using System.Collections.Generic;
using _Scripts.CellGeneration;
using _Scripts.Helper;
using UnityEngine;

namespace _Scripts.old.CellMapGenerator
{
    public class PseudoRandomCellMapGenerator
    {
        // private properties
        private Cell[,] _cellMap;
        private List<Cell> _outdoorMap;
        private List<Cell> _indoorMap;

        /**
         * Generate a CellMap
         */
        public Cell[,] GenerateCellMap(Vector2Int resolution, bool useRandomSeed, string seed, int indoorsPercentage)
        {
            _cellMap = new Cell[resolution.x, resolution.y];

            if (useRandomSeed)
            {
                seed = Time.time.ToString();
            }

            System.Random prng = new System.Random(seed.GetHashCode());

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    Cell cell = new Cell(x, y);

                    // Next(n) -> value between inclusive 0 and exclusive n
                    if (prng.Next(101) < indoorsPercentage)
                    {
                        cell.Indoors = true;
                    }
                    else
                    {
                        cell.Indoors = false;
                    }

                    _cellMap[x, y] = cell;
                }
            }

            return _cellMap;
        }

        /**
         * Smooth the cellMap
         */
        public Cell[,] SmoothCellMap(Cell[,] cellMap, int smoothSteps, Vector2Int resolution)
        {
            if (cellMap == null)
            {
                Debug.LogError("CellMap to smooth equals null.");
                return null;
            }

            for (int i = 0; i < smoothSteps; i++)
            {
                var xDimension = cellMap.GetLength(0);
                var yDimension = cellMap.GetLength(1);

                Cell[,] tempCellMap = new Cell[xDimension, yDimension];

                for (int x = 0; x < xDimension; x++)
                {
                    for (int y = 0; y < yDimension; y++)
                    {
                        tempCellMap[x, y] = ApplyFloorRules(cellMap[x, y], resolution);
                    }
                }

                cellMap = tempCellMap;
            }


            GetNeighbours(cellMap);

            return cellMap;
        }


        /**
         * Apply rules to the cell
         */
        private Cell ApplyFloorRules(Cell cell, Vector2Int resolution)
        {
            Cell tempCell = cell;

            int neighbours = GetSimilarNeighbours(cell, /*out tempCell,*/ resolution);

            if (neighbours >= 4)
            {
                if (cell.Indoors == true)
                {
                    tempCell.Indoors = true;
                }
                else
                {
                    tempCell.Indoors = false;
                }
            }
            else
            {
                if (cell.Indoors == true)
                {
                    tempCell.Indoors = false;
                }
                else
                {
                    tempCell.Indoors = true;
                }
            }

            cell = tempCell;
            return cell;
        }

        /**
         * Get the number of similar neighbours
         */
        private int GetSimilarNeighbours(Cell tempCell, /*out Cell outCell,*/ Vector2Int resolution)
        {
            // Cell cell = new Cell(tempCell.cellIndex);

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

                    bool neighbourVal = _cellMap[xPos, yPos].Indoors;

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

        private void GetNeighbours(Cell[,] cellMap)
        {
            for (int x = 0; x < cellMap.GetLength(0); x++)
            {
                for (int y = 0; y < cellMap.GetLength(1); y++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int xPos = cellMap[x, y].CellIndex.x + i;
                            int yPos = cellMap[x, y].CellIndex.y + j;

                            // skip the incoming cell, and cell coordinates that are not in the map
                            if ((xPos == cellMap[x, y].CellIndex.x && yPos == cellMap[x, y].CellIndex.y) || xPos < 0 ||
                                yPos < 0 ||
                                xPos >= cellMap.GetLength(0) || yPos >= cellMap.GetLength(1))
                            {
                                continue;
                            }

                            cellMap[x, y].Neighbours.Add(_cellMap[xPos, yPos]);
                        }
                    }
                }
            }
        }
    }
}