using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    //יצירת מטריצה מהתמונה
    public static class MagnifiedPixels
    {
        const int numX = 40, numY = 30;

        //"צבע עיקרי ב"פיקסל מוגדל 
        private static int MainColor(int n, int m, ref Bitmap b1)
        {
            List<int> countsColor = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//מערך המונים לצבעים
            List<Color> arrC = new List<Color>() {
                Color.FromArgb(255, Color.Black.R,Color.Black.G,Color.Black.B),
                Color.FromArgb(255, Color.White.R,Color.White.G,Color.White.B),
                Color.FromArgb(255, Color.Gray.R,Color.Gray.G,Color.Gray.B),
                Color.FromArgb(255, Color.Red.R,Color.Red.G,Color.Red.B),
                Color.FromArgb(255, Color.Pink.R,Color.Pink.G,Color.Pink.B),
                Color.FromArgb(255, Color.Chocolate.R,Color.Chocolate.G,Color.Chocolate.B),
                Color.FromArgb(255, Color.Orange.R,Color.Orange.G,Color.Orange.B),
                Color.FromArgb(255, Color.Yellow.R,Color.Yellow.G,Color.Yellow.B),
                Color.FromArgb(255, Color.YellowGreen.R,Color.YellowGreen.G,Color.YellowGreen.B),
                Color.FromArgb(255, Color.Green.R,Color.Green.G,Color.Green.B),
                Color.FromArgb(255, Color.LightBlue.R,Color.LightBlue.G,Color.LightBlue.B),
                Color.FromArgb(255,0, 53, 138),
                Color.FromArgb(255,166,14,178),
            };//מערך הצבעים
            int x = Convert.ToInt32(b1.Width / numX), y = Convert.ToInt32(b1.Height / numY);
            for (int i = n * x; i < n * x + x; i++)
                for (int j = m * y; j < m * y + y; j++)
                    if (arrC.IndexOf(b1.GetPixel(i, j)) >= 0 && arrC.IndexOf(b1.GetPixel(i, j)) <= 12)
                        countsColor[arrC.IndexOf(b1.GetPixel(i, j))]++;
            return countsColor.IndexOf(countsColor.Max());
        }

        //הגדלת פיקסלים והחזרת מטריצה עם מספרים שמייצגים את הצבעים
        public static int[,] SetPixels(ref Bitmap b)
        {
            int[,] data = new int[numY,numX];
            for (int i = 0; i < numY; i++)
                for (int j = 0; j < numX; j++)
                    data[i, j] = MainColor(j, i, ref b);
            return data;
        }
    }
}
