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
    /// Интерфейс декларирующий свойства и методы для точки в трехмерном пространстве.
    /// </summary>
    public interface IPoint3D
    {
        /// <summary>
        /// X координата.
        /// </summary>
        float X { get; set; }

        /// <summary>
        /// Y координата.
        /// </summary>
        float Y { get; set; }

        /// <summary>
        /// Z координата.
        /// </summary>
        float Z { get; set; }

        /// <summary>
        /// Преобразоваени в объект System.Numerics.Vector3.
        /// </summary>
        /// <returns></returns>
        Vector3 ToVector3();

        /// <summary>
        /// Преобразование в объект System.Drawing.PointF.
        /// </summary>
        /// <returns></returns>
        PointF ToPointF();        
    }
}
