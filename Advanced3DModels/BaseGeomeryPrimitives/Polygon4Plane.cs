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
        readonly int _nX, _nY;

        public Polygon4Plane(IPoint3D p1, IPoint3D p2, IPoint3D p3, IPoint3D p4, float sizePrimitive, Color baseColor, string name)
        {
            Name = name;
            float[] d = new float[2];

            d[0] = Vector3.Distance(p1.ToVector3(), p2.ToVector3());
            d[1] = Vector3.Distance(p3.ToVector3(), p4.ToVector3());

            _nX = Math.Max(Convert.ToInt32(d.Min() / sizePrimitive), 1);

            d[0] = Vector3.Distance(p3.ToVector3(), p2.ToVector3());
            d[1] = Vector3.Distance(p1.ToVector3(), p4.ToVector3());

            _nY = Math.Max(Convert.ToInt32(d.Min() / sizePrimitive), 1);

            for (int j = 0; j <= _nY; j++)
            {
                float xLeft = (p4.X - p1.X) * j / _nY + p1.X;
                float yLeft = (p4.Y - p1.Y) * j / _nY + p1.Y;
                float zLeft = (p4.Z - p1.Z) * j / _nY + p1.Z;

                float xRight = (p3.X - p2.X) * j / _nY + p2.X;
                float yRight = (p3.Y - p2.Y) * j / _nY + p2.Y;
                float zRight = (p3.Z - p2.Z) * j / _nY + p2.Z;

                for (int i = 0; i <= _nX; i++)
                {
                    float x = (xRight - xLeft) * i / _nX + xLeft;
                    float y = (yRight - yLeft) * i / _nX + yLeft;
                    float z = (zRight - zLeft) * i / _nX + zLeft;

                    _point3Ds.Add(ResolverInterface.ResolveIPoint3D(x, y, z));
                }
            }

            bool bLeftTopRightBottom = true;

            for (int j = 0; j < _nY; j++)
            {
                bool bDirection = bLeftTopRightBottom;

                for (int i = 0; i < _nX; i++)
                {
                    IPoint3D point1 = _point3Ds[(j + 0) * (_nX + 1) + i + 0];
                    IPoint3D point2 = _point3Ds[(j + 0) * (_nX + 1) + i + 1];
                    IPoint3D point3 = _point3Ds[(j + 1) * (_nX + 1) + i + 1];
                    IPoint3D point4 = _point3Ds[(j + 1) * (_nX + 1) + i + 0];

                    if (bDirection)
                    {
                        _triangles.Add(new Triangle(point1, point3, point2) { BaseColor = baseColor });
                        _triangles.Add(new Triangle(point1, point4, point3) { BaseColor = baseColor });
                    }
                    else
                    {
                        _triangles.Add(new Triangle(point1, point4, point2) { BaseColor = baseColor });
                        _triangles.Add(new Triangle(point4, point3, point2) { BaseColor = baseColor });
                    }

                    bDirection = !bDirection;
                }

                bLeftTopRightBottom = !bLeftTopRightBottom;
            }
        }

        public void SetTexture(byte[] rgbValues, int stride, int width, int height, bool stretch)
        {
            bool bLeftTopRightBottom = true;
            int index, xPixel1, yPixel1, xPixel2, yPixel2;
            int triIterator = 0;

            if (stretch)
            {
                float kx = Convert.ToSingle(width - 1) / _nX;
                float ky = Convert.ToSingle(height - 1) / _nY;

                for (int j = 0; j < _nY; j++)
                {
                    bool bDirection = bLeftTopRightBottom;
                    yPixel1 = Convert.ToInt32(ky * (j + 0));
                    yPixel2 = Convert.ToInt32(ky * (j + 1));

                    for (int i = 0; i < _nX; i++)
                    {
                        xPixel1 = Convert.ToInt32(kx * (i + 0));
                        xPixel2 = Convert.ToInt32(kx * (i + 1));

                        Color[] colors = new Color[4];

                        index = yPixel1 * stride + xPixel1 * 3;
                        colors[0] = Color.FromArgb(rgbValues[index + 2], rgbValues[index + 1], rgbValues[index + 0]);

                        index = yPixel1 * stride + xPixel2 * 3;
                        colors[1] = Color.FromArgb(rgbValues[index + 2], rgbValues[index + 1], rgbValues[index + 0]);

                        index = yPixel2 * stride + xPixel2 * 3;
                        colors[2] = Color.FromArgb(rgbValues[index + 2], rgbValues[index + 1], rgbValues[index + 0]);

                        index = yPixel2 * stride + xPixel1 * 3;
                        colors[3] = Color.FromArgb(rgbValues[index + 2], rgbValues[index + 1], rgbValues[index + 0]);

                        if (bDirection)
                        {
                            _triangles[triIterator++].TextureColors = new Color[] { colors[0], colors[2], colors[1] };
                            _triangles[triIterator++].TextureColors = new Color[] { colors[0], colors[3], colors[2] };
                        }
                        else
                        {
                            _triangles[triIterator++].TextureColors = new Color[] { colors[0], colors[3], colors[1] };
                            _triangles[triIterator++].TextureColors = new Color[] { colors[3], colors[2], colors[1] };
                        }

                        bDirection = !bDirection;
                    }

                    bLeftTopRightBottom = !bLeftTopRightBottom;
                }
            }
            else
            {
                IPoint3D point1 = _point3Ds[0];
                IPoint3D point2 = _point3Ds[1];
                IPoint3D point3 = _point3Ds[(_nX + 1) + 1];

                float w = Vector3.Distance(point1.ToVector3(), point2.ToVector3());
                float h = Vector3.Distance(point2.ToVector3(), point3.ToVector3());

                float x1, x2, y1, y2;

                for (int j = 0; j < _nY; j++)
                {
                    bool bDirection = bLeftTopRightBottom;

                    x1 = 0;
                    y1 = (j + 0) * h;
                    y2 = (j + 1) * h;

                    yPixel1 = Convert.ToInt32(y1);
                    yPixel2 = Convert.ToInt32(y2);

                    for (int i = 0; i < _nX; i++)
                    {
                        x2 = x1 + w;

                        xPixel1 = Convert.ToInt32(x1);
                        xPixel2 = Convert.ToInt32(x2);

                        Color[] colors = new Color[4];

                        // 0
                        index = (yPixel1 % height) * stride + (xPixel1 % width) * 3;
                        colors[0] = Color.FromArgb(rgbValues[index + 2], rgbValues[index + 1], rgbValues[index + 0]);

                        // 1
                        index = (yPixel1 % height) * stride + (xPixel2 % width) * 3;
                        colors[1] = Color.FromArgb(rgbValues[index + 2], rgbValues[index + 1], rgbValues[index + 0]);

                        // 2
                        index = (yPixel2 % height) * stride + (xPixel2 % width) * 3;
                        colors[2] = Color.FromArgb(rgbValues[index + 2], rgbValues[index + 1], rgbValues[index + 0]);

                        // 3
                        index = (yPixel2 % height) * stride + (xPixel1 % width) * 3;
                        colors[3] = Color.FromArgb(rgbValues[index + 2], rgbValues[index + 1], rgbValues[index + 0]);

                        if (bDirection)
                        {
                            _triangles[triIterator++].TextureColors = new Color[] { colors[0], colors[2], colors[1] };
                            _triangles[triIterator++].TextureColors = new Color[] { colors[0], colors[3], colors[2] };
                        }
                        else
                        {
                            _triangles[triIterator++].TextureColors = new Color[] { colors[0], colors[3], colors[1] };
                            _triangles[triIterator++].TextureColors = new Color[] { colors[3], colors[2], colors[1] };
                        }

                        x1 = x2;
                        bDirection = !bDirection;
                    }

                    bLeftTopRightBottom = !bLeftTopRightBottom;
                }
            }
        }
    }
}
