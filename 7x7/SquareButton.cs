using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace _7x7
{
    public class SquareButton : Button
    {


        public new string Background
        {
            get
            {
                return base.Background.ToString();
            }
            set
            {
                //do animation
                Rectangle foregroundRect = (Rectangle)Template.FindName("foregroundRect", this);
                SolidColorBrush newColor = (SolidColorBrush)new BrushConverter().ConvertFromString(value);
                DoubleAnimation animation = new DoubleAnimation();
                animation.Duration = TimeSpan.FromMilliseconds(150);
                if (value.Equals(MainWindow.buttonEmptyColor))
                {
                    animation.To = 0;
                    foregroundRect.BeginAnimation(WidthProperty, animation);
                    foregroundRect.BeginAnimation(HeightProperty, animation);
                    foregroundRect.Width = 0;
                    foregroundRect.Height = 0;
                }
                else
                {
                    foregroundRect.Fill = newColor;
                    animation.To = 52;
                    foregroundRect.BeginAnimation(WidthProperty, animation);
                    foregroundRect.BeginAnimation(HeightProperty, animation);
                    foregroundRect.Width = 52;
                    foregroundRect.Height = 52;
                }



            }
        }

        private bool crossVisible = false;
        public bool CrossVisible
        {
            get
            {
                return crossVisible;
            }
            set
            {
                if (value)
                    ((Image)Template.FindName("cross", this)).Visibility = System.Windows.Visibility.Visible;
                else
                    ((Image)Template.FindName("cross", this)).Visibility = System.Windows.Visibility.Hidden;
                crossVisible = value;
            }
        }

        
    }
}
