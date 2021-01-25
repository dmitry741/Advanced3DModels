using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace Models3DLib
{
    public class LightModel
    {
        public ILightSource LightSource { get; set; }

        public Color GetColor(Triangle triangle, Color color)
        {
            const float cMinimalPart = 0.2f;

            Vector3 v1 = triangle.Normal;
            Vector3 v2 = LightSource.GetRay(triangle.Point0);

            float dot = Vector3.Dot(v1, v2);
            float cosinus = dot / (v1.Length() * v2.Length());

            float R = color.R * cMinimalPart;
            float G = color.G * cMinimalPart;
            float B = color.B * cMinimalPart;
            Color clResult;

            if (cosinus < 0)
            {
                clResult = Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));
            }
            else
            {

            }


            return Color.Black;
        }
    }
}
