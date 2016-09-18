using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace _7x7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Game newGame;
        
        SquareButton selectedButton;  //currently selected button
        int selectedButtonIndex;
        public static string buttonEmptyColor = "#FFDDDDDD";

        List<int> notAvailableSquares;

        public MainWindow()
        {
            InitializeComponent();
            newGame = new Game();
            notAvailableSquares = new List<int>();
        }


        private void WrapPanel_Click(object sender, RoutedEventArgs e)
        {
            try {
                SquareButton clickedButton = (SquareButton)e.OriginalSource;
                int clickedButtonIndex = int.Parse(clickedButton.Tag.ToString());

                if (selectedButton == null) //previously no button was in selected mode
                {
                    if (newGame.squares[clickedButtonIndex] != null)    //check that pressed button is not empty
                    {
                        SelectSquare(clickedButton);    //set button to selected
                    }
                } else
                {
                    selectedButtonIndex = int.Parse(selectedButton.Tag.ToString());

                    if (selectedButton == clickedButton)    //user pressed the same button two times
                    {
                        clickedButton.Opacity = 1;  //just reset selection
                        selectedButton = null;
                        DeleteCrosses();
                    } else if (!notAvailableSquares.Contains(clickedButtonIndex))   //check that pressed button is available
                    {
                        newGame.makeMove(selectedButtonIndex, clickedButtonIndex);
                        clickedButton.Opacity = 1;
                        selectedButton.Opacity = 1;
                        selectedButton = null;
                        DeleteCrosses();

                        score.Content = newGame.Score.ToString(); //update score label
                        level.Content = newGame.level.ToString();   //update level label
                        toNextLevel.Content = newGame.LinesToGoToNextLevel.ToString();
                        coming.Content = newGame.howMuchGenerate.ToString();

                        if (newGame.GameOver)   //check on game over
                        {
                            MessageBox.Show("Game over");
                            StartNewGame(); //start new game if game over
                        }
                        RefreshField();
                    }

                    
                }
            } catch (InvalidCastException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UIElementCollection buttons = gameField.Children;
            for (int i = 0; i < buttons.Count; i++)
                ((Button)buttons[i]).Tag = i.ToString();  //set tags for buttons here because i'm lazy to do it manually in XAML


            StartNewGame();  //start new game as soon as windows loads
            RefreshField();
        }

        private void SelectSquare(SquareButton clickedButton)
        {
            selectedButton = clickedButton;
            selectedButtonIndex = int.Parse(selectedButton.Tag.ToString());
            
            notAvailableSquares = newGame.findNotAvailableSquaresFor(selectedButtonIndex);

            for (int i = 0; i < notAvailableSquares.Count; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("pack://application:,,,/7x7;component/Resources/cross.png"));
                StackPanel pnl = new StackPanel();
                pnl.Children.Add(img);
                ((SquareButton)this.FindName("button" + notAvailableSquares[i])).Content = pnl;   //put cross to button
            }

            clickedButton.Opacity = 0.5;    //little fade out for selected button
        }

        private void RefreshField()
        {
            for (int i = 0; i < Game.MAX_COUNT; i++)
            {
                string color = newGame.squares[i];
                if (color != null)
                {
                    ((SquareButton)this.FindName("button" + i)).Background = color;  //set color for button<number>
                } else
                {
                    ((SquareButton)this.FindName("button" + i)).Background = buttonEmptyColor;    //reset color
                }
            }
        }

        private void DeleteCrosses()
        {
            for (int i = 0; i < newGame.squares.Length; i++)
            {
                ((Button)this.FindName("button" + i)).Content = null;   //delete cross from button
            }
        }

        private void StartNewGame()
        {
            newGame = new Game();   
            newGame.GenerateNewSquares();
            score.Content = newGame.Score.ToString(); //update score label
            level.Content = newGame.level.ToString();   //update level label
            toNextLevel.Content = newGame.LinesToGoToNextLevel.ToString();
            coming.Content = newGame.howMuchGenerate.ToString();
            DeleteCrosses();
            RefreshField();
        }

        private void startNewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

    }
}
