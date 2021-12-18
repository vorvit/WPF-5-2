using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WPF_5_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void inkCanvas_Gesture(object sender, InkCanvasGestureEventArgs e)
        {
            String gestures = "";
            foreach (GestureRecognitionResult res in e.GetGestureRecognitionResults())
                gestures += res.ApplicationGesture.ToString() + "; ";
            gestureName.Text = gestures;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (InkCanvasEditingMode mode in Enum.GetValues(typeof(InkCanvasEditingMode)))
                lstEditingMode.Items.Add(mode);
            lstEditingMode.SelectedItem = inkCanvas.EditingMode;
        }
 
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Графические файлы (*.jpg; *.bmp; *.png)|*.jpg; *.bmp; *.png|Все файлы (*.*)|*.*";
            if (openDialog.ShowDialog() == true)
                image.Source = new BitmapImage(new Uri(openDialog.FileName, UriKind.Absolute));
            else
                image.Source = null;
        }

        private void MenuItem_Click1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Графические файлы (*.jpg; *.bmp; *.png)|*.jpg; *.bmp; *.png|Все файлы (*.*)|*.*";
            if (saveDialog.ShowDialog() == true)
            {
                int margin = (int)inkCanvas.Margin.Left;
                int width = (int)inkCanvas.ActualWidth - margin;
                int height = (int)inkCanvas.ActualHeight - margin;
                RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
                rtb.Render(inkCanvas);
                using (FileStream fs = new FileStream(saveDialog.FileName, FileMode.Create))
                {
                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(fs);
                }
            }
        }

        private void MenuItem_Click2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
