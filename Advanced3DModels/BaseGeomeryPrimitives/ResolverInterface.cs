using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    class ResolverInterface
    {
        public static IPoint3D ResolveIPoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        public static IPoint3D ResolveIPoint3D(IPoint3D point)
        {
            return ResolveIPoint3D(point.X, point.Y, point.Z);
        }
    }
}
