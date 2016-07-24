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
        private static readonly string newFilePath = @"D:\Programming\C#\Visual_studio_projects\ImageEncoder\Images\";
        private const int numberOfImages = 5;
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            Bitmap image = new Bitmap(originalFilePath);
            Random rng = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            for (int p = 0; p < numberOfImages; p++)
            {
                Console.WriteLine($"{p+1} out of {numberOfImages}");
                KnownColor randomColorName = names[rng.Next(names.Length)];
                if (File.Exists(newFilePath + $"COLOR{randomColorName.ToString()}.bmp"))
                {
                    Console.WriteLine("File alredy exists");
                    Console.WriteLine("   " + $"COLOR{randomColorName.ToString()}.bmp" );
                    continue;
                }
                Color randomColor = Color.FromKnownColor(randomColorName);
                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }
                for (int i = 0; i < image.Width; i++)
                {
                    for (int o = 0; o < image.Height; o++)
                    {
                        image.SetPixel(i, o, randomColor);
                    }
                }
                image.Save( newFilePath + $"COLOR{randomColorName.ToString()}.bmp" );
                Convert.ToString( new byte(), 2 );
                Encoding.ASCII.GetBytes( "Hello" );
            }
            Console.WriteLine("Finished");
        }
    }
}
