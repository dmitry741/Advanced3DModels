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
    enum RenderModelType
    {
        Triangulations,
        FillFull,
        FillWhite
    }

    enum RenderFillTriangle
    {
        Flat,
        Gouraud
    }

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
           if (renderType == RenderModelType.Triangulations)
            {
                Color color = Color.FromArgb(255 - backColor.R, 255 - backColor.G, 255 - backColor.B);
                Pen pen = new Pen(color);

                foreach(Plane plane in model.Planes)
                {
                    foreach(Triangle triangle in plane.Triangles)
                    {
                        g.DrawPolygon(pen, triangle.Points);
                    }                    
                }
            }
            else if (renderType == RenderModelType.FillFull)
            {
                IEnumerable<Plane> planesForRender = model.Planes.Where(x => x.VisibleBackSide || x.Normal.Z < 0);

                LightModelParameters lightModelParameters = new LightModelParameters
                {
                    LightSources = new List<ILightSource> { lightSource },
                    PointObserver = pointObserver,
                    Fog = ifog
                };

                foreach (Plane plane in planesForRender)
                {
                    lightModelParameters.Normal =  plane.Normal;
                    lightModelParameters.Reflection = plane.Reflection;
                    lightModelParameters.ReflectionBrightness = plane.ReflectionBrightness;
                    lightModelParameters.ReflcetionCone = plane.ReflectionCone;

                    foreach (Point3DColor pc in plane.Points)
                    {
                        lightModelParameters.Point = pc;
                        lightModelParameters.BaseColor = pc.BaseColor;

                        pc.ColorForRender = LightModel.GetColor(lightModelParameters);
                    }
                }

                IEnumerable<Triangle> triangles = planesForRender.SelectMany(x => x.Triangles);

                IEnumerable<Triangle> trianglesForRender = model.NeedToSort ?
                    triangles.OrderByDescending(t => t.MinZ).AsEnumerable() :
                    triangles;

                if (renderFillTriangle == RenderFillTriangle.Flat)
                {
                    foreach (Triangle triangle in trianglesForRender)
                    {
                        Brush brush = new SolidBrush(triangle.Point0.ColorForRender);
                        g.FillPolygon(brush, triangle.Points);
                    }
                }
                else
                {
                    foreach (Triangle triangle in trianglesForRender)
                    {
                        PointF[] points = triangle.Points;
                        Color[] surroundColors = triangle.Point3Ds.Select(pc => pc.ColorForRender).ToArray();

                        PathGradientBrush pthGrBrush = new PathGradientBrush(points)
                        {
                            SurroundColors = surroundColors,
                            CenterPoint = points[0],
                            CenterColor = triangle.Point3Ds[0].ColorForRender
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
            else if (renderType == RenderModelType.FillWhite)
            {
                IEnumerable<Plane> planesForRender = model.Planes.Where(x => x.VisibleBackSide || x.Normal.Z < 0);
                IEnumerable<Triangle> triangles = planesForRender.SelectMany(x => x.Triangles);

                IEnumerable<Triangle> trianglesForRender = model.NeedToSort ?
                    triangles.OrderByDescending(t => t.MinZ).AsEnumerable() :
                    triangles;

                Color colorPen = Color.FromArgb(255 - backColor.R, 255 - backColor.G, 255 - backColor.B);
                Color colorBrush = backColor;

                Pen pen = new Pen(colorPen);
                Brush brush = new SolidBrush(colorBrush);

                foreach (Triangle triangle in trianglesForRender)
                {
                    g.FillPolygon(brush, triangle.Points);
                    g.DrawPolygon(pen, triangle.Points);
                }
            }
        }
    }
}
