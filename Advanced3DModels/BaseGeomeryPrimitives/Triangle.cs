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
        readonly IPoint3D[] _points;

        public Triangle(IPoint3D point1, IPoint3D point2, IPoint3D point3)
        {
            _points = ResolverInterface.ResolveArrayIPoint3D(3);

            _points[0] = point1;
            _points[1] = point2;
            _points[2] = point3;
        }

        public Triangle(IPoint3D[] points)
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

        public bool VisibleBackSide { get; set; } = false;

        public Color BaseColor { get; set; } = Color.LightGreen;

        public float ReflectionBrightness { get; set; } = 80.0f;

        public float ReflectionCone { get; set; } = 24600;

        public bool Reflection { get; set; } = true;

        public bool AllowToGouraudMethod { get; set; } = true;

        public float MinZ => _points.Min(p => p.Z);

        public IPoint3D Point0 => _points[0];

        public IPoint3D[] Point3Ds => _points;

        public PointF[] Points => _points.Select(x => x.ToPointF()).ToArray();
    }
}
