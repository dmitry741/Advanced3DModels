using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class Edge
    {
        readonly Point3D _point1, _point2;

        public Edge(Point3D point1, Point3D point2)
        {
            _point1 = point1;
            _point2 = point2;
        }

        public Point3D Point1 => _point1;
        public Point3D Point2 => _point2;
    }
}
