using System;
using System.Numerics;

namespace Models3DLib
{
    public interface IContains
    {
        bool Contains(float X, float Y);
    }

    public class ShpereContains : IContains
    {
        IPoint3D _center;
        float _radius;

        public ShpereContains(IPoint3D center, float radius)
        {
            _center = center;
            _radius = radius;
        }

        public bool Contains(float X, float Y)
        {
            return (X - _center.X) * (X - _center.X) + (Y - _center.Y) * (Y - _center.Y) < _radius * _radius;
        }
    }

    public class TriangleContains : IContains
    {
        public Triangle TestTriangle { get; set; }

        public bool Contains(float X, float Y)
        {
            Vector3 v1 = TestTriangle.Point3Ds[1].ToVector3() - TestTriangle.Point3Ds[0].ToVector3();
            Vector3 v2 = TestTriangle.Point3Ds[2].ToVector3() - TestTriangle.Point3Ds[0].ToVector3();
            float S = Vector3.Cross(v1, v2).Length() / 2; // площадь треугольника

            Vector3 testVector = ResolverInterface.ResolveIPoint3D(X, Y, 0).ToVector3();

            Vector3 vt0 = TestTriangle.Point3Ds[0].ToVector3() - testVector;
            Vector3 vt1 = TestTriangle.Point3Ds[1].ToVector3() - testVector;
            Vector3 vt2 = TestTriangle.Point3Ds[2].ToVector3() - testVector;

            float S1 = Vector3.Cross(vt2, vt0).Length() / 2;
            float S2 = Vector3.Cross(vt0, vt1).Length() / 2;
            float S3 = Vector3.Cross(vt1, vt2).Length() / 2;

            return Math.Abs(S - S1 - S2 - S3) < 0.01;
        }
    }
}
