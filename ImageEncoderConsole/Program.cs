using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ImageEncoderConsole
{
    class Program
    {
        private static readonly string originalFilePath = @"D:\Programming\C#\Visual_studio_projects\ImageEncoder\Images\largeImage.bmp";
        private static readonly string newFilePath = @"D:\Programming\C#\Visual_studio_projects\ImageEncoder\Images\CreatedImage.bmp";
        private static readonly string staticText = "ctp ";
        private const int numberOfImages = 5;
        static void Main(string[] args)
        {
            Console.WriteLine( "Starting" );
            SaveToImage( originalFilePath, newFilePath, staticText );
            Console.WriteLine( ExtractFromImage( newFilePath ) );
            TestFileData(newFilePath);
            //Bitmap image = new Bitmap(newFilePath);
            Console.WriteLine( "Ending" );
            Console.ReadKey();
        }

        private static void testBiteSetter( bool endWith )
        {
            Console.WriteLine( "input number" );
            string input = Console.ReadLine();
            byte exampleByte = Convert.ToByte( input, 2 );
            exampleByte = SetLastBit( exampleByte, endWith );
            string output = Convert.ToString( exampleByte, 2 );
            Console.WriteLine( "result is: " + output.PadLeft(8, '0') );
        }

        private static byte SetLastBit( byte originalByte, bool LastBitValue )
        {
            return (byte) ( LastBitValue ? ( originalByte | ( 1 << 0 ) ) : ( originalByte & ~( 1 << 0 ) ) );
        }

        private static void TestSeparateBits( string input )
        {
            Console.WriteLine("printing text: " + input);
            foreach( bool bit in SeparateBits( input ) )
            {
                Console.WriteLine("   " + bit.ToString() );
            }
        }

        private static bool[] SeparateBits( string text )
        {
            byte[] byteArray = Encoding.ASCII.GetBytes( text );
            bool[] result = new bool[ text.Length * 8 ];
            int resultIndex = 0;
            foreach( byte letter in byteArray )
            {
                string binaryString = Convert.ToString( letter, 2).PadLeft(8, '0');
                foreach( char bit in binaryString )
                {
                    result[ resultIndex++ ] = bit == '1';
                }
            }
            return result;
        }

        private static void TestIfItWasSaved( string path1, string path2 )
        {
            Bitmap image1 = new Bitmap( path1 );
            Bitmap image2 = new Bitmap( path2 );

            for( int row = 0 ; row < image1.Height ; row++ )
            {
                for( int column = 0 ; column < image1.Width ; column++ )
                {
                    Color originalColor = image1.GetPixel( column, row );
                    Color newColor = image2.GetPixel( column, row );
                    Console.WriteLine( originalColor.ToString() + " " + newColor.ToString() );
                    if( column > 100 )
                    {
                        return;
                    }
                }
            }
        }

        private static string ExtractFromImage( string path )
        {
            StringBuilder resultString = new StringBuilder();
            Bitmap image = new Bitmap( path );
            int blockSize = int.MaxValue;
            for( int i = 0 ; i < (image.Width * image.Height * 3) / 8 ; i += 8 )
            {
                if( resultString.Length >= blockSize )
                {
                    resultString.Remove( 0, 10 );
                    break;
                }
                byte tmpByte = 0;
                tmpByte = (byte) ( tmpByte | ( GetByteFromImage(image, i ) << 7 ));
                tmpByte = (byte) ( tmpByte | ( GetByteFromImage( image, i + 1 ) << 6 ) );
                tmpByte = (byte) ( tmpByte | ( GetByteFromImage( image, i + 2 ) << 5 ) );
                tmpByte = (byte) ( tmpByte | ( GetByteFromImage( image, i + 3 ) << 4 ) );
                tmpByte = (byte) ( tmpByte | ( GetByteFromImage( image, i + 4 ) << 3 ) );
                tmpByte = (byte) ( tmpByte | ( GetByteFromImage( image, i + 5 ) << 2 ) );
                tmpByte = (byte) ( tmpByte | ( GetByteFromImage( image, i + 6 ) << 1 ) );
                tmpByte = (byte )( tmpByte | GetByteFromImage( image, i + 7 ) );
                char readCharacter = Convert.ToChar( tmpByte );
                if( i > 1000 )
                {
                    break;
                }
                resultString.Append( readCharacter );
                if( resultString.Length == 10 )
                {
                    blockSize = Convert.ToInt32( resultString.ToString() ) + resultString.Length;
                }
            }
            return resultString.ToString().Replace( '\0', ' ' );
        }

        private static byte[] GetBytesFromImage( string path )
        {
            Bitmap image = new Bitmap( path );
            byte[] byteArray = new byte[image.Height * image.Width * 3];
            int byteArrayIndexer = 0;
            for( int row = 0 ; row < image.Height ; row++ )
            {
                for( int column = 0 ; column < image.Width ; column++ )
                {
                    Color pixelColor = image.GetPixel( column, row );
                    byteArray[ byteArrayIndexer++ ] = pixelColor.R;
                    byteArray[ byteArrayIndexer++ ] = pixelColor.G;
                    byteArray[ byteArrayIndexer++ ] = pixelColor.B;
                }
            }
            return byteArray;
        }

        private static byte GetByteFromImage( Bitmap image, int index )
        {
            int pixelIndex = index / 3;
            int row = pixelIndex / image.Width;
            int column = pixelIndex - image.Width * row;
            byte result = 0;
            switch( index % 3 )
            {
                case 0:
                    result =  image.GetPixel( column, row ).R;
                    break;
                case 1:
                    result =  image.GetPixel( column, row ).G;
                    break;
                case 2:
                    result =  image.GetPixel( column, row ).B;
                    break;
                default:
                    throw new IndexOutOfRangeException("Pixel not in image?");
            }
            return (byte)( result & ~254 );
        }

        private static void SaveToImage( string originalPath, string targetPath, string text )
        {
            Bitmap image = new Bitmap( originalPath );
            bool[] bitArray = SeparateBits( text.Length.ToString().PadLeft(10, '0') + text );

            int bitArrayIndex = 0;
            for( int row = 0 ; row < image.Height ; row++ )
            {
                for( int column = 0 ; column < image.Width ; column++ )
                {
                    Color originalColor = image.GetPixel( column, row );
                    byte red = SetLastBit( originalColor.R, bitArray[bitArrayIndex++]);
                    if( bitArrayIndex >= bitArray.Length )
                    {
                        break;
                    }
                    byte green = SetLastBit( originalColor.G, bitArray[bitArrayIndex++]);
                    if( bitArrayIndex >= bitArray.Length )
                    {
                        break;
                    }
                    byte blue = SetLastBit( originalColor.B, bitArray[bitArrayIndex++]);
                    if( bitArrayIndex >= bitArray.Length )
                    {
                        break;
                    }
                    Color newColor = Color.FromArgb(originalColor.A, red, green, blue);
                    image.SetPixel( column, row, newColor );
                }
                if( bitArrayIndex >= bitArray.Length )
                {
                    break;
                }
            }
            if( File.Exists( targetPath ) )
            {
                File.Delete( targetPath );
            }
            image.Save( targetPath, System.Drawing.Imaging.ImageFormat.Bmp );
        }

        private static void TestFileData( string path )
        {
            Bitmap image = new Bitmap( path );
            for( int i = 0 ; i < 200 ; i++ )
            {
                Console.Write( GetByteFromImage( image, i ) );
                if( (i+1) % 8 == 0 )
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
