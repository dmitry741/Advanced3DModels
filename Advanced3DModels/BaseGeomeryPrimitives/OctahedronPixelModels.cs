using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Models3DLib
{
    public class OctahedronPixelModels : AbstractConvexPixelModel
    {
        public OctahedronPixelModels(float scaleFactor)
        {
            float s = Convert.ToSingle(Math.Sqrt(2) / 2);

            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(scaleFactor * s, 0, 0));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(0, scaleFactor * s, 0));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(-scaleFactor * s, 0, 0));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(0, -scaleFactor * s, 0));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(0, 0, scaleFactor * s));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(0, 0, -scaleFactor * s));

            _triangles.Add(new Triangle(_point3Ds[0], _point3Ds[1], _point3Ds[4]));
            _triangles.Add(new Triangle(_point3Ds[1], _point3Ds[2], _point3Ds[4]));
            _triangles.Add(new Triangle(_point3Ds[2], _point3Ds[3], _point3Ds[4]));
            _triangles.Add(new Triangle(_point3Ds[3], _point3Ds[0], _point3Ds[4]));

            _triangles.Add(new Triangle(_point3Ds[1], _point3Ds[0], _point3Ds[5]));
            _triangles.Add(new Triangle(_point3Ds[2], _point3Ds[1], _point3Ds[5]));
            _triangles.Add(new Triangle(_point3Ds[3], _point3Ds[2], _point3Ds[5]));
            _triangles.Add(new Triangle(_point3Ds[0], _point3Ds[3], _point3Ds[5]));
        }
    }
}
