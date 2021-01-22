using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    public class Triangle
    {
        readonly Point3D[] _points;

        public Triangle(Point3D point1, Point3D point2, Point3D point3)
        {
            _points = new Point3D[3] { point1, point2, point3 };
        }

        public Vector3 Normal
        {
            get
            {
                Vector3 v1 = _points[1].ToVector3() - _points[0].ToVector3();
                Vector3 v2 = _points[2].ToVector3() - _points[0].ToVector3();

                return Vector3.Cross(v1, v2);
            }
        }

        public float Min => _points.Min(p => p.Z);

        public PointF[] Points => _points.Select(x => x.ToPointF()).ToArray();
    }
}
