using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    public class Polygon4Plane : Plane
    {
        public Polygon4Plane(Point3D p1, Point3D p2, Point3D p3, Point3D p4, float sizePrimitive)
        {
            float[] d = new float[2];

            d[0] = Vector3.Distance(p1.ToVector3(), p2.ToVector3());
            d[1] = Vector3.Distance(p3.ToVector3(), p4.ToVector3());

            int NX = Math.Max(Convert.ToInt32(d.Min() / sizePrimitive), 1);

            d[0] = Vector3.Distance(p3.ToVector3(), p2.ToVector3());
            d[1] = Vector3.Distance(p1.ToVector3(), p4.ToVector3());

            int NY = Math.Max(Convert.ToInt32(d.Min() / sizePrimitive), 1);

            for (int j = 0; j <= NY; j++)
            {
                float xLeft = (p4.X - p1.X) * j / NY + p1.X;
                float yLeft = (p4.Y - p1.Y) * j / NY + p1.Y;
                float zLeft = (p4.Z - p1.Z) * j / NY + p1.Z;

                float xRight = (p3.X - p2.X) * j / NY + p2.X;
                float yRight = (p3.Y - p2.Y) * j / NY + p2.Y;
                float zRight = (p3.Z - p2.Z) * j / NY + p2.Z;

                for (int i = 0; i <= NX; i++)
                {
                    float x = (xRight - xLeft) * i / NX + xLeft;
                    float y = (yRight - yLeft) * i / NX + yLeft;
                    float z = (zRight - zLeft) * i / NX + zLeft;

                    _point3Ds.Add(new Point3D(x, y, z));
                }
            }

            bool bLeftTopRightBottom = true;

            for (int j = 0; j < NY; j++)
            {
                bool bDirection = bLeftTopRightBottom;

                for (int i = 0; i < NX; i++)
                {
                    Point3D point1 = _point3Ds[(j + 0) * (NX + 1) + i + 0];
                    Point3D point2 = _point3Ds[(j + 0) * (NX + 1) + i + 1];
                    Point3D point3 = _point3Ds[(j + 1) * (NX + 1) + i + 1];
                    Point3D point4 = _point3Ds[(j + 1) * (NX + 1) + i + 0];

                    if (bDirection)
                    {
                        _triangles.Add(new Triangle(point1, point3, point2));
                        _triangles.Add(new Triangle(point1, point4, point3));
                    }
                    else
                    {
                        _triangles.Add(new Triangle(point1, point4, point2));
                        _triangles.Add(new Triangle(point4, point3, point2));
                    }

                    bDirection = !bDirection;
                }

                bLeftTopRightBottom = !bLeftTopRightBottom;
            }
        }

        public Polygon4Plane(Point3D p1, Point3D p2, Point3D p3, Point3D p4, float sizePrimitive, Color color, string name)
        {
            Name = name;
            float[] d = new float[2];

            d[0] = Vector3.Distance(p1.ToVector3(), p2.ToVector3());
            d[1] = Vector3.Distance(p3.ToVector3(), p4.ToVector3());

            int NX = Math.Max(Convert.ToInt32(d.Min() / sizePrimitive), 1);

            d[0] = Vector3.Distance(p3.ToVector3(), p2.ToVector3());
            d[1] = Vector3.Distance(p1.ToVector3(), p4.ToVector3());

            int NY = Math.Max(Convert.ToInt32(d.Min() / sizePrimitive), 1);

            for (int j = 0; j <= NY; j++)
            {
                float xLeft = (p4.X - p1.X) * j / NY + p1.X;
                float yLeft = (p4.Y - p1.Y) * j / NY + p1.Y;
                float zLeft = (p4.Z - p1.Z) * j / NY + p1.Z;

                float xRight = (p3.X - p2.X) * j / NY + p2.X;
                float yRight = (p3.Y - p2.Y) * j / NY + p2.Y;
                float zRight = (p3.Z - p2.Z) * j / NY + p2.Z;

                for (int i = 0; i <= NX; i++)
                {
                    float x = (xRight - xLeft) * i / NX + xLeft;
                    float y = (yRight - yLeft) * i / NX + yLeft;
                    float z = (zRight - zLeft) * i / NX + zLeft;

                    _point3Ds.Add(new Point3D(x, y, z));
                }
            }

            bool bLeftTopRightBottom = true;

            for (int j = 0; j < NY; j++)
            {
                bool bDirection = bLeftTopRightBottom;

                for (int i = 0; i < NX; i++)
                {
                    Point3D point1 = _point3Ds[(j + 0) * (NX + 1) + i + 0];
                    Point3D point2 = _point3Ds[(j + 0) * (NX + 1) + i + 1];
                    Point3D point3 = _point3Ds[(j + 1) * (NX + 1) + i + 1];
                    Point3D point4 = _point3Ds[(j + 1) * (NX + 1) + i + 0];

                    if (bDirection)
                    {
                        _triangles.Add(new Triangle(point1, point3, point2, color));
                        _triangles.Add(new Triangle(point1, point4, point3, color));
                    }
                    else
                    {
                        _triangles.Add(new Triangle(point1, point4, point2, color));
                        _triangles.Add(new Triangle(point4, point3, point2, color));
                    }

                    bDirection = !bDirection;
                }

                bLeftTopRightBottom = !bLeftTopRightBottom;
            }
        }
    }
}
