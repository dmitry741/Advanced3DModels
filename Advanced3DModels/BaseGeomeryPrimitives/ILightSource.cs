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
        Vector3 GetRay(Point3D point);
    }
}
