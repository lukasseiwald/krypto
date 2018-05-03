using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace Watermarking
{
    class MainClass
    {
        public static Bitmap watermarkPixels(Bitmap img, int einbettungsstärke, int seed){

            Bitmap watermarkedImg = new Bitmap(img);
            Random rnd = new Random(seed); 
            Color c;

            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    c = img.GetPixel(j, i);

                    int random = rnd.Next(2);
                    if (random == 0)
                        random = -1 * einbettungsstärke;
                    else
                        random = einbettungsstärke;

                    //Console.WriteLine("pixel green before: " + c.G);
                    //Console.WriteLine("pixel green after: " + addRandomNumber(c.G, random));

                    Color newC = Color.FromArgb(c.R, addRandomNumber(c.G, random), c.B);

                    watermarkedImg.SetPixel(j, i, newC);
                }
            }
            double korrelationsErgebnis = detectWatermarkedImage(watermarkedImg, seed);
            Console.Write("mit watermark: " + korrelationsErgebnis);

            return watermarkedImg;
        }


        public static double detectWatermarkedImage(Bitmap img, int seed){
            Random rnd = new Random(seed);
            double korrelationsErgebnis = 0;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color c = img.GetPixel(j, i);
                    int g = c.G;

                    int plusOrMinus = rnd.Next(2);
                    if (plusOrMinus == 0)
                        plusOrMinus = -1;

                    g = plusOrMinus * g;

                    korrelationsErgebnis += g;
                }
            }

            korrelationsErgebnis /= (img.Height * img.Width);

            return korrelationsErgebnis;
        }

        public static byte addRandomNumber(byte colorValue, int randomNumber){
            if ((colorValue + randomNumber) < 0)
                return 0;
            else if ((colorValue + randomNumber) > 255)
                return 255;
            else
            {
                return ((byte)(colorValue + randomNumber));
            }
            
        }

        public static void Main(string[] args)
        {
            try
            {
                Bitmap img = new Bitmap("lena.png");

                Console.WriteLine("Bitte eine Einbettungsstäarke wählen (1-10): ");
                int einbettungsstärke = Convert.ToInt32(Console.ReadLine());

                int seed = 87939;
                //Random rnd = new Random(seed);

                Bitmap watermarkedImg = watermarkPixels(img, einbettungsstärke, seed);

                //double korrelationsErgebnis = detectWatermarkedImage(img, seed);

                //Console.WriteLine("ohne watermark: " + korrelationsErgebnis);

                watermarkedImg.Save("watermarked.png", ImageFormat.Png);
            }

            catch (Exception e)
            {
                Console.WriteLine("Ups, there has been a problem" + e.Message);
            }
        }
    }
}
