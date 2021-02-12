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
        readonly Point3DColor[] _points;

        public Triangle(Point3DColor point1, Point3DColor point2, Point3DColor point3)
        {
            _points = new Point3DColor[] { point1, point2, point3 };
        }

        public Triangle(Point3DColor[] points, Color color)
        {
            _points = points;
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

        public float MinZ => _points.Min(p => p.Z);

        public Point3DColor Point0 => _points[0];

        public Point3DColor[] Point3Ds => _points;

        public PointF[] Points => _points.Select(x => x.ToPointF()).ToArray();
    }
}
