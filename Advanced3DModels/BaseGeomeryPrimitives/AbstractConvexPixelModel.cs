using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class AbstractConvexPixelModel : IPixelsModel
    {
        protected List<IPoint3D> _point3Ds = new List<IPoint3D>();
        protected List<Triangle> _triangles = new List<Triangle>();
        IContains _icontains;
        Triangle _lastin = null;

        #region === private methods ===

        protected Triangle GetProjectionZ0(Triangle triangle)
        {
            IPoint3D[] points = triangle.Point3Ds.Select(p => ResolverInterface.ResolveIPoint3D(p.X, p.Y, 0)).ToArray();
            return new Triangle(points);
        }

        protected bool Contains(Triangle triangle, float X, float Y)
        {
            _icontains = new TriangleContains { TestTriangle = triangle };
            return _icontains.Contains(X, Y);
        }

        #endregion

        public RectangleF BoundRect
        {
            get
            {
                IEnumerable<Triangle> visibleTriangles = _triangles.Where(t => t.Normal.Z < 0);
                IEnumerable<IPoint3D> point3Ds = visibleTriangles.SelectMany(t => t.Point3Ds);

                float left = point3Ds.Min(p => p.X);
                float top = point3Ds.Min(p => p.Y);
                float right = point3Ds.Max(p => p.X);
                float bottom = point3Ds.Max(p => p.Y);

                return new RectangleF(left, top, right - left + 1, bottom - top + 1);
            }
        }

        public bool Contains(float X, float Y)
        {
            IEnumerable<Triangle> visibleTriangles = _triangles.Where(t => t.Normal.Z < 0);
            _lastin = visibleTriangles.FirstOrDefault(t => Contains(GetProjectionZ0(t), X, Y));
            return _lastin != null;
        }

        public Color GetColor(float X, float Y)
        {
            return _lastin.BaseColor;
        }

        public Vector3 GetNormal(float X, float Y)
        {
            return _lastin.Normal;
        }

        public float GetZ(float X, float Y)
        {
            Vector3 normal = _lastin.Normal;
            float D = -Vector3.Dot(normal, _lastin.Point0.ToVector3());
            float Z = (-normal.X * X - normal.Y * Y - D) / normal.Z;

            return Z;
        }

        public void Transform(Matrix4x4 matrix)
        {
            foreach (IPoint3D point in _point3Ds)
            {
                Vector3 vector = Vector3.Transform(point.ToVector3(), matrix);

                point.X = vector.X;
                point.Y = vector.Y;
                point.Z = vector.Z;
            }
        }

        public bool ReflectionEnable(float X, float Y)
        {
            return _lastin.ReflectionEnable;
        }

        public float ReflectionBrightness(float X, float Y)
        {
            return 80;
        }

        public float ReflcetionCone(float X, float Y)
        {
            return 2000f;
        }
    }
}
