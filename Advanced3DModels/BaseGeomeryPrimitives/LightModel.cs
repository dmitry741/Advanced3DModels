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
        public static Color GetColor(Triangle triangle, ILightSource lightSource)
        {
            const float cMinimalPart = 0.1f;

            Vector3 vectorNormal = Vector3.Normalize(triangle.Normal);
            Vector3 vectorToLightPoint = Vector3.Normalize(lightSource.GetRay(triangle.Point0));

            float cosinus = Vector3.Dot(vectorNormal, vectorToLightPoint);
            Color color = triangle.Color;
            float lerp = (1 - cMinimalPart) * (1 + cosinus) / 2 + cMinimalPart;

            float R = color.R * lerp;
            float G = color.G * lerp;
            float B = color.B * lerp;

            if (triangle.Reflection)
            {
                Vector3 vectorReflect = Vector3.Reflect(-vectorToLightPoint, vectorNormal);
                Vector3 vectorToObserver = new Vector3(0, 0, -1);

                cosinus = Vector3.Dot(vectorReflect, vectorToObserver);
                
                if (cosinus > 0)
                {
                    float a = triangle.ReflectionBrightness;
                    float b = triangle.ReflectionCone;
                    float x = 1 - cosinus;

                    float reflection = Convert.ToSingle(a * Math.Exp(-b * x * x));

                    R += reflection;
                    G += reflection;
                    B += reflection;

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                }
            }

            return Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));
        }
    }
}
