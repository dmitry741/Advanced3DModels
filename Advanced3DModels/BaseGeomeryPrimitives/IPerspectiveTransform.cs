using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models3DLib
{
    /// <summary>
    /// Декларация методов для перспективных преобразований.
    /// </summary>
    public interface IPerspectiveTransform
    {
        /// <summary>
        /// Метод выполняет перспективное преобразование
        /// </summary>
        /// <param name="point">Исходная точка.</param>
        /// <param name="center">Центр перспективы.</param>
        /// <returns>Point3D объект.</returns>
        Point3D Transform(Point3D point, Point3D center);
    }
}
