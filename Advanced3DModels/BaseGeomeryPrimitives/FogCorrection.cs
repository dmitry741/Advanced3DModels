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

        public FogCorrection(float density)
        {
            _density = density;
        }

        private float Fog(float d)
        {
            return d > 0 ? 1.0f / (1.0f + _density * d * d) : 1.0f;
        }

        public Color Correct(float d, Color color)
        {
            float cFogColor = 255.0f;
            float[] channels = new float[] { color.R, color.G, color.B };
            float t = Fog(d);
            IEnumerable<int> fcc = channels.Select(x => Convert.ToInt32((x - cFogColor) * t + cFogColor));
            return Color.FromArgb(fcc.ElementAt(0), fcc.ElementAt(1), fcc.ElementAt(2));
        }
    }
}
