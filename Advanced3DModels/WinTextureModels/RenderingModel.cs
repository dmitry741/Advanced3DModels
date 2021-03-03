using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Models3DLib;
using System.Drawing.Drawing2D;

namespace WinTextureModels
{
    class RenderingModel
    {
        public static void Render(Graphics g, 
            Model model, ILightSource lightSource, 
            IPoint3D pointObserver,
            RenderModelType renderType)
        {
            IEnumerable<Triangle> triangles = model.GetTrianglesForRender(renderType);

            LightModelParameters lightModelParameters = new LightModelParameters
            {
                LightSources = new List<ILightSource>() { lightSource },
                PointObserver = pointObserver
            };

            foreach (Triangle triangle in triangles)
            {
                lightModelParameters.Normal = triangle.Normal;
                lightModelParameters.ReflectionEnable = triangle.ReflectionEnable;
                lightModelParameters.ReflectionBrightness = triangle.ReflectionBrightness;
                lightModelParameters.ReflcetionCone = triangle.ReflectionCone;

                Color[] surroundColors = new Color[3];

                for (int i = 0; i < 3; i++)
                {
                    lightModelParameters.Point = triangle.Point3Ds[i];
                    lightModelParameters.BaseColor = triangle.TextureColors[i];
                    surroundColors[i] = LightModel.GetColor(lightModelParameters);
                }

                PointF[] points = triangle.Points;

                PathGradientBrush pthGrBrush = new PathGradientBrush(points)
                {
                    SurroundColors = surroundColors,
                    CenterPoint = points[0],
                    CenterColor = surroundColors[0]
                };

                for (int i = 0; i < 3; i++)
                {
                    LinearGradientBrush brush = new LinearGradientBrush(points[i], points[(i + 1) % 3], surroundColors[i], surroundColors[(i + 1) % 3]);
                    g.DrawLine(new Pen(brush), points[i], points[(i + 1) % 3]);
                }

                g.FillPolygon(pthGrBrush, points);
            }
        }
    }
}
