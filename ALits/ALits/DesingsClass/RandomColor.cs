using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using ALits.ViewPages;
using ALits.Models;
using System.Globalization;

namespace ALits.DesingsClass
{
    public class RandomColor
    {
        public RandomColor()
        {

        }
        public string RandomColorPicker(int color)
        {
             if (color % 3 == 0)
            {
                return "#54c1e5";
            }
            else if (color % 2 == 0)
            {
                return "#bfeb80";
            }
             else 
            {
                return "#db7093";
            }
        }
        public string ChangeColorBrightness( string ColorHex)
        {
            float correctionFactor = -0.4f;
            Color color = HexToColor(ColorHex);
            Color colorChanged = ChangeColorBrightness(color, correctionFactor);
            string hex ="#"+ colorChanged.R.ToString("X2") + colorChanged.G.ToString("X2") + colorChanged.B.ToString("X2");
            return hex;
        }
        public static Color HexToColor(string hexString)
        {
            //replace # occurences
            if (hexString.IndexOf('#') != -1)
                hexString = hexString.Replace("#", "");

            int r, g, b = 0;

            r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            return Color.FromArgb(r, g, b);
        }
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
    }
}
