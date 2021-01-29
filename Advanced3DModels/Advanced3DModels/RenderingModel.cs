using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Models3DLib;

namespace Advanced3DModels
{
    enum RenderType
    {
        Edges,
        Triangulations,
        FillFull,
        FillWhite
    }

    class RenderingModel
    {
        public static void Render(Graphics g, Model model, AbstractLightSource lightSource, Point3D pointObserver, RenderType renderType)
        {
            if (renderType == RenderType.Edges)
            {
                foreach(Edge edge in model.Edges)
                {
                    g.DrawLine(Pens.Black, edge.Point1.ToPointF(), edge.Point2.ToPointF());
                }
            }
            else if (renderType == RenderType.Triangulations)
            {
                foreach(Plane plane in model.Planes)
                {
                    foreach(Triangle triangle in plane.Triangles)
                    {
                        g.DrawPolygon(Pens.Black, triangle.Points);
                    }                    
                }
            }
            else if (renderType == RenderType.FillFull)
            {
                List<Triangle> triangles = new List<Triangle>();

                foreach (Plane plane in model.Planes)
                {
                    if (!plane.VisibleBackSide)
                    {
                        if (plane.Normal.Z < 0)
                        {
                            triangles.AddRange(plane.Triangles);
                        }
                    }
                    else
                    {
                        triangles.AddRange(plane.Triangles);
                    }
                }

                IEnumerable<Triangle> trianglesForRendering = model.NeedToSort ?
                    triangles.OrderBy(t => t.Min).AsEnumerable() :
                    triangles;

                foreach (Triangle triangle in trianglesForRendering)
                {
                    Color color = LightModel.GetColor(triangle, triangle.Color, new List<AbstractLightSource> { lightSource }, pointObserver);
                    Brush brush = new SolidBrush(color);
                    g.FillPolygon(brush, triangle.Points);
                }
            }
            else if (renderType == RenderType.FillWhite)
            {
                List<Triangle> triangles = new List<Triangle>();

                foreach (Plane plane in model.Planes)
                {
                    if (!plane.VisibleBackSide)
                    {
                        if (plane.Normal.Z < 0)
                        {
                            triangles.AddRange(plane.Triangles);
                        }
                    }
                    else
                    {
                        triangles.AddRange(plane.Triangles);
                    }
                }

                IEnumerable<Triangle> trianglesForRendering = model.NeedToSort ?
                    triangles.OrderBy(t => t.Min).AsEnumerable() :
                    triangles;

                foreach (Triangle triangle in trianglesForRendering)
                {
                    g.FillPolygon(Brushes.White, triangle.Points);
                    g.DrawPolygon(Pens.Black, triangle.Points);
                }
            }
        }
    }
}
