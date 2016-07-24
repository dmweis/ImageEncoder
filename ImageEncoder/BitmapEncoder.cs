using System;
using System.Drawing;
using System.Text;

namespace ImageEncoder
{
    public class BitmapEncoder
    {
        public static string DecodeImage( string imagePath )
        {
            StringBuilder resultString = new StringBuilder();
            Bitmap image = new Bitmap( imagePath );
            int blockSize = int.MaxValue;
            for( int i = 0 ; i < ( image.Width * image.Height * 3 ) / 8 ; i += 8 )
            {
                if( resultString.Length >= blockSize )
                {
                    resultString.Remove( 0, 10 );
                    break;
                }
                byte tmpByte = 0;
                tmpByte = (byte)( tmpByte | ( GetByteFromImage( image, i ) << 7 ) );
                tmpByte = (byte)( tmpByte | ( GetByteFromImage( image, i + 1 ) << 6 ) );
                tmpByte = (byte)( tmpByte | ( GetByteFromImage( image, i + 2 ) << 5 ) );
                tmpByte = (byte)( tmpByte | ( GetByteFromImage( image, i + 3 ) << 4 ) );
                tmpByte = (byte)( tmpByte | ( GetByteFromImage( image, i + 4 ) << 3 ) );
                tmpByte = (byte)( tmpByte | ( GetByteFromImage( image, i + 5 ) << 2 ) );
                tmpByte = (byte)( tmpByte | ( GetByteFromImage( image, i + 6 ) << 1 ) );
                tmpByte = (byte)( tmpByte | GetByteFromImage( image, i + 7 ) );
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

        public static void EncodeImage( string originalImagePath, string newImagePath, string message )
        {
            Bitmap image = new Bitmap( originalImagePath );
            bool[] bitArray = SeparateBits( message.Length.ToString().PadLeft(10, '0') + message );

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
            image.Save( newImagePath, System.Drawing.Imaging.ImageFormat.Bmp );
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

        private static byte SetLastBit( byte originalByte, bool LastBitValue )
        {
            return (byte)( LastBitValue ? ( originalByte | ( 1 << 0 ) ) : ( originalByte & ~( 1 << 0 ) ) );
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
                    result = image.GetPixel( column, row ).R;
                    break;
                case 1:
                    result = image.GetPixel( column, row ).G;
                    break;
                case 2:
                    result = image.GetPixel( column, row ).B;
                    break;
                default:
                    throw new IndexOutOfRangeException( "Pixel not in image?" );
            }
            return (byte)( result & ~254 );
        }
    }
}
