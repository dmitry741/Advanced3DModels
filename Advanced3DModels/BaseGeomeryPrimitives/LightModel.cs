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

        public static Color GetColor(Point3D point, Vector3 Normal, bool reflection, float reflectionBrightness, float reflectionCone, Color baseColor, AbstractLightSource lightSource, Point3D pointObserver, IFog ifog)
        {
            Point3D point0 = point;
            Vector3 vectorNormal = Vector3.Normalize(Normal);
            Vector3 vectorToLightPoint = Vector3.Normalize(lightSource.GetRay(point0));

            float cosinus = Vector3.Dot(vectorNormal, vectorToLightPoint);
            float lerp = (1 - MinimalPart) * (1 + cosinus) / 2 + MinimalPart;

            float R = baseColor.R * lerp;
            float G = baseColor.G * lerp;
            float B = baseColor.B * lerp;

            if (reflection)
            {
                Vector3 vectorReflect = Vector3.Reflect(-vectorToLightPoint, vectorNormal);
                Vector3 vectorToObserver = pointObserver.ToVector3() - point0.ToVector3();

                cosinus = Vector3.Dot(vectorReflect, Vector3.Normalize(vectorToObserver));
                
                if (cosinus > 0)
                {
                    float a = reflectionBrightness;
                    float b = reflectionCone;
                    float x = 1 - cosinus;

                    float refl = lightSource.Weight * a / (1 + b * x * x);

                    R += refl;
                    G += refl;
                    B += refl;

                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                }
            }

            Color colorForRender = Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));

            if (ifog != null && ifog.Enabled)
            {
                colorForRender = ifog.Correct(point.Z, colorForRender);
            }

            return colorForRender;
        }

        public static Color GetColor(LightModelParameters lightModelParameters)
        {
            Vector3 vectorNormal = Vector3.Normalize(lightModelParameters.Normal);

            float R = lightModelParameters.BaseColor.R * MinimalPart;
            float G = lightModelParameters.BaseColor.G * MinimalPart;
            float B = lightModelParameters.BaseColor.B * MinimalPart;

            foreach (AbstractLightSource ls in lightModelParameters.LightSources)
            {
                Vector3 vectorToLightPoint = Vector3.Normalize(ls.GetRay(lightModelParameters.Point));

                float cosinus = Vector3.Dot(vectorNormal, vectorToLightPoint);
                float lerp = ls.Weight * (1 - MinimalPart) * (1 + cosinus) / 2;

                float rls = lightModelParameters.BaseColor.R * lerp;
                float gls = lightModelParameters.BaseColor.G * lerp;
                float bls = lightModelParameters.BaseColor.B * lerp;

                if (lightModelParameters.Reflection)
                {
                    Vector3 vectorReflect = Vector3.Reflect(-vectorToLightPoint, vectorNormal);
                    Vector3 vectorToObserver = lightModelParameters.PointObserver.ToVector3() - lightModelParameters.Point.ToVector3();

                    cosinus = Vector3.Dot(vectorReflect, Vector3.Normalize(vectorToObserver));

                    if (cosinus > 0)
                    {
                        float a = lightModelParameters.ReflectionBrightness;
                        float b = lightModelParameters.ReflcetionCone;
                        float x = 1 - cosinus;

                        float refl = ls.Weight * a / (1 + b * x * x);

                        rls += refl;
                        gls += refl;
                        bls += refl;
                    }
                }

                R += rls;
                G += gls;
                B += bls;
            }

            if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;

            Color colorForRender = Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));

            if (lightModelParameters.Fog != null && lightModelParameters.Fog.Enabled)
            {
                colorForRender = lightModelParameters.Fog.Correct(lightModelParameters.Point.Z, colorForRender);
            }

            return colorForRender;
        }
    }
}
