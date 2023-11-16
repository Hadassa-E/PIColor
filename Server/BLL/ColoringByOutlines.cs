using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    //מחלקה לצביעת תמונה על פי קווי המתאר
    public static class ColoringByOutlines
    {
        //צביעת צורות בצבע אחיד
        public static bool ColoringByOutLines(ref Bitmap original,ref Bitmap outLines)
        {
            List<int> sumColors = new List<int> { };
            List<Color> colors = new List<Color>() {
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
                Color.FromArgb(255,166, 14, 178),
            };
            List<Point> coloringPoints = new List<Point>();
            List<Point> PointsInShapes = new List<Point>();
            List<List<Point>> areaToAllShapes = FindAreasShapes.FindAreas(ref outLines);
            if (areaToAllShapes == null)
                return false;
            for (int i = 0; i < areaToAllShapes.Count(); i++)
            {
                sumColors.Clear();
                for (int j = 0; j < 13; j++)
                    sumColors.Add(0);
                for (int j = 0; j < areaToAllShapes[i].Count(); j++)
                    sumColors[colors.IndexOf(original.GetPixel(areaToAllShapes[i][j].X, areaToAllShapes[i][j].Y))]++;
                //סיווג צבעים וצביעת האיזור לפי הצבע הראשי
                Color c = colors[sumColors.IndexOf(sumColors.Max())];
                for (int j = 0; j < areaToAllShapes[i].Count(); j++)
                    outLines.SetPixel(areaToAllShapes[i][j].X, areaToAllShapes[i][j].Y, c);

            }
            return true;
        }
    }
}
