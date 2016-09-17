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

        public MainWindow()
        {
            InitializeComponent();
            newGame = new Game();
        }


        private void WrapPanel_Click(object sender, RoutedEventArgs e)
        {
            try {
                Button clickedButton = (Button)e.OriginalSource;
                if (selectedButton == null)
                {
                    if (newGame.squares[int.Parse(clickedButton.Tag.ToString())] != null)
                    {
                        selectedButton = clickedButton;

                        List<int> notAvailableSquares = newGame.findNotAvailableSquares(int.Parse(selectedButton.Tag.ToString()));
                        for (int i = 0; i < notAvailableSquares.Count; i++)
                        {
                            
                            ((Button)this.FindName("button" + i)).Content = "{StaticResource MyImage}";
                        }

                        clickedButton.Opacity = 0.5;
                    }
                } else
                {
                    if (selectedButton == clickedButton)
                    {
                        clickedButton.Opacity = 1;
                        selectedButton = null;
                    } else
                    {
                        newGame.makeMove(int.Parse(selectedButton.Tag.ToString()), int.Parse(clickedButton.Tag.ToString()));
                        clickedButton.Opacity = 1;
                        selectedButton.Opacity = 1;
                        selectedButton = null;
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


            newGame.GenerateNewSquares(3);
            RefreshField();
        }

        private void RefreshField()
        {
            for (int i = 0; i < Game.MAX_COUNT; i++)
            {
                string color = newGame.squares[i];
                if (color != null)
                {
                    ((Button)this.FindName("button" + i)).Background = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
                } else
                {
                    ((Button)this.FindName("button" + i)).Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFDDDDDD");
                }
            }
        }

        
    }
}
