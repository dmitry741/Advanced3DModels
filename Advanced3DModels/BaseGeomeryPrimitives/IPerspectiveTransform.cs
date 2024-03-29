﻿using System;
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
        /// Перспективное преобразование.
        /// </summary>
        /// <param name="point">Исходная точка.</param>
        /// <param name="center">Центр перспективы.</param>
        /// <returns>IPoint3D объект. Точка с учетом перспективного преобразования.</returns>
        IPoint3D Transform(IPoint3D point, IPoint3D center);
    }
}
