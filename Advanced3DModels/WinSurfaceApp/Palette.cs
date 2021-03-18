using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSurfaceApp
{
    /// <summary>
    /// Класс представляющий набор цветов.
    /// </summary>
    class Palette
    {
        #region === private ===

        Color GetGradientColor(int iteration, Color color1, Color color2, int gradientCount)
        {
            double r1 = color1.R;
            double g1 = color1.G;
            double b1 = color1.B;

            double r2 = color2.R;
            double g2 = color2.G;
            double b2 = color2.B;

            double k = Convert.ToDouble(iteration) / Convert.ToDouble(gradientCount);

            double r = k * (r2 - r1) + r1;
            double g = k * (g2 - g1) + g1;
            double b = k * (b2 - b1) + b1;

            return Color.FromArgb(Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b));
        }

        #endregion

        public List<Color> BaseColors { get; set; }
        public int GradientCount { get; set; } = 2;

        public List<Color> CreatePalette()
        {
            List<Color> palette = new List<Color>();

            for (int i = 0; i < BaseColors.Count - 1; i++)
            {
                for (int j = 0; j < GradientCount; j++)
                {
                    palette.Add(GetGradientColor(j, BaseColors[i], BaseColors[i + 1], GradientCount));
                }
            }

            palette.Add(BaseColors[BaseColors.Count - 1]);

            return palette;
        }
    }
}
