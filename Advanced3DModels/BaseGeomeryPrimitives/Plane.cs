using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    public class Plane
    {
        protected List<Point3D> _point3Ds;
        protected List<Triangle> _triangles;

        public Plane()
        {
            _point3Ds = new List<Point3D>();
            _triangles = new List<Triangle>();
        }

        public Plane(IEnumerable<Triangle> triangles)
        {
            _point3Ds = new List<Point3D>();
            _triangles = triangles.ToList();
        }
        
        #region === public ===

        public bool VisibleBackSide { get; set; } = false;

        public IEnumerable<Point3D> Points => _point3Ds;

        public IEnumerable<Triangle> Triangles => _triangles;

        public Vector3 Normal => _triangles[0].Normal;

        public string Name { get; set; }

        #endregion
    }
}
