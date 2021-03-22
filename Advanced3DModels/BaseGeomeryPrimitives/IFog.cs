using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Models3DLib
{
    /// <summary>
    /// Интерфейс декларирующий методы для учета влияния тумана на рендеринг модели.
    /// </summary>
    public interface IFog
    {
        /// <summary>
        /// Коррекция цвета с учетом тумана.
        /// </summary>
        /// <param name="z">Z координата.</param>
        /// <param name="color">Цвет модели.</param>
        /// <returns>Объект Color. Скорректированный с учетом тумана цвет.</returns>
        Color Correct(float z, Color color);

        /// <summary>
        /// Флаг необходимости коррекции цвета с учетом тумана.
        /// </summary>
        bool Enabled { get; set; }
    }
}
