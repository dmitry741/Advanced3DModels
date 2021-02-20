using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Models3DLib;
using System.Drawing.Drawing2D;

namespace Advanced3DModels
{    
    class RenderingModel
    {
        public static void Render(Graphics g, 
            Model model, 
            ILightSource lightSource, 
            IPoint3D pointObserver, 
            IFog ifog, 
            RenderModelType renderType, 
            RenderFillTriangle renderFillTriangle,
            Color backColor)
        {
            IEnumerable<Triangle> triangles = model.GetTrianglesForRender(renderType);

            if (renderType == RenderModelType.Triangulations)
            {
                Color color = Color.FromArgb(255 - backColor.R, 255 - backColor.G, 255 - backColor.B);
                Pen pen = new Pen(color);

                foreach(Triangle triangle in triangles)
                {
                    g.DrawPolygon(pen, triangle.Points);
                }                    
            }
            else if (renderType == RenderModelType.FillFull)
            {
                LightModelParameters lightModelParameters = new LightModelParameters
                {
                    LightSources = new List<ILightSource>() { lightSource },
                    PointObserver = pointObserver,
                    Fog = ifog
                };

                foreach (Triangle triangle in triangles)
                {
                    lightModelParameters.Normal = triangle.Normal;
                    lightModelParameters.ReflectionEnable = triangle.ReflectionEnable;
                    lightModelParameters.ReflectionBrightness = triangle.ReflectionBrightness;
                    lightModelParameters.ReflcetionCone = triangle.ReflectionCone;

                    if (renderFillTriangle == RenderFillTriangle.Flat || !triangle.AllowToGouraudMethod)
                    {
                        lightModelParameters.Point = triangle.Point0;
                        lightModelParameters.BaseColor = triangle.BaseColor;
                        Color color = LightModel.GetColor(lightModelParameters);

                        Brush brush = new SolidBrush(color);
                        g.FillPolygon(brush, triangle.Points);
                    }
                    else
                    {
                        Color[] surroundColors = new Color[3];

                        for (int i = 0; i < 3; i++)
                        {
                            lightModelParameters.Point = triangle.Point3Ds[i];
                            lightModelParameters.BaseColor = triangle.BaseColor;
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
            else if (renderType == RenderModelType.FillSolidColor)
            {
                Color colorPen = Color.FromArgb(255 - backColor.R, 255 - backColor.G, 255 - backColor.B);
                Color colorBrush = backColor;
                Pen pen = new Pen(colorPen);
                Brush brush = new SolidBrush(colorBrush);

                foreach (Triangle triangle in triangles)
                {
                    g.FillPolygon(brush, triangle.Points);
                    g.DrawPolygon(pen, triangle.Points);
                }
            }
        }
    }
}
