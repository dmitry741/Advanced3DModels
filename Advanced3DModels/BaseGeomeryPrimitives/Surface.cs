using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Models3DLib
{
    /// <summary>
    /// Делегат представляющий собой функцию двух переменных.
    /// </summary>
    /// <param name="X">X координата.</param>
    /// <param name="Y">Y координата.</param>
    /// <returns>Значение функции (float).</returns>
    public delegate float Function3D(float X, float Y);

    /// <summary>
    /// Класс реализующий изогнутую поверхность.
    /// </summary>
    public class Surface : Polygon4Plane
    {
        public Surface(IPoint3D p1, IPoint3D p2, IPoint3D p3, IPoint3D p4, float sizePrimitive) :
            base(p1, p2, p3, p4, sizePrimitive, Color.LightGreen, string.Empty) 
        {
            foreach (Triangle triangle in _triangles)
            {
                triangle.VisibleBackSide = true;
            }

            _point3Ds.Add(p1);
            _point3Ds.Add(p2);
            _point3Ds.Add(p3);
        }

        RectangleF BoundRect
        {
            get
            {
                float xmin = _point3Ds.Min(v => v.X);
                float xmax = _point3Ds.Max(v => v.X);
                float ymin = _point3Ds.Min(v => v.Y);
                float ymax = _point3Ds.Max(v => v.Y);

                return new RectangleF(xmin, ymin, xmax - xmin + 1, ymax - ymin + 1);
            }
        }

        IPoint3D ConvertToReal(RectangleF realBoundRect, RectangleF compBoundRect, IPoint3D point)
        {
            float x = realBoundRect.Width / compBoundRect.Width * (point.X - compBoundRect.X) + realBoundRect.X;
            float y = realBoundRect.Bottom - realBoundRect.Height / compBoundRect.Height * (point.Y - compBoundRect.Top);

            return ResolverInterface.ResolveIPoint3D(x, y, point.Z);
        }

        /// <summary>
        /// Нормаль к опорным точкам поверхности.
        /// </summary>
        public Vector3 Normal
        {
            get
            {
                int count = _point3Ds.Count();
                Triangle triangle = new Triangle(_point3Ds[count - 3], _point3Ds[count - 2], _point3Ds[count - 1]);
                return triangle.Normal;
            }
        }

        /// <summary>
        /// Создание поверхности как графика функции z=f(x, y).
        /// </summary>
        /// <param name="realBoundRect">Область в реальной системе координат.</param>
        /// <param name="function3D">Функция z=f(x, y).</param>
        /// <param name="ZMinComp">Минимальное значение Z в компьютерной системе координат.</param>
        /// <param name="ZmaxComp">Максимальное значение Z в компьютерной системе координат.</param>
        /// <param name="color">Цвет поверхности.</param>
        public void CreateSurface(RectangleF realBoundRect, Function3D function3D, float ZMinComp, float ZmaxComp, Color color)
        {
            RectangleF compBoundRect = BoundRect;

            // вычисляем минимальное и максимальное значения по оси Z
            float ZMinReal = float.MaxValue;
            float ZMaxReal = float.MinValue;

            foreach(IPoint3D point in _point3Ds)
            {
                IPoint3D realPoint = ConvertToReal(realBoundRect, compBoundRect, point);
                float z = function3D(realPoint.X, realPoint.Y);

                if (z < ZMinReal) ZMinReal = z;
                if (z > ZMaxReal) ZMaxReal = z;
            }

            foreach (IPoint3D point in _point3Ds)
            {
                IPoint3D realPoint = ConvertToReal(realBoundRect, compBoundRect, point);
                float z = function3D(realPoint.X, realPoint.Y);

                point.Z = (ZmaxComp - ZMinComp) / (ZMaxReal - ZMinReal) * (z - ZMinReal) + ZMinComp;
            }

            // цвет для треугольников
            foreach (Triangle triangle in _triangles)
            {                
                triangle.BaseColor = color;
            }
        }

        /// <summary>
        /// Создание поверхности как графика функции z=f(x, y).
        /// </summary>
        /// <param name="realBoundRect">Область в реальной системе координат.</param>
        /// <param name="function3D">Функция z=f(x, y).</param>
        /// <param name="ZMinComp">Минимальное значение Z в компьютерной системе координат.</param>
        /// <param name="ZmaxComp">Максимальное значение Z в компьютерной системе координат.</param>
        /// <param name="palette">Массив цветов для раскраски по высоте (Z).</param>
        public void CreateSurface(RectangleF realBoundRect, Function3D function3D, float ZMinComp, float ZmaxComp, Color[] palette)
        {
            CreateSurface(realBoundRect, function3D, ZMinComp, ZmaxComp, Color.Black);

            // раскраска теругольников по высоте
            foreach(Triangle triangle in _triangles)
            {
                float Z = triangle.Point3Ds.Average(point => point.Z);
                int index = Convert.ToInt32((palette.Length - 1) / (ZmaxComp - ZMinComp) * (Z - ZMinComp));
                triangle.BaseColor = palette[index];
            }
        }
    }
}
