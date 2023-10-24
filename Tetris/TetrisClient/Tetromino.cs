using System;
using System.Collections.Generic;

namespace TetrisClient
{
    public class Tetromino 
    {
        public int XCoordinate = 0;
        public int YCoordinate = 0;

        public TetrominoEnum TetrominoEnum { get; set; }
        public Matrix Matrix { get; set; }

        public Tetromino(TetrominoEnum tetrominoEnum) 
        {
            this.TetrominoEnum = tetrominoEnum;
            Matrix = NewMatrix();
        }

        public List<(int, int)> CurrentCoordinates()
        {
            List<(int, int)> coordinates = new List<(int, int)>();
            var values = Matrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] == 1)
                    {
                        coordinates.Add((i + YCoordinate, j + XCoordinate));
                    }
                }
            }
            return coordinates;
        }

        // diverse tetromino shapes in matrix form, based on TetrominoEnum class
        public Matrix NewMatrix() 
        {
            return TetrominoEnum switch
            {
                TetrominoEnum.SquareShape => new Matrix(new int[,]
                                {{ 1, 1 },{ 1, 1 }}),
                TetrominoEnum.IShape => new Matrix(new int[,]
{{ 0, 0, 0, 0 }, { 1, 1, 1, 1 },{ 0, 0, 0, 0 },{ 0, 0, 0, 0 }}),
                TetrominoEnum.LShape => new Matrix(new int[,]
{{ 0, 0, 1 },{ 1, 1, 1 },{ 0, 0, 0 }}),
                TetrominoEnum.SShape => new Matrix(new int[,]
{{ 0, 1, 1 },{ 1, 1, 0 },{ 0, 0, 0 }}),
                TetrominoEnum.TShape => new Matrix(new int[,]
{{ 0, 1, 0 },{ 1, 1, 1 },{ 0, 0, 0 }}),
                TetrominoEnum.ZShape => new Matrix(new int[,]
{{ 1, 1, 0 },{ 0, 1, 1 },{ 0, 0, 0 }}),
                TetrominoEnum.JShape => new Matrix(new int[,]
{{ 1, 0, 0 },{ 1, 1, 1 },{ 0, 0, 0 }}),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }


}
