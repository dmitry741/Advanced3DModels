using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Перечисление типов отрисовки модели.
    /// </summary>
    public enum RenderModelType
    {
        /// <summary>
        /// Только трегуольники без закрашивания.
        /// </summary>
        Triangulations,

        /// <summary>
        /// Полный рендеринг с освещением модели. 
        /// </summary>
        FillFull,

        /// <summary>
        /// Рендеринг модели без освещения. Все треугольники закрашиваются одном цветом.
        /// </summary>
        FillSolidColor
    }

    /// <summary>
    /// Перечисление типов освещения модели.
    /// </summary>
    public enum RenderFillTriangle
    {
        /// <summary>
        /// Вычисления цвета треугольника по одной точке (вершине).
        /// </summary>
        Flat0,

        /// <summary>
        /// Вычисления цвета треугольника по трем точкам (вершинам).
        /// </summary>
        Flat3,

        /// <summary>
        /// Метод Гуро. Градиентная закраска треугольника.
        /// </summary>
        Gouraud
    }
}
