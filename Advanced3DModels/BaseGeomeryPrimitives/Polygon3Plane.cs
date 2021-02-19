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

            _point3Ds.Add(new Point3DColor(p1.X, p1.Y, p1.Z) { BaseColor = baseColor });

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

                    _point3Ds.Add(new Point3DColor(x, y, z) { BaseColor = baseColor });
                }

                int trCount = 2 * (i - 1) + 1;
                int index1, index2, index3;

                for (int j = 0; j < trCount; j++)
                {
                    if (j % 2 == 0)
                    {
                        index1 = j + (i - 1) * i / 2 - j / 2;
                        index2 = j + (i + 1) * i / 2 - j / 2;
                        index3 = index2 + 1;
                    }
                    else
                    {
                        index1 = j - 1 + (i - 1) * i / 2 - j / 2;
                        index2 = j + (i + 1) * i / 2 - j / 2;
                        index3 = index1 + 1;
                    }

                    Point3DColor[] point3DColors = new Point3DColor[]
                    {
                        _point3Ds[index1], _point3Ds[index2], _point3Ds[index3]
                    };

                    _triangles.Add(new Triangle(point3DColors));
                }
            }

            // huuihiootr5r655r6

            /*for (int i = 0; i < split; i++)
            {
                float xLeft1 = (p2.X - p1.X) * i / split + p1.X;
                float yLeft1 = (p2.Y - p1.Y) * i / split + p1.Y;
                float zLeft1 = (p2.Z - p1.Z) * i / split + p1.Z;

                float xRight1 = (p3.X - p1.X) * i / split + p1.X;
                float yRight1 = (p3.Y - p1.Y) * i / split + p1.Y;
                float zRight1 = (p3.Z - p1.Z) * i / split + p1.Z;

                float xLeft2 = (p2.X - p1.X) * (i + 1) / split + p1.X;
                float yLeft2 = (p2.Y - p1.Y) * (i + 1) / split + p1.Y;
                float zLeft2 = (p2.Z - p1.Z) * (i + 1) / split + p1.Z;

                float xRight2 = (p3.X - p1.X) * (i + 1) / split + p1.X;
                float yRight2 = (p3.Y - p1.Y) * (i + 1) / split + p1.Y;
                float zRight2 = (p3.Z - p1.Z) * (i + 1) / split + p1.Z;

                int nHor = 2 * i + 1;

                Point3DColor point1 = new Point3DColor(xLeft1, yLeft1, zLeft1);
                Point3DColor point2 = new Point3DColor(xLeft2, yLeft2, zLeft2);

                for (int j = 0; j < nHor; j++)
                {
                    float x, y, z;

                    Point3DColor[] point3DColors = new Point3DColor[3];

                    point3DColors[0] = Point3DColor.DeepCopy(point1);
                    point3DColors[1] = Point3DColor.DeepCopy(point2);

                    if (j % 2 == 0)
                    {
                        x = (xRight2 - xLeft2) * (j / 2 + 1) / (i + 1) + xLeft2;
                        y = (yRight2 - yLeft2) * (j / 2 + 1) / (i + 1) + yLeft2;
                        z = (zRight2 - zLeft2) * (j / 2 + 1) / (i + 1) + zLeft2;

                        point3DColors[2] = new Point3DColor(x, y, z) { BaseColor = baseColor };
                        point2 = point3DColors[2];
                        
                    }
                    else
                    {
                        x = (xRight1 - xLeft1) * (j / 2 + 1) / i + xLeft1;
                        y = (yRight1 - yLeft1) * (j / 2 + 1) / i + yLeft1;
                        z = (zRight1 - zLeft1) * (j / 2 + 1) / i + zLeft1;

                        point3DColors[2] = new Point3DColor(x, y, z) { BaseColor = baseColor };
                        point1 = point3DColors[2];
                    }

                   _triangles.Add(new Triangle(point3DColors));
                }
            }*/
        }
    }
}
