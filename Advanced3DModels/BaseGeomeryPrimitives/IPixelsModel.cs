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
        bool Contains(float X, float Y);
        RectangleF BoundRect { get; }
        Vector3 GetNormal(float X, float Y);
        float GetZ(float X, float Y);
        Color GetColor(float X, float Y);
    }
}
