using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public interface IPixelsModel
    {
        bool Contains(PointF point);
        RectangleF BoundRect { get; }
        Vector3 GetNormal(float X, float Y);
    }
}
