using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        HubConnection connection;
        public TetrisEngine engine;
        public MainWindow()
        {
            InitializeComponent();
            engine = new TetrisEngine();
            engine.StartGame();
            TimerSetup();

            connection = new HubConnectionBuilder()
                .WithUrl("http://127.0.0.1:5000/TetrisHub")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

        }

        // Diverse controls voor tetris, left and right voor de X-coordinate movement, up en down voor rotation en space om de tetromino naar beneden te droppen.
        public void KeyDown(object sender, KeyEventArgs e)
        {
            {
                // added check so you can't move while the game is paused
                if (this.PauseButton.Content.ToString() != "Resume")
                {
                    switch (e.Key)
                    {
                        case Key.Right:
                            engine.KeyboardRight();
                            break;
                        case Key.Left:
                            engine.KeyboardLeft();
                            break;
                        case Key.Up:
                            engine.KeyboardUp();
                            break;
                        case Key.Down:
                            engine.KeyboardDown();
                            break;
                        case Key.Space:
                            engine.KeyboardSpace();
                            break;
                        default:
                            return;
                    }
                }
            }
        }

        //setup for dispatcher
        public void TimerSetup()
        {
            //every tick method DispatcherTimer_Tick is called   
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            //this happens every 200ms
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();
        }

        // ticks every x ms, used to update the game 
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            PlaceNextTetrominoWindow();
            // check for rows to be cleared and adds score if true
            if (engine.CheckIfRowIsFilled(engine.board)) 
            {
                engine.AddScoreForRowComplete();
            }

            this.Level.Text = "Level: " + engine.level;
            this.Lines.Text = "Lines: " + engine.lines;
            this.Scoreboard.Text = "Score: " + engine.score;       
            
            // add score to GUI     
            // tetromino keeps falling until there is a collission with another tetromino or the bottom is touched 
            if (!engine.CheckForBottom(engine.tetromino) && engine.board.checkCollision(engine.tetromino))
            {
                PlaceRectangles();
                engine.tetromino.YCoordinate++;
                placeGhostPiece();
            }
            else
            {
                try
                {
                    engine.board.updateLandedGameboard(engine.tetromino);
                    engine.NextTetronimo();
                } catch (Exception ex)
                {
                    engine.StartGame();
                }
            }
        }

        // Add next tetronimo in the gui
        public void PlaceNextTetrominoWindow()
        {
            //loops through tetromino 1 coords and places it on the board
            NextTetrisGrid.Children.Clear();
            int[,] values = engine.loadedTetrominos[1].Matrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] == 0) continue;

                    Rectangle rectangle = new Rectangle()
                    {
                        Width = 25, // Width of cell in grid
                        Height = 25, // Height of cell in grid
                        Stroke = Brushes.White, // Edge
                        StrokeThickness = 1, // Thickness edge
                        Fill = Brushes.Red, // Backgroundcolor
                    };

                    NextTetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                    Grid.SetRow(rectangle, i + 1); // Zet de rij
                    Grid.SetColumn(rectangle, j); // Zet de kolom
                }
            }
        }

        //places ghostpiece
        public void placeGhostPiece()
        {
            //adds ghostpiece by using its coords
            engine.addGhostPiece().CurrentCoordinates().ForEach(coords =>
            {
                Rectangle rectangle = new Rectangle()
                {
                    Width = 25, 
                    Height = 25, 
                    Stroke = Brushes.White, 
                    StrokeThickness = 0.5, 
                    Opacity = 0.35,
                    Fill = Brushes.Red
                };

                TetrisGrid.Children.Add(rectangle);
                Grid.SetRow(rectangle, coords.Item1 - 1); // Zet de rij
                Grid.SetColumn(rectangle, coords.Item2); // Zet de kolom
            });
        }

        //places rectangles from landedboard into the gui by looping through and checking for 1
        public void PlaceRectangles()
        {
            //make method check if next tick allowed and return boolean, use coords as param
            TetrisGrid.Children.Clear();
            for (int i = 0; i < engine.board.landedBoard.GetLength(0); i++)
            {
                for (int j = 0; j < engine.board.landedBoard.GetLength(1); j++)
                {
                    if (engine.board.landedBoard[i, j] == 1)
                    {
                        Rectangle rectangle = new Rectangle()
                        {
                            Width = 25, 
                            Height = 25, 
                            Stroke = Brushes.White, 
                            StrokeThickness = 1, 
                            Fill = Brushes.Red
                        };

                            TetrisGrid.Children.Add(rectangle);
                        //the y coordinate here is replaced by the ++ which causes it to go down
                        Grid.SetRow(rectangle, i); 
                        Grid.SetColumn(rectangle, j); 
                    }
                }
            }

            engine.tetromino.CurrentCoordinates().ForEach(x =>
            {
                Rectangle rectangle = new Rectangle()
                {
                    Width = 25, 
                    Height = 25, 
                    Stroke = Brushes.White, 
                    StrokeThickness = 2,
                    Fill = Brushes.Red
                };

                TetrisGrid.Children.Add(rectangle);
                Grid.SetRow(rectangle, x.Item1);
                Grid.SetColumn(rectangle, x.Item2);
            });
            engine.board.updateGameboard(engine.tetromino);
        }

        // Quits the application, solution found here https://stackoverflow.com/questions/2820357/how-do-i-exit-a-wpf-application-programmatically
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            if (this.PauseButton.Content.ToString() == "Pause")
            {
                SendPause();
                this.PauseButton.Content = "Resume";

            }
            else
            {
                dispatcherTimer.Start();
                this.PauseButton.Content = "Pause";
            }
        }

        private async void SendPause()
        {
            try
            {
                await connection.InvokeAsync("Pause");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
    }
}
