using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    //המחלקה הראשית לעיבוד תמונה
    public class ProcessPictureBLL : IprocessPictureBLL
    {
        const int width = 800, height = 600;
        Bitmap originalPic;
        Bitmap outlinePic;
        public ProcessPictureBLL()
        {
        }

        //פונקציה ראשית שמזמנת את כל שלבי עיבוד התמונה
        public int[,] ProcessPicture(Bitmap original)
        {
            originalPic = new Bitmap(original, width, height);
            Bitmap outlinePic = new Bitmap(originalPic);

            //תמונה שחור ולבן
            Outlines.ToBlackWhite(ref outlinePic);

            //קווי המתאר של התמונה
            Outlines.ToOutslines(ref outlinePic);

            //צמצום גוונים וסיווג צבעים לתמונה המקורית
            Bitmap colorPic = ColorClassification.MainClassify(ref originalPic);

            //זיהוי היקפי צורות ושטחי צורות
            //צביעת כל צורה בצבע אחיד
            bool flag=ColoringByOutlines.ColoringByOutLines(ref colorPic, ref outlinePic);

            if (!flag)//אם אין צורות סגורות בתמונה
                return null;

            //יצירת מטריצה להחזרה מהשרת
            int[,] mat = MagnifiedPixels.SetPixels(ref outlinePic);
            return mat;
        }
    }
}
