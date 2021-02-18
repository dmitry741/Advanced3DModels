using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Models3DLib
{
    public class Point3DColor : Point3D
    {
        public Color ColorForRender { get; set; } = Color.Black;

        public Color BaseColor { get; set; } = Color.LightGreen;

        public Point3DColor(float X, float Y, float Z) : base(X, Y, Z) { }

        public static Point3DColor DeepCopy(Point3DColor point3DColor)
        {
            return new Point3DColor(point3DColor.X, point3DColor.Y, point3DColor.Z) 
            { 
                BaseColor = point3DColor.BaseColor
            };
        }
    }
}
