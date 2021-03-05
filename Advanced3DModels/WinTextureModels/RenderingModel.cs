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
                LightSources = new List<ILightSource> { lightSource },
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
                    lightModelParameters.BaseColor = (triangle.TextureColors != null) ? triangle.TextureColors[i] : triangle.BaseColor;
                    surroundColors[i] = LightModel.GetColor(lightModelParameters);
                }

                double R = surroundColors.Average(color => color.R);
                double G = surroundColors.Average(color => color.G);
                double B = surroundColors.Average(color => color.B);

                Color averageColor = Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));

                Brush brush = new SolidBrush(averageColor);
                g.FillPolygon(brush, triangle.Points);

                /*PointF[] points = triangle.Points;

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
                }*/
            }
        }
    }
}
