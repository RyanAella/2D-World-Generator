using UnityEngine;

namespace _Scripts
{
    public class CellMapGenerator
    {
        // private properties
        private Cell[,] _cellMap;

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
                        cell.indoors = true;
                    }
                    else
                    {
                        cell.indoors = false;
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
            // if (cellMap == null) return;
        
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

            GetSimilarNeighbours(cellMap);

            return cellMap;
        }

        /**
         * Apply rules to the cell
         */
        private Cell ApplyFloorRules(Cell cell, Vector2Int resolution)
        {
            Cell tempCell = cell;
        
            int neighbours = GetSimilarNeighbours(cell, /*out tempCell,*/ resolution);
            
            // Check if neighbours fit rules
            // if (cell.indoors)
            // {
            //     if (neighbours >= 4)
            //     {
            //         tempCell.indoors = cell.indoors;
            //     }
            //     else if (neighbours < 4)
            //     {
            //         tempCell.indoors = !cell.indoors;
            //     }
            // }
            // else if (!cell.indoors)
            // {
            //     if (neighbours >= 4)
            //     {
            //         tempCell.indoors = !cell.indoors;
            //     }
            //     else if (neighbours < 4)
            //     {
            //         tempCell.indoors = cell.indoors;
            //     }
            // }
            
            if (neighbours >= 4)
            {
                if (cell.indoors == true)
                {
                    tempCell.indoors = true;
                }
                else
                {
                    tempCell.indoors = false;
                }
                
            }
            else
            {
                if (cell.indoors == true)
                {
                    tempCell.indoors = false;
                }
                else
                {
                    tempCell.indoors = true;
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
            Cell cell = new Cell(tempCell.cellIndex);
        
            bool myVal = tempCell.indoors;
            int similarNeighbourCount = 0;
        
            // get the coordinates of all 8 neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int xPos = tempCell.cellIndex.x + x;
                    int yPos = tempCell.cellIndex.y + y;
        
                    // skip the incoming cell, and cell coordinates that are not in the map
                    if ((xPos == tempCell.cellIndex.x && yPos == tempCell.cellIndex.y) || xPos < 0 || yPos < 0 ||
                        xPos >= resolution.x || yPos >= resolution.y)
                    {
                        Debug.Log("Skip position");
                        continue;
                    }
        
                    bool neighbourVal = _cellMap[xPos, yPos].indoors;
        
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

        private void GetSimilarNeighbours(Cell[,] cellMap)
        {
            for (int x = 0; x < cellMap.GetLength(0); x++)
            {
                for (int y = 0; y < cellMap.GetLength(1); y++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int xPos = cellMap[x,y].cellIndex.x + i;
                            int yPos = cellMap[x,y].cellIndex.y + j;
        
                            // skip the incoming cell, and cell coordinates that are not in the map
                            if ((xPos == cellMap[x,y].cellIndex.x && yPos == cellMap[x,y].cellIndex.y) || xPos < 0 || yPos < 0 ||
                                xPos >= cellMap.GetLength(0) || yPos >= cellMap.GetLength(1))
                            {
                                Debug.Log("Skip position");
                                continue;
                            }

                            cellMap[x,y].neighbours.Add(_cellMap[xPos, yPos]);
                        }
                    }
                }
            }
        }
    }
}