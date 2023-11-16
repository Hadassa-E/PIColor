using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    //מחלקה לזיהוי צורות סגורות בתמונה
    public static class ShapesRecognition
    {
        static Color black = Color.FromArgb(255, 0, 0, 0);

        //פונקציית עזר לחישוב אומדן של שטח הצורה
        private static double polygonArea(List<Point> points)
        {
            double area = 0.0;
            int j = points.Count() - 1;
            for (int i = 0; i < points.Count(); i++)
            {
                area += (points[j].X + points[i].X) * (points[j].Y - points[i].Y);
                j = i;
            }
            return Math.Abs(area / 2.0);
        }

        //פונקציה המקבלת את התמונה של קווי המתאר
        //הפונקציה מחזירה רשימה של נקודות ההיקף של הצורות הסגורות
        public static SortedList<double, List<Point>> ShapesRecognitionFromImage(ref Bitmap outLinesPic)
        {
            List<Point> existPoints = new List<Point>();
            SortedList<double, List<Point>> shapes = new SortedList<double, List<Point>>();//רשימה ממוינת לצורות סגורות
            List<Point> pointsToShape = new List<Point>();
            for (int i = 0; i < Outlines.width; i++)
                for (int j = 0; j < Outlines.height; j++)
                {
                    //אם הנקודה היא חלק מקו מתאר ועדיין לא שובצה באף צורה
                    if (outLinesPic.GetPixel(i, j) == black && !existPoints.Contains(new Point(i, j)))
                    {
                        pointsToShape.Clear();//ניקוי הרשימה לצורה הבאה
                        Point thisPoint = new Point(i, j);
                        pointsToShape = FindOneShape(thisPoint,existPoints,outLinesPic);
                        double s = polygonArea(pointsToShape);
                        if (s > 200)//אם הצורה איננה קטנה מידי
                        {
                            int k = 0;
                            while (k < pointsToShape.Count())//הוספת הנקודות לרשימת הנקודות ששובצו
                            {
                                existPoints.Add(pointsToShape[k]);
                                k++;
                            }

                            while (shapes.ContainsKey(s))//במקרה שיש צורה נוספת באותו גודל
                                s += 0.1;
                            shapes.Add(s, new List<Point>(pointsToShape));
                        }

                    }
                }
            if (shapes.Count() <= 1)//אם אין צורות מלבד המסגרת
                return null;
            return shapes;
        }
        //פונקציה לזיהוי היקף צורה אחת התמונה
        private static List<Point> FindOneShape(Point thisPoint,List<Point> existPoints,Bitmap outLinesPic)
        {
            Point nextPoint=new Point(thisPoint.X,thisPoint.Y);
            List<Point> points = new List<Point>();
            do
            {
                points.Add(nextPoint);
                thisPoint = new Point(nextPoint.X, nextPoint.Y);
                //➡ (i,j+1)
                if (!points.Contains(new Point(thisPoint.X, thisPoint.Y + 1)) && outLinesPic.Height > thisPoint.Y + 1 && outLinesPic.GetPixel(thisPoint.X, thisPoint.Y + 1) == black)
                    nextPoint = new Point(thisPoint.X, thisPoint.Y + 1);
                //⬇ (i+1, j) 
                else if (!points.Contains(new Point(thisPoint.X + 1, thisPoint.Y)) && outLinesPic.Width > thisPoint.X + 1 && outLinesPic.GetPixel(thisPoint.X + 1, thisPoint.Y) == black)
                    nextPoint = new Point(thisPoint.X + 1, thisPoint.Y);
                //⬅ (i ,j-1) 
                else if (!points.Contains(new Point(thisPoint.X, thisPoint.Y - 1)) && thisPoint.Y > 0 && outLinesPic.GetPixel(thisPoint.X, thisPoint.Y - 1) == black)
                    nextPoint = new Point(thisPoint.X, thisPoint.Y - 1);
                //⬆ (i-1, j)
                else if (!points.Contains(new Point(thisPoint.X - 1, thisPoint.Y)) && thisPoint.X > 0 && outLinesPic.GetPixel(thisPoint.X - 1, thisPoint.Y) == black)
                    nextPoint = new Point(thisPoint.X - 1, thisPoint.Y);
                //↘ (i+1,j+1)
                else if (!points.Contains(new Point(thisPoint.X + 1, thisPoint.Y + 1)) && outLinesPic.Height > thisPoint.Y + 1 && outLinesPic.Width > thisPoint.X + 1 && outLinesPic.GetPixel(thisPoint.X + 1, thisPoint.Y + 1) == black)
                    nextPoint = new Point(thisPoint.X + 1, thisPoint.Y + 1);
                //↙ (i+1,j-1)
                else if (!points.Contains(new Point(thisPoint.X + 1, thisPoint.Y - 1)) && outLinesPic.Width > thisPoint.X + 1 && thisPoint.Y > 0 && outLinesPic.GetPixel(thisPoint.X + 1, thisPoint.Y - 1) == black)
                    nextPoint = new Point(thisPoint.X + 1, thisPoint.Y - 1);
                //↖ (i-1,j-1)
                else if (!points.Contains(new Point(thisPoint.X - 1, thisPoint.Y - 1)) && thisPoint.Y > 0 && thisPoint.X > 0 && outLinesPic.GetPixel(thisPoint.X - 1, thisPoint.Y - 1) == black)
                    nextPoint = new Point(thisPoint.X - 1, thisPoint.Y - 1);
                //↗ (i-1,j+1)
                else if (!points.Contains(new Point(thisPoint.X - 1, thisPoint.Y + 1)) && thisPoint.X > 0 && outLinesPic.Height > thisPoint.Y + 1 && outLinesPic.GetPixel(thisPoint.X, thisPoint.Y + 1) == black)
                    nextPoint = new Point(thisPoint.X - 1, thisPoint.Y + 1);
            } while (!existPoints.Contains(nextPoint) && !points.Contains(nextPoint));
            Point lastPoint = points.FirstOrDefault(x => (x.X == thisPoint.X && x.Y == thisPoint.Y + 1) ||//➡ (i,j+1)
                        (x.X == thisPoint.X + 1 && x.Y == thisPoint.Y + 1) ||//↘ (i+1,j+1)
                        (x.X == thisPoint.X + 1 && x.Y == thisPoint.Y) ||//⬇ (i+1,j) 
                        (x.X == thisPoint.X + 1 && x.Y == thisPoint.Y - 1) ||//↙ (i+1,j-1)
                        (x.X == thisPoint.X && x.Y == thisPoint.Y - 1) ||//⬅ (i,j-1)
                        (x.X == thisPoint.X - 1 && x.Y == thisPoint.Y - 1) ||//↖ (i-1,j-1)
                        (x.X == thisPoint.X - 1 && x.Y == thisPoint.Y) ||//⬆ (i-1, j)
                        (x.X == thisPoint.X - 1 && x.Y == thisPoint.Y + 1));//↗ (i-1,j+1)
            points.Add(lastPoint);
            int p = points.IndexOf(lastPoint);
            //מחיקת התחום שמחוץ לצורה הסגורה
            points.RemoveRange(0, p);
            return points;
        }
    }
}
