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
            
            if (renderType == RenderModelType.Triangulations)
            {
                Color color = Color.FromArgb(255 - backColor.R, 255 - backColor.G, 255 - backColor.B);
                Pen pen = new Pen(color);
                IEnumerable<Triangle> triangles = model.GetTrianglesForRender(null, null, null, renderType);

                foreach(Triangle triangle in triangles)
                {
                    g.DrawPolygon(pen, triangle.Points);
                }                    
            }
            else if (renderType == RenderModelType.FillFull)
            {
                IEnumerable<Triangle> triangles = model.GetTrianglesForRender(new List<ILightSource> { lightSource }, pointObserver, ifog, renderType);

                foreach (Triangle triangle in triangles)
                {
                    if (renderFillTriangle == RenderFillTriangle.Flat || !triangle.AllowToGouraudMethod)
                    {
                        Brush brush = new SolidBrush(triangle.Point0.ColorForRender);
                        g.FillPolygon(brush, triangle.Points);
                    }
                    else
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
            else if (renderType == RenderModelType.FillSolidColor)
            {
                Color colorPen = Color.FromArgb(255 - backColor.R, 255 - backColor.G, 255 - backColor.B);
                Color colorBrush = backColor;
                Pen pen = new Pen(colorPen);
                Brush brush = new SolidBrush(colorBrush);

                IEnumerable<Triangle> triangles = model.GetTrianglesForRender(null, null, null, renderType);

                foreach (Triangle triangle in triangles)
                {
                    g.FillPolygon(brush, triangle.Points);
                    g.DrawPolygon(pen, triangle.Points);
                }
            }
        }
    }
}
