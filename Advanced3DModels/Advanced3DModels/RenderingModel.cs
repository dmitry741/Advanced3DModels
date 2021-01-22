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
        public static void Render(Graphics g, Model model, RenderType renderType)
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
                    if (plane.Normal.Z < 0)
                    {
                        triangles.AddRange(plane.Triangles);
                    }
                }

                IOrderedEnumerable<Triangle> orderedTriangles = triangles.OrderBy(t => t.Min);

                foreach(Triangle triangle in orderedTriangles)
                {
                    g.FillPolygon(Brushes.White, triangle.Points);
                    g.DrawPolygon(Pens.Black, triangle.Points);
                }
            }
        }
    }
}
