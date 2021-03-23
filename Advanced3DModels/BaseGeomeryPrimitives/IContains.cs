using System;
using System.Numerics;

namespace Models3DLib
{
    /// <summary>
    /// Интерфейс декларирующий методы для определения принадлежности точки на плоскости некторой области.
    /// </summary>
    public interface IContains
    {
        /// <summary>
        /// Определяет принадлежность точки области.
        /// </summary>
        /// <param name="X">X координата.</param>
        /// <param name="Y">Y координата. </param>
        /// <returns>True если точка принадлежит области, False в противном случае.</returns>
        bool Contains(float X, float Y);
    }

    /// <summary>
    /// Класс для определения принадлежности точки кругу.
    /// </summary>
    public class ShpereContains : IContains
    {
        readonly IPoint3D _center;
        readonly float _radius;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="center">Центр круга.</param>
        /// <param name="radius">Радиус круга.</param>
        public ShpereContains(IPoint3D center, float radius)
        {
            _center = center;
            _radius = radius;
        }

        public bool Contains(float X, float Y)
        {
            return (X - _center.X) * (X - _center.X) + (Y - _center.Y) * (Y - _center.Y) < _radius * _radius;
        }
    }

    /// <summary>
    /// Класс для определения принадлежности точки треугольнику.
    /// </summary>
    public class TriangleContains : IContains
    {
        /// <summary>
        /// Трегольник для определения принадлежности ему точки.
        /// </summary>
        public Triangle TestTriangle { get; set; }

        public bool Contains(float X, float Y)
        {
            Vector3 v1 = TestTriangle.Point3Ds[1].ToVector3() - TestTriangle.Point3Ds[0].ToVector3();
            Vector3 v2 = TestTriangle.Point3Ds[2].ToVector3() - TestTriangle.Point3Ds[0].ToVector3();
            float S = Vector3.Cross(v1, v2).Length() / 2; // площадь треугольника как половина модуля векторного произведения.

            Vector3 testVector = ResolverInterface.ResolveIPoint3D(X, Y, 0).ToVector3();

            Vector3 vt0 = TestTriangle.Point3Ds[0].ToVector3() - testVector;
            Vector3 vt1 = TestTriangle.Point3Ds[1].ToVector3() - testVector;
            Vector3 vt2 = TestTriangle.Point3Ds[2].ToVector3() - testVector;

            float S1 = Vector3.Cross(vt2, vt0).Length() / 2;
            float S2 = Vector3.Cross(vt0, vt1).Length() / 2;
            float S3 = Vector3.Cross(vt1, vt2).Length() / 2;

            return Math.Abs(S - S1 - S2 - S3) < 0.01;
        }
    }
}
