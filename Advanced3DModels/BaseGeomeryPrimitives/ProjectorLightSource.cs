using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class ProjectorLightSource : ILightSource
    {
        Vector3 LightSource { get; set; }

        public Vector3 GetRay(Point3D point)
        {
            return LightSource;
        }
    }
}
