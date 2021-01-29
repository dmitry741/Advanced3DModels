using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class ProjectorLightSource : AbstractLightSource
    {
        public Vector3 VectorLightSource { get; set; }

        public override Vector3 GetRay(Point3D point)
        {
            return VectorLightSource;
        }
    }
}
