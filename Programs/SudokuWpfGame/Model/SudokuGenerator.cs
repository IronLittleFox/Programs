using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuWpfGame.Model
{
    public class SudokuGenerator
    {
        private List<List<int>> sudoku;
        public List<List<int>> Sudoku
        {
            get { return sudoku; }
            set { sudoku = value; }
        }

        private List<List<int>> sudokuWidthRemovingNumbers;
        public List<List<int>> SudokuWidthRemovingNumbers
        {
            get { return sudokuWidthRemovingNumbers; }
            set { sudokuWidthRemovingNumbers = value; }
        }

        private int size;

        public SudokuGenerator(int size)
        {
            this.size = size;
            sudoku = Enumerable.Range(0, 3 * size).Select(_ => new List<int>(Enumerable.Repeat(0, size * size))).ToList();
            FillGrid();

            sudokuWidthRemovingNumbers = sudoku.Select(row => new List<int>(row)).ToList();
            SudokuRemovingNumbers();
        }

        private bool FillGrid()
        {
            //List<int> numberList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<int> numberList = Enumerable.Range(1, size * size).ToList();
            Random random = new Random();
            // Znajdź następną pustą komórkę
            for (int i = 0; i < 9 * size * size; i++)
            {
                int row = i / (3 * size);
                int col = i % (3 * size);

                if (sudoku[row][col] == 0)
                {
                    numberList = numberList.OrderBy(n => random.Next()).ToList();

                    foreach (int value in numberList)
                    {
                        // Sprawdź, czy ta wartość nie została już użyta w tym wierszu lub kolumnie
                        if (!Enumerable.Range(0, 3 * size).Any(col => sudoku[row][col] == value)
                            && !Enumerable.Range(0, 3 * size).Any(row => sudoku[row][col] == value))
                        {
                            // Zidentyfikuj, na którym z 9 kwadratów pracujemy
                            int minRow, maxRow, minCol, maxCol;
                            minRow = row < size ? 0 : (row < 2 * size ? size : (2 * size));
                            maxRow = row < size ? size : (row < 2 * size ? 2 * size : (3 * size));
                            minCol = col < size ? 0 : (col < 2 * size ? size : (2 * size));
                            maxCol = col < size ? size : (col < 2 * size ? 2 * size : (3 * size));

                            List<List<int>> square = sudoku.Skip(minRow).Take(maxRow - minRow).Select(rowList => rowList.Skip(minCol).Take(maxCol - minCol).ToList()).ToList();

                            // Sprawdź, czy ta wartość nie została już użyta w tym kwadracie 3x3
                            if (!square.Any(row => row.Contains(value)))
                            {
                                sudoku[row][col] = value;

                                if (!sudoku.Any(rowList => rowList.Any(n => n == 0)))
                                    return true;
                                else
                                {
                                    if (FillGrid())
                                        return true;
                                }
                                sudoku[row][col] = 0;
                            }
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        private void SudokuRemovingNumbers()
        {
            int attempts = 5;
            Random random = new Random();
            while (attempts > 0)
            {
                //#Select a random cell that is not already empty
                int row, col;
                do
                {
                    row = random.Next(0, 3 * size);
                    col = random.Next(0, 3 * size);
                } while (sudokuWidthRemovingNumbers[row][col] == 0);

                //#Remember its cell value in case we need to put it back  
                int backup = sudokuWidthRemovingNumbers[row][col];
                sudokuWidthRemovingNumbers[row][col] = 0;

                //# Take a full copy of the grid
                List<List<int>> gridCopy = sudokuWidthRemovingNumbers.Select(row => new List<int>(row)).ToList();

                int numberOfSolutions = 0;

                SolveGrid(gridCopy, ref numberOfSolutions);
                if (numberOfSolutions != 1)
                {
                    sudokuWidthRemovingNumbers[row][col] = backup;
                    attempts -= 1;
                }
            }
        }

        private void SolveGrid(List<List<int>> grid, ref int numberOfSolutions)
        {
            for (int i = 0; i < 9 * size * size; i++)
            {
                int row = i / (3 * size);
                int col = i % (3 * size);
                if (grid[row][col] == 0)
                {
                    for (int value = 1; value <= size * size; value++)
                    {
                        // Sprawdź, czy ta wartość nie została już użyta w tym wierszu lub kolumnie
                        if (!Enumerable.Range(0, 3 * size).Any(col => grid[row][col] == value)
                            && !Enumerable.Range(0, 3 * size).Any(row => grid[row][col] == value))
                        {
                            // Zidentyfikuj, na którym z 9 kwadratów pracujemy
                            int minRow, maxRow, minCol, maxCol;
                            minRow = row < size ? 0 : (row < 2 * size ? size : (2 * size));
                            maxRow = row < size ? size : (row < 2 * size ? 2 * size : (3 * size));
                            minCol = col < size ? 0 : (col < 2 * size ? size : (2 * size));
                            maxCol = col < size ? size : (col < 2 * size ? 2 * size : (3 * size));

                            List<List<int>> square = grid.Skip(minRow).Take(maxRow - minRow).Select(rowList => rowList.Skip(minCol).Take(maxCol - minCol).ToList()).ToList();

                            // Sprawdź, czy ta wartość nie została już użyta w tym kwadracie 3x3
                            if (!square.Any(row => row.Contains(value)))
                            {
                                grid[row][col] = value;
                                if (!grid.Any(rowList => rowList.Any(n => n == 0)))
                                {
                                    grid[row][col] = 0;
                                    numberOfSolutions++;
                                    return;
                                }
                                SolveGrid(grid, ref numberOfSolutions);
                                if (numberOfSolutions > 1)
                                    return;
                            }
                        }
                    }
                    return;
                }
            }
            return;
        }
    }
}
