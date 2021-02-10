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
        public Color ColorForRender { get; set; }

        public Point3DColor(float X, float Y, float Z) : base(X, Y, Z)
        {
            ColorForRender = Color.Black;
        }
    }
}
