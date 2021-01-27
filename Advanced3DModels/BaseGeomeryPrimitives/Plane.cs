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
        protected List<Point3D> _point3Ds = new List<Point3D>();
        readonly protected List<Triangle> _triangles = new List<Triangle>();
        
        #region === public ===

        public bool VisibleBackSide { get; set; } = false;

        public IEnumerable<Point3D> Points => _point3Ds;

        public IEnumerable<Triangle> Triangles => _triangles;

        public Vector3 Normal => _triangles[0].Normal;

        public void Transform(Matrix4x4 matrix)
        {
            IEnumerable<Vector4> transformedVectors = _point3Ds.Select(x => Vector4.Transform(x.ToVector3(), matrix));
            _point3Ds = transformedVectors.Select(v => new Point3D(v.X, v.Y, v.Z)).ToList();
        }

        public string Name { get; set; }

        #endregion
    }
}
