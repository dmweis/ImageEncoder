
using System.Windows;
using System.Windows.Controls;
using System.IO;
using ImageEncoder;

namespace ImageEncoderWPF
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
            dialog.FileName = "largeImage";
            dialog.DefaultExt = ".bmp";
            dialog.Filter = "Bitmap Files (.bmp)|*.bmp";
            bool? success = dialog.ShowDialog();
            if( success == true )
            {
                path_field.Text = dialog.FileName;
            }
        }

        private void Encode_Click( object sender, RoutedEventArgs e )
        {
            BitmapEncoder.EncodeImage( path_field.Text, path_field.Text.Replace( ".bmp", "_ENCODED.bmp" ), message_box.Text );
            MessageBox.Show( "Encoidng finished" );
        }
    }
}
