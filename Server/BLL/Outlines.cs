using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    //מחלקה לחילוץ קווי מתאר מהתמונה המקורית
    public static class Outlines
    {
        public const int width = 800, height = 600;//גובה ורוחב קבועים לתמונה
        static int numX = 40;
        static int numY = 30;
        const Int32 avg = 200;//ערך ממוצע
        const Int32 FF = 255;
        const Int32 zero = 0;

        //פונקציה הממירה את התמונה לגווני שחור ולבן
        public static void ToBlackWhite(ref Bitmap picMap)
        {
            Color newColor;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    Color pixelColor = picMap.GetPixel(i, j);
                    if (pixelColor.R <= avg && pixelColor.G <= avg && pixelColor.B <= avg)
                        newColor = Color.FromArgb(zero, zero, zero);//שחור
                    else
                        newColor = Color.FromArgb(FF, FF, FF);//לבן
                    picMap.SetPixel(i, j, newColor);
                }
        }

        //פונקציה המחלצת את קווי המתאר של התמונה
        public static void ToOutslines(ref Bitmap WhiteBlack)
        {
            Color black = Color.Black, white = Color.White;
            for (int i = 0; i < width - 1; i++)
                for (int j = 0; j < height - 1; j++)
                {
                    if ((WhiteBlack.GetPixel(i, j) != WhiteBlack.GetPixel(i + 1, j)) ||// הפיקסל שונה מהפיקסל שמתחתיו 
                    (WhiteBlack.GetPixel(i, j) != WhiteBlack.GetPixel(i, j + 1)) ||//שונה מימינו
                    (WhiteBlack.GetPixel(i, j) != WhiteBlack.GetPixel(i + 1, j + 1)))//או שונה מהפיקסל מתחתיו בצד ימין
                    {
                        WhiteBlack.SetPixel(i, j, black);

                    }
                    else
                        WhiteBlack.SetPixel(i, j, white);
                }
            //מסגרת לתמונה בשביל הרקע
            for (int i = 0; i < height; i++)
            {
                WhiteBlack.SetPixel(width - 1, i, Color.Black);
                WhiteBlack.SetPixel(0, i, Color.Black);
            }
            for (int i = 0; i < width; i++)
            {
                WhiteBlack.SetPixel(i, height - 1, Color.Black);
                WhiteBlack.SetPixel(i, 0, Color.Black);
            }
        }
    }
}
