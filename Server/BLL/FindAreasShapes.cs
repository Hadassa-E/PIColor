using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BLL
{
    //מחלקה למציאת שטחי הצורות שבתמונה
    public static class FindAreasShapes
    {

        //פונקציית עזר הבודקת האם הנקודה נמצאת בתוך הצורה
        private static bool IsPointInShape(Point point, List<Point> shape)
        {
            int count = shape.Count;
            bool inside = false;

            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                //Y אם הנקודה נמצאת בין הקודקוד הנוכחי לקודקוד הקודם על ידי השוואת  
                if (((shape[i].Y > point.Y) != (shape[j].Y > point.Y)) &&
                        //חישוב מתמטי לבדיקה
                        //אם הטלת הקרן מהנקודה לימין חותכת את הקצה בין הקודקוד הנוכחי לקודקוד הקודם
                        (point.X < (shape[j].X - shape[i].X) * (point.Y - shape[i].Y) / (shape[j].Y - shape[i].Y) + shape[i].X))
                    inside = !inside;
            }
            return inside;
        }

        //פונקציה  המקבלת תמונה של קווי מיתאר
        //הפונקציה מחזירה רשימת שטחי הצורות הסגורות בתמונה 
        public static List<List<Point>> FindAreas(ref Bitmap outLinesPic)
        {
            //זיהוי צורות סגורות על פי קווי המתאר
            SortedList<double, List<Point>> shapes = ShapesRecognition.ShapesRecognitionFromImage(ref outLinesPic);
            if (shapes == null)//אין  צורות סגורות
                return null;
            List<List<Point>> areas = new List<List<Point>>();
            for (int i = 0; i < shapes.Count; i++)
                areas.Add(new List<Point>());
            for (int i = 0; i < Outlines.width; i++)
                for (int j = 0; j < Outlines.height; j++)
                {
                    bool flag = false;
                    for (int k = 0; k < shapes.Count() && !flag; k++)
                        if (IsPointInShape(new Point(i, j), shapes.GetValueAtIndex(k)))
                        {
                            areas[k].Add(new Point(i, j));
                            flag = true;
                        }
                }
            return areas;
        }

    }
}
