using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class FogCorrection : IFog
    {
        float _density = 1;
        float _zeroLevel = 0;

        public bool Enabled { get; set; }

        public FogCorrection(float density, float zeroLevel)
        {
            _density = density;
            _zeroLevel = zeroLevel;
        }

        private float Fog(float d)
        {
            return d > 0 ? 1.0f / (1.0f + _density * d * d) : 1.0f;
        }

        public Color Correct(float z, Color color)
        {
            float cFogColor = 255.0f;
            float distance = z - _zeroLevel;
            float t = Fog(distance);

            int R = Convert.ToInt32((color.R - cFogColor) * t + cFogColor);
            int G = Convert.ToInt32((color.G - cFogColor) * t + cFogColor);
            int B = Convert.ToInt32((color.B - cFogColor) * t + cFogColor);

            return Color.FromArgb(R, G, B);
        }
    }
}
