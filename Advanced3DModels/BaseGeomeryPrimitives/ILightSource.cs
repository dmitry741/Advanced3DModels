using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Models3DLib
{
    public interface ILightSource
    {
        float Weight { get; set; }
        Vector3 GetRay(IPoint3D point);
    }
}
