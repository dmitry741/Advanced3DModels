using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Models3DLib
{
    /// <summary>
    /// Интерфейс декларирующий  методы для источника света.
    /// </summary>
    public interface ILightSource
    {
        /// <summary>
        /// Значимость источника света.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// Получение вектора света для точки.
        /// </summary>
        /// <param name="point">Точка в пространтсве.</param>
        /// <returns>Вектор луча света для данной точки.</returns>
        Vector3 GetRay(IPoint3D point);
    }
}
