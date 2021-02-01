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
        private static float MinimalPart => 0.1f;

        public static Color GetColor(Triangle triangle, Color color, AbstractLightSource lightSource, Point3D pointObserver)
        {
            Point3D point0 = triangle.Point0;
            Vector3 vectorNormal = Vector3.Normalize(triangle.Normal);
            Vector3 vectorToLightPoint = Vector3.Normalize(lightSource.GetRay(point0));

            float cosinus = Vector3.Dot(vectorNormal, vectorToLightPoint);
            float lerp = (1 - MinimalPart) * (1 + cosinus) / 2 + MinimalPart;

            float R = color.R * lerp;
            float G = color.G * lerp;
            float B = color.B * lerp;

            if (triangle.Reflection)
            {
                Vector3 vectorReflect = Vector3.Reflect(-vectorToLightPoint, vectorNormal);
                Vector3 vectorToObserver = new Vector3(pointObserver.X - point0.X, pointObserver.Y - point0.Y, pointObserver.Z - point0.Z);

                cosinus = Vector3.Dot(vectorReflect, Vector3.Normalize(vectorToObserver));
                
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

        public static Color GetColor(Triangle triangle, Color color, IEnumerable<AbstractLightSource> lightSources, Point3D pointObserver)
        {
            Point3D point0 = triangle.Point0;
            Vector3 vectorNormal = Vector3.Normalize(triangle.Normal);

            float R = color.R * MinimalPart;
            float G = color.G * MinimalPart;
            float B = color.B * MinimalPart;

            foreach (AbstractLightSource ls in lightSources)
            {
                Vector3 vectorToLightPoint = Vector3.Normalize(ls.GetRay(point0));

                float cosinus = Vector3.Dot(vectorNormal, vectorToLightPoint);
                float lerp = ls.Weight * (1 - MinimalPart) * (1 + cosinus) / 2;

                float rls = color.R * lerp;
                float gls = color.G * lerp;
                float bls = color.B * lerp;

                if (triangle.Reflection)
                {
                    Vector3 vectorReflect = Vector3.Reflect(-vectorToLightPoint, vectorNormal);
                    Vector3 vectorToObserver = new Vector3(pointObserver.X - point0.X, pointObserver.Y - point0.Y, pointObserver.Z - point0.Z);

                    cosinus = Vector3.Dot(vectorReflect, Vector3.Normalize(vectorToObserver));

                    if (cosinus > 0)
                    {
                        float a = triangle.ReflectionBrightness;
                        float b = triangle.ReflectionCone;
                        float x = 1 - cosinus;

                        float reflection = ls.Weight * a / (1 + b * x * x);

                        rls += reflection;
                        gls += reflection;
                        bls += reflection;
                    }
                }

                R += rls;
                G += gls;
                B += bls;
            }

            if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;

            return Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));
        }
    }
}
