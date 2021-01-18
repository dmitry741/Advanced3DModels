using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Models3DLib
{
    public class Polygon4Plane : AbstractPlane
    {
        public Polygon4Plane(Point3D p1, Point3D p2, Point3D p3, Point3D p4, float sizePrimitive)
        {
            float[] d = new float[2];

            d[0] = Vector3.Distance(p1.ToVector3(), p2.ToVector3());
            d[1] = Vector3.Distance(p3.ToVector3(), p4.ToVector3());

            float Nhor = d.Min() / sizePrimitive;

            d[0] = Vector3.Distance(p3.ToVector3(), p2.ToVector3());
            d[1] = Vector3.Distance(p1.ToVector3(), p4.ToVector3());

            float Nver = d.Min() / sizePrimitive;

            bool bLeftTopToRightBottom;

            for (int j = 0; j < Nver; j++)
            {
                float xLeftTop = (p4.X - p1.X) * j / Nver + p1.X;
                float yLeftTop = (p4.Y - p1.Y) * j / Nver + p1.Y;
                float zLeftTop = (p4.Z - p1.Z) * j / Nver + p1.Z;

                float xLeftBottom = (p4.X - p1.X) * (j + 1) / Nver + p1.X;
                float yLeftBottom = (p4.Y - p1.Y) * (j + 1) / Nver + p1.Y;
                float zLeftBottom = (p4.Z - p1.Z) * (j + 1) / Nver + p1.Z;

                float xRightTop = (p3.X - p2.X) * j / Nver + p2.X;
                float yRightTop = (p3.Y - p2.Y) * j / Nver + p2.Y;
                float zRightTop = (p3.Z - p2.Z) * j / Nver + p2.Z;

                float xRightBottom = (p3.X - p2.X) * (j + 1) / Nver + p2.X;
                float yRightBottom = (p3.Y - p2.Y) * (j + 1) / Nver + p2.Y;
                float zRightBottom = (p3.Z - p2.Z) * (j + 1) / Nver + p2.Z;

                Point3D point1 = new Point3D(xLeftTop, yLeftTop, zLeftTop);
                Point3D point4 = new Point3D(xLeftBottom, yLeftBottom, zLeftBottom);

                bLeftTopToRightBottom = true;

                for (int i = 0; i < Nhor; i++)
                {
                    //float x1 = (xRightTop - xLeftTop) * i / Nhor + xLeftTop;
                    //float y1 = (yRightTop - yLeftTop) * i / Nhor + yLeftTop;
                    //float z1 = (zRightTop - zLeftTop) * i / Nhor + zLeftTop;

                   // Point3D point1 = new Point3D(x1, y1, z1);

                    float x2 = (xRightTop - xLeftTop) * (i + 1) / Nhor + xLeftTop;
                    float y2 = (yRightTop - yLeftTop) * (i + 1) / Nhor + yLeftTop;
                    float z2 = (zRightTop - zLeftTop) * (i + 1) / Nhor + zLeftTop;

                    Point3D point2 = new Point3D(x2, y2, z2);

                    float x3 = (xRightBottom - xLeftBottom) * (i + 1) / Nhor + xLeftBottom;
                    float y3 = (yRightBottom - yLeftBottom) * (i + 1) / Nhor + yLeftBottom;
                    float z3 = (zRightBottom - zLeftBottom) * i / Nhor + zLeftBottom;

                    Point3D point3 = new Point3D(x3, y3, z3);

                    //float x4 = (xRightBottom - xLeftBottom) * i / Nhor + xLeftBottom;
                    //float y4 = (yRightBottom - yLeftBottom) * i / Nhor + yLeftBottom;
                    //float z4 = (zRightBottom - zLeftBottom) * i / Nhor + zLeftBottom;

                    //Point3D point4 = new Point3D(x4, y4, z4);

                    // TODO:
                    _point3Ds.Add(point1);
                    _point3Ds.Add(point2);
                    _point3Ds.Add(point3);
                    _point3Ds.Add(point4);

                    if (bLeftTopToRightBottom)
                    {
                        _triangles.Add(new Triangle(point1, point2, point3));
                        _triangles.Add(new Triangle(point1, point3, point4));
                    }
                    else
                    {
                        _triangles.Add(new Triangle(point1, point2, point4));
                        _triangles.Add(new Triangle(point2, point3, point4));
                    }


                    point1 = point2;
                    point4 = point3;

                    bLeftTopToRightBottom = !bLeftTopToRightBottom;
                }
            }

        }
    }
}
