using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    public class Polygon3Plane : Plane
    {
        public Polygon3Plane(IPoint3D p1, IPoint3D p2, IPoint3D p3, float sizePrimitive, Color baseColor, string name)
        {
            Name = name;

            float[] d = new float[]
            {
                Vector3.Distance(p1.ToVector3(), p2.ToVector3()),
                Vector3.Distance(p2.ToVector3(), p3.ToVector3()),
                Vector3.Distance(p3.ToVector3(), p1.ToVector3())
            };

            int split = Math.Max(Convert.ToInt32(d.Min() / sizePrimitive), 1);

            _point3Ds.Add(ResolverInterface.ResolveIPoint3D(p1));

            for (int i = 1; i <= split; i++)
            {
                float xLeft = (p2.X - p1.X) * i / split + p1.X;
                float yLeft = (p2.Y - p1.Y) * i / split + p1.Y;
                float zLeft = (p2.Z - p1.Z) * i / split + p1.Z;

                float xRight = (p3.X - p1.X) * i / split + p1.X;
                float yRight = (p3.Y - p1.Y) * i / split + p1.Y;
                float zRight = (p3.Z - p1.Z) * i / split + p1.Z;

                for (int j = 0; j <= i; j++)
                {
                    float x = (xRight - xLeft) * j / i + xLeft;
                    float y = (yRight - yLeft) * j / i + yLeft;
                    float z = (zRight - zLeft) * j / i + zLeft;

                    _point3Ds.Add(ResolverInterface.ResolveIPoint3D(x, y, z));
                }

                int trCount = 2 * (i - 1) + 1;
                int index1, index2, index3;

                for (int j = 0; j < trCount; j++)
                {
                    if (j % 2 == 0)
                    {
                        index1 = j / 2 + (i - 1) * i / 2;
                        index2 = j / 2 + (i + 1) * i / 2;
                        index3 = index2 + 1;
                    }
                    else
                    {
                        index1 = j - 1 + (i - 1) * i / 2 - j / 2;
                        index2 = j + (i + 1) * i / 2 - j / 2;
                        index3 = index1 + 1;
                    }

                    IPoint3D[] point3DColors = ResolverInterface.ResolveArrayIPoint3D(3);

                    point3DColors[0] = _point3Ds[index1];
                    point3DColors[1] = _point3Ds[index2];
                    point3DColors[2] = _point3Ds[index3];

                    _triangles.Add(new Triangle(point3DColors) { BaseColor = baseColor });
                }
            }
        }
    }
}
