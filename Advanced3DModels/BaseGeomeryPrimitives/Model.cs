using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Models3DLib
{
    public class Model
    {
        #region === members ===

        protected List<Plane> _planes = new List<Plane>();
        protected List<Edge> _edges = new List<Edge>();
        protected List<Point3D> _points = new List<Point3D>();

        #endregion

        #region === public ===

        public IEnumerable<Edge> Edges => _edges;
        public IEnumerable<Plane> Planes => _planes;

        public void Transform(Matrix4x4 matrix)
        {
            foreach (Point3D point in _points)
            {
                Vector3 vector = Vector3.Transform(point.ToVector3(), matrix);

                point.X = vector.X;
                point.Y = vector.Y;
                point.Z = vector.Z;
            }

            foreach(Plane plane in _planes)
            {
                foreach (Point3D point in plane.Points)
                {
                    Vector3 vector = Vector3.Transform(point.ToVector3(), matrix);

                    point.X = vector.X;
                    point.Y = vector.Y;
                    point.Z = vector.Z;
                }
            }
        }

        #endregion

        #region === presets ===

        public static Model Cube(float edgeLen, float sizePrimitive)
        {
            List<Point3D> points = new List<Point3D>
            {
                new Point3D(-edgeLen / 2, edgeLen / 2, edgeLen / 2),
                new Point3D(edgeLen / 2, edgeLen / 2, edgeLen / 2),
                new Point3D(edgeLen / 2, -edgeLen / 2, edgeLen / 2),
                new Point3D(-edgeLen / 2, -edgeLen / 2, edgeLen / 2),
                new Point3D(-edgeLen / 2, edgeLen / 2, -edgeLen / 2),
                new Point3D(edgeLen / 2, edgeLen / 2, -edgeLen / 2),
                new Point3D(edgeLen / 2, -edgeLen / 2, -edgeLen / 2),
                new Point3D(-edgeLen / 2, -edgeLen / 2, -edgeLen / 2)
            };

            List<Plane> planes = new List<Plane>
            {
                new Polygon4Plane(points[0], points[1], points[2], points[3], sizePrimitive),
                new Polygon4Plane(points[0], points[4], points[5], points[1], sizePrimitive),
                new Polygon4Plane(points[1], points[5], points[6], points[2], sizePrimitive),
                new Polygon4Plane(points[2], points[6], points[7], points[3], sizePrimitive),
                new Polygon4Plane(points[3], points[7], points[4], points[0], sizePrimitive),
                new Polygon4Plane(points[7], points[6], points[5], points[4], sizePrimitive)
            };

            List<Edge> edges = new List<Edge>
            {
                new Edge(points[0], points[1]),
                new Edge(points[1], points[2]),
                new Edge(points[2], points[3]),
                new Edge(points[3], points[0]),
                new Edge(points[4], points[5]),
                new Edge(points[5], points[6]),
                new Edge(points[6], points[7]),
                new Edge(points[7], points[4]),
                new Edge(points[0], points[4]),
                new Edge(points[1], points[5]),
                new Edge(points[2], points[6]),
                new Edge(points[3], points[7])
            };

            return new Model
            {
                _planes = planes,
                _points = points,
                _edges = edges
            };
        }

        #endregion
    }
}
