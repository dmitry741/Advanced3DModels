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

        public Sphere(IPoint3D center, float radius)
        {
            _center = center;
            _radius = radius;
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
            float rootvar = _radius * _radius - (X - _center.X) * (X - _center.X) - (Y - _center.Y) * (Y - _center.Y);
            float Z = _center.Z - Convert.ToSingle(Math.Sqrt(rootvar));
            return new Vector3(X - _center.X, Y - _center.Y, Z - _center.Z);
        }

        public IPoint3D GetPoint(float X, float Y)
        {
            float rootvar = _radius * _radius - (X - _center.X) * (X - _center.X) - (Y - _center.Y) * (Y - _center.Y);
            float Z = _center.Z - Convert.ToSingle(Math.Sqrt(rootvar));
            return ResolverInterface.ResolveIPoint3D(X, Y, Z);
        }
    }
}
