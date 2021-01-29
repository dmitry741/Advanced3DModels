using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

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

        public void AddPlane(Plane plane)
        {
            _planes.Add(plane);
        }

        public void AddEdges(IEnumerable<Edge> edges)
        {
            _edges.AddRange(edges);
        }

        public void AddPoints(IEnumerable<Point3D> point3Ds)
        {
            _points.AddRange(point3Ds);
        }

        public bool NeedToSort { get; set; } = false;
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

        #region === private ===

        private static Model SquareTilePanel(float edgeLen, float sizePrimitive, int tileRowCount, float zLevel, Color color1, Color color2)
        {
            float xStart = -edgeLen / 2.0f;
            float yStart = -edgeLen / 2.0f;
            float xEnd = edgeLen / 2.0f;
            float yEnd = edgeLen / 2.0f;

            float xs = xStart;
            float ys = yStart;

            Model model = new Model();

            for (int i = 0; i < tileRowCount; i++)
            {
                float xe = (xEnd - xStart) * (i + 1) / tileRowCount + xStart;

                for (int j = 0; j < tileRowCount; j++)
                {
                    float ye = (yEnd - yStart) * (j + 1) / tileRowCount + yStart;

                    Point3D point1 = new Point3D(xs, ys, zLevel);
                    Point3D point2 = new Point3D(xe, ys, zLevel);
                    Point3D point3 = new Point3D(xe, ye, zLevel);
                    Point3D point4 = new Point3D(xs, ye, zLevel);

                    Plane panel = new Polygon4Plane(point1, point2, point3, point4, sizePrimitive, (i + j).ToString());

                    foreach(Triangle triangle in panel.Triangles)
                    {
                        triangle.Color = (i + j) % 2 == 0 ? color1 : color2;
                    }

                    model.AddPlane(panel);

                    ys = ye;
                }

                xs = xe;
            }

            return model;
        }

        #endregion

        #region === models ===

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
                new Polygon4Plane(points[0], points[1], points[2], points[3], sizePrimitive, "0"),
                new Polygon4Plane(points[0], points[4], points[5], points[1], sizePrimitive, "1"),
                new Polygon4Plane(points[1], points[5], points[6], points[2], sizePrimitive, "2"),
                new Polygon4Plane(points[2], points[6], points[7], points[3], sizePrimitive, "3"),
                new Polygon4Plane(points[3], points[7], points[4], points[0], sizePrimitive, "4"),
                new Polygon4Plane(points[7], points[6], points[5], points[4], sizePrimitive, "5")
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

        public static Model CubeColored(float edgeLen, float sizePrimitive)
        {
            Model model = Cube(edgeLen, sizePrimitive);
            Color[] cls = { Color.LightGreen, Color.Brown, Color.Gold, Color.Cornsilk, Color.DarkBlue, Color.BurlyWood };

            for (int i = 0; i < cls.Length; i++)
            {
                foreach (Triangle triangle in model.Planes.ElementAt(i).Triangles)
                {
                    triangle.Color = cls[i];
                }
            }

            return model;
        }

        /*public static Model Panel(float edgeLen, float sizePrimitive, int tileRowCount, Color color1, Color color2)
        {
            const float cHeight = 16.0f;

            for (int i = 0; i < tileRowCount; i++)
            {
                
            }

        }*/

        #endregion
    }
}
