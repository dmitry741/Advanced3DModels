using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Интерфейс декларирующий методы для моделей, которые рендерятся попиксельно.
    /// </summary>
    public interface IPixelsModel
    {
        /// <summary>
        /// Принадлежность точки модели.
        /// </summary>
        /// <param name="X">X координата.</param>
        /// <param name="Y">Y координата.</param>
        /// <returns>True если точка принадлежит модели, False в противном случае.</returns>
        bool Contains(float X, float Y);

        /// <summary>
        /// Прямоугольная область на плоскости XOY, в которую вписана модель.
        /// </summary>
        RectangleF BoundRect { get; }

        /// <summary>
        /// Получение вектора нормали для заданной точки.
        /// </summary>
        /// <param name="X">X координата.</param>
        /// <param name="Y">Y координата.</param>
        /// <returns>Вектор нормали.</returns>
        Vector3 GetNormal(float X, float Y);

        /// <summary>
        /// Получение Z координаты по точке на плоскости.
        /// </summary>
        /// <param name="X">X координата.</param>
        /// <param name="Y">Y координата.</param>
        /// <returns>Z координата.</returns>
        float GetZ(float X, float Y);

        /// <summary>
        /// Получение цвета модели в точке на плоскости.
        /// </summary>
        /// <param name="X">X координата.</param>
        /// <param name="Y">Y координата.</param>
        /// <returns>Объект Color.</returns>
        Color GetColor(float X, float Y);

        /// <summary>
        /// Преобразование модели (Параллельный перенос или вращение).
        /// </summary>
        /// <param name="matrix">Матрица преобразования.</param>
        void Transform(Matrix4x4 matrix);

        /// <summary>
        /// Флаг использования зеркальной составляющей освещения в данной точке.
        /// </summary>
        /// <param name="X">X координата.</param>
        /// <param name="Y">Y координата.</param>
        /// <returns>True если используется зеркальная составляющая, False в противном случае.</returns>
        bool ReflectionEnable(float X, float Y);

        /// <summary>
        /// Яркость зеркальной составляющей освещения в данной точке.
        /// </summary>
        /// <param name="X">X координата.</param>
        /// <param name="Y">Y координата.</param>
        /// <returns>Яркость зеркальной составляющей.</returns>
        float ReflectionBrightness(float X, float Y);

        /// <summary>
        /// Конус зеркальной составляющей освещения в данной точке.
        /// </summary>
        /// <param name="X">X координата.</param>
        /// <param name="Y">Y координата.</param>
        /// <returns>Конус зеркальной составляющей.</returns>
        float ReflcetionCone(float X, float Y);
    }
}
