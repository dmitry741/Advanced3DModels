using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Models3DLib
{
    public abstract class AbstractLightSource
    {
        public float Weight { get; set; } = 1.0f;
        public abstract Vector3 GetRay(IPoint3D point);
    }
}
