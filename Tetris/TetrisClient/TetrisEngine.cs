using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisClient
{
    public class TetrisEngine
    {

        public Gameboard board;
        public List<Tetromino> loadedTetrominos = new List<Tetromino>();
        public Tetromino tetromino;
        public Tetromino ghostTetromino;
        public TetrominoEnum tetrominoEnum;
        public int score;
        public int lines;
        public int level;

        // Starts the game by initialising a new random tetromino and gameboard, after this the tetromino is added to the gameboard at the starting position
        public void StartGame()
        {
            // Source: https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration
            // Method picks a random TetrominoEnum using Random class
            Array values = Enum.GetValues(typeof(TetrominoEnum));
            Random random = new Random();
            TetrominoEnum randomTetromino = (TetrominoEnum)values.GetValue(random.Next(values.Length));
            // init two tetrominos
            tetromino = new Tetromino(randomTetromino);
            Tetromino tetromino_two = new Tetromino(randomTetromino);
            // add tetrominos to the list
            loadedTetrominos.Add(tetromino);
            loadedTetrominos.Add(tetromino_two);

            board = new Gameboard();
            level = 1;
            score = 0;
            lines = 0;
            board.addTetronimo(tetromino);
        }

        //adds ghostpiece using data from falling tetronimo
        public Tetromino addGhostPiece()
        {
            ghostTetromino = new Tetromino(tetromino.TetrominoEnum);
            ghostTetromino.XCoordinate = tetromino.XCoordinate;
            ghostTetromino.YCoordinate = tetromino.YCoordinate;
            ghostTetromino.Matrix = tetromino.Matrix;

            //drops ghostpiece down until there is colission
            while (board.checkCollision(ghostTetromino) && !CheckForBottom(ghostTetromino))
            {
                ghostTetromino.YCoordinate++;
            }
            return ghostTetromino;
        }


        //places next tetromino on the board
        public void NextTetronimo()
        {
            // Delete tetromino from gameboard
            board.removeTetronimo(tetromino);
            // Remove first tetromino from list
            loadedTetrominos.RemoveAt(0);
            // Load next tetromino
            tetromino = loadedTetrominos[0];

            // Source: https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration
            // Method picks a random TetrominoEnum using Random class
            Array values = Enum.GetValues(typeof(TetrominoEnum));
            Random random = new Random();
            TetrominoEnum randomTetromino = (TetrominoEnum)values.GetValue(random.Next(values.Length));

            // Newest tetromino is addded to the list
            Tetromino tetromino_two = new Tetromino(randomTetromino);
            loadedTetrominos.Add(tetromino_two);
            // New tetromino is added to the board
            board.addTetronimo(tetromino);
        }

        //When a row is filled in the board the score gets increased and the number of lines completed gets increased by one
        public void AddScoreForRowComplete()
        {
            lines = lines + 1;
            score = score + 10;
        }

        // Code for rotation using up arrow
        public void KeyboardUp()
        {

            if (board.checKeyboardRightCollision(tetromino) && board.checKeyboardLeftCollision(tetromino) && board.checkRotateCollision(tetromino, "rotate90"))
            {
                tetromino.Matrix = tetromino.Matrix.Rotate90();
                board.updateGameboard(tetromino);
            }
        }

        // Code for rotation using down arrow
        public void KeyboardDown()
        {
            if (board.checKeyboardRightCollision(tetromino) && board.checKeyboardLeftCollision(tetromino) && board.checkRotateCollision(tetromino, "reverse90"))
            {
                tetromino.Matrix = tetromino.Matrix.Rotate90CounterClockwise();
                board.updateGameboard(tetromino);
            }
            
        }

        // Code for moving to the left using left arrow
        public void KeyboardLeft()
        {
            if (board.checkBoardRangeLeft(tetromino) && board.checKeyboardLeftCollision(tetromino))
            {
                tetromino.XCoordinate--;
            }
        }

        // Code for moving to the right using right arrow
        public void KeyboardRight()
        {
            if (board.checkBoardRangeRight(tetromino) && board.checKeyboardRightCollision(tetromino))
            {
                tetromino.XCoordinate++;
            }
        }

        // Code for dropping tetronimo down using spacebar, tetris keeps dropping until collision happens
        public void KeyboardSpace()
        { 
            while (!CheckForBottom(tetromino) && board.checkCollision(tetromino))
            {
                tetromino.YCoordinate++;
            }
        }

        //Checks if tetromino is at the bottom of the board
        public bool CheckForBottom(Tetromino usedTetromino) 
        {
            var query = usedTetromino.CurrentCoordinates().Any(coords => coords.Item1 > 15);
            return query;

        }

        //checks if a row is full, if it is it removes the row, needs optimization
        public bool CheckIfRowIsFilled(Gameboard board)
        {
            //this array stores the values of an individual row, once its determined the row is not full, the array gets emptied and the next row gets loaded
            List<int> arr = new List<int>();
            bool isFilled = false;

            //loops through the landedBoard per row
            for (int y = 15; y >= 0; y--)
            {
                for (int x = 0; x <= 9; x++)
                {
                    arr.Add(board.landedBoard[y, x]);
                }
                // if the row does not contain a 0 it means it is filled and so it can be removed, if it contains a 0 it means its not filled so it gets cleared in the else
                if (!arr.Contains(0))
                {
                    int rowThatIsFull = y;
                    isFilled = true;
                    //makes row that is full 0 to remove it
                    for(int b = 0; b <= 9; b++)
                    {
                        board.landedBoard[rowThatIsFull, b] = 0;
                    }
                    for (int i = rowThatIsFull; i >= 0; i--)
                    {
                        for (int j = 0; j < board.landedBoard.GetLength(1); j++)
                        {
                            int o = i + 1;
                            if(board.landedBoard[i, j] == 1)
                            {
                                if(o >= 15)
                                {
                                    o = 15;
                                }
                                //moves all the rows that are above the cleared row down by 1
                                board.landedBoard[i, j] = 0;
                                board.landedBoard[o, j] = 1;
                            }
                        }
                    }
                }
                else
                {
                    arr.Clear();
                }
            }
            return isFilled;
        }
    }// ADD COMMENT VOOR LATER -> CLEAR LINE ARTICLE

}
