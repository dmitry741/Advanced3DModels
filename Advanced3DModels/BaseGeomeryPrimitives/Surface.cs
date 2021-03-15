using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public delegate double Function3D(float X, float Y);

    public class Surface : Polygon4Plane
    {
        public Surface(IPoint3D p1, IPoint3D p2, IPoint3D p3, IPoint3D p4, float sizePrimitive, Color baseColor, string name) :
            base(p1, p2, p3, p4, sizePrimitive, baseColor, name) { }

        RectangleF BoundRect
        {
            get
            {
                float xmin = _point3Ds.Min(v => v.X);
                float xmax = _point3Ds.Max(v => v.X);
                float ymin = _point3Ds.Min(v => v.Y);
                float ymax = _point3Ds.Max(v => v.Y);

                return new RectangleF(xmin, ymin, xmax - xmin + 1, ymax - ymin + 1);
            }
        }

        IPoint3D ConvertToReal(RectangleF realBoundRect, RectangleF compBoundRect, IPoint3D point)
        {
            float x = realBoundRect.Width / compBoundRect.Width * (point.X - compBoundRect.X) + realBoundRect.X;
            float y = realBoundRect.Bottom - realBoundRect.Height / compBoundRect.Height * (point.Y - compBoundRect.Top);

            return ResolverInterface.ResolveIPoint3D(x, y, point.Z);
        }


        public void CreateSurface(RectangleF real, Function3D function3D, float ZMin, float Zmax)
        {
            RectangleF boundRect = BoundRect;

            // вычисляем минимальное и макстмальное значения по оси Z

        }
    }
}
