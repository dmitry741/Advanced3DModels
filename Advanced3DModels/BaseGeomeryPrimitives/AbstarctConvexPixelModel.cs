using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class AbstarctConvexPixelModel : IPixelsModel
    {
        protected List<IPoint3D> _point3Ds = new List<IPoint3D>();
        protected List<Triangle> _triangles = new List<Triangle>();

        #region === private methods ===

        protected Triangle GetProjectionZ0(Triangle triangle)
        {
            IPoint3D[] points = triangle.Point3Ds.Select(p => ResolverInterface.ResolveIPoint3D(p.X, p.Y, 0)).ToArray();
            return new Triangle(points);
        }

        protected bool Contains(Triangle triangle, float X, float Y)
        {
            Vector3 v1 = triangle.Point3Ds[1].ToVector3() - triangle.Point3Ds[0].ToVector3();
            Vector3 v2 = triangle.Point3Ds[2].ToVector3() - triangle.Point3Ds[0].ToVector3();
            float S = Vector3.Cross(v1, v2).Length() / 2; // площадь треугольника

            IPoint3D testPoint = ResolverInterface.ResolveIPoint3D(X, Y, 0);

            Vector3 vt0 = triangle.Point3Ds[0].ToVector3() - testPoint.ToVector3();
            Vector3 vt1 = triangle.Point3Ds[1].ToVector3() - testPoint.ToVector3();
            Vector3 vt2 = triangle.Point3Ds[2].ToVector3() - testPoint.ToVector3();

            float S1 = Vector3.Cross(vt2, vt0).Length() / 2;
            float S2 = Vector3.Cross(vt0, vt1).Length() / 2;
            float S3 = Vector3.Cross(vt1, vt2).Length() / 2;

            return Math.Abs(S - S1 - S2 - S3) < 0.001;
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
            return visibleTriangles.Any(t => Contains(GetProjectionZ0(t), X, Y));
        }

        public Color GetColor(float X, float Y)
        {
            IEnumerable<Triangle> visibleTriangles = _triangles.Where(t => t.Normal.Z < 0);
            Triangle triangle = visibleTriangles.First(t => Contains(GetProjectionZ0(t), X, Y));
            return triangle.BaseColor;
        }

        public Vector3 GetNormal(float X, float Y)
        {
            IEnumerable<Triangle> visibleTriangles = _triangles.Where(t => t.Normal.Z < 0);
            Triangle triangle = visibleTriangles.First(t => Contains(GetProjectionZ0(t), X, Y));
            return triangle.Normal;
        }

        public float GetZ(float X, float Y)
        {
            // TODO:
            throw new NotImplementedException();
        }


    }
}
