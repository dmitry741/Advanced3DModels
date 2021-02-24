using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    public class Sphere : IPixelsModel
    {
        IPoint3D _center;
        float _radius;
        Color _baseColor;

        public Sphere(IPoint3D center, float radius, Color baseColor)
        {
            _center = center;
            _radius = radius;
            _baseColor = baseColor;
        }

        public RectangleF BoundRect
        {
            get
            {
                return new RectangleF
                {
                    X = _center.X - _radius,
                    Y = _center.Y - _radius,
                    Width = 2 * _radius,
                    Height = 2 * _radius
                };
            }
        }

        public bool Contains(float X, float Y)
        {
            return (X - _center.X) * (X - _center.X) + (Y - _center.Y) * (Y - _center.Y) < _radius * _radius;
        }

        public Vector3 GetNormal(float X, float Y)
        {
            float Z = GetZ(X, Y);
            return new Vector3(X - _center.X, Y - _center.Y, Z - _center.Z);
        }

        public float GetZ(float X, float Y)
        {
            float rootvar = _radius * _radius - (X - _center.X) * (X - _center.X) - (Y - _center.Y) * (Y - _center.Y);
            return _center.Z - Convert.ToSingle(Math.Sqrt(rootvar));
        }

        public Color GetColor(float X, float Y)
        {
            return _baseColor;
        }
    }
}
