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
        Triangulations,
        FillFull,
        FillWhite
    }

    class RenderingModel
    {
        public static void Render(Graphics g, Model model, AbstractLightSource lightSource, Point3D pointObserver, IFog ifog, RenderType renderType, Color backColor)
        {
           if (renderType == RenderType.Triangulations)
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
                    triangles.OrderByDescending(t => t.Min).AsEnumerable() :
                    triangles;

                List<AbstractLightSource> lsCollection = new List<AbstractLightSource> { lightSource };

                foreach (Triangle triangle in trianglesForRendering)
                {
                    Color color = LightModel.GetColor(triangle.Point0, triangle.Normal, triangle.Reflection, triangle.ReflectionBrightness, triangle.ReflectionCone, triangle.Color, lsCollection, pointObserver, ifog);
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
                    triangles.OrderByDescending(t => t.Min).AsEnumerable() :
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
