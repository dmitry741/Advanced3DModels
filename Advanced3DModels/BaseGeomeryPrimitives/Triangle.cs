using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Models3DLib
{
    /// <summary>
    /// Класс реализующий треугольник.
    /// </summary>
    public class Triangle
    {
        readonly IPoint3D[] _points;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="point1">Вершина 1.</param>
        /// <param name="point2">Вершина 2.</param>
        /// <param name="point3">Вершина 3.</param>
        public Triangle(IPoint3D point1, IPoint3D point2, IPoint3D point3)
        {
            _points = new IPoint3D[3];

            _points[0] = point1;
            _points[1] = point2;
            _points[2] = point3;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="points">Массив из трех точек.</param>
        public Triangle(IPoint3D[] points)
        {
            _points = points;
        }

        /// <summary>
        /// Нормаль к треугольнику.
        /// </summary>
        public Vector3 Normal
        {
            get
            {
                Vector3 v1 = _points[1].ToVector3() - _points[0].ToVector3();
                Vector3 v2 = _points[2].ToVector3() - _points[0].ToVector3();

                return Vector3.Cross(v1, v2);
            }
        }

        /// <summary>
        /// Флаг видимости с обеих сторон.
        /// </summary>
        public bool VisibleBackSide { get; set; } = false;

        /// <summary>
        /// Массив цветов.
        /// </summary>
        public Color[] TextureColors { get; set; }

        /// <summary>
        /// Исходный цвет треугольника.
        /// </summary>
        public Color BaseColor { get; set; } = Color.LightGreen;

        /// <summary>
        /// Цвета для отображения.
        /// </summary>
        public Color[] RenderColors { get; set; } = new Color[3];

        /// <summary>
        /// Яркость зеркальной составляющей освещения.
        /// </summary>
        public float ReflectionBrightness { get; set; } = 80.0f;

        /// <summary>
        /// Конус зеркальной составляющей освещения.
        /// </summary>
        public float ReflectionCone { get; set; } = 24600;

        /// <summary>
        /// Флаг использования зеркальной составляющей освещения для данного треугольника.
        /// </summary>
        public bool ReflectionEnable { get; set; } = true;

        /// <summary>
        /// Флаг возможности применения метода Гуро для рендеренга треугольника.
        /// </summary>
        public bool AllowToGouraudMethod { get; set; } = true;

        /// <summary>
        /// Минимальное значение по Z.
        /// </summary>
        public float MinZ => _points.Min(p => p.Z);

        /// <summary>
        /// Первая вершина в треугольнике.
        /// </summary>
        public IPoint3D Point0 => _points[0];

        /// <summary>
        /// Массив вершин треугольника.
        /// </summary>
        public IPoint3D[] Point3Ds => _points;

        /// <summary>
        /// Массив объектов PointF.
        /// </summary>
        public PointF[] Points => _points.Select(x => x.ToPointF()).ToArray();
    }
}
