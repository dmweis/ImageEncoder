using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using ImageEncoder;

namespace ImageDecoderWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            path_field.Text = Directory.GetCurrentDirectory() + @"\largeImage.bmp";
        }

        private void browse_button_Click( object sender, RoutedEventArgs e )
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.FileName = "largeImage";
            dialog.DefaultExt = ".bmp";
            dialog.Filter = "Bitmap Files (.bmp)|*.bmp";
            bool? success = dialog.ShowDialog();
            if( success == true )
            {
                path_field.Text = dialog.FileName;
                message_box.Text = BitmapEncoder.DecodeImage( path_field.Text );
            }
        }

        private void Dencode_Click( object sender, RoutedEventArgs e )
        {
            message_box.Text = BitmapEncoder.DecodeImage( path_field.Text );
        }
    }
}
