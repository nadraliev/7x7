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

        Button selectedButton;  //currently selected button
        int selectedButtonIndex;

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
                Button clickedButton = (Button)e.OriginalSource;
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

                        score.Content = newGame.deletedRows.ToString(); //update score label

                        if (newGame.GameOver)   //check on game over
                        {
                            MessageBox.Show("Game over");
                            newGame = new Game();   //start new game if game over
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


            newGame.GenerateNewSquares();  //start new game as soon as windows loads
            RefreshField();
        }

        private void SelectSquare(Button clickedButton)
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
                ((Button)this.FindName("button" + notAvailableSquares[i])).Content = pnl;   //put cross to button
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
                    ((Button)this.FindName("button" + i)).Background = (SolidColorBrush)new BrushConverter().ConvertFromString(color);  //set color for button<number>
                } else
                {
                    ((Button)this.FindName("button" + i)).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDDDDDD");    //reset color
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
        
    }
}
