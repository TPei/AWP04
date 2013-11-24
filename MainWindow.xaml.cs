using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UE04
{
    /// <summary>
    /// interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stack<Stroke> redoStack = new Stack<Stroke>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void undoButton(object sender, RoutedEventArgs e)
        {
            // if there are saved strokes on the canvas
            // remove the last there and add it to the redo stack 
            if (this.myInkCanvas.Strokes.Count > 0)
            {
                this.redoStack.Push(this.myInkCanvas.Strokes.Last());
                this.myInkCanvas.Strokes.Remove(this.myInkCanvas.Strokes.Last());
            }
        }

        private void redoButton(object sender, RoutedEventArgs e)
        {
            // if there is something on the redo stack:
            // remove the first there and add it to the strokes list
            if (this.redoStack.Count > 0)
            {
                this.myInkCanvas.Strokes.Add(this.redoStack.Pop());
            }
        }

        // is called when any of the three color sliders (red, green, blue) is changed
        private void changeColor(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // get color from Sliders and create a new RGB Color
            Color brushColor = new Color();
            brushColor.R = (byte)this.red.Value;
            brushColor.G = (byte)this.green.Value;
            brushColor.B = (byte)this.blue.Value;
            brushColor.A = 255;
            
            // change brush accordingly and set as inkcanvas brush
            this.myInkCanvas.DefaultDrawingAttributes.Color = brushColor;
            
            // for information purposes also change "stift" label background color
            this.label2.Background = new SolidColorBrush(brushColor);
        }

        // change in thickness slider
        private void thicknessValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int thickness = (int)this.sliderStaerke.Value;

            myInkCanvas.DefaultDrawingAttributes.Height = thickness;
            myInkCanvas.DefaultDrawingAttributes.Width = thickness;
        }

        protected override void OnPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            // if the user pressed somewhere on the canvas,
            // the redo stack is emptied
            // e.g. back - back - draw new => redo shouldn't do anything anymore
            Point click = e.GetPosition(this);
            double width = click.X;
            if (width < myInkCanvas.ActualWidth)
                redoStack.Clear();
        }
    }
}
