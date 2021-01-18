using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Models3DLib
{
    public abstract class AbstractPlane
    {
        protected List<Point3D> _point3Ds = new List<Point3D>();
        readonly protected List<Triangle> _triangles = new List<Triangle>();

        #region === public ===

        public bool VisibleBackSize { get; set; } = true;

        public IEnumerable<Point3D> Points => _point3Ds;

        public IEnumerable<Triangle> Triangles => _triangles;

        public void Transform(Matrix4x4 matrix)
        {
            IEnumerable<Vector4> transformedVectors = _point3Ds.Select(x => Vector4.Transform(x.ToVector3(), matrix));
            _point3Ds = transformedVectors.Select(v => new Point3D(v.X, v.Y, v.Z)).ToList();
        }

        #endregion
    }
}
