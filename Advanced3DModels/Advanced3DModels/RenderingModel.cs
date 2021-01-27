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
        Fill
    }

    class RenderingModel
    {
        public static void Render(Graphics g, Model model, ILightSource lightSource, RenderType renderType)
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
            else if (renderType == RenderType.Fill)
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

                var orderedTriangles = triangles.OrderBy(t => t.Min);                

                foreach (Triangle triangle in orderedTriangles)
                {
                    Color color = LightModel.GetColor(triangle, lightSource);
                    Brush brush = new SolidBrush(color);
                    g.FillPolygon(brush, triangle.Points);
                    //g.DrawPolygon(Pens.Black, triangle.Points);
                }
            }
        }
    }
}
