﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Класс реализующий сферу как модель для попиксельного рендеринга.
    /// </summary>
    public class SpherePixelModel : IPixelsModel
    {
        IPoint3D _center;
        float _radius;
        Color _baseColor;
        IContains _icontainer;

        public SpherePixelModel(IPoint3D center, float radius, Color baseColor)
        {
            _center = center;
            _radius = radius;
            _icontainer = new ShpereContains(center, radius);
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
            return _icontainer.Contains(X, Y);
        }

        public Vector3 GetNormal(float X, float Y)
        {
            return new Vector3(X - _center.X, Y - _center.Y, GetZ(X, Y) - _center.Z);
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

        public void Transform(Matrix4x4 matrix)
        {
            Vector3 vector = Vector3.Transform(_center.ToVector3(), matrix);

            _center.X = vector.X;
            _center.Y = vector.Y;
            _center.Z = vector.Z;
        }

        public bool ReflectionEnable(float X, float Y)
        {
            return true;
        }

        public float ReflectionBrightness(float X, float Y)
        {
            return 80f;
        }

        public float ReflcetionCone(float X, float Y)
        {
            return 1000f;
        }
    }
}
