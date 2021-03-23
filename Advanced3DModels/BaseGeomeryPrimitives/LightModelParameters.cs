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
    /// Класс, представляющий набор параметров необходимых для расчета освещения модели.
    /// </summary>
    public class LightModelParameters
    {
        /// <summary>
        /// Точка для которой расчитывается освещенность.
        /// </summary>
        public IPoint3D Point { get; set; }

        /// <summary>
        /// Нормаль к поверхности в данной точке.
        /// </summary>
        public Vector3 Normal { get; set; }

        /// <summary>
        /// Флаг использования зеркальной составляющей освещения в данной точке.
        /// </summary>
        public bool ReflectionEnable { get; set; }

        /// <summary>
        /// Яркость зеркальной составляющей освещения в данной точке.
        /// </summary>
        public float ReflectionBrightness { get; set; }

        /// <summary>
        /// Конус зеркальной составляющей освещения в данной точке.
        /// </summary>
        public float ReflcetionCone { get; set; }

        /// <summary>
        /// Исходный цвет модели в данной точке.
        /// </summary>
        public Color BaseColor { get; set; }

        /// <summary>
        /// Коллекция источников цвета.
        /// </summary>
        public IEnumerable<ILightSource> LightSources { get; set; }

        /// <summary>
        /// Точка наблюдения.
        /// </summary>
        public IPoint3D PointObserver { get; set; }

        /// <summary>
        /// Интерфейс коррекции с учетом тумана.
        /// </summary>
        public IFog Fog { get; set; }
    }
}
