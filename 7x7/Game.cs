using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7x7
{
    public class Game
    {
        public string[] squares;    //contains colors for each square
        public readonly string[] colors = new string[5]
        {
            "#f44336",
            "#9c27b0",
            "#2196f3",
            "#4caf50",
            "#ff9800"
        };



        public const int MAX_COUNT = 49;

        public int deletedRows = 0; //score
        private int howMuchGenerate = 3;

        public bool GameOver { get
            {
                return countNullSquares() == 0; //game over if no empty squares
            } }
        
        public Game()
        {
            squares = new string[MAX_COUNT];
        } 

        public void GenerateNewSquares()
        {
            Random random = new Random();
            int generated = 0;
            int place = 0;
            int color = 0;
            while (generated < howMuchGenerate && countNullSquares() != 0)    //try to generate if there are empty squares
            {
                color = random.Next(0, colors.Length);
                place = random.Next(0, MAX_COUNT);
                if (squares[place] == null) //check that generated place is empty
                {
                    squares[place] = colors[color];

                    findAndDeleteLinesFor(place);   //maybe generated sqares will make line

                    generated++;
                }
            }
        }

        private bool findAndDeleteLinesFor(int square)  //tells if smth was deleted
        {
            List<int> forDelete = findLine(square);
            if (forDelete.Count != 0)
            {
                deleteSquares(forDelete);
                return true;
            }
            else return false;
        }

        private int countNullSquares()
        {
            int result = 0;
            for (int i = 0; i < MAX_COUNT; i++)
            {
                if (squares[i] == null)
                    result++;
            }
            return result;
        }

        public void findAvailableSquares(int square, ref List<int> result)
        {
            //recursive method. it's like depth-first search in graphs
            int[] offsets = new int[4] { -7, 1, 7, -1 };    //we can move up, right, down, left
            for (int i = 0; i < offsets.Length; i++)
            {
                int newSquare = square + offsets[i];
                if (newSquare >= 0 && newSquare < 49 && squares[newSquare] == null && !result.Contains(newSquare))
                {
                    if (!((i == 1 || i == 3) && isNewLine(square, offsets[i]))) //do not go to new line if we're moving left|right
                    {
                        result.Add(newSquare);
                        findAvailableSquares(newSquare, ref result);
                    }
                }

            }
        }

        private bool isNewLine(int square, int offset)
        {
            return square / 7 != (square + offset) / 7;
        }

        public List<int> findNotAvailableSquaresFor(int square)
        {
            List<int> result = new List<int>();
            List<int> availableSquares = new List<int>();
            findAvailableSquares(square, ref availableSquares);
            for (int i = 0; i < MAX_COUNT; i++)
                if (!availableSquares.Contains(i) && squares[i] == null)
                    result.Add(i);
            return result;
        }

        public void makeMove(int from, int to)
        {
            if (squares[to] == null)
            {
                //move squares
                squares[to] = squares[from];
                squares[from] = null;

                //check for rows
                if (!findAndDeleteLinesFor(to))
                    GenerateNewSquares();
            }
        }

        public List<int> findLine(int square)    //returns numbers of squares that needs to be deleted
        {

            //check for columns
            List<int> column = new List<int>();
            column.Add(square);
            int cursor = square;
            int movedCursor;
            List<int> offsets = new List<int>() { 7, -7};
            for (int i = 0; i < offsets.Count; i++)
            {
                movedCursor = cursor + offsets[i];
                while (movedCursor >= 0 && movedCursor < 49 &&  //check that index not is out of range
                    squares[movedCursor] == squares[square] &&   //check that squares have the same color
                    !column.Contains(movedCursor))   //check that that square not already in the list
                {
                    column.Add(movedCursor);
                    cursor = movedCursor;
                }

                cursor = square;
            }
            
            //check for rows
            List<int> row = new List<int>();
            row.Add(square);
            cursor = square;
            offsets = new List<int>() { 1, -1};
            for (int i = 0; i < offsets.Count; i++)
            {
                movedCursor = cursor + offsets[i];
                while (!isNewLine(cursor, offsets[i]) &&    //do not include multi-line rows
                    movedCursor >= 0 && movedCursor < 49 &&
                    squares[movedCursor] == squares[square] &&
                    !row.Contains(movedCursor))
                {
                    row.Add(movedCursor);
                    cursor = movedCursor;
                }

                cursor = square;
            }

            //check for right diagonals
            List<int> rightDiagonal = new List<int>();
            rightDiagonal.Add(square);
            cursor = square;
            offsets = new List<int>() {8, -8};
            for (int i = 0; i < offsets.Count; i++)
            {
                movedCursor = cursor + offsets[i];
                while (Math.Abs(cursor / 7 - (movedCursor) / 7) < 2 &&  //same as before. but for diagonal this means 2 transfers of a line
                    movedCursor >= 0 && movedCursor < 49 &&
                    squares[movedCursor] == squares[square] &&
                    !rightDiagonal.Contains(movedCursor))
                {
                    rightDiagonal.Add(movedCursor);
                    cursor = movedCursor;
                }

                cursor = square;
            }

            //check for left diagonals
            List<int> leftDiagonal = new List<int>();
            leftDiagonal.Add(square);
            cursor = square;
            offsets = new List<int>() {6, -6 };
            for (int i = 0; i < offsets.Count; i++)
            {
                movedCursor = cursor + offsets[i];
                while (Math.Abs(cursor / 7 - (movedCursor) / 7) < 2 &&
                    movedCursor >= 0 && movedCursor < 49 &&
                    squares[movedCursor] == squares[square] &&
                    !leftDiagonal.Contains(movedCursor))
                {
                    leftDiagonal.Add(movedCursor);
                    cursor = movedCursor;
                }

                cursor = square;
            }


            //add to result columns,rows,diagonals more than four squares
            List<int> result = new List<int>();
            if (column.Count >= 4) result.AddRange(column);
            if (row.Count >= 4) result.AddRange(row);
            if (rightDiagonal.Count >= 4) result.AddRange(rightDiagonal);
            if (leftDiagonal.Count >= 4) result.AddRange(leftDiagonal);

            return result;
        }

        private void deleteSquares(List<int> forDelete)
        {
            for (int i = 0; i < forDelete.Count; i++)
            {
                squares[forDelete[i]] = null;
            }
            deletedRows++;
        }

        
    }
}
