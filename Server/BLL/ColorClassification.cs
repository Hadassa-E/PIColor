using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    //מחלקה לסיווג צבעים
    public static class ColorClassification
    {
        //HSL סיווג צבעים לפי ערכי
        private static Color ClassifyByHSL(Color c)
        {
            float hue = c.GetHue();
            float sat = c.GetSaturation();
            float lgt = c.GetBrightness();
            if (lgt < 0.2) return Color.Black;//שחור
            if (lgt > 0.8) return Color.White;//לבן
            if (sat < 0.25) return Color.Gray;//אפור
            if (hue < 5) return Color.Red;//אדום
            if (hue < 15) return Color.Pink;//ורוד
            if (hue < 20) return Color.Chocolate;//חום
            if (hue < 50) return Color.Orange;//כתום
            if (hue < 65) return Color.Yellow;//צהוב
            if (hue < 110) return Color.YellowGreen;//ירוק בהיר
            if (hue < 150) return Color.Green;//ירוק כהה
            if (hue < 220) return Color.LightBlue;//תכלת
            if (hue < 270) return Color.FromArgb(0, 53, 138);//כחול
            if (hue < 320) return Color.FromArgb(166, 14, 178);//סגול
            if (hue < 350) return Color.Pink;//ורוד
            else return Color.Red;//אדום
        }

        #region פונקציות עזר לצמצום גוונים על ידי מסכות

        //מיסוך צבע
        private static Color cutOff(Color c, byte mask)
        {
            return Color.FromArgb(255, c.R & mask, c.G & mask, c.B & mask);
        }

        //הפרשי ערכי הפיקסלים
        private static int GetDifference(Color color1, Color color2)
        {
            int diff = 0;
            diff += Math.Abs(color1.R - color2.R);
            diff += Math.Abs(color1.G - color2.G);
            diff += Math.Abs(color1.B - color2.B);
            return diff;
        }

        //הצבע הכי דומה
        private static Color SimilarPixel(Color OriginalPixel, Color c1, Color c2, Color c3)
        {
            if (GetDifference(OriginalPixel, c1) < GetDifference(OriginalPixel, c2) && GetDifference(OriginalPixel, c1) < GetDifference(OriginalPixel, c3))
                return c1;
            else
                if (GetDifference(OriginalPixel, c2) < GetDifference(OriginalPixel, c1) && GetDifference(OriginalPixel, c2) < GetDifference(OriginalPixel, c3))
                return c2;
            else
                return c3;
        }

        // dictionaryהוספת צבע ל
        private static void AddToHisto(Dictionary<Color, int> histo, Color c)
        {
            if (histo.ContainsKey(c))
                histo[c] = histo[c] + 1;
            else
                histo.Add(c, 1);
        }

        //ספירה כמה פעמים יש כל צבע בתמונה
        private static Dictionary<Color, int> CountColors(Bitmap original, ref Bitmap coloringPic)
        {
            Dictionary<Color, int> histo = new Dictionary<Color, int>();
            for (int x = 0; x < original.Size.Width; x++)
            {
                for (int y = 0; y < original.Size.Height; y++)
                {
                    //mask: 11100000
                    Color c1 = cutOff(original.GetPixel(x, y), 255 << 5 & 0xff);
                    AddToHisto(histo, c1);

                    //mask: 11010000
                    Color c2 = cutOff(original.GetPixel(x, y), 208 & 0xff);
                    AddToHisto(histo, c2);

                    //mask: 10110000
                    Color c3 = cutOff(original.GetPixel(x, y), 176 & 0xff);
                    AddToHisto(histo, c3);

                    Color theC = SimilarPixel(original.GetPixel(x, y), c1, c2, c3);
                    coloringPic.SetPixel(x, y, theC);
                }
            }
            return histo;
        }

        // מיפוי כל צבע בתמונה לצבע התואם הקרוב ביותר בצבעים הנפוצים ביותר
        private static Dictionary<Color, Color> CreateMap(List<KeyValuePair<Color, int>> orderHistro, List<Color> mostUsedColors)
        {
            double temp;
            Dictionary<Color, double> dist = new Dictionary<Color, double>();//עבור הצבעים המקוריים והממוצע בינהם לבין הצבע הנפוץ
            Dictionary<Color, Color> mapping = new Dictionary<Color, Color>();//עבור הצביעם הממופים

            foreach (var o in orderHistro)
            {
                dist.Clear();
                foreach (Color c in mostUsedColors)//מאה הצבעים הנפוצים
                {
                    temp = Math.Abs(o.Key.R - c.R) +
                           Math.Abs(o.Key.G - c.G) +
                           Math.Abs(o.Key.B - c.B);
                    //הכנסה של הצבע
                    //המפתח זה הצבע המקורי
                    //והערך הוא חישוב ממוצע בין הצבע המקורי לצבע הנפוץ ביותר
                    dist.Add(c, temp);
                }
                //מיון לפי צבע הממוצע
                var min = dist.OrderBy(k => k.Value).FirstOrDefault();
                //mappingהכנסת הצבע הכי מתאים ל
                mapping.Add(o.Key, min.Key);
            }
            return mapping;
        }

        // יצירת תמונה עם הצבעים שמופו
        private static Bitmap MapColors(Bitmap coloringPic, Dictionary<Color, Color> mapping)
        {
            Bitmap result = new Bitmap(coloringPic);

            for (int x = 0; x < result.Size.Width; x++)
                for (int y = 0; y < result.Size.Height; y++)
                {
                    Color color;
                    //11100000
                    color = cutOff(result.GetPixel(x, y), 255 << 5 & 0xff);
                    if (mapping.ContainsKey(color))
                    {
                        Color mappedColor = mapping[color];
                        Color newColor = ClassifyByHSL(mappedColor);
                        result.SetPixel(x, y, newColor);
                    }
                    else
                    {
                        //10110000
                        color = cutOff(result.GetPixel(x, y), 208 & 0xff);
                        if (mapping.ContainsKey(color))
                        {
                            Color mappedColor = mapping[color];
                            Color newColor = ClassifyByHSL(mappedColor);
                            result.SetPixel(x, y, newColor);
                        }
                        else
                        {
                            //10110000
                            color = cutOff(result.GetPixel(x, y), 176 & 0xff);
                            Color mappedColor = mapping[color];
                            Color newColor = ClassifyByHSL(mappedColor);
                            result.SetPixel(x, y, newColor);
                        }
                    }
                }
            return result;
        }

        #endregion

        //פונקציה לצמצום גוונים על ידי מסכות
        public static Bitmap MainClassify(ref Bitmap original)
        {
            Bitmap coloringPic = new Bitmap(original);
            // מונה גוונים ממוסכים בתמונה
            Dictionary<Color, int> histo = CountColors(original, ref coloringPic);
            //כל הצבעים שבתמונה ממויינים מהנפוץ ביותר
            List<KeyValuePair<Color, int>> orderHisto = histo.OrderByDescending(a => a.Value).ToList();
            //מאה הצבעים הנפוצים ביותר
            List<Color> mostUsedColors = orderHisto.Select(x => x.Key)
                                                   .Take(100)
                                                   .ToList();
            // מיפוי כל צבע בתמונה לצבע התואם הקרוב ביותר בצבעים הנפוצים ביותר
            Dictionary<Color, Color> mapping = CreateMap(orderHisto, mostUsedColors);
            //יצירת תמונה מהצבעים שמופו
            Bitmap result = MapColors(coloringPic, mapping);

            return result;
        }


    }
}
