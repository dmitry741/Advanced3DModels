using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    public interface IPoint3D
    {
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }
        Vector3 ToVector3();
        PointF ToPointF();
    }
}
