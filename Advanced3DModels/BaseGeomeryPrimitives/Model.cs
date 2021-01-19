using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class Model
    {
        #region === members ===

        List<Plane> _planes = new List<Plane>();
        List<Edge> _edges = new List<Edge>();
        List<Point3D> _points = new List<Point3D>();

        #endregion

        #region === public ===

        public IEnumerable<Plane> Planes => _planes;

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
