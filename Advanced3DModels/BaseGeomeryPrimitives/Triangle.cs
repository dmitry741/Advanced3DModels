using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace BaseGeomeryPrimitives
{
    public class Triangle
    {
        public Point3D Point1 { get; set; }
        public Point3D Point2 { get; set; }
        public Point3D Point3 { get; set; }

        public Vector3 Normal
        {
            get
            {
                Vector3 left = Point2.ToVector3() - Point1.ToVector3();
                Vector3 right = Point3.ToVector3() - Point1.ToVector3();

                return Vector3.Cross(left, right);
            }
        }
    }
}
