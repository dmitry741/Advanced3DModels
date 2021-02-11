using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class PointLightSource : AbstractLightSource
    {
        public IPoint3D LightPoint { get; set; }

        public override Vector3 GetRay(IPoint3D point)
        {
            return new Vector3(LightPoint.X - point.X, LightPoint.Y - point.Y, LightPoint.Z - point.Z);
        }
    }
}
