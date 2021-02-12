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
        public static void Render(Graphics g, Model model, ILightSource lightSource, IPoint3D pointObserver, IFog ifog, RenderType renderType, Color backColor)
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

                List<Triangle> triangles = new List<Triangle>();

                foreach (Plane plane in planesForRender)
                {
                    triangles.AddRange(plane.Triangles);
                }

                IEnumerable<Triangle> trianglesForRender = model.NeedToSort ?
                    triangles.OrderByDescending(t => t.MinZ).AsEnumerable() :
                    triangles;

                foreach (Triangle triangle in trianglesForRender)
                {
                    Brush brush = new SolidBrush(triangle.Point0.ColorForRender);
                    g.FillPolygon(brush, triangle.Points);
                }
            }
            else if (renderType == RenderType.FillWhite)
            {
                IEnumerable<Plane> planesForRender = model.Planes.Where(x => x.VisibleBackSide || x.Normal.Z < 0);
                List<Triangle> triangles = new List<Triangle>();

                foreach (Plane plane in planesForRender)
                {
                    triangles.AddRange(plane.Triangles);
                }

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
