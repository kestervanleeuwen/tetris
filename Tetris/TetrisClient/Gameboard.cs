using System.Collections.Generic;
using System.Linq;

namespace TetrisClient
{
    public class Gameboard
    {
        public int[,] gameBoard { get; set; }
        public int[,] landedBoard { get; set;  }

        //Gameboard is the array we use to define the playable grid area
        public Gameboard()
        {
            gameBoard = new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };

            //landedBoard contains all the tetromino's that have 'landed' and therefore don't require the players control
            landedBoard = new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };
        }

        //resets the gameboard
        public void resetGameboard()
        {
            gameBoard = new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };
        }


        //adds a new tetromino to the gameboard whenever called
        public void addTetronimo(Tetromino tetromino)
        {
            //values contains the shape of the tetromino
            int[,] matrixValue = tetromino.Matrix.Value;
            for (int i = 0; i < matrixValue.GetLength(0); i++)
            {
                for (int j = 0; j < matrixValue.GetLength(1); j++)
                {
                    //wherever in the shape there is a 1 that position will be added to the gameboard, generating a tetromino out of blocks
                    if (matrixValue[i, j] == 1)
                    {
                        gameBoard[i, j] = 1;
                    }
                }
            }
        }

        //checks collision of tetromino on the board
        public bool checkCollision(Tetromino tetromino)
        {
            // initialise new list for collision cells and set default return to true
            var flag = true;
            List<(int, int)> surroundingCells = new List<(int, int)>();

            //uses the coordinates of a tetromino to check if it is within the y value of the board, if it is it gets added to the List
            var query = tetromino.CurrentCoordinates().Where(coords => coords.Item1 < 16);
            foreach (var cell in query) { surroundingCells.Add((cell.Item1, cell.Item2)); }

            //if landedBoard contains a 1 that is also present in the list flag is set to false
            surroundingCells.ForEach(surroundingCoords => 
            {
                if (landedBoard[surroundingCoords.Item1, surroundingCoords.Item2] == 1)
                {
                    flag = false;
                }
            });
            return flag;
        }

        public bool checkRotateCollision(Tetromino tetromino, string rotateType)
        {
            Tetromino rotateTetromino = new Tetromino(tetromino.TetrominoEnum);
            rotateTetromino.XCoordinate = tetromino.XCoordinate;
            rotateTetromino.YCoordinate = tetromino.YCoordinate;
            rotateTetromino.Matrix = tetromino.Matrix;
            bool result = true;

            switch (rotateType.ToLower())
            {
                case "rotate90":
                    rotateTetromino.Matrix = rotateTetromino.Matrix.Rotate90();
                    result = !rotateTetromino.CurrentCoordinates().Any(coords => coords.Item2 < 0 || coords.Item2 > 9 || coords.Item1 > 15);
                    break;
                case "reverse90":
                    rotateTetromino.Matrix = rotateTetromino.Matrix.Rotate90CounterClockwise();
                    result = !rotateTetromino.CurrentCoordinates().Any(coords => coords.Item2 < 0 || coords.Item2 > 9 || coords.Item1 > 15);
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        //checks collision when tetromino is moved to the right
        public bool checKeyboardRightCollision(Tetromino tetromino)
        {
            var flag = true;
            List<(int, int)> surroundingCells = new List<(int, int)>();

            var query = tetromino.CurrentCoordinates().Where(coords => coords.Item2 > 0 && coords.Item2 < 9);
            foreach (var cell in query) { surroundingCells.Add((cell.Item1, cell.Item2 + 1)); }

            surroundingCells.ForEach(surroundingCoords =>   
            {
                if (landedBoard[surroundingCoords.Item1, surroundingCoords.Item2] == 1)
                {
                    flag = false;
                }
            });
            return flag;
        }

        //checks collision when tetromino is moved to the left 
        public bool checKeyboardLeftCollision(Tetromino tetromino)
        {
            var flag = true;
            List<(int, int)> surroundingCells = new List<(int, int)>();
            
            var query = tetromino.CurrentCoordinates().Where(coords => coords.Item2 > 0 && coords.Item2 < 9);
            foreach (var cell in query) { surroundingCells.Add((cell.Item1, cell.Item2 - 1)); }

            surroundingCells.ForEach(surroundingCoords =>
            {
                if (landedBoard[surroundingCoords.Item1, surroundingCoords.Item2] == 1)
                {
                    flag = false;
                }
            });
            return flag;
        }

        //checks left side of board for collission
        public bool checkBoardRangeLeft(Tetromino tetromino)
        {
            bool flag = true;
            //loops through coords of tetromino to check if one is on x=0
            tetromino.CurrentCoordinates().ForEach((coords) =>
            {
                if (coords.Item2 == 0)
                {
                    flag = false;
                }
            });
            return flag;
        }

        public bool checkBoardRangeRight(Tetromino tetromino)
        {
            bool flag = true;
            tetromino.CurrentCoordinates().ForEach((coords) =>
            {
                if (coords.Item2 == 9)
                {
                    flag = false;
                }
            });
            return flag;
        }

        //updates gameboard with new tetromino position
        public void updateGameboard(Tetromino tetromino)
        {
            //resets gameboard and places tetromino on new coords
            resetGameboard();
            tetromino.CurrentCoordinates().ForEach(coordinates =>
            {
                gameBoard[coordinates.Item1, coordinates.Item2] = 1;
            });
        }

        //updates landedboard with new landed tetromino position
        public void updateLandedGameboard(Tetromino tetromino)
        {
            //loops through tetromino coords and places this in the landedboard
            var query = tetromino.CurrentCoordinates().Where(coords => landedBoard[coords.Item1 - 1, coords.Item2] != 1);
            foreach (var coords in query)
            {
                landedBoard[coords.Item1 - 1, coords.Item2] = 1;
            }

        }

        //removes tetromino from the board
        public void removeTetronimo(Tetromino tetromino)
        {
            //loops through gameboard and checks if value aligns with tetromino position and sets to 0 if true
            int[,] values = tetromino.Matrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] == 1 && gameBoard[i, j] == 1)
                    {
                        gameBoard[i, j] = 0;
                    }
                }
            }
        }
    }
}
