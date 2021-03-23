using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Класс модели параллепипеда для попиксельного рендеринга.
    /// </summary>
    public class ParallelepipedPixelModel : AbstractConvexPixelModel
    {
        public ParallelepipedPixelModel(float width, float height, float depth, Color[] colors)
        {
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(-width / 2, height / 2, depth / 2));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(width / 2, height / 2, depth / 2));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(width / 2, -height / 2, depth / 2));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(-width / 2, -height / 2, depth / 2));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(-width / 2, height / 2, -depth / 2));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(width / 2, height / 2, -depth / 2));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(width / 2, -height / 2, -depth / 2));
            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(-width / 2, -height / 2, -depth / 2));

            // 0
            _triangles.Add(new Triangle(_point3Ds[0], _point3Ds[3], _point3Ds[2]) { BaseColor = colors[0] });
            _triangles.Add(new Triangle(_point3Ds[0], _point3Ds[2], _point3Ds[1]) { BaseColor = colors[0] });

            // 1
            _triangles.Add(new Triangle(_point3Ds[4], _point3Ds[0], _point3Ds[1]) { BaseColor = colors[1] });
            _triangles.Add(new Triangle(_point3Ds[4], _point3Ds[1], _point3Ds[5]) { BaseColor = colors[1] });

            // 2
            _triangles.Add(new Triangle(_point3Ds[5], _point3Ds[1], _point3Ds[2]) { BaseColor = colors[2] });
            _triangles.Add(new Triangle(_point3Ds[5], _point3Ds[2], _point3Ds[6]) { BaseColor = colors[2] });

            // 3
            _triangles.Add(new Triangle(_point3Ds[6], _point3Ds[2], _point3Ds[3]) { BaseColor = colors[3] });
            _triangles.Add(new Triangle(_point3Ds[6], _point3Ds[3], _point3Ds[7]) { BaseColor = colors[3] });

            // 4
            _triangles.Add(new Triangle(_point3Ds[7], _point3Ds[3], _point3Ds[0]) { BaseColor = colors[4] });
            _triangles.Add(new Triangle(_point3Ds[7], _point3Ds[0], _point3Ds[4]) { BaseColor = colors[4] });

            // 5     
            _triangles.Add(new Triangle(_point3Ds[4], _point3Ds[6], _point3Ds[7]) { BaseColor = colors[5] });
            _triangles.Add(new Triangle(_point3Ds[4], _point3Ds[5], _point3Ds[6]) { BaseColor = colors[5] });
        }
    }
}
