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
                    LightSources = new List<ILightSource> { lightSource },
                    PointObserver = pointObserver,
                    Fog = ifog
                };

                foreach (Triangle triangle in triangles)
                {
                    lightModelParameters.Normal = triangle.Normal;
                    lightModelParameters.ReflectionEnable = triangle.ReflectionEnable;
                    lightModelParameters.ReflectionBrightness = triangle.ReflectionBrightness;
                    lightModelParameters.ReflcetionCone = triangle.ReflectionCone;
                    lightModelParameters.BaseColor = triangle.BaseColor;

                    if (renderFillTriangle == RenderFillTriangle.Flat0 || !triangle.AllowToGouraudMethod)
                    {
                        lightModelParameters.Point = triangle.Point0;
                        Color color = LightModel.GetColor(lightModelParameters);

                        Brush brush = new SolidBrush(color);
                        g.FillPolygon(brush, triangle.Points);
                    }
                    else if (renderFillTriangle == RenderFillTriangle.Flat3)
                    {                        
                        Color[] colors = new Color[3];

                        for (int i = 0; i < 3; i++)
                        {
                            lightModelParameters.Point = triangle.Point3Ds[i];
                            colors[i] = LightModel.GetColor(lightModelParameters);
                        }

                        int R = Convert.ToInt32(colors.Average(x => x.R));
                        int G = Convert.ToInt32(colors.Average(x => x.G));
                        int B = Convert.ToInt32(colors.Average(x => x.B));

                        Color color = Color.FromArgb(R, G, B);
                        Brush brush = new SolidBrush(color);

                        g.FillPolygon(brush, triangle.Points);
                    }
                    else
                    {
                        Color[] surroundColors = new Color[3];

                        for (int i = 0; i < 3; i++)
                        {
                            lightModelParameters.Point = triangle.Point3Ds[i];
                            surroundColors[i] = LightModel.GetColor(lightModelParameters);
                        }

                        PointF[] points = triangle.Points;

                        PathGradientBrush pthGrBrush = new PathGradientBrush(points)
                        {
                            SurroundColors = surroundColors,
                            CenterPoint = points[0],
                            CenterColor = surroundColors[0]
                        };                        

                        g.FillPolygon(pthGrBrush, points);

                        for (int i = 0; i < 3; i++)
                        {
                            System.Numerics.Vector2 v1 = new System.Numerics.Vector2(points[i].X, points[i].Y);
                            System.Numerics.Vector2 v2 = new System.Numerics.Vector2(points[(i + 1) % 3].X, points[(i + 1) % 3].Y);
                            float len = System.Numerics.Vector2.Distance(v1, v2);

                            float x1 = (points[(i + 1) % 3].X - points[i].X) * (-1) / len + points[i].X;
                            float y1 = (points[(i + 1) % 3].Y - points[i].Y) * (-1) / len + points[i].Y;

                            float x2 = (points[(i + 1) % 3].X - points[i].X) * (len + 1) / len + points[i].X;
                            float y2 = (points[(i + 1) % 3].Y - points[i].Y) * (len + 1) / len + points[i].Y;

                            PointF point1 = new PointF(x1, y1);
                            PointF point2 = new PointF(x2, y2);

                            Brush brush = new LinearGradientBrush(point1, point2, surroundColors[i], surroundColors[(i + 1) % 3]);
                            g.DrawLine(new Pen(brush), points[i], points[(i + 1) % 3]);
                        }
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
