﻿using System;
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

        public static Color GetColor(LightModelParameters lightModelParameters)
        {
            Vector3 vectorNormal = Vector3.Normalize(lightModelParameters.Normal);

            float R = lightModelParameters.BaseColor.R * MinimalPart;
            float G = lightModelParameters.BaseColor.G * MinimalPart;
            float B = lightModelParameters.BaseColor.B * MinimalPart;

            foreach (ILightSource ls in lightModelParameters.LightSources)
            {
                Vector3 vectorToLightPoint = Vector3.Normalize(ls.GetRay(lightModelParameters.Point));

                float cosinus = Vector3.Dot(vectorNormal, vectorToLightPoint);
                float lerp = ls.Weight * (1 - MinimalPart) * (1 + cosinus) / 2;

                R += lightModelParameters.BaseColor.R * lerp;
                G += lightModelParameters.BaseColor.G * lerp;
                B += lightModelParameters.BaseColor.B * lerp;

                if (lightModelParameters.ReflectionEnable)
                {
                    Vector3 vectorReflect = Vector3.Reflect(-vectorToLightPoint, vectorNormal);
                    Vector3 vectorToObserver = lightModelParameters.PointObserver.ToVector3() - lightModelParameters.Point.ToVector3();

                    cosinus = Vector3.Dot(vectorReflect, Vector3.Normalize(vectorToObserver));

                    if (cosinus > 0)
                    {
                        float a = lightModelParameters.ReflectionBrightness;
                        float b = lightModelParameters.ReflcetionCone;
                        float x = 1 - cosinus;

                        float refl = a / (1 + b * x * x);

                        R += refl;
                        G += refl;
                        B += refl;
                    }
                }
            }

            if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;

            Color colorForRender = Color.FromArgb(lightModelParameters.BaseColor.A, Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));

            return (lightModelParameters.Fog != null && lightModelParameters.Fog.Enabled) ? 
                lightModelParameters.Fog.Correct(lightModelParameters.Point.Z, colorForRender) :
                colorForRender;
        }
    }
}
